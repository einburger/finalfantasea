using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCamera : MonoBehaviour
{
    private CinemachineCameraOffset cameraOffset;
    private FishingCursorTarget fishingCursor;

    private void Awake() 
    {
       cameraOffset = GetComponent<CinemachineCameraOffset>();
       fishingCursor = GetComponent<FishingCursorTarget>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) {
            if (!cameraOffset.enabled) {
                cameraOffset.enabled = true;
                fishingCursor.enabled = true;
            }
        } else {
            if (cameraOffset.enabled) {
                cameraOffset.enabled = false;
                fishingCursor.enabled = false;
            }
        } 
    }
}
