using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;

    void Start() {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update() {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.AddForce(300 * Time.deltaTime * (Camera.main.transform.right * moveValue.x + Camera.main.transform.forward * moveValue.y));
    }
}
