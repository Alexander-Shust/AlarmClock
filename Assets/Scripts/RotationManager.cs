using UnityEngine;

public class RotationManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}