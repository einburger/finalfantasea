﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject fishPrefab;

    private int spawnMax = 300;
    private int spawnCount = 0;

    private void Start() {
        spawnFish();    
    }

    private void spawnFish() {
        while (canSpawn()) {
            float xpos = transform.position.x + Random.Range(-transform.localScale.x/2, transform.localScale.x/2);
            float ypos = transform.localPosition.y + Random.Range(-transform.localScale.y/2, transform.localScale.y/2);
            float zpos = transform.position.z + Random.Range(-transform.localScale.z/2, transform.localScale.z/2);
            Vector3 randPos = new Vector3(xpos, ypos, zpos);
            GameObject fish = Instantiate(fishPrefab, randPos, Quaternion.identity);

            float randomScale = Random.Range(0.3f, 0.5f);
            fish.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            spawnCount++;
        }
    }

    private bool canSpawn() {
        return spawnCount < spawnMax;
    }

    private void pickEyes() { }
    private void pickFins() { } 
    private void pickHat() { }
    private void pickFacialHair() { }
}
