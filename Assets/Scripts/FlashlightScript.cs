using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    private Transform parentTransform;
    private Light flashlight;
    private bool isOnFlashlight;

    void Start() {
        parentTransform = transform.parent;
        if(parentTransform == null ) {
            Debug.LogError("FlashlightScript: parentTransform not found");
        }
        flashlight = GetComponent<Light>();
        isOnFlashlight = true;
        FlashlightState.charge = 2.0f;
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
}
