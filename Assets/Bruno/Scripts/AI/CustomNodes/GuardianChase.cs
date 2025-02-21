using UnityEngine;
using MBT;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("Native/GuardianChase")]
    public class GuardianChase : Leaf
    {
        public Blackboard blackboard;
        private GameObjectVariable _mSelf;
        private GameObjectVariable _mTarget;
        private IntVariable _mId;
        private GuardianMob _mMob;
        
        public override void OnEnter()
        {
            _mSelf = blackboard.GetVariable<GameObjectVariable>("Self");
            _mTarget = blackboard.GetVariable<GameObjectVariable>("Target");
            _mId = blackboard.GetVariable<IntVariable>("id");
            _mMob = _mSelf.Value.GetComponent<GuardianMob>();
            _mTarget.Value = _mMob.player;
        }
        
        public override NodeResult Execute()
        {
            _mMob.DisableIsStunned();
            
            if (!_mMob.PlayerDetected())
            {
                _mId.Value = 0;
                _mMob.agent.ResetPath();
                _mMob.SetIdleAnimation();
                return NodeResult.success;
            }

            if (_mMob.gotHit)
            {
                _mId.Value = 4;
                _mMob.SetIsStunned();
                return NodeResult.success;
            }
            
            _mId.Value = 1;
            _mMob.agent.SetDestination(_mTarget.Value.transform.position);
            _mMob.SetWalkAnimation();

            if (_mMob.IsCloseToAttack(_mTarget.Value))//
            {
                _mId.Value = 3;
                _mMob.SetAttack();
                _mMob.agent.ResetPath();
                return NodeResult.success; 
            }
            
            return NodeResult.running;
        }
    }
}
