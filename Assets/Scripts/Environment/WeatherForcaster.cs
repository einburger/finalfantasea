using UnityEngine;

public class WeatherForcaster : MonoBehaviour
{
    [SerializeField] private AnimationCurve percipitationCurve = null;
    [SerializeField] private WeatherPanel weatherPanel = null;
    float[] weeklyPercipitation = new float[7];
    bool canUpdate = true;
    public void LockUpdates() { canUpdate = false; }
    public void UnlockUpdates() { canUpdate = true; }
    public void RecalculateWeeklyWeather()
    {
        if (!canUpdate) {
            return;
        }
        string[] forcast = new string[7];
        for (int i = 0; i < 7; i ++) {
            weeklyPercipitation[i] = Mathf.Clamp01(percipitationCurve.Evaluate(Random.value));
            forcast[i] = RainLevel(weeklyPercipitation[i]);
        }    
        weatherPanel.SetPercipitationText(forcast);
    }

    public void MarkDay(int day) 
    {
        weatherPanel.MarkDay(day);
    }

    string RainLevel(float amountOfRain) {
        if (amountOfRain < 0.002f) {
            return "Clear";
        } else if (amountOfRain < 0.35f) {
            return "Drizzle";
        } else if (amountOfRain < 0.90f) {
            return "Rain";
        } else {
            return "Downpour";
        }
    }

    public float GetWeatherReport(int day) {
        return weeklyPercipitation[day];
    }
}
