using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Transform character;
    private InputAction lookAction;
    private Vector3 cameraAngles;
    private Vector3 r;
    private float sensitivityH = 10f;
    private float sensitivityV = -6f;

    void Start() {
        lookAction = InputSystem.actions.FindAction("Look");
        cameraAngles = this.transform.eulerAngles;
        character = GameObject.Find("Character").transform; 
        r = this.transform.position - character.position;
    }

    void Update() {
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        if (lookValue != Vector2.zero) {
            cameraAngles.x += lookValue.y * Time.deltaTime * sensitivityV;
            cameraAngles.y += lookValue.x * Time.deltaTime * sensitivityH;

            cameraAngles.x = Mathf.Clamp(cameraAngles.x, 35, 75);

            this.transform.eulerAngles = cameraAngles;
        }
        this.transform.position = character.position + Quaternion.Euler(0, cameraAngles.y, 0) * r;
    }
}
