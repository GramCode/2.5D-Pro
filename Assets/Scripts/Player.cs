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
    private bool _snapToLedge = false;
    private bool _onLedge = false;
    private Ledge _activeLedge;
    private int _coins;

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
        SnapToLedge();

        if (_onLedge)
        {
            float vertical = Input.GetAxis("Vertical");

            if (vertical > 0)
            {
                AnimationStateManager.Instance.SetClimbAnimation();
                _onLedge = false;
            }

        }
    }

    private void Move()
    {
        if (_controller.isGrounded)
        {
            _yVelocity = 0;

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

        if (_controller.enabled)
            _controller.Move(_velocity * Time.deltaTime);

    }

    public void LedgeGrabbed(Ledge currentLedge)
    {
        _controller.enabled = false;
        AnimationStateManager.Instance.SetLedgeAnimation(true);
        AnimationStateManager.Instance.SetSpeedState(0);
        AnimationStateManager.Instance.SetJumpState(false);
        _snapToLedge = true;
        _onLedge = true;
        _activeLedge = currentLedge;
    }

    private void SnapToLedge()
    {
        if (_snapToLedge)
        {
            transform.position = Vector3.Lerp(transform.position, _activeLedge.LedgeGrabPosition(), 30 * Time.deltaTime);

            if (Vector3.Distance(transform.position, _activeLedge.LedgeGrabPosition()) < 0.1f)
            {
                _snapToLedge = false;
            }
        }
    }

    public void ClimbUpComplete()
    {
        transform.position = _activeLedge.StandUpPosition();
        AnimationStateManager.Instance.SetLedgeAnimation(false);
    }

    public void StandUpComplete()
    {
        _controller.enabled = true;
    }

    public void ResetVelocity()
    {
        _yVelocity = 0;
    }

    public void AddCoin()
    {
        _coins++;
        UIManager.Instance.UpdateCoinsText(_coins);
    }
}
