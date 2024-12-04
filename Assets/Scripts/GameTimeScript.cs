using TMPro;
using UnityEngine;

public class GameTimeScript : MonoBehaviour
{
    private TMPro.TextMeshProUGUI timeTMP;
    private float gameTime = 0f;

    void Start() {
        timeTMP = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update() {
        gameTime += Time.deltaTime;
        timeTMP.text = gameTime.ToString("00:00");
    }
}
