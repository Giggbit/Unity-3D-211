using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Transform character;
    private InputAction lookAction;
    private Vector3 cameraAngles;
    private Vector3 r;

    // sensitivity
    private float sensitivityH = 10f;
    private float sensitivityV = -6f;

    // distance
    private float minFpvDistance = 1f;
    private float maxFpvDistance = 18f;
   

    void Start() {
        lookAction = InputSystem.actions.FindAction("Look");
        cameraAngles = this.transform.eulerAngles;
        character = GameObject.Find("Character").transform; 
        r = this.transform.position - character.position;
    }

    void Update() {
        Vector2 scrollWheel = Input.mouseScrollDelta;
        if(scrollWheel.y != 0) {
            if(r.magnitude > minFpvDistance && r.magnitude < maxFpvDistance) {
                float rr = r.magnitude * (1 - scrollWheel.y / 10);
                if(rr <= minFpvDistance ) {
                    r *= 0.01f;
                    GameState.isFpv = true;
                }
                else if(rr >= maxFpvDistance) {
                    r *= 1f;
                }
                else {
                    r *= (1 - scrollWheel.y / 10);
                }
            }
            else if(scrollWheel.y < 0) {
                r *= 100f;
                GameState.isFpv = false;
            }
        }

        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        if (lookValue != Vector2.zero) {
            cameraAngles.x += lookValue.y * Time.deltaTime * GameState.lookSensitivityY;
            cameraAngles.y += lookValue.x * Time.deltaTime * GameState.lookSensitivityX;

            if(r.y < 0.90f) {
                cameraAngles.x = Mathf.Clamp(cameraAngles.x, -10, 40);
            }
            else {
                cameraAngles.x = Mathf.Clamp(cameraAngles.x, 35, 75);
            }

            this.transform.eulerAngles = cameraAngles;
        }
        this.transform.position = character.position + Quaternion.Euler(0, cameraAngles.y, 0) * r;
    }
}
