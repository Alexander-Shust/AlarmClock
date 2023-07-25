using TMPro;
using UnityEngine;

namespace Displays
{
    public class Electronic : ClockDisplay
    {
        [SerializeField] private TMP_InputField _hoursText;
        [SerializeField] private TMP_InputField _minutesText;
        [SerializeField] private TMP_InputField _secondsText;
    
        private int _hours;
        private int _minutes;
        private int _seconds;

        private int Hours
        {
            get => _hours;
            set
            {
                if (value == Hours)
                {
                    return;
                }

                _hours = value;
                _hoursText.text = _hours.ToString("00");
            }
        }
    
        private int Minutes
        {
            get => _minutes;
            set
            {
                if (value == Minutes)
                {
                    return;
                }

                _minutes = value;
                _minutesText.text = _minutes.ToString("00");
            }
        }
    
        private int Seconds
        {
            get => _seconds;
            set
            {
                if (value == Seconds)
                {
                    return;
                }

                _seconds = value;
                _secondsText.text = _seconds.ToString("00");
            }
        }

        public float CurrentTime => _hours * 3600.0f + _minutes * 60.0f + _seconds;

        public override void UpdateTime(float time, bool freezeSeconds = false)
        {
            var totalSeconds = (int) Mathf.Floor(time);
            var totalMinutes = totalSeconds / 60;
            var totalHours = totalMinutes / 60;
            Hours = totalHours % 24;
            Minutes = totalMinutes % 60;
            Seconds = freezeSeconds ? 0 : totalSeconds % 60;
        }

        private void Awake()
        {
            _hoursText.onValueChanged.AddListener(OnHoursChanged);
            _minutesText.onValueChanged.AddListener(OnMinutesChanged);
            _secondsText.onValueChanged.AddListener(OnSecondsChanged);
        }

        private void OnHoursChanged(string text)
        {
            if (int.TryParse(text, out var hours))
            {
                Hours = hours;
            }
        }

        private void OnMinutesChanged(string text)
        {
            if (int.TryParse(text, out var minutes))
            {
                Minutes = minutes;
            }
        }

        private void OnSecondsChanged(string text)
        {
            if (int.TryParse(text, out var seconds))
            {
                Seconds = seconds;
            }
        }
    }
}