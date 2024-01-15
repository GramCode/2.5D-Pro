using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField]
    private Transform _respawnLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
                StartCoroutine(CharacterControllerRoutine(cc));
                AnimationStateManager.Instance.SetSpeedState(0);
                AnimationStateManager.Instance.SetJumpState(false);
            }

            other.transform.position = _respawnLocation.position;

            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.ResetVelocityOnDead();
            }

            

        }
    }

    IEnumerator CharacterControllerRoutine(CharacterController cc)
    {
        yield return new WaitForSeconds(0.5f);
        cc.enabled = true;
    }
}
