using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 9f;
    [SerializeField]
    private float _gravity = 30f;
    [SerializeField]
    private float _jumpHeight = 12f;
    [SerializeField]
    private float _climbLadderSpeed = 2f;

    private Vector3 _velocity, _direction;
    private float _yVelocity;
    private bool _jumping = false;
    private bool _snapToLedge = false;
    private bool _onLedge = false;
    private Ledge _activeLedge;
    private int _coins;
    private bool _canClimbLadder = false;
    private float _startingGravity;
    private bool _climbingLadder = false;
    private Ladder _activeLadder;
    private bool _canMove = true;
    private bool _climbingLadderFinished = false;
    private LadderClimbFinished _activeLadderTopTrigger;
    private bool _canUpdateAnimationSpeed = false;

    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (_controller == null)
            Debug.LogError("The character controller in Player is NULL");

        _startingGravity = _gravity;
    }

    void Update()
    {
        if (_canClimbLadder && _jumping == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartClimbingLadder();
            }
        }

        if (_climbingLadder == false)
        {
            if (_canMove == true)
                Move();
        }
        else
        {
            if (_climbingLadderFinished == true)
                return;

            ClimbingLadder();
        }

        SnapToLedge();

        ClimbUpLedge();
    }
    
    private void StartClimbingLadder()
    {
        AnimationStateManager.Instance.SetClimbDirectionAnimation(0);
        AnimationStateManager.Instance.SetClimbLadderAnimation(true);
        _gravity = 0;
        _canMove = false;
        StartCoroutine(WaitToClimbLadderRoutine());
        transform.position = _activeLadder.LadderSnapPosition();
    }

    IEnumerator WaitToClimbLadderRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        AnimationStateManager.Instance.SetSpeedState(0);
        _climbingLadder = true;
    }

    private void Move()
    {
        if (_controller.isGrounded)
        {
            _yVelocity = 0;

            AnimationStateManager.Instance.SetLadderClimbFinishedAnimation(false);
            _climbingLadderFinished = false;

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

    private void ClimbingLadder()
    {
        float vertical = Input.GetAxis("Vertical");

        if (_canUpdateAnimationSpeed)
            AnimationStateManager.Instance.AnimationSpeed(Mathf.Abs(vertical));

        AnimationStateManager.Instance.SetClimbDirectionAnimation(vertical);

        Vector3 dir = new Vector3(0, vertical, 0);
        Vector3 velocity = dir * _climbLadderSpeed;

        _controller.Move(velocity * Time.deltaTime);

        if (_controller.isGrounded)
        {
            AnimationStateManager.Instance.SetClimbLadderAnimation(false);
            _climbingLadder = false;
            _gravity = _startingGravity;
            AnimationStateManager.Instance.AnimationSpeed(1);
            _canMove = true;
        }
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

    private void ClimbUpLedge()
    {
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

    public void CanUpdateAnimationSpeed()
    {
        _canUpdateAnimationSpeed = true;
    }

    public void ClimbLadder(bool climb, Ladder ladder)
    {
        _canClimbLadder = climb;
        _activeLadder = ladder;
    }

    public void ClimbingLadderFinished(LadderClimbFinished ladderClimbFinished)
    {
        if (_climbingLadder)
        {
            _canUpdateAnimationSpeed = false;
            AnimationStateManager.Instance.AnimationSpeed(1);
            _climbingLadderFinished = true;
            _activeLadderTopTrigger = ladderClimbFinished;
            _controller.enabled = false;
        }

    }

    public void ClimbingLadderComplete()
    {
        transform.position = _activeLadderTopTrigger.LadderTopPosition();
        _climbingLadder = false;
        AnimationStateManager.Instance.SetSpeedState(0);
        AnimationStateManager.Instance.SetClimbLadderAnimation(_climbingLadder);
        AnimationStateManager.Instance.SetClimbDirectionAnimation(0);
        _canMove = true;
        _gravity = _startingGravity;
    }
    
}
