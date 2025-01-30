using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Bruno.Scripts.AI
{
    public class MobSpawner : MonoBehaviour
    {
        public List<NativeMob> mobs;
        public GameObject floor;
        private bool m_Spawned; 
        private float m_SpawnRange = 3.0f;
        private float m_NavMeshSearchRange = 1.0f;
        private Collider m_Collider;
        
        public Collider collider => m_Collider;
        
        private void Start()
        {
            m_Collider = floor.GetComponent<BoxCollider>();
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (m_Spawned) return;
            if (mobs.Count < 0) return;
        
            
            foreach (var mob in mobs)
            {
                if (!mob) break;
                mob.gameObject.SetActive(true);
                RandomSpawner(mob);
                m_Spawned = true;
            }
            
        }
        
        private void RandomSpawner(NativeMob mob)
        {
            var maxAttempts = 10; // Limit attempts to find a valid position
            var randomPoint = Vector3.zero;
            var validPositionFound = false;

            for (var i = 0; i < maxAttempts; i++)
            {
                m_SpawnRange = Random.Range(0, m_Collider.bounds.extents.z) * 0.1f;
                randomPoint = transform.position + Random.insideUnitSphere * m_SpawnRange;
                randomPoint.y = transform.position.y;

                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, m_NavMeshSearchRange, NavMesh.AllAreas))
                {
                    var tooCloseToOtherMobs = false;

                    foreach (var existingMob in mobs)
                    {
                        if (Vector3.Distance(hit.position, existingMob.transform.position) < 1.5f)
                        {
                            tooCloseToOtherMobs = true;
                            break;
                        }
                    }

                    if (!tooCloseToOtherMobs)
                    {
                        randomPoint = hit.position;
                        validPositionFound = true;
                        break;
                    }
                }
            }

            if (validPositionFound)
            {
                Instantiate(mob, randomPoint, Quaternion.identity);
            }
        }
        
    }
}
