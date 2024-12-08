using System.Linq;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light[] dayLights;
    private Light[] nightLights;
    private AudioSource dayAmbientSound;
    private AudioSource nightAmbientSound;
    private AudioSource switchDaySound;

    void Start() {
        dayLights = GameObject.FindGameObjectsWithTag("dayLight").Select(g => g.GetComponent<Light>()).ToArray();
        nightLights = GameObject.FindGameObjectsWithTag("nightLight").Select(g => g.GetComponent<Light>()).ToArray();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        dayAmbientSound = audioSources[0];
        nightAmbientSound = audioSources[1];
        switchDaySound = audioSources[2];

        switchDaySound.volume = GameState.effectsVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");

        dayAmbientSound.volume = GameState.ambientVolume;
        nightAmbientSound.volume = GameState.ambientVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "AmbientVolume");

        SwitchDay();
    }

    void Update() {
        if(Input.GetKeyUp(KeyCode.N)) { 
            SwitchDay();
        }
    }

    private void SwitchDay() {
        switchDaySound.Play();
        GameState.isDay = !GameState.isDay;
        foreach (Light light in dayLights) { 
            light.enabled = GameState.isDay;
        }
        foreach (Light light in nightLights) { 
            light.enabled = !GameState.isDay;
        }

        if (GameState.isDay) {
            dayAmbientSound.Play();
            nightAmbientSound.Stop();
        }
        else if (!GameState.isDay) { 
            dayAmbientSound.Stop();
            nightAmbientSound.Play();
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data) { 
        if(eventName == "AmbientVolume") {
            dayAmbientSound.volume = (float)data;
            nightAmbientSound.volume = (float)data;
        }
        else if (eventName == "EffectsVolume") {
            switchDaySound.volume = (float)data;
        }
    }
    private void OnDestroy() {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "AmbientVolume");
    }
}
