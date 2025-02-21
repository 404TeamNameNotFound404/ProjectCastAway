using MBT;
using UnityEngine;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("Native/GuardianStun")]
    public class GuardianStun : Leaf
    {
        public Blackboard blackboard;
        private GameObjectVariable _mSelf;
        private GameObjectVariable _mTarget;
        private IntVariable _mId;
        private GuardianMob _mMob;

        private float _mTimer;
        private const float MTimeUntilReset = 16.0f;
        
        public override void OnEnter()
        {
            _mSelf = blackboard.GetVariable<GameObjectVariable>("Self");
            _mTarget = blackboard.GetVariable<GameObjectVariable>("Target");
            _mId = blackboard.GetVariable<IntVariable>("id");
            _mMob = _mSelf.Value.GetComponent<GuardianMob>();
        }

        public override NodeResult Execute()
        { 
            _mId.Value = 2;
            _mTimer += Time.deltaTime * 5.0f;
            _mMob.agent.ResetPath();
            
            if (_mTimer >= MTimeUntilReset)
            {
                // if player is seen after stun go chase it
            
                if (_mMob.PlayerDetected())
                {
                    _mId.Value = 3;
                    _mMob.agent.ResetPath();
                    _mMob.DisableIsStunned();
                    _mMob.SetWalkAnimation();
                    return NodeResult.success;
                }
                
                
                // otherwise idle state again for the sake of simplicity
                _mId.Value = 0;
                _mMob.agent.ResetPath();
                _mMob.DisableIsStunned();
                _mMob.SetIdleAnimation();
                
                return NodeResult.success;
            }
            
            _mMob.SetIsStunned();
            return NodeResult.running;
        }

        public override void OnExit()
        {
            _mTimer = 0.0f;
        }
    }
}
