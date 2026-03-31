using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AutoPlaySaver))]
public class AutoPlaySaverInspector : Editor
{
    public override void OnInspectorGUI()
    {
        AutoPlaySaver saver = (AutoPlaySaver)target;
        
        // Save Mode Dropdown
        EditorGUILayout.PropertyField(serializedObject.FindProperty("saveMode"));
        
        // Auto-save settings (conditional)
        if (saver.saveMode == AutoPlaySaver.SaveMode.Auto)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("autoSaveInterval"));
        }
        
        // Save options
        EditorGUILayout.PropertyField(serializedObject.FindProperty("saveTransform"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("saveAllComponents"));
        
        // Ignore list
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreComponents"));
        
        serializedObject.ApplyModifiedProperties();
        
        GUILayout.Space(20);
        
        // 🔥 BIG COLORED BUTTONS
        EditorGUILayout.BeginHorizontal();
        
        // SAVE BUTTON - Green
        if (GUILayout.Button("💾 SAVE STATE", GUILayout.Height(40), GUILayout.ExpandWidth(true)))
        {
            saver.SaveState();
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        
        // LOAD BUTTON - Blue  
        if (GUILayout.Button("📂 LOAD STATE", GUILayout.Height(40), GUILayout.ExpandWidth(true)))
        {
            saver.LoadSavedState();
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        
        // CLEAR BUTTON - Red
        if (GUILayout.Button("🗑️ CLEAR SAVE", GUILayout.Height(40), GUILayout.ExpandWidth(true)))
        {
            saver.GetType().GetMethod("DeleteSave", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(saver, null);
        }
        
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        // Status
        EditorGUILayout.HelpBox(saver.hasSaveFile ? 
            $"✅ Save File: {Path.GetFileName(saver.saveFilePath)}" : 
            "❌ No save file found", 
            saver.hasSaveFile ? MessageType.Info : MessageType.Warning);
        
        // File path
        EditorGUILayout.LabelField("Full Path:", saver.saveFilePath);
    }
}
#endif