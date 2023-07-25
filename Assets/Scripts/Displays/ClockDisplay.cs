using UnityEngine;

namespace Displays
{
    public abstract class ClockDisplay : MonoBehaviour
    {
        public virtual void UpdateTime(float time, bool freezeSeconds = false)
        {
        
        }
    }
}