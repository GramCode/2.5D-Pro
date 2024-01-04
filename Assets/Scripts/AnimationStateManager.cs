using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    private static AnimationStateManager _instance;
    public static AnimationStateManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Animation Sater Manager is NULL");

            return _instance;
        }
    }

    public enum AnimationState
    {
        idle,
        run
    }

    private Animator _anim;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();

        if (_anim == null)
            Debug.LogError("Animator in Player is NULL");
    }

    public void SetSpeedState(float speed)
    {
        _anim.SetFloat("Speed", Mathf.Abs(speed));
    }

    public void SetJumpState(bool jump)
    {
        _anim.SetBool("Jumping", jump);
    }


}
