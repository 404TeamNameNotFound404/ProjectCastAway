using System;
using UnityEngine;

namespace Bruno.Scripts.AI.Engage
{
    public class Arrow : MonoBehaviour
    {
        private float m_Timer = 0.0f;
        private float m_TimeToLive = 5.0f;
        private Rigidbody m_Rigidbody;
        private GameObject m_Target;
        /// <summary>
        /// The agent that shot it
        /// </summary>
        [SerializeField] private GameObject source;

        [SerializeField] private float trajectorySpeed = 30.0f;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.mass = 2.0f;
            m_Rigidbody.useGravity = true;
            m_Target = GameObject.FindGameObjectWithTag("Player");
            var targetDirection = source.transform.forward; 
            m_Rigidbody.AddForce(targetDirection * (200.0f * Time.deltaTime), ForceMode.Impulse);
            m_Rigidbody.linearVelocity = transform.forward * trajectorySpeed;
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
            
            if (m_Rigidbody.linearVelocity.magnitude > 0.1f)
            {
                transform.forward = m_Rigidbody.linearVelocity.normalized;
            }

            m_Timer += Time.deltaTime;

            if (m_Timer >= m_TimeToLive)
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
