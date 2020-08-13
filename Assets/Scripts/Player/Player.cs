using System;
using System.Collections;
using UnityEngine;

namespace Fishnet {

public class Player : MonoBehaviour
{
    [SerializeField] private FishingCursorTarget fishingCursor;
    [SerializeField] private GameObject lure, player, raft;
    [SerializeField] private Transform raftTransform;
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

    public void CastLure() {
        Vector3 initialPosition = new Vector3(Camera.main.transform.position.x, 0f, Camera.main.transform.position.z);
        Vector3 finalPosition = new Vector3(fishingCursor.transform.position.x, 0f, fishingCursor.transform.position.z);
        float deltaZ = Vector3.Distance(finalPosition, initialPosition);

        GameObject go = new GameObject();
        go.transform.position = Camera.main.transform.position;
        go.transform.LookAt(new Vector3(fishingCursor.transform.position.x, Camera.main.transform.position.y, fishingCursor.transform.position.z));

        float time = deltaZ / 10f;
        float a_t_sqr = 0.5f * Physics.gravity.y * time * time;
        float verticalDisplacement = fishingCursor.transform.position.y - Camera.main.transform.position.y;
        float targetVerticalVelocity = (verticalDisplacement - a_t_sqr) / time;

        Vector3 projectileVelocity = new Vector3(0f, targetVerticalVelocity, 10f);
        Vector3 globalProjectileVelocity = go.transform.TransformDirection(projectileVelocity);

        lure.transform.position = Camera.main.transform.position;
        lure.GetComponent<Rigidbody>().velocity = globalProjectileVelocity;
    }

    public bool InMotion() {
        float horizontal = 0f;
        float vertical = 0f;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction.sqrMagnitude > 0f;
    }

    public void LockToRaft() {
        player.transform.position = raftTransform.position + new Vector3(0f, 0.2f, 0f);
        player.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void ChangeMover() {
        raft.GetComponent<BoatMovement>().enabled = !raft.GetComponent<BoatMovement>().enabled;
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
