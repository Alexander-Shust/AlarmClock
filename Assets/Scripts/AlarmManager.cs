using Displays;
using UnityEngine;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private Electronic _electronic;
    [SerializeField] private Button _alarmButton;

    private void Awake()
    {
        _alarmButton.onClick.AddListener(BeginSettingAlarm);
    }

    private void BeginSettingAlarm()
    {
        _timeManager.IsPaused = true;
        _alarmButton.onClick.RemoveListener(BeginSettingAlarm);
        _alarmButton.onClick.AddListener(EndSettingAlarm);
    }

    private void EndSettingAlarm()
    {
        _alarmButton.onClick.RemoveListener(EndSettingAlarm);
        _alarmButton.onClick.AddListener(BeginSettingAlarm);
        _timeManager.SetAlarm(_electronic.CurrentTime);
    }

    private void OnDestroy()
    {
        _alarmButton.onClick.RemoveAllListeners();
    }
}