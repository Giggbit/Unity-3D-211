using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Transform character;
    private InputAction lookAction;
    private Vector3 cameraAngles;
    private Vector3 r;   

    void Start() {
        lookAction = InputSystem.actions.FindAction("Look");
        cameraAngles = this.transform.eulerAngles;
        character = GameObject.Find("Character").transform; 
        r = this.transform.position - character.position;
    }

    void Update() {
        Vector2 scrollWheel = Input.mouseScrollDelta;
        if(scrollWheel.y != 0) {
            if(r.magnitude > GameState.minFpvDistance && r.magnitude < GameState.maxFpvDistance) {
                float rr = r.magnitude * (1 - scrollWheel.y / 10);
                if(rr <= GameState.minFpvDistance ) {
                    r *= 0.01f;
                    GameState.isFpv = true;
                }
                else if(rr >= GameState.maxFpvDistance) {
                    r *= 1f;
                }
                else {
                    r *= (1 - scrollWheel.y / 10);
                }
            }
            else if(scrollWheel.y < 0) {
                r *= 100f;
                r *= (1 - scrollWheel.y / 10);
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
