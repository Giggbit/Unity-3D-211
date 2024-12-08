using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject content;
    private Slider effectsSlider;
    private Slider ambientSlider;
    private Toggle isOffSoundsToggle;

    void Start() {
        Transform contentTransform = transform.Find("Content");
        content = contentTransform.gameObject;
        if(content.activeInHierarchy) {
            Time.timeScale = 0.0f;
        }
        effectsSlider = contentTransform.Find("Sound/EffectBackground/EffectSlider").GetComponent<Slider>();
        OnEffectsSliderChanged(effectsSlider.value);

        ambientSlider = contentTransform.Find("Sound/AmbientBackground/AmbientSlider").GetComponent<Slider>();
        OnAmbientSliderChanged(ambientSlider.value);

        isOffSoundsToggle = contentTransform.Find("Sound/Toggle").GetComponent<Toggle>();
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            Time.timeScale = content.activeInHierarchy ? 1.0f  : 0.0f;
            content.SetActive( ! content.activeInHierarchy );
        }
        OnSoundToggleChanged(isOffSoundsToggle.isOn);
    }

    public void OnEffectsSliderChanged(float value) {
        GameState.TriggerGameEvent("EffectsVolume", GameState.effectsVolume = value);
    }

    public void OnAmbientSliderChanged(float value) {
        GameState.TriggerGameEvent("AmbientVolume", GameState.ambientVolume = value);
    }

    public void OnSoundToggleChanged(bool value) { 
        if(value) {
            OnEffectsSliderChanged(0);
            OnAmbientSliderChanged(0);
        }
        else {
            OnEffectsSliderChanged(effectsSlider.value);
            OnAmbientSliderChanged(ambientSlider.value);
        }
    }
}
