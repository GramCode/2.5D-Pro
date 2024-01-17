using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private Transform _snapPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.ClimbLadder(true, this);

                if (player.IsClimbingLadder() == false)
                    UIManager.Instance.DisplayInteractionText(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.ClimbLadder(false, this);
                UIManager.Instance.DisplayInteractionText(false);
            }
        }
    }

    public Vector3 LadderSnapPosition()
    {
        return _snapPosition.position;
    }
}
