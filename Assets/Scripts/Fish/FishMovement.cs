using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public Vector3 targetPosition { get; set; }

    private Transform fishSpawnBoxTransform;    
    private Vector3 velocity = Vector3.zero;
    private float turnOmega = 0f;
    private float turnSmoothing = 0.7f;

    [SerializeField]
    private float wiggleIntensityConstant = 10f;

    private void Awake() 
    {
        var obj = GameObject.FindWithTag("Respawn");
        fishSpawnBoxTransform = obj.transform;
        setRandomTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (reachedTarget()) {
            setRandomTargetPosition();
        }  

        updateHeading();
        updateWiggle();
    }

    private void setRandomTargetPosition() 
    {
        float xpos = fishSpawnBoxTransform.position.x + Random.Range(-fishSpawnBoxTransform.localScale.x/2, fishSpawnBoxTransform.localScale.x/2);
        float ypos = fishSpawnBoxTransform.position.y + Random.Range(-fishSpawnBoxTransform.localScale.y/2, fishSpawnBoxTransform.localScale.y/2);
        float zpos = fishSpawnBoxTransform.position.z + Random.Range(-fishSpawnBoxTransform.localScale.z/2, fishSpawnBoxTransform.localScale.z/2);
        targetPosition = new Vector3(xpos, ypos, zpos);
    }

    private bool reachedTarget()
    {
        return (targetPosition - transform.position).sqrMagnitude < 5f; 
    }

    private void updateWiggle() 
    {
        float wiggleIntensity = velocity.sqrMagnitude;
        GetComponentInChildren<Renderer>().material.SetFloat("_wiggleSpeed", 10f);
        GetComponentInChildren<Renderer>().material.SetFloat("_wiggleIntensity",  wiggleIntensity / wiggleIntensityConstant);
    }

    private void updateHeading() 
    {
        Vector3 relativePos = targetPosition - transform.position;
        Quaternion targetAngle = Quaternion.LookRotation(relativePos, Vector3.up);
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle.eulerAngles.y, ref turnOmega, turnSmoothing);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDir = transform.rotation * Vector3.forward;
        Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 10f);
        transform.position += moveDir * velocity.sqrMagnitude * Time.deltaTime;
    }
}
