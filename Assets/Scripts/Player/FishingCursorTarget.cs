using UnityEngine;
using UnityEngine.VFX;

public class FishingCursorTarget : MonoBehaviour
{
    private VisualEffect cursorEffect;

    private int layerMask;

    private Vector3 velocity;

    private void Awake() {
        layerMask = LayerMask.GetMask("Water");
        cursorEffect = GetComponentInChildren<VisualEffect>();
    }

    public void DrawCursor()
    {
        Debug.Log(cursorEffect.GetUInt("Rate"));

        RaycastHit hitInfo;
        float distance = 30;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask)) {
            if (cursorEffect.GetUInt("Rate") == 0) {
                transform.position = hitInfo.point;
                cursorEffect.SetUInt("Rate", 10);
                Debug.Log(cursorEffect.GetUInt("Rate"));
            }
            transform.position = Vector3.SmoothDamp(transform.position, hitInfo.point, ref velocity, 0.2f);
        } 
    }

    public void EraseCursor()
    {
        cursorEffect.SetUInt("Rate", 0);
    }
}
