using System;
using System.Net;
using System.Net.Sockets;
using Displays;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TimeServer _timeServer;
    [SerializeField] private float _syncInterval = 30.0f;
    [SerializeField] private float _timeMultiplier = 10.0f;
    [SerializeField] private ClockDisplay[] _displays;
    [SerializeField] private GameObject _alarmPanel;

    public bool IsPaused { get; set; }

    public float CurrentTime => _currentTime;
    
    private float _currentTime;
    private float _alarmTime;
    private float _timeToSync;

    private bool _isAlarmSet;

    public void SetAlarm(float time)
    {
        _alarmTime = time;
        _isAlarmSet = true;
        SyncTime();
        IsPaused = false;
    }

    private void TriggerAlarm()
    {
        _isAlarmSet = false;
        _alarmPanel.SetActive(true);
    }

    private void Start()
    {
        _alarmPanel.SetActive(false);
        _timeToSync = _syncInterval;
        SyncTime();
        UpdateDisplays();
    }

    private void FixedUpdate()
    {
        if (IsPaused)
        {
            return;
        }
        
        var deltaTime = Time.deltaTime;
        _timeToSync -= deltaTime;
        if (_timeToSync <= 0.0f)
        {
            _timeToSync = _syncInterval;
            SyncTime();
        }
        else
        {
            _currentTime += deltaTime * _timeMultiplier;
        }
        UpdateDisplays();
        
        if (_isAlarmSet && Mathf.Abs(_currentTime - _alarmTime) <= 0.5f)
        {
            TriggerAlarm();
        }
    }

    private void UpdateDisplays()
    {
        foreach (var display in _displays)
        {
            display.UpdateTime(_currentTime);
        }
    }
    
    private void SyncTime()
    {
        var ntpServer = _timeServer switch
        {
            TimeServer.Windows => "time.windows.com",
            TimeServer.NtpOrg => "time.nist.gov",
            _ => throw new ArgumentOutOfRangeException()
        };
        var ntpData = new byte[48];
        ntpData[0] = 0x1B; 

        var addresses = Dns.GetHostEntry(ntpServer).AddressList;
        var ipEndPoint = new IPEndPoint(addresses[0], 123);

        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();
        }
    
        var intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
        var fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];

        var milliseconds = intPart * 1000 + fractPart * 1000 / 0x100000000L;
        var networkDateTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)milliseconds);

        var localTime = networkDateTime.ToLocalTime();
        _currentTime = localTime.Hour * 3600 + 
                       localTime.Minute * 60 + 
                       localTime.Second +
                       localTime.Millisecond / 1000.0f;
    }

    public enum TimeServer
    {
        Windows,
        NtpOrg
    }
}