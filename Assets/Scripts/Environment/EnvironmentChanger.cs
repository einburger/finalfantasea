using UnityEngine;
using UnityEngine.VFX;

[ExecuteAlways] public class EnvironmentChanger : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private EnvironmentPreset preset;

    [SerializeField, Range(0f, 2f)] private float speedOfTime;
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField, Range(0, 7)] private float dayOfWeek;
    private float timeSinceLoad = 0f;
    private const int hoursPerDay = 24;
    private const int hoursPerWeek = 168;
    private const int PercipitationConstant = 10000;

    [SerializeField] private GameObject firefliesSystem;
    [SerializeField] private VisualEffect rainSystem;
    [SerializeField] private WeatherForcaster weatherForcast;
    private bool firefliesActive = false;

    private void Update() {
        if (!preset) {
            return;
        }

        if (Application.isPlaying) {
            timeSinceLoad = Time.timeSinceLevelLoad * speedOfTime;
            timeOfDay = timeSinceLoad % hoursPerDay;
            dayOfWeek = (timeSinceLoad % hoursPerWeek) / 24f;

            int quantizedDayOfWeek = (int)(dayOfWeek % 7);
            if (quantizedDayOfWeek == 0) {
                weatherForcast.RecalculateWeeklyWeather();
                weatherForcast.LockUpdates();
            } 
            if (quantizedDayOfWeek == 6) {
                weatherForcast.UnlockUpdates();
            }

            float percipitation = weatherForcast.GetWeatherReport(quantizedDayOfWeek);
            weatherForcast.MarkDay(quantizedDayOfWeek);
            rainSystem.SetInt("percipitation", (int)(percipitation * PercipitationConstant));
        }

        UpdateLighting(timeOfDay / 24f);
    }

    private void UpdateLighting(float percentTime) {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(percentTime);
        RenderSettings.fogColor = preset.fogColor.Evaluate(percentTime);
        Camera.main.backgroundColor = preset.skyColor.Evaluate(percentTime);

        if (directionalLight) {
            directionalLight.color = preset.sunColor.Evaluate(percentTime);
        }

        if (percentTime > 0.40f && percentTime < 0.60f) {
            if (!firefliesActive) {
                firefliesSystem.GetComponent<VisualEffect>().SendEvent("SpawnFlies");
                firefliesActive = true;
            }
        } else {
            if (firefliesActive) {
                firefliesSystem.GetComponent<VisualEffect>().SendEvent("KillFlies");
                firefliesActive = false;
            }
        }
    }

    private void OnValidate()
    {
        if (directionalLight) {
            return;
        }

        if (RenderSettings.sun) {
            directionalLight = RenderSettings.sun;
        } else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights) {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}
