using UnityEngine;
using UnityEngine.UI;

public class PercipitationText : MonoBehaviour
{
    [SerializeField] private int day = 0;
    Image checkbox;
    Text text;
    WeatherPanel panel;

    void Start()
    {
       panel = GetComponentInParent<WeatherPanel>();
       text = GetComponentInChildren<Text>(); 
       checkbox = GetComponentInChildren<Image>();
    }

    public void SetText(string percipitation) {
        text.text = percipitation;
    }

    void Update() {
        if (panel.updated[day]) {
            SetText(panel.GetForcast(day));
            panel.updated[day] = false;
        }

        if (panel.dayPassed[day]) {
            if (!checkbox.enabled) {
                checkbox.enabled = true;
            }
        } else  {
            if (checkbox.enabled) {
                checkbox.enabled = false;
            }
        }
    }
}
