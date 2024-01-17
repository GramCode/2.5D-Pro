using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sounds
    {
        Coin,
        Death,
        Jump,
        Land,
        Effort,
        Grab,
    }

    public enum FootSounds
    {
        RighFootstep,
        LeftFootstep,
        ClimbingLadder
    }

    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Sound Manager is NULL");

            return _instance;
        }
    }

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _footstepsAudioSource;
    [SerializeField]
    private AudioSource _coinsAudioSource;
    [SerializeField]
    private AudioClip[] _clips; //0 = Death, 1 = Jump, 2 = Land a jump, 3 = effort, 4 = grab, 5 = Righ Footstep, 6 = Left Footstep, 7 = Climbing Ladder

    private void Awake()
    {
        _instance = this;
    }

    public void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.Coin:
                _coinsAudioSource.Play();
                return;
            case Sounds.Death:
                _audioSource.clip = _clips[0];
                break;
            case Sounds.Jump:
                _audioSource.clip = _clips[1];
                break;
            case Sounds.Land:
                _audioSource.clip = _clips[2];
                break;
            case Sounds.Effort:
                _audioSource.clip = _clips[3];
                break;
            case Sounds.Grab:
                _audioSource.clip = _clips[4];
                break;
            
            default:
                break;
        }

        
        _audioSource.Play();
    }

    public void PlayFootstepsSound(FootSounds sound)
    {
        switch (sound)
        {
            case FootSounds.RighFootstep:
                _footstepsAudioSource.clip = _clips[5];
                break;
            case FootSounds.LeftFootstep:
                _footstepsAudioSource.clip = _clips[6];
                break;
            case FootSounds.ClimbingLadder:
                _footstepsAudioSource.clip = _clips[7];
                break;
            default:
                break;
        }

        _footstepsAudioSource.Play();
    }
}
