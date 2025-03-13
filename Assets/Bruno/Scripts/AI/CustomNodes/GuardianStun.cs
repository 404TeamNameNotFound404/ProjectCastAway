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
            _mId.Value = 4;
            _mTimer += Time.deltaTime * 5.0f;
            _mMob.agent.ResetPath();
            _mMob.SetIsStunned();

            if (_mTimer >= MTimeUntilReset)
            {
                Debug.Log("Guardian stun timer has expired");
                // if player is seen after stun go chase it

                if (_mMob.PlayerDetected())
                {
                    _mTimer = 0.0f;
                    _mId.Value = 3;
                    _mMob.DisableIsStunned();
                    _mMob.SetWalkAnimation();
                    return NodeResult.success;
                }
                // otherwise idle state again for the sake of simplicity

                if (!_mMob.PlayerDetected())
                {
                    _mTimer = 0.0f;
                    _mId.Value = 0;
                    _mMob.DisableIsStunned();
                    _mMob.SetIdleAnimation();
                    return NodeResult.success;
                }
            }

            return NodeResult.running;
        }

        public override void OnExit()
        {
            _mTimer = 0.0f;
            _mMob.gotHit = false;
        }
    }
}
