using UnityEngine;
using UnityEngine.UI;

public class BatteryIndicatorScript : MonoBehaviour
{
    private Image image;
    private FlashlightScript flashlightScript;

    void Start() {
        image = GetComponent<Image>();
        flashlightScript = GameObject.Find("flashlight").GetComponent<FlashlightScript>();
    }

    void Update() {
        image.fillAmount = flashlightScript.chargeLevel;
        if(image.fillAmount > 0.8f ) {
            image.color = Color.green;
        }
        else if (image.fillAmount > 0.3f) {
            image.color = Color.yellow;
        }
        else {
            image.color = Color.red;
        }
    }
}
