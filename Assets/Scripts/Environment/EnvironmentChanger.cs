using UnityEngine;
using UnityEngine.VFX;

[ExecuteAlways] public class EnvironmentChanger : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private EnvironmentPreset preset;

    [SerializeField, Range(0, 24)] private float timeOfDay;

    [SerializeField] private GameObject rainSystem;
    [SerializeField] private GameObject firefliesSystem;
    private bool firefliesActive = false;

    private void Update() {
        if (!preset) {
            return;
        }

        if (Application.isPlaying) {
            timeOfDay += Time.deltaTime * 0.5f;
            timeOfDay %= 24;
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
