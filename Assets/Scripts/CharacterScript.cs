using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;
    private float batteryCharge;

    void Start() {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        batteryCharge = 0.5f;
    }

    void Update() {
        Vector3 f = Camera.main.transform.forward;
        f.y = 0.0f;
        if (f == Vector3.zero) { 
            f = Camera.main.transform.up;
            f.y = 0.0f;
        }
        f.Normalize();

        Vector3 r = Camera.main.transform.right;
        r.y = 0.0f;
        r.Normalize();

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.AddForce(300 * Time.deltaTime * (r * moveValue.x + f * moveValue.y));
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("battery")) {
            //GameState.collectedItems.Add("Battery", batteryCharge);
            GameState.TriggerGameEvent("Battery", new GameEvents.MessageEvent {
                message = "+ " + batteryCharge + " battery charge",
                data = batteryCharge,
            });
            FlashlightState.charge += batteryCharge;
            Destroy(other.gameObject);
        }
    }
}
