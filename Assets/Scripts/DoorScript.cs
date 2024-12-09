using System.Linq;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    private string requiredKey = "1";
    private float openingTime = 2f;
    private float timeout = 0f;
    private AudioSource closeSound;
    private AudioSource openSound;

    void Start() {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        closeSound = audioSources[0];
        openSound = audioSources[1];

        closeSound.volume = GameState.effectsVolume;
        openSound.volume = GameState.effectsVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update() {
        if(timeout > 0f) {
            transform.Translate(Time.deltaTime / openingTime, 0, 0);
            timeout -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("character")) {
            if (GameState.collectedItems.ContainsKey("Key" + requiredKey)) {
                openSound.Play();
                GameState.TriggerGameEvent("Door", 
                    new GameEvents.MessageEvent {
                        message = "Door opening",
                        data = requiredKey,
                    }
                );
                timeout = openingTime;
            }
            else {
                closeSound.Play();
                GameState.TriggerGameEvent("Door", 
                    new GameEvents.MessageEvent {
                        message = "Find the key!",
                        data = requiredKey,
                    }
                );
            }
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data) { 
        if(eventName == "EffectsVolume") { 
            openSound.volume = (float)data;
            closeSound.volume = (float)data;
        }
    }
}
