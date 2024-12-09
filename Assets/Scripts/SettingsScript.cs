using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject content;
    private Slider effectsSlider;
    private Slider ambientSlider;
    private Slider sensitivityXSlider;
    private Slider sensitivityYSlider;
    private Toggle isOffSoundsToggle;
    private Toggle linkToggle;
    private TextMeshProUGUI reloadMessageTMP;

    void Start() {
        Transform contentTransform = transform.Find("Content");
        content = contentTransform.gameObject;
        if (content.activeInHierarchy) {
            Time.timeScale = 0.0f;
        }
        effectsSlider = contentTransform.Find("Sound/EffectBackground/EffectSlider").GetComponent<Slider>();
        ambientSlider = contentTransform.Find("Sound/AmbientBackground/AmbientSlider").GetComponent<Slider>();
        isOffSoundsToggle = contentTransform.Find("Sound/Toggle").GetComponent<Toggle>();
        sensitivityXSlider = contentTransform.Find("Controls/SensitivityBackground/XSensitivitySlider").GetComponent<Slider>();
        sensitivityYSlider = contentTransform.Find("Controls/SensitivityBackground/YSensitivitySlider").GetComponent<Slider>();
        linkToggle = contentTransform.Find("Controls/SensitivityBackground/linkToggle").GetComponent<Toggle>();
        reloadMessageTMP = contentTransform.Find("Controls/ReloadMessageTMP").GetComponent<TextMeshProUGUI>();

        OnEffectsSliderChanged(effectsSlider.value);
        OnAmbientSliderChanged(ambientSlider.value);
        OnSensitivityXSliderChanged(sensitivityXSlider.value);
        if (!linkToggle.isOn) OnSensitivityYSliderChanged(sensitivityYSlider.value);

        reloadMessageTMP.enabled = false;

        LoadSettings();
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            OnClose();
        }
        OnSoundToggleChanged(isOffSoundsToggle.isOn);
    }

    private void LoadSettings() { 
        if(PlayerPrefs.HasKey(nameof(effectsSlider))) {
            OnEffectsSliderChanged(
                PlayerPrefs.GetFloat(nameof(effectsSlider))
            );
        }
        if(PlayerPrefs.HasKey(nameof(ambientSlider))) {
            OnAmbientSliderChanged(
                PlayerPrefs.GetFloat(nameof(ambientSlider))
            );
        }
        if(PlayerPrefs.HasKey(nameof(isOffSoundsToggle))) {
            isOffSoundsToggle.isOn = PlayerPrefs.GetInt(nameof(isOffSoundsToggle)) > 0;
        }
        if (PlayerPrefs.HasKey(nameof(sensitivityXSlider))) {
            OnSensitivityXSliderChanged(
                PlayerPrefs.GetFloat(nameof(sensitivityXSlider))
            );
        }
        if (PlayerPrefs.HasKey(nameof(sensitivityYSlider))) {
            OnSensitivityYSliderChanged(
                PlayerPrefs.GetFloat(nameof(sensitivityYSlider))
            );
        }
        if (PlayerPrefs.HasKey(nameof(linkToggle))) {
            linkToggle.isOn = PlayerPrefs.GetInt(nameof(linkToggle)) > 0;
        }
    }

    public void OnSaveButtonClick() { 
        PlayerPrefs.SetFloat(nameof(effectsSlider), effectsSlider.value);
        PlayerPrefs.SetFloat(nameof(ambientSlider), ambientSlider.value);
        PlayerPrefs.SetInt(nameof(isOffSoundsToggle), isOffSoundsToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat(nameof(sensitivityXSlider), sensitivityXSlider.value);
        PlayerPrefs.SetFloat(nameof(sensitivityYSlider), sensitivityYSlider.value);
        PlayerPrefs.SetInt(nameof(linkToggle), linkToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnResetButtonClick() {
        PlayerPrefs.DeleteAll();
        reloadMessageTMP.enabled = true;
        reloadMessageTMP.text = "Please restart the game to change settings";
    }

    public void OnEffectsSliderChanged(float value) {
        effectsSlider.value = value;
        GameState.TriggerGameEvent("EffectsVolume", GameState.effectsVolume = value);
    }

    public void OnAmbientSliderChanged(float value) {
        ambientSlider.value = value;
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

    public void OnSensitivityXSliderChanged(float value) {
        float sens = Mathf.Lerp(1, 10, value);
        GameState.lookSensitivityX = sens;
        if(linkToggle.isOn) { 
            sensitivityYSlider.value = value;
            GameState.lookSensitivityY = -sens;
        }
    }

    public void OnSensitivityYSliderChanged(float value) {
        float sens = Mathf.Lerp(1, 10, value);
        GameState.lookSensitivityY = -sens;
        if(linkToggle.isOn) { 
            sensitivityXSlider.value = value;
            GameState.lookSensitivityX = sens;
        }
    }

    public void OnClose() {
        Time.timeScale = content.activeInHierarchy ? 1.0f : 0.0f;
        content.SetActive(!content.activeInHierarchy);
    }
}
