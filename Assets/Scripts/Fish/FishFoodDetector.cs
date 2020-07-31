using UnityEngine;

public class FishFoodDetector : MonoBehaviour
{
    public Collider fishSensorCollider;

    private Collider baitCollider;
    private Transform baitTransform;

    private FishMovement fishMovement;

    private void Awake() 
    {
        GameObject baitObject = GameObject.FindWithTag("Bait");
        baitCollider = baitObject.GetComponent<Collider>();    
        baitTransform = baitObject.GetComponent<Transform>();    
        fishMovement = GetComponent<FishMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectedFood()) {
            fishMovement.targetPosition = new Vector3(baitTransform.position.x, transform.position.y, baitTransform.position.z);
        }    
    }

    private bool detectedFood() 
    {
        return fishSensorCollider.bounds.Intersects(baitCollider.bounds); 
    }
}
