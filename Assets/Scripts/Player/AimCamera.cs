using UnityEngine;

public class AimCamera : MonoBehaviour
{
    [SerializeField]
    public FishingCursorTarget fishingCursor;

    private CinemachineCameraOffset cameraOffset;

    private void Awake() 
    {
       cameraOffset = GetComponent<CinemachineCameraOffset>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) {
            if (!cameraOffset.enabled) {
                cameraOffset.enabled = true;
            }
            fishingCursor.DrawCursor();
        } else {
            if (cameraOffset.enabled) {
                cameraOffset.enabled = false;
            }
            fishingCursor.EraseCursor();
        } 
    }
}
