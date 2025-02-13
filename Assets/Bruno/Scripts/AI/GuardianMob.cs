using MBT;
using UnityEngine;
using UnityEngine.AI;

namespace Bruno.Scripts.AI
{
    public class GuardianMob : Mob
    {
        private static readonly int Velocity = Animator.StringToHash("velocity");
        private static readonly int Property = Animator.StringToHash("isStunned?");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private CapsuleCollider _mDaggerCollider;

        new void Start()
        {
            base.Start();
            _mDaggerCollider = GameObject.Find("Dagger").GetComponent<CapsuleCollider>();
            _mDaggerCollider.enabled = false;
        }
      
        new void Update()
        {
            base.Update();
        }


        public void SetWalkAnimation()
        {
            MAnimator.SetFloat(Velocity, 1.0f);
        }

        public void SetIdleAnimation()
        {
            MAnimator.SetFloat(Velocity, 0.0f);
        }

        public void EnableDaggerHitbox()
        {
            _mDaggerCollider.enabled = true;
        }

        public void DisableDaggerHitbox()
        {
            _mDaggerCollider.enabled = false;
        }

        public void SetIsStunned()
        {
            MAnimator.SetBool(Property, true);
        }

        public void DisableIsStunned()
        {
            MAnimator.SetBool(Property, false);
        }

        public void SetAttack()
        {
            MAnimator.SetTrigger(Attack);
        }
        
        
        protected new void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }
    }
}
