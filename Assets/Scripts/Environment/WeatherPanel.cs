using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherPanel : MonoBehaviour
{
    string[] forcast = new string[7];
    public bool[] updated = new bool[7];
    public bool[] dayPassed = new bool[7];

    public void SetPercipitationText(string[] nextForcast) {
        for (int i = 0; i < forcast.Length; i++) {
            forcast[i] = nextForcast[i];
            updated[i] = true;
            dayPassed[i] = false;
        }
    }

    public void MarkDay(int day) {
        dayPassed[day] = true;
    }

    public string GetForcast(int i) {
        return forcast[i];
    }
}
