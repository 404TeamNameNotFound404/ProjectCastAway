using UnityEngine;
using TMPro;

namespace Bruno.Scripts
{
    public class DayNightCycle : MonoBehaviour
    { 
        [SerializeField] private float cycleSpeed = 30.0f;
        [SerializeField] private TMP_Text text;
        private Light m_Light;
        private GameObject m_LightObject;
        private static bool _dayTime = true;
        private const float DayInSeconds = 24.0f * 60.0f * 60.0f;
        private float m_DayTime;
        private float m_TimeUntilNight;
        private float m_TimeUntilSunrise;
        private float m_ContinuousAngle;
        
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
            _dayTime = IsDay(initialAngle, 5.0f, 180.0f);
            text.fontSize = 23;
        }
        
        private void LateUpdate()
        {
            var angle = m_LightObject.transform.rotation.eulerAngles.x; 
            var rotationAmount = cycleSpeed * Time.deltaTime;
            m_LightObject.transform.Rotate(Vector3.right, rotationAmount);
            
            const float sunRiseAngle = 0.0f;  
            const float sunSetAngle = 180.0f;  
            const float fullDayAngle = 360.0f;  
            
            const float secondsPerDegree = DayInSeconds / fullDayAngle;
            angle = NormalizeAngle(angle);
            
            m_ContinuousAngle += rotationAmount;

            // Normalize continuous angle to ensure it doesn't grow indefinitely
            if (m_ContinuousAngle >= fullDayAngle)
                m_ContinuousAngle -= fullDayAngle;
           
            
            if (m_ContinuousAngle >= sunRiseAngle && m_ContinuousAngle < sunSetAngle)
            {
                var degreesUntilSunset = sunSetAngle - m_ContinuousAngle;
                m_TimeUntilNight = degreesUntilSunset * secondsPerDegree;
                text.text = $"Time until Sunset: {FormatTime(m_TimeUntilNight)}";
            }
            else
            {
                var degreesUntilSunrise = (sunRiseAngle - m_ContinuousAngle + fullDayAngle) % fullDayAngle;
                m_TimeUntilSunrise = degreesUntilSunrise * secondsPerDegree;
                text.text = $"Time until Sunset: {FormatTime(m_TimeUntilSunrise)}";
            }
            
            var currentDayState = IsDay(angle, sunRiseAngle, sunSetAngle);
            
            if (currentDayState != _dayTime)
            {
                _dayTime = currentDayState;

                if (_dayTime)
                {
                    Debug.Log("It's daytime!");
                    m_TimeUntilSunrise = 0;
                }
                else
                {
                    Debug.Log("It's nighttime!");
                    m_TimeUntilNight = 0;
                }
            }
        }

        private static float NormalizeAngle(float angle)
        {
            return (angle % 360.0f + 360.0f) % 360.0f;
        }
        
        private string FormatTime(float timeInSeconds)
        {
            var hours = Mathf.FloorToInt(timeInSeconds / 3600);
            var minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
            var seconds = Mathf.FloorToInt(timeInSeconds % 60);
            
            if (hours > 0)
            {
                return $"{hours:D2}:{minutes:D2}:{seconds:D2}"; 
            }

            return $"{minutes:D2}:{seconds:D2}"; 
        }

        private static bool IsDay(float angle, float sunRiseAngle, float sunSetAngle)
        {
            return angle >= sunRiseAngle && angle < sunSetAngle;
        }
    }
}
