using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField]
    private Transform _ledgeGrab, _standUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LedgeGrabCheck"))
        {
            Player player = other.transform.parent.GetComponent<Player>();

            if (player != null)
                player.LedgeGrabbed(this);

        }
    }

    public Vector3 LedgeGrabPosition()
    {
        return _ledgeGrab.position;
    }

    public Vector3 StandUpPosition()
    {
        return _standUp.position;
    }
}
