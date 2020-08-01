using UnityEngine;

public class FishingCursorTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject fishingCursor;
    private int layerMask;

    private Vector3 velocity;

    private void Awake() {
        layerMask = LayerMask.GetMask("Water");
        fishingCursor.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        float distance = 30;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask)) {
            if (!fishingCursor.activeInHierarchy) {
                fishingCursor.transform.position = hitInfo.point;
                fishingCursor.SetActive(true);
            }
            fishingCursor.transform.position = Vector3.SmoothDamp(fishingCursor.transform.position, hitInfo.point, ref velocity, 0.2f);
        } else {
        }
    }
    private void OnDisable() {
        fishingCursor.SetActive(false);    
    }
}
