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
            float horizontal = Input.GetAxis("Horizontal");

            _direction = new Vector3(horizontal, 0, 0);

            _velocity = _direction * _speed;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
            }
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
        }

        _velocity.y = _yVelocity;

        _controller.Move(_velocity * Time.deltaTime);

    }
}
