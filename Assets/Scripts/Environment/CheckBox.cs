using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    WeatherPanel panel;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
       panel = GetComponentInParent<WeatherPanel>(); 
       image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
