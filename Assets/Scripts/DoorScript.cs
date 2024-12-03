using System.Linq;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    private string requiredKey = "1";
    private float openingTime = 2f;
    private float timeout = 0f; 

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("character")) {
            if (GameState.collectedItems.ContainsKey("Key" + requiredKey)) {
                GameState.TriggerGameEvent("Door", 
                    new GameEvents.MessageEvent {
                        message = "Door opening",
                        data = requiredKey,
                    }
                );
                timeout = openingTime;
            }
            else {
                GameState.TriggerGameEvent("Door", 
                    new GameEvents.MessageEvent {
                        message = "Find the key!",
                        data = requiredKey,
                    }
                );
            }
        }
    }

    void Start() {
        
    }

    void Update() {
        if(timeout > 0f) {
            transform.Translate(Time.deltaTime / openingTime, 0, 0);
            timeout -= Time.deltaTime;
        }
    }
}
