using System;
using UnityEngine;
using MBT;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Quaternion = System.Numerics.Quaternion;

namespace Bruno.Scripts.AI
{
    public class NativeMob : Mob
    {
        [Header("Engage")] 
        [SerializeField] [Range(1.0f, 5.5f)]  private float maxDistance = 2.0f;
        [SerializeField] private GameObject arrowSpawner;
        [SerializeField] private GameObject arrowPrefab;
        
        public bool attackComplete { get; private set; }
       
        protected new void Start()
        {
            base.Start();
        }
    
        private new void Update()
        {
            base.Update();
        }

        protected new void FixedUpdate()
        {
            base.FixedUpdate();
        }
        
        /// <summary>
        /// Shot arrow - Couldn't adapt the throw function due to camera forward calculation within player.Throw()
        /// </summary>
        public void Pop()
        {
            if (!arrowSpawner || !arrowPrefab)
                return;
            Instantiate(arrowPrefab, arrowSpawner.transform.position, arrowSpawner.transform.rotation);
            attackComplete = false;
        }

        public void AttackComplete()
        {
            attackComplete = true;
        }
        
        public override bool IsCloseToAttack(GameObject target)
        {
            var distanceVector = (target.transform.position - agent.transform.position);
            var closestDistance = Vector3.Dot(agent.transform.forward, distanceVector);

            if (closestDistance >= attackAreaThreshold)
            {
                transform.LookAt(target.transform, Vector3.up);
                return true;
            }

            return false;
        }
        
        
        /**
         * Function used to keep native mob (armed with bow)
         * not too close from the player while chasing it
         * <param name="target"> Player </param>
         * <returns> true if too close </returns>
         */
        public bool IsPlayerTooClose(GameObject target)
        {
            var dist = Vector3.Distance(target.transform.position, agent.transform.position);

            if (dist <= maxDistance)
            {
                return true;
            }

            return false;
        }

        public void SetIdleAnimation()
        {
            if (!MAnimator) return;
            MAnimator.SetFloat("velocity", 0.0f);
        }

        public void SetWalkAnimation()
        {
            if(!MAnimator) return;
            MAnimator.SetFloat("velocity", 1.0f);
        }

        public void SetAttackAnimation()
        {
            if(!MAnimator) return;
            MAnimator.SetBool("attack", true);
        }

        public void DisableAttackAnimation()
        {
            if(!MAnimator) return;
            MAnimator.SetBool("attack", false);
        }
        
        protected new void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }
    }
}
