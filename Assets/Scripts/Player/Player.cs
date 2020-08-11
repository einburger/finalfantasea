using System;
using System.Collections;
using UnityEngine;

namespace Fishnet {

public class Player : MonoBehaviour
{
    [SerializeField] private FishingCursorTarget fishingCursor;
    [SerializeField] public Animator animationController;
    private CinemachineCameraOffset cameraOffset;
    public CharacterStatePushdown cursorStateStack;
    public CharacterStatePushdown movementStateStack;

    public void Aim() {
        if (!cameraOffset.enabled) {
            cameraOffset.enabled = true;
        }
        // do other stuff
        // draw trajectory
    }

    public void ResetAim() {
        if (cameraOffset.enabled) {
            cameraOffset.enabled = false;
        }
        fishingCursor.EraseCursor();
    }

    public void DrawCursor() {
        fishingCursor.DrawCursor();
    }

    public bool InMotion() {
        float horizontal = 0f;
        float vertical = 0f;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction.sqrMagnitude > 0f;
    }

    void Start() {
        cameraOffset = GetComponentInChildren<CinemachineCameraOffset>();

        cursorStateStack = new CharacterStatePushdown();
        cursorStateStack.PushState(new ZoomedOutState());

        movementStateStack = new CharacterStatePushdown();
        movementStateStack.PushState(new IdleState());
    }

    void Update() {
        cursorStateStack.Update(this);
        movementStateStack.Update(this);
    }
}

}
