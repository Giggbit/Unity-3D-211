using UnityEngine;

public class BatteryScript : MonoBehaviour
{

    private float batteryCharge = 0.5f;
    private AudioSource collectSound;
    private float destroyTimeout;

    void Start() {
        collectSound = GetComponent<AudioSource>();
        destroyTimeout = 0f;
        collectSound.volume = GameState.effectsVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update() {
        if(destroyTimeout > 0) { 
            destroyTimeout -= Time.deltaTime;
            if(destroyTimeout < 0) {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(gameObject.CompareTag("battery")) {
            collectSound.Play();
            GameState.TriggerGameEvent("Battery", new GameEvents.MessageEvent {
                message = "+ " + batteryCharge + " battery charge",
                data = batteryCharge,
            });
            FlashlightState.charge += batteryCharge;
            destroyTimeout = 0.3f;
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data) { 
        if(eventName == "EffectsVolume") { 
            collectSound.volume = (float)data;
        }
    }

    private void OnDestroy() {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}
