using UnityEngine;
using MBT;
namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("Native/GuardianIdle")]
    public class GuardianIdle : Leaf
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
            
           // if detected go to chase behaviour
           
            if (_mMob.PlayerDetected())
            {
                _mId.Value = 1;
                _mMob.SetWalkAnimation();
                return NodeResult.success;
            }
            
            // if got hit by something got stunned
            if (_mMob.gotHit)
            {
                _mId.Value = 2;
                _mMob.SetIsStunned();
                Debug.Log("transitioning to got hit");
                return NodeResult.success;
            }
            
            //if no conditions are met just stay idle and humble
            _mMob.DisableIsStunned();
            _mMob.SetIdleAnimation();
            _mId.Value = 0;
            _mMob.agent.ResetPath();
            
            return NodeResult.running;
        }
    }
}
