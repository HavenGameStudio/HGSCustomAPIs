using System;
using System.Collections.Generic;
using HGS.LastBastion.Character.CharacterAbilities;
using HGS.LastBastion.Core.Managers;
using UnityEngine;


namespace HGS.LastBastion.Character.Core
{
    public class HGSCharacterController : MonoBehaviour
    {
        private HGSCharacter character;

        private void Awake()
        {
            character = GetComponentInParent<HGSCharacter>();
        }

        void OnEnable()
        {
            character.OnCharacterInitialize += InitializeController;
        }

        private void OnDisable()
        {
            character.OnCharacterInitialize -= InitializeController;
        }

        /// <summary>
        /// Initializes the controller.
        /// </summary>
        private void InitializeController()
        {
            if (character.characterType != CharacterType.Player)
            {
                return;
            }

            List<CharacterAbility> charAbilities = new List<CharacterAbility>();
            InputManager inputManager = FindFirstObjectByType<InputManager>();

            for (int i = 0; i < character._characterAbilities.Length; i++)
            {
                charAbilities.Add(character._characterAbilities[i]);
            }

            foreach (var ability in charAbilities)
            {
                ability.RegisterInput(inputManager);
            }

        }

    }
}
