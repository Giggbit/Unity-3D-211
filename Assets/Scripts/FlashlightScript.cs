using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    private Transform parentTransform;
    private Light flashlight;
    private bool isOnFlashlight;
    private AudioSource switchFlashlightSound;

    public float chargeLevel => FlashlightState.charge;

    void Start() {
        parentTransform = transform.parent;
        if(parentTransform == null ) {
            Debug.LogError("FlashlightScript: parentTransform not found");
        }
        flashlight = GetComponent<Light>();
        isOnFlashlight = true;
        FlashlightState.charge = 2.0f;
        switchFlashlightSound = GetComponent<AudioSource>();
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update() {
        if(parentTransform == null) return;

        if(FlashlightState.charge > 0 && !GameState.isDay && isOnFlashlight) { 
            flashlight.intensity = FlashlightState.charge;
            FlashlightState.charge -= Time.deltaTime / FlashlightState.workTime;
        }

        if(GameState.isFpv) {
            transform.forward = Camera.main.transform.forward;
        }
        else {
            Vector3 f = Camera.main.transform.forward;
            f.y = 0.0f;
            if (f == Vector3.zero) f = Camera.main.transform.up;
            transform.forward = f.normalized;
        }

        if(Input.GetKeyUp(KeyCode.R)) {
            switchFlashlightSound.Play();
            if(flashlight.enabled == true) {
                flashlight.enabled = false;
                isOnFlashlight = false;
            }
            else {
                flashlight.enabled = true;
                isOnFlashlight = true;
            }
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data) { 
        if(eventName == "EffectsVolume") { 
            switchFlashlightSound.volume = (float)data;
        }
    }

    private void OnDestroy() {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}
