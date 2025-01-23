using UnityEngine;

namespace Bruno.Scripts
{
    public class DayNightCycle : MonoBehaviour
    { 
        private Light m_Light;
        private GameObject m_LightObject;
        [SerializeField] private float cycleSpeed = 30.0f;
        private static bool _dayTime = true;
        
        /// <summary>
        /// Return the day/night cycle time (if true is daytime otherwise is nighttime)
        /// </summary>
        public static bool isDayTime => _dayTime;
        private void Start()
        {
            m_Light = GetComponent<Light>();
            m_LightObject = m_Light.gameObject;
            _dayTime = true;
            var initialAngle = m_LightObject.transform.rotation.eulerAngles.x;
            _dayTime = IsDay(initialAngle);
        }

        private void LateUpdate()
        {
            var angle = m_LightObject.transform.rotation.eulerAngles.x;
            var rotationAmount = cycleSpeed * Time.deltaTime;
            m_LightObject.transform.Rotate(Vector3.right,  rotationAmount);

            var day = IsDay(angle);

            if (day != _dayTime)
            {
                _dayTime = day;
                
                if (_dayTime)
                {
                    Debug.Log("It's daytime!");
                    Debug.Log($"Daytime {isDayTime}");
                }
                else
                {
                    Debug.Log("It's nighttime!");
                    Debug.Log($"Daytime {isDayTime}");
                }
            }
        }


        private bool IsDay(float angle)
        {
            return angle >= 0 && angle <= 180;
        }
    }
}
