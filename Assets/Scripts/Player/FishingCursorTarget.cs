using UnityEngine;
using UnityEngine.VFX;

public class FishingCursorTarget : MonoBehaviour
{
    private VisualEffect cursorEffect;
    private int layerMask;
    private Vector3 velocity;
    private bool cursorDisplayed = false;
    private MeshRenderer cursor;

    private void Awake() {
        layerMask = LayerMask.GetMask("Water");
        // cursorEffect = GetComponentInChildren<VisualEffect>();
        cursor = GetComponentInChildren<MeshRenderer>();
    }

    public void DrawCursor()
    {
        RaycastHit hitInfo;
        float distance = 1000;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask)) {
            if (!cursorDisplayed) {
                cursorDisplayed = true;
                // cursorEffect.SendEvent("EnterAim");
                cursor.enabled = true;
                transform.position = hitInfo.point;
            }
            transform.position = Vector3.SmoothDamp(transform.position, hitInfo.point, ref velocity, 0.1f);
        } 
    }

    public void EraseCursor()
    {
        cursorDisplayed = false;
        //cursorEffect.SendEvent("ExitAim");
        cursor.enabled = false;
    }
}
