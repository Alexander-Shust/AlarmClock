using Displays;
using UnityEngine;

public class DraggableHand : MonoBehaviour
{
    [SerializeField] private HandType _handType = HandType.Hours;
    [SerializeField] private Transform _rotationBase;
    [SerializeField] private Transform _handBase;
    [SerializeField] private ClockFace _clockFace;
    [SerializeField] private Electronic _electronic;
    [SerializeField] private TimeManager _timeManager;
    
    private const float Step = 3.0f;
    private const float Multiplier = 120.0f;
    private float _rotationStep;

    private void Awake()
    {
        _rotationStep = _handType == HandType.Hours ? Step : Step / 60;
    }

    private void OnMouseDrag()
    {
        if (!_timeManager.IsPaused)
        {
            return;
        }
        
        var handRotation = _handBase.eulerAngles.z;
        if (handRotation < 0.0f)
        {
            handRotation += 360.0f;
        }
        var current = Input.mousePosition;
        var x = current.x - Screen.width / 2.0f;
        var y = current.y - Screen.height / 2.0f;
        if (x >= 0 && y >= 0 && handRotation - Step <= 360.0f && handRotation + Step >= 270.0f)
        {
            if (Mathf.Atan2(y, x) * 180.0f / Mathf.PI <= handRotation - 270.0f)
            {
                RotateClockwise();
            }
            else
            {
                RotateCounterClockwise();
            }
        }
        else if (x <= 0 && y >= 0 && handRotation - Step <= 90.0f && handRotation + Step >= 0.0f)
        {
            if (Mathf.Atan2(-x, y) * 180.0f / Mathf.PI <= handRotation)
            {
                RotateClockwise();
            }
            else
            {
                RotateCounterClockwise();
            }
        }
        else if (x <= 0 && y <= 0 && handRotation - Step <= 180.0f && handRotation + Step >= 90.0f)
        {
            if (Mathf.Atan2(y, x) * 180.0f / Mathf.PI <= handRotation - 90.0f)
            {
                RotateClockwise();
            }
            else
            {
                RotateCounterClockwise();
            }
        }
        else if (x >= 0 && y <= 0 && handRotation - Step <= 270.0f && handRotation + Step >= 180.0f)
        {
            if (Mathf.Atan2(x, -y) * 180.0f / Mathf.PI <= handRotation - 180.0f)
            {
                RotateClockwise();
            }
            else
            {
                RotateCounterClockwise();
            }
        }
    }

    private void RotateClockwise(bool counter = false)
    {
        var newTime = (-_rotationBase.eulerAngles.z + _rotationStep * (counter ? -1 : 1)) * Multiplier + 43200.0f;
        if (_timeManager.CurrentTime > newTime && newTime < 43200.0f)
        {
            newTime += 43200.0f;
        }
        _clockFace.UpdateTime(newTime, true);
        _electronic.UpdateTime(newTime, true);
    }

    private void RotateCounterClockwise()
    {
        RotateClockwise(true);
    }
}

public enum HandType
{
    Hours,
    Minutes
}