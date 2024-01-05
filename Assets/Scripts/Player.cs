using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;
    [SerializeField]
    private float _gravity = 25f;
    [SerializeField]
    private float _jumpHeight = 12f;

    private Vector3 _velocity, _direction;
    private float _yVelocity;
    private bool _jumping = false;

    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (_controller == null)
            Debug.LogError("The character controller in Player is NULL");
    }

    void Update()
    {
        Move();   
    }

    private void Move()
    {
        if (_controller.isGrounded)
        {
            if (_jumping)
            {
                _jumping = false;
                AnimationStateManager.Instance.SetJumpState(_jumping);
            }

            float horizontal = Input.GetAxisRaw("Horizontal");

            _direction = new Vector3(0, 0, horizontal);
            AnimationStateManager.Instance.SetSpeedState(horizontal);
            _velocity = _direction * _speed;
            
            Vector3 facingDirection = transform.localEulerAngles;

            if (horizontal > 0)
            {
                facingDirection.y = 0;
            }
            else if (horizontal < 0)
            {
                facingDirection.y = 180;
            }

            transform.eulerAngles = facingDirection;
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _jumping = true;
                AnimationStateManager.Instance.SetJumpState(_jumping);
            }
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
        }

        _velocity.y = _yVelocity;

        _controller.Move(_velocity * Time.deltaTime);

    }

    public void LedgeGrabbed()
    {
        _controller.enabled = false;
        AnimationStateManager.Instance.SetLedgeAnimation(true);
    }
}
