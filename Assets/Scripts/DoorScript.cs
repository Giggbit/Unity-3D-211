using System.Linq;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    private string requiredKey = "1";
    private float openingTime = 2f;
    private float timeout = 0f;
    private bool isClosed = true;
    private float openedPart = 0.5f;
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
            float t = Time.deltaTime / openingTime;
            transform.Translate(t + 0.001f, 0, 0);
            if(timeout >= openedPart && timeout - timeout < openedPart) {
                GameState.room += 1;
            }
            timeout -= t;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("character") && isClosed) {
            if (GameState.collectedItems.ContainsKey("Key" + requiredKey)) {
                bool isInTime = (float)GameState.collectedItems["Key" + requiredKey] > 0;
                GameState.TriggerGameEvent("Door", 
                    new GameEvents.MessageEvent {
                        message = "Door opening" + (isInTime ? "faster" : "slowly"),
                        data = requiredKey,
                    }
                );
                if(!isInTime) {
                    openingTime *= 3;
                }
                timeout = 1f;
                isClosed = false;
                openSound.Play();
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
