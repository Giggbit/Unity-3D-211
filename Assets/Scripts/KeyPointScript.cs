using UnityEngine;

public class KeyPointScript : MonoBehaviour
{
    [SerializeField]
    private string keyPointName = "1";
    [SerializeField]
    private float timeout = 5.0f;
    [SerializeField]
    private int room = -1;

    private float leftTime;
    private AudioSource getKeySound;

    public float part { get; private set; }

    void Start() {
        leftTime = timeout;
        part = 1.0f;

        getKeySound = GetComponent<AudioSource>();
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update() {
        if (GameState.room == room && leftTime > 0) {
            leftTime -= Time.deltaTime;
            if (leftTime < 0) leftTime = 0;
            part = leftTime / timeout;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("character")) {
            GameState.collectedItems.Add("Key" + keyPointName, part);
            GameState.TriggerGameEvent("KeyPoint", new GameEvents.MessageEvent {
                message = "Key " + keyPointName,
                data = part,
            });
            getKeySound.Play();
            Destroy(gameObject);
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data) { 
        if(eventName == "EffectsVolume") { 
            getKeySound.volume = (float)data;
        }
    }

    private void OnDestroy() {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}
