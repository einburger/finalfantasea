using UnityEngine;

public class FishingCursorTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject fishingCursor;
    private int layerMask;

    private Vector3 velocity;

    private void Awake() {
        layerMask = LayerMask.GetMask("Water");
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        float distance = 10;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask)) {
            fishingCursor.SetActive(true);
            fishingCursor.transform.position = Vector3.SmoothDamp(fishingCursor.transform.position, hitInfo.point, ref velocity, 0.2f);
        } else {
            fishingCursor.SetActive(false);
        }
    }
}
