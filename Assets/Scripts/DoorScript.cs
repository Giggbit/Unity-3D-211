using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.name == "Character") { 
            ToastScript.ShowToast("Find the key!");
        }
    }

    void Start() {
        
    }

    void Update() {
        
    }
}
