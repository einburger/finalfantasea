using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    LineRenderer line = null;
    [SerializeField] Transform lureTransform = null;
    [SerializeField] Transform rodTransform = null;
    void Start()
    {
       line = GetComponent<LineRenderer>(); 
       line.startColor = Color.white;
       line.endColor = Color.white;
       line.startWidth = 0.01f;
       line.endWidth = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, rodTransform.transform.position);    
        line.SetPosition(1, lureTransform.transform.position);    
    }
}
