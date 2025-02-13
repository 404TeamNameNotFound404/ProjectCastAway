using System;
using UnityEngine;

namespace Bruno.Scripts.AI.Engage
{
    public class Arrow : MonoBehaviour
    {
        private float _mTimer = 0.0f;
        private float _mTimeToLive = 5.0f;
        private Rigidbody _mRigidbody;
        private GameObject _mTarget;
        /// <summary>
        /// The agent that shot it
        /// </summary>
        [SerializeField] private GameObject source;

        [SerializeField] private float trajectorySpeed = 30.0f;

        private void Start()
        {
            _mRigidbody = GetComponent<Rigidbody>();
            _mRigidbody.isKinematic = false;
            _mRigidbody.mass = 2.0f;
            _mRigidbody.useGravity = true;
            _mTarget = GameObject.FindGameObjectWithTag("Player");
            var targetDirection = source.transform.forward; 
            _mRigidbody.AddForce(targetDirection * (200.0f * Time.deltaTime), ForceMode.Impulse);
            _mRigidbody.linearVelocity = transform.forward * trajectorySpeed;
        }
        
        private void FixedUpdate()
        {
            if (!source)
            {
                Debug.Log("Source object not valid or not instantiated on scene");
                return;
            }
            
            // if (m_Timer == 0) // Ensure force is applied only once
            // {
            //     var targetDirection = transform.forward;
            //     m_Rigidbody.AddForce(targetDirection * 20.0f, ForceMode.Impulse);
            // }
            
            if (_mRigidbody.linearVelocity.magnitude > 0.1f)
            {
                transform.forward = _mRigidbody.linearVelocity.normalized;
            }

            _mTimer += Time.deltaTime;

            if (_mTimer >= _mTimeToLive)
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
