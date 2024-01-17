using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public void PlayRightFootstep()
    {
        SoundManager.Instance.PlayFootstepsSound(SoundManager.FootSounds.RighFootstep);
    }

    public void PlayLeftFootstep()
    {
        SoundManager.Instance.PlayFootstepsSound(SoundManager.FootSounds.LeftFootstep);
    }

    public void PlayLadderSound()
    {
        SoundManager.Instance.PlayFootstepsSound(SoundManager.FootSounds.ClimbingLadder);
    }
}
