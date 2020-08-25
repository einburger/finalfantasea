using UnityEngine;

public class RodOrienter : MonoBehaviour
{
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;

    Quaternion initialRotation = Quaternion.identity;
    void Start() {
        initialRotation = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = rightHandTransform.position - leftHandTransform.position;
        var rotation = Quaternion.LookRotation(dir.normalized);
        rotation *= initialRotation;
        transform.rotation = rotation; 
        transform.position = leftHandTransform.position + 0.5f * transform.forward;
    }
}
