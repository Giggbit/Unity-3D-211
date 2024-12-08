using UnityEngine;
using UnityEngine.UI;

public class BatteryIndicatorScript : MonoBehaviour
{
    private Image image;
    private FlashlightScript flashlightScript;
    private AudioSource lowChargeSound;

    void Start() {
        image = GetComponent<Image>();
        flashlightScript = GameObject.Find("flashlight").GetComponent<FlashlightScript>();

        lowChargeSound = GetComponent<AudioSource>();
        lowChargeSound.volume = GameState.effectsVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update() {
        image.fillAmount = flashlightScript.chargeLevel;
        if(image.fillAmount > 0.8f ) {
            image.color = Color.green;
        }
        else if (image.fillAmount > 0.3f) {
            image.color = Color.yellow;
            lowChargeSound.Play();
        }
        else {
            image.color = Color.red;
            //lowChargeSound.Play();
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data) { 
        if(eventName == "EffectsVolume") { 
            lowChargeSound.volume = (float)data;
        }
    }

    private void OnDestroy() {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}
