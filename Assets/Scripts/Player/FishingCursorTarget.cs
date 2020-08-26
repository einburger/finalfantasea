using UnityEngine;
using UnityEngine.VFX;

public class FishingCursorTarget : MonoBehaviour
{
    private VisualEffect cursorEffect;
    private int layerMask;
    private Vector3 velocity;
    private bool cursorDisplayed = false;
    private MeshRenderer cursor;
    public float currentHeight = 0f;
    [SerializeField] private WaveData waveData = null;

    private void Awake() {
        layerMask = LayerMask.GetMask("Water");
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
                cursor.enabled = true;
                transform.position = hitInfo.point;
            }
            transform.position = Vector3.SmoothDamp(transform.position, hitInfo.point, ref velocity, 0.1f);
            currentHeight = waveData.GetWaveHeight(transform.position);
        } 
    }

    public void EraseCursor()
    {
        cursorDisplayed = false;
        cursor.enabled = false;
    }
}
