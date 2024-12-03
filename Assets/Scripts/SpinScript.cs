using UnityEngine;

public class SpinScript : MonoBehaviour
{
    [SerializeField]
    private float period = 2.0f;

    void Start() {
        
    }

    void Update() {
        this.transform.Rotate(0, Time.deltaTime / period * 360, 0, Space.World);
    }
}
