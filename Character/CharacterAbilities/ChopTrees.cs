using System.Collections;
using System.Collections.Generic;
using HGS.LastBastion.Core.Stats;
using HGS.LastBastion.Resource;
using UnityEngine;
using UnityEngine.AI;

namespace HGS.LastBastion.Character.CharacterAbilities
{
    public class ChopTrees : CharacterAbility
    {
        private NavMeshAgent agent;
        private Trees[] trees;
        CharacterHandleWeapon charWeapon;
        private CharacterMovement charMovement;
        private CharacterOrientation orientation;

        protected override void Awake()
        {
            base.Awake();
            agent = character.GetComponent<NavMeshAgent>();
            charMovement = GetComponent<CharacterMovement>();
            orientation = GetComponent<CharacterOrientation>();
            charWeapon = GetComponent<CharacterHandleWeapon>();
            InitializeAgent();
            trees = FindObjectsByType<Trees>(FindObjectsSortMode.None);

            foreach (var tree in trees)
            {
                tree.OnTreeClicked += ChopTree;
            }
        }

        private void InitializeAgent()
        {
            // float speed = (character.FindAbility<CharacterMovement>() == null) ? character.GetComponentInChildren<CharacterMovement>().walkSpeed : character.FindAbility<CharacterMovement>().walkSpeed;
            agent.speed = charMovement.walkSpeed;
            agent.stoppingDistance = 1;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.acceleration = 100f;
            agent.angularSpeed = 0f;
        }
        private void ChopTree(Trees tree)
        {
            Collider2D targetCollider = tree.GetComponent<BoxCollider2D>();
            Vector2 closestPoint = targetCollider.ClosestPoint(character.transform.position);
            Health damageable = tree.GetComponent<Health>();
            StartCoroutine(ChopTreeCoroutine(closestPoint, damageable));
        }

        IEnumerator ChopTreeCoroutine(Vector3 treePos, Health damageable)
        {
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent is null. Cannot start ChopTreeCoroutine.");
                yield break;
            }
            agent.isStopped = false;
            // Calculate path before committing to movement
            NavMeshPath path = new NavMeshPath();
            bool pathFound = agent.CalculatePath(treePos, path);

            if (!pathFound || path.status != NavMeshPathStatus.PathComplete)
            {
                Debug.LogWarning("Tree destination is not reachable.");
                agent.isStopped = false;
                yield break;
            }
            charMovement.ChangePerformType(AbilityPerformType.Script);
            // Move toward the tree
            // agent.SetDestination(treePos);
            charMovement.SetAIMovement(treePos, agent);
            PerformAnimation(AnimatorParemeterType.Bool, AnimatorHelper.WalkingAnimationParameter, true);

            // Wait until the agent reaches the destination
            while (!HasReachedDestination())
            {
                yield return null;
            }

            charMovement.StopMoving();
            charMovement.ChangePerformType(AbilityPerformType.FixedUpdate);
            Debug.Log("Destination reached. Start chopping!");

            character.ChangeState(Statemachine.CharacterStates.ConditionStates.Chopping);
            while (damageable.Alive())
            {
                charWeapon.AimAtTarget(treePos);
                charWeapon.UseWeapon();
                yield return null;
            }
            character.ChangeState(Statemachine.CharacterStates.ConditionStates.Normal);

        }

        bool HasReachedDestination()
        {
            if (agent.pathPending)
                return false;

            if (agent.remainingDistance > agent.stoppingDistance)
                return false;

            return !agent.hasPath || agent.velocity.sqrMagnitude < 0.01f;
        }

    }
}
