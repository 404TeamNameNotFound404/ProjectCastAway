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
        
        protected new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override bool IsCloseToAttack(GameObject target)
        {
            if (!PlayerDetected()) return false;
            var distance = Vector3.Distance(target.transform.position, agent.transform.position);
            return distance <= attackAreaThreshold;
        }

        #region Animations
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
        #endregion
        
        protected new void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }
        
        
        #region Animations

        public void EnableHitbox()
        {
            _mDaggerCollider.enabled = true;
        }

        public void DisableHitbox()
        {
            _mDaggerCollider.enabled = false;
        }
        #endregion
    }
}
