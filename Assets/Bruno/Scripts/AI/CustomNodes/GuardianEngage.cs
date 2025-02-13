using MBT;
using UnityEngine;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("Native/GuardianEngage")]
    public class GuardianEngage : Leaf
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
        }
        
        public override NodeResult Execute()
        {
            _mMob.agent.ResetPath();
            
            _mId.Value = 3;
            _mTarget.Value = _mMob.player;
            _mMob.SetAttack();
            
            if (!_mMob.PlayerDetected())
            {
                _mMob.agent.ResetPath();
                _mId.Value = 0;
                _mMob.SetIdleAnimation();
                return NodeResult.success;
            }

            if (_mMob.PlayerDetected())
            {
                _mId.Value = 1;
                _mMob.SetWalkAnimation();
                return NodeResult.success;
            }
            
            return NodeResult.running;
        }
    }
}
