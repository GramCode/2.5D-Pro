using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimbFinished : MonoBehaviour
{
    [SerializeField]
    private Transform _ladderTopPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AnimationStateManager.Instance.SetLadderClimbFinishedAnimation(true);

            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.ClimbingLadderFinished(this);
            }
        }
    }

    public Vector3 LadderTopPosition()
    {
        return _ladderTopPosition.position;
    }
}
