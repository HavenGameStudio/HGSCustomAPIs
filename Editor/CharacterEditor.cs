using HGS.AI;
using HGS.LastBastion.Character.CharacterAbilities;
using HGS.LastBastion.Character.Core;
using HGS.LastBastion.Character.Inventory;
using HGS.LastBastion.Core.Stats;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace HGS.LastBastion.CustomInspector
{
    [CustomEditor(typeof(HGSCharacter), true)]
    [CanEditMultipleObjects]
    public class CharacterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(
    "HGSCharacter is the core component that defines any playable or AI-controlled character. It handles character type, health, abilities, movement states, and initialization. Based on the selected type, it links models, animators, and AI behaviors to make the character function correctly in-game.",
    MessageType.Info
);

            EditorGUILayout.Space();
            serializedObject.Update();
            HGSCharacter character = (HGSCharacter)target;

            EditorGUILayout.LabelField("Movement State", character.characterMovementState.ToString());
            EditorGUILayout.LabelField("Condition State", character.characterCondition.ToString());
            EditorGUILayout.Space();

            if (character.characterAnimator == null)
            {
                if (character.GetComponentInChildren<Animator>() != null)
                {
                    character.characterAnimator = character.GetComponentInChildren<Animator>();
                }
            }

            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Autobuild", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This tool auto-builds a character by adding all the necessary components and configuring their values based on the selected character type (Playable, Enemy AI, or Ally AI).",
                MessageType.Info
            );
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "⚠️ WARNING: Pressing the AutoBuild button will reset your character and all components to their default settings. Do NOT proceed if you've already customized component values!",
                MessageType.Warning
            );

            EditorGUILayout.Space();

            if (GUILayout.Button("Build Playable Character"))
            {
                GenerateCharacter(CharacterType.Player, true);
            }
            if (GUILayout.Button("Build AI Enemy Character"))
            {
                character.characterType = CharacterType.AI;
                GenerateCharacter(CharacterType.AI, true, AIType.Enemy);
            }
            if (GUILayout.Button("Build AI Ally Character"))
            {
                character.characterType = CharacterType.AI;
                GenerateCharacter(CharacterType.AI, true, AIType.Ally);
            }
        }
        /// <summary>
        /// Generates a character based on the type and AI type.
        /// </summary>
        /// <param name="characterType">The type of the character to generate</param>
        /// <param name="withWeapon">is the character comes with a weapon?</param>
        /// <param name="_aIType">AI type of the AI Character to generate</param>
        private void GenerateCharacter(CharacterType characterType, bool withWeapon, AIType _aIType = AIType.NoneAI)
        {
            HGSCharacter character = (HGSCharacter)target;

            Debug.Log(character.name + " : Character Autobuild Start");

            BoxCollider2D boxCollider2D = (character.GetComponent<BoxCollider2D>() == null) ? character.AddComponent<BoxCollider2D>() : character.GetComponent<BoxCollider2D>();
            boxCollider2D.size = new Vector2(0.5f, 0.5f);
            boxCollider2D.offset = Vector2.zero;
            boxCollider2D.isTrigger = false;

            Rigidbody2D rigidbody2D = (character.GetComponent<Rigidbody2D>() == null) ? character.gameObject.AddComponent<Rigidbody2D>() : character.GetComponent<Rigidbody2D>(); ;
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.simulated = true;
            rigidbody2D.useAutoMass = false;
            rigidbody2D.mass = 1;
            rigidbody2D.linearDamping = 1;
            rigidbody2D.angularDamping = 0.05f;
            rigidbody2D.gravityScale = 0;
            rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAwake;
            rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

            //MODEL GAMEOBJECT CREATION
            GameObject model = new GameObject("Model");
            model.transform.SetParent(character.transform, worldPositionStays: false);
            model.transform.localPosition = Vector3.zero;
            character.characterModel = model;

            //CREATING SPRITE RENDERER
            SpriteRenderer spriteRenderer = (model.GetComponent<SpriteRenderer>() == null) ? model.AddComponent<SpriteRenderer>() : model.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Foreground";
            spriteRenderer.sortingOrder = 0;

            //CREATING ANIMATOR
            model.AddComponent<Animator>();

            //HEALTH
            Health health = (character.GetComponent<Health>() == null) ? character.AddComponent<Health>() : character.GetComponent<Health>();
            health.maximumHealth = 100f;
            health.SetCharacterModel(model);
            health.EditOnDeathCondition(true, true, true);
            character.health = health;
            Debug.Log($"Health created");

            //CREATE CHARACTERABILITIES
            GameObject abilityObj = new GameObject("Abilities");
            abilityObj.transform.SetParent(character.transform, worldPositionStays: false);
            abilityObj.transform.localPosition = Vector3.zero;
            Debug.Log($"Abilies created as a child");

            CharacterMovement characterMovement = (abilityObj.GetComponent<CharacterMovement>() == null) ? abilityObj.AddComponent<CharacterMovement>() : abilityObj.GetComponent<CharacterMovement>();
            characterMovement.walkSpeed = 2f;
            characterMovement.abilityName = "CharacterMovement";
            Debug.Log($"CharacterMovement Initialized");

            CharacterOrientation characterOrientation = (abilityObj.GetComponent<CharacterOrientation>() == null) ? abilityObj.AddComponent<CharacterOrientation>() : abilityObj.GetComponent<CharacterOrientation>();
            characterOrientation.SetFacingType(FacingType.Movement);
            characterOrientation.SetOrientationType(OrientationType.ModelFlip);
            characterOrientation.SetPerformType(AbilityPerformType.Update);
            characterOrientation.abilityName = "CharacterOrientation";
            Debug.Log($"CharacterOrientation Initialized");

            //IF WE ARE GOING TO CREATE CHARACTER WITH WEAPON THEN CREATE WEAPON HANDLE
            if (withWeapon)
            {
                //CREATE A WEAPON ATTACHMENT OR HANDLE WEAPON
                //WE WILL USE IT AS CHARACTERHANDLEWEAPON ABILITY
                GameObject weaponHandle = new GameObject("Handle Weapon");
                weaponHandle.transform.SetParent(character.transform, worldPositionStays: false);
                weaponHandle.transform.position = Vector3.zero;
                CharacterHandleWeapon characterHandleWeapon = (abilityObj.GetComponent<CharacterHandleWeapon>() == null) ? abilityObj.AddComponent<CharacterHandleWeapon>() : abilityObj.GetComponent<CharacterHandleWeapon>();
                characterHandleWeapon.SetWeaponAttachment(weaponHandle.transform);
                characterHandleWeapon.abilityName = "CharacterHandleWeapon";
                Debug.Log($"Character Handle Weapon Initialized");

            }

            //WE ADD CONTROLLER AND INVENTORY IF THIS IS PLAYER
            if (characterType == CharacterType.Player)
            {
                HGSCharacterController characterController = (character.GetComponent<HGSCharacterController>() == null) ? character.AddComponent<HGSCharacterController>() : character.GetComponent<HGSCharacterController>();
                CharacterInventory inventory = (character.GetComponent<CharacterInventory>() == null) ? character.AddComponent<CharacterInventory>() : character.GetComponent<CharacterInventory>();
                character.tag = "Player";
                character.gameObject.layer = LayerMask.NameToLayer("Player");
            }


            //WE CREATE HEALTHBAR
            if (character.CreateHealthBar)
            {
                GameObject healthBar = Instantiate(character.healthBarObject.gameObject);
                healthBar.name = "Health Bar";
                healthBar.transform.SetParent(character.transform, worldPositionStays: false);
                RectTransform rectTransform = healthBar.GetComponent<RectTransform>();
                rectTransform.position = new Vector3(0f, .7f, 0f);
                Debug.Log($"Healthbar Created");
            }

            //*********THIS SECTION IS FOR AI ONLY***********
            if (characterType != CharacterType.AI) return;

            GenerateBrain(character, _aIType);

            Debug.Log(character.name + " : Character Autobuild Complete");
        }

        /// <summary>
        /// Generates brain for AI Characters
        /// </summary>
        /// <param name="character">The owner of this brain</param>
        /// <param name="_aIType">Type of AI (ALLY, ENEMY)</param>
        public void GenerateBrain(HGSCharacter character, AIType _aIType)
        {
            Debug.Log($"Generating AIBrain for {character.name}");
            //BRAIN CREATION
            GameObject brain = new GameObject("AIBrain");
            brain.transform.SetParent(character.transform, worldPositionStays: false);
            brain.transform.position = Vector3.zero;

            AIBrain aIBrain = brain.AddComponent<AIBrain>();
            character.brain = aIBrain;
            aIBrain.Owner = character.gameObject;

            //ADD NECESSARY AIACTION AND AIDECISION
            //***********AIACTION****************
            AIActionDoNothing actionDoNothing = (brain.GetComponent<AIActionDoNothing>() == null) ? brain.AddComponent<AIActionDoNothing>() : brain.GetComponent<AIActionDoNothing>();

            AIActionMoveTowardsTarget actionMoveAI = (brain.GetComponent<AIActionMoveTowardsTarget>() == null) ? brain.AddComponent<AIActionMoveTowardsTarget>() : brain.GetComponent<AIActionMoveTowardsTarget>();

            AIActionUseWeapon useWeapon = (brain.GetComponent<AIActionUseWeapon>() == null) ? brain.AddComponent<AIActionUseWeapon>() : brain.GetComponent<AIActionUseWeapon>();

            //**********AIDECISION***************
            AIDecisionTargetDetectRadius2D decisionTargetDetectRadius2D = (brain.GetComponent<AIDecisionTargetDetectRadius2D>() == null) ? brain.AddComponent<AIDecisionTargetDetectRadius2D>() : brain.GetComponent<AIDecisionTargetDetectRadius2D>();
            decisionTargetDetectRadius2D.radius = 3f;
            decisionTargetDetectRadius2D.targetCheckFrequency = 2f;

            AIDecisionDistanceToTargetColliderEdge decisionDistanceToTargetColliderEdge = (brain.GetComponent<AIDecisionDistanceToTargetColliderEdge>() == null) ? brain.AddComponent<AIDecisionDistanceToTargetColliderEdge>() : brain.GetComponent<AIDecisionDistanceToTargetColliderEdge>();
            decisionDistanceToTargetColliderEdge.Distance = 1f;
            decisionDistanceToTargetColliderEdge.ComparisonMode = AIDecisionDistanceToTargetColliderEdge.ComparisonModes.LowerThan;

            AIDecisionTimeInState decisionTimeInState = (brain.GetComponent<AIDecisionTimeInState>() == null) ? brain.AddComponent<AIDecisionTimeInState>() : brain.GetComponent<AIDecisionTimeInState>();

            //CHANGE LAYER OR TAG
            //MODIFY THE NECCESSARY TARGET LAYER OF AI ACTION OR AI DECISION
            switch (_aIType)
            {
                case AIType.Ally:
                    character.gameObject.tag = "Ally";
                    character.gameObject.layer = LayerMask.NameToLayer("Ally");
                    decisionTargetDetectRadius2D.targetLayer = LayerMask.GetMask("Enemy");
                    break;
                case AIType.Enemy:
                    character.gameObject.tag = "Enemy";
                    character.gameObject.layer = LayerMask.NameToLayer("Enemy");
                    decisionTargetDetectRadius2D.targetLayer = LayerMask.GetMask("Ally", "Player", "ArcherTower");
                    break;
                default:
                    break;
            }

            Debug.Log($"{character.name}'s AIBrain successfully generated");
        }
    }
}
