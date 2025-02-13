using System;
using UnityEngine;
using MBT;
using UnityEngine.AI;

namespace Bruno.Scripts.AI
{
    public class NativeMob : Mob
    {
        [Header("Engage")]
        [SerializeField] private GameObject arrowSpawner;
        [SerializeField] private GameObject arrowPrefab;
       
        protected new void Start()
        {
            base.Start();
        }
    
        private new void Update()
        {
            base.Update();
        }
        
        /// <summary>
        /// Shot arrow - Couldn't adapt the throw function due to camera forward calculation within player.Throw()
        /// </summary>
        public void Pop()
        {
            if (!arrowSpawner || !arrowPrefab)
                return;
            Instantiate(arrowPrefab, arrowSpawner.transform.position, arrowSpawner.transform.rotation);
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
