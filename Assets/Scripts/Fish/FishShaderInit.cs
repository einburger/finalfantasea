using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishShaderInit : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Renderer>().material.SetFloat("_randomWiggleOffset", Random.Range(-1f, 1f));
    }
}
