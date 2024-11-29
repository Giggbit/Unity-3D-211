using System.Linq;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light[] dayLights;
    private Light[] nightLights;
    //private bool isDay;
    
    void Start() {
        dayLights = GameObject.FindGameObjectsWithTag("dayLight").Select(g => g.GetComponent<Light>()).ToArray();
        nightLights = GameObject.FindGameObjectsWithTag("nightLight").Select(g => g.GetComponent<Light>()).ToArray();
        SwitchDay();
    }

    void Update() {
        if(Input.GetKeyUp(KeyCode.N)) { 
            SwitchDay();
        }
    }

    private void SwitchDay() {
        GameState.isDay = !GameState.isDay;
        foreach (Light light in dayLights) { 
            light.enabled = GameState.isDay;
        }
        foreach (Light light in nightLights) { 
            light.enabled = !GameState.isDay;
        }
    }
}
