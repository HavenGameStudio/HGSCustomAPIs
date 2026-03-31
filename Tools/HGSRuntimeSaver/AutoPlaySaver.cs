#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class AutoPlaySaver : MonoBehaviour
{
    [Header("🔄 SAVE MODE")]
    [SerializeField] public SaveMode saveMode = SaveMode.Manual;
    
    [Header("⏱️ Auto Save (if enabled)")]
    [Range(0.1f, 5f)]
    public float autoSaveInterval = 1f;
    
    [Header("⚙️ What to Save")]
    public bool saveTransform = true;
    public bool saveAllComponents = true;
    public List<string> ignoreComponents = new List<string>();
    
    [Header("📊 Status")]
    public bool hasSaveFile;
    public string saveFilePath;
    
    public enum SaveMode
    {
        Manual,
        Auto
    }
    
    private SavedComponentState savedState;
    private string savePath;
    private float lastSaveTime;
    
    void OnEnable()
    {
        savePath = GetSavePath();
        saveFilePath = savePath;
        RefreshStatus();
        
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        EditorSceneManager.sceneSaved += OnSceneSaved;
        Undo.undoRedoPerformed += OnUndoRedo;
        
        if (saveMode == SaveMode.Auto)
            InvokeRepeating(nameof(AutoSave), 0.1f, autoSaveInterval);
    }
    
    void OnDisable()
    {
        CancelInvoke(nameof(AutoSave));
        
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorSceneManager.sceneSaved -= OnSceneSaved;
        if (Undo.undoRedoPerformed != null)
            Undo.undoRedoPerformed -= OnUndoRedo;
        
        SaveState(); // Always save on disable
    }
    
    void Update()
    {
        // 🔥 CRITICAL: EditMode auto-detection
        if (saveMode == SaveMode.Auto && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if (Time.time - lastSaveTime > 0.3f && HasChanges())
            {
                SaveState();
            }
        }
    }
    
    void OnPlayModeChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredPlayMode:
                LoadSavedState();
                break;
                
            case PlayModeStateChange.ExitingPlayMode:
                SaveState(); // 🔥 Save PlayMode changes
                break;
                
            case PlayModeStateChange.EnteredEditMode:
                // 🔥 CRITICAL: Apply to EditMode Scene!
                LoadSavedState();
                EditorUtility.SetDirty(gameObject);
                EditorUtility.SetDirty(transform);
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
                break;
        }
        RefreshStatus();
    }
    
    void OnSceneSaved(Scene scene)
    {
        if (scene == gameObject.scene)
            SaveState();
    }
    
    void OnUndoRedo()
    {
        LoadSavedState();
    }
    
    #region 💾 CORE METHODS
    [ContextMenu("💾 Save State")]
    public void SaveState()
    {
        savedState = CaptureCurrentState();
        string json = JsonUtility.ToJson(savedState, true);
        File.WriteAllText(savePath, json);
        
        lastSaveTime = Time.time;
        RefreshStatus();
        Debug.Log($"<color=green>💾 [{gameObject.name}] Saved → Scale stays forever!</color>");
    }
    
    [ContextMenu("📂 Load State")]
    public void LoadSavedState()
    {
        if (!File.Exists(savePath)) 
        {
            Debug.LogWarning($"❌ [{gameObject.name}] No save file");
            return;
        }
        
        try
        {
            string json = File.ReadAllText(savePath);
            savedState = JsonUtility.FromJson<SavedComponentState>(json);
            
            // 🔥 IMMEDIATELY APPLY TO SCENE
            RestoreState(savedState);
            
            #if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
            EditorUtility.SetDirty(transform);
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
            #endif
            
            Debug.Log($"<color=cyan>📂 [{gameObject.name}] Loaded → Scale restored!</color>");
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ [{gameObject.name}] Load error: {e.Message}");
        }
    }
    
    void AutoSave()
    {
        if (saveMode == SaveMode.Auto)
            SaveState();
    }
    #endregion
    
    #region 🎯 STATE CAPTURE/RESTORE
    SavedComponentState CaptureCurrentState()
    {
        SavedComponentState state = new SavedComponentState
        {
            objectName = gameObject.name,
            objectID = gameObject.GetInstanceID().ToString(),
            sceneName = gameObject.scene.name,
        };
        
        if (saveTransform)
        {
            state.transformState = new TransformState
            {
                position = transform.position,
                rotation = transform.rotation,
                localPosition = transform.localPosition,
                localRotation = transform.localRotation,
                localScale = transform.localScale,
            };
        }
        
        if (saveAllComponents)
        {
            Component[] components = GetComponents<Component>();
            foreach (Component comp in components)
            {
                if (ShouldSave(comp))
                {
                    try
                    {
                        ComponentState compState = new ComponentState
                        {
                            typeName = comp.GetType().FullName,
                            jsonData = JsonUtility.ToJson(comp, true)
                        };
                        state.components.Add(compState);
                    }
                    catch { }
                }
            }
        }
        
        return state;
    }
    
    void RestoreState(SavedComponentState state)
    {
        if (state == null) return;
        
        if (saveTransform && state.transformState != null)
        {
            transform.position = state.transformState.position;
            transform.rotation = state.transformState.rotation;
            transform.localPosition = state.transformState.localPosition;
            transform.localRotation = state.transformState.localRotation;
            transform.localScale = state.transformState.localScale;
        }
        
        foreach (var compState in state.components)
        {
            try
            {
                Type compType = Type.GetType(compState.typeName);
                if (compType != null)
                {
                    Component comp = GetComponent(compType);
                    if (comp != null)
                        JsonUtility.FromJsonOverwrite(compState.jsonData, comp);
                }
            }
            catch { }
        }
    }
    
    bool ShouldSave(Component comp)
    {
        if (comp == null) return false;
        if (comp is Behaviour behaviour && !behaviour.enabled) return false;
        string name = comp.GetType().Name;
        return !ignoreComponents.Contains(name) && name != nameof(AutoPlaySaver);
    }
    
    bool HasChanges()
    {
        return savedState == null || 
               transform.localScale != savedState.transformState?.localScale;
    }
    #endregion
    
    #region 🛠️ UTILITIES
    string GetSavePath()
    {
        string safeName = gameObject.name.Replace('/', '_').Replace('\\', '_').Replace(' ', '_');
        return Path.Combine(Application.persistentDataPath, $"AutoPlay_{safeName}_{gameObject.scene.name}.json");
    }
    
    void RefreshStatus()
    {
        hasSaveFile = File.Exists(savePath);
    }
    
    [ContextMenu("🗑️ Delete Save File")]
    public void DeleteSaveFile()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            savedState = null;
            RefreshStatus();
            Debug.Log($"<color=red>🗑️ [{gameObject.name}] Save deleted!</color>");
        }
    }
    #endregion
}

// Data classes
[System.Serializable]
public class SavedComponentState
{
    public string objectName, objectID, sceneName;
    public TransformState transformState;
    public List<ComponentState> components = new List<ComponentState>();
}

[System.Serializable]
public class TransformState
{
    public Vector3 position, localPosition, localScale;
    public Quaternion rotation, localRotation;
}

[System.Serializable]
public class ComponentState
{
    public string typeName, jsonData;
}
#endif