using System.Collections;
using HGS.LastBastion.Character;
using HGS.LastBastion.Character.Core;
using HGS.LastBastion.Character.Statemachine;
using HGS.LastBastion.Core.Managers;
using HGS.Tools;
using UnityEngine;
using UnityEngine.AI;

namespace HGS.LastBastion.Character.CharacterAbilities
{
    [AddComponentMenu("Haven Game Studio/Character/Abilities/Character Movement")]
    public class CharacterMovement : CharacterAbility
    {
        public override string HelpBoxText()
        {
            return "Handles character movement for both player-controlled and AI characters. Uses Rigidbody2D for player movement and NavMeshAgent for AI navigation. Also manages walking animations based on movement state.";
        }

        public float walkSpeed;

        [HGSReadOnly]
        public Vector2 movement;
        [HGSReadOnly]

        private NavMeshAgent agent;
        protected override void Awake()
        {
            base.Awake();

            agent = GetComponentInParent<NavMeshAgent>();
            InitializeNavmeshAgent();

        }

        /// <summary>
        /// Initialize the NavMeshAgent
        /// </summary>
        private void InitializeNavmeshAgent()
        {
            if (agent == null)
            {
                Debug.LogWarning($"There is no navmesh agent in parent or in this {name} gameobject. This agent will not move unless you add it");
                return;
            }

            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = walkSpeed;
            agent.acceleration = 100f;
        }
        public override bool RegisterInput(InputManager inputManager)
        {
            // if (!base.RegisterInput(inputManager))
            // {
            //     return false;
            // }
            inputManager.OnMoveEvent += SetMovement;
            return true;
        }
        protected override void PerformAbility()
        {
            SetMovement(movement);
        }
        private void Update()
        {
            if (agent != null && !agent.isStopped)
            {
                movement = new Vector2(agent.desiredVelocity.x, agent.desiredVelocity.z);
            }
        }

        /// <summary>
        /// Sets the movement of a character based on input.
        /// Used by player.
        /// </summary>
        /// <param name="_movement">The movement value of the input</param>
        private void SetMovement(Vector2 _movement)
        {
            if (HasCondition(character.characterCondition))
            {
                return;
            }
            agent.isStopped = true;
            movement = new Vector2(_movement.x, _movement.y).normalized;

            _rb.linearVelocity = movement * walkSpeed;


            bool isWalking = movement != Vector2.zero;
            PerformAnimation(AnimatorParemeterType.Bool, AnimatorHelper.WalkingAnimationParameter, isWalking);
            if (isWalking)
            {
                character.ChangeState(CharacterStates.MovementStates.Walking);
            }
            else
            {
                character.ChangeState(CharacterStates.MovementStates.Idle);
            }
        }


        /// <summary>
        /// Sets the movement of the AI.
        /// </summary>
        /// <param name="targetPosition">The destination of the AI or the target position</param>
        public void SetAIMovement(Vector3 targetPosition)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPosition);
            movement = agent.velocity;

            bool isWalking = !agent.isStopped; //if the agent is stopped then we set the iswalking to false
            PerformAnimation(AnimatorParemeterType.Bool, AnimatorHelper.WalkingAnimationParameter, isWalking);

        }
        public void SetAIMovement(Vector3 targetPosition, NavMeshAgent agent)
        {
            character.ChangeState(CharacterStates.MovementStates.Walking);
            agent.isStopped = false;
            agent.SetDestination(targetPosition);
            bool isWalking = !agent.isStopped; //if the agent is stopped then we set the iswalking to false
            PerformAnimation(AnimatorParemeterType.Bool, AnimatorHelper.WalkingAnimationParameter, isWalking);

        }

        public void StopMoving()
        {
            _rb.linearVelocity = Vector3.zero;
            movement = Vector2.zero;
            agent.isStopped = true;
            animator.SetBool("Walking", false);
            character.ChangeState(CharacterStates.MovementStates.Idle);
            Debug.Log($"Stopped Moving || agent is stopped? : {agent.isStopped}");
        }
    }
}
