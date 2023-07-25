using UnityEngine;

namespace Displays
{
    public class ClockFace : ClockDisplay
    {
        [SerializeField] private Transform _secondHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private Transform _hourHand;
    
        public override void UpdateTime(float time, bool freezeSeconds = false)
        {
            var secondAngle = -time * 6;
            _secondHand.rotation = Quaternion.Euler(0, 0, freezeSeconds ? 0 : secondAngle);
            _minuteHand.rotation = Quaternion.Euler(0, 0, secondAngle / 60.0f);
            _hourHand.rotation = Quaternion.Euler(0, 0, secondAngle / 720.0f);
        }
    }
}