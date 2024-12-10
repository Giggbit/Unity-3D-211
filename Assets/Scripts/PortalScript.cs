using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [SerializeField]
    private Transform exitPortal;

    private void OnTriggerEnter(Collider other) {
        other.transform.position = exitPortal.position;
    }
}
