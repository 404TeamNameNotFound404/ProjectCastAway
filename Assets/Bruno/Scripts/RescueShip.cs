using System;
using UnityEngine;
using UnityEngine.AI;

namespace Bruno.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class RescueShip : MonoBehaviour
    {
        private Rigidbody _rb;
        [SerializeField] private Transform anchor;
        [SerializeField] [Range(1.0f, 11.6f)] private float speed;
        private void Start()
        {
             gameObject.SetActive(false);
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            var velocity = (anchor.position - transform.position).normalized * speed;

            var distance = Vector3.Distance(anchor.position, transform.position);
            velocity.y = 0;

            if (distance <= 1.0f)
            {
                velocity = Vector3.zero;
                //TODO ADD WIN SCREEN
            }

            _rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }
        
    }
}
