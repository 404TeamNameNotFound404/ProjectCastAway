using System;
using UnityEngine;
using UnityEngine.AI;

namespace Bruno.Scripts.AI.Engage
{
    public class Arrow : MonoBehaviour
    {
        private float _mTimer;
        private const float MTimeToLive = 5.0f;
        private Rigidbody _mRigidbody;
        private GameObject _mTarget;
        /// <summary>
        /// The agent that shot it
        /// </summary>
        [SerializeField] private GameObject source;
        [SerializeField] private float trajectorySpeed = 30.0f;
        
        private Player _player;
        private Mob _mob;

        private void Start()
        {
            _mRigidbody = GetComponent<Rigidbody>();
            _mRigidbody.isKinematic = false;
            _mRigidbody.mass = 2.0f;
            _mRigidbody.useGravity = false;
          
            _mTarget = GameObject.FindGameObjectWithTag("Player");
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _mob = source.GetComponent<Mob>();
             var agent = source.GetComponent<NavMeshAgent>();
             var targetDirection = (_mTarget.transform.position - transform.position).normalized;
            _mRigidbody.AddForce(targetDirection * 250.0f, ForceMode.Impulse);
            //_mRigidbody.linearVelocity = transform.forward * trajectorySpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return; 
            _player.TakeDamage(_mob.damageDataRef.ApplyArrowDamage());
        }

        private void FixedUpdate()
        {
            if (!source)
            {
                Debug.Log("Source object not valid or not instantiated on scene");
                return;
            }
            
            // if (_mTimer == 0) // Ensure force is applied only once
            // {
            //     var targetDirection = transform.forward;
            //     _mRigidbody.AddForce(targetDirection * 20.0f, ForceMode.Impulse);
            // }
            //
            // if (_mRigidbody.linearVelocity.magnitude > 0.1f)
            // {
            //     transform.forward = _mRigidbody.linearVelocity.normalized;
            // }
            
            _mTimer += Time.deltaTime;
            
            if (_mTimer >= MTimeToLive)
            {
                Destroy(gameObject);
            }
        }

        // void Update()
        // {
        //   
        //     m_Timer += Time.deltaTime * 2.0f;
        //     
        //     if (m_Timer >= m_TimeToLive)
        //     {
        //         Destroy(gameObject);
        //         m_Timer = 0.0f;
        //     }
        // }
    }
}
