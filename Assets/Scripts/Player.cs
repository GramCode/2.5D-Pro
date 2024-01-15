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
    [SerializeField]
    private float _rollDistance = 8f;

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
    private bool _isRolling = false;
    private Vector3 _rollDirection;
    private float _rollInput;
    private Vector3 _rollVelocity;
    private bool _falling = false;
    private bool _dead = false;
    private float _xVelocity;
    private bool _canFall = false;

    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (_controller == null)
            Debug.LogError("The character controller in Player is NULL");

        _startingGravity = _gravity;

        StartCoroutine(CanFallRoutine());
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
            if (_canMove == true && _isRolling == false)
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

        if (_isRolling)
        {
            _rollInput = Input.GetAxisRaw("Horizontal");
            _rollVelocity = _rollDirection * _rollDistance;

            if (_controller.isGrounded == false)
            {
                _yVelocity -= _gravity * Time.deltaTime;
                _rollVelocity.y = _yVelocity;

                if (_yVelocity < -7)
                    AnimationStateManager.Instance.SetFallingAnimation();
            }
            else
            {
                _yVelocity = 0;
            }

            _controller.Move(_rollVelocity * Time.deltaTime);
            AnimationStateManager.Instance.SetSpeedState(_rollInput);
        }

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

            if (horizontal > 0 || horizontal < 0 && _isRolling == false)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _yVelocity = 0;
                    Roll(horizontal);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _jumping = true;
                AnimationStateManager.Instance.SetJumpState(_jumping);
            }

            if (_falling && !_dead)
            {
                _falling = false;
                AnimationStateManager.Instance.SetLandedAnimation();
            }

            if (_dead)
            {
                _falling = false;
                _dead = false;
                _yVelocity = 0;
            }

            _xVelocity = horizontal;
        }
        else
        {
            if (_isRolling)
                return;
            
            _yVelocity -= _gravity * Time.deltaTime;
            
            if (_falling == false && _yVelocity < -6 && _jumping == false && _canFall)
            {
                AnimationStateManager.Instance.SetFallingAnimation();
                _falling = true;
            }
            
            if (_yVelocity < -15 && _canFall && _jumping)
            {
                AnimationStateManager.Instance.SetFallingAnimation();
                AnimationStateManager.Instance.SetSpeedState(Mathf.Abs(_xVelocity));
                _falling = true;
            }                       
        }

        _velocity.y = _yVelocity;

        if (_controller.enabled)
            _controller.Move(_velocity * Time.deltaTime);

    }

    private void Roll(float horizontal)
    {
        AnimationStateManager.Instance.SetRollAnimation();
        _canMove = false;
        _isRolling = true;
        _rollDirection = new Vector3(0, 0, horizontal);
        if (_controller.isGrounded)
            StartCoroutine(RollCompleteRoutine());
    }

    IEnumerator RollCompleteRoutine()
    {
        yield return new WaitForSeconds(0.7f);
        if (_rollInput == 0)
            _rollDirection /= 2;
        yield return new WaitForSeconds(0.2f);
        if (_rollInput == 0)
            _rollDirection = Vector3.zero;
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
        _canFall = false;
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
        StartCoroutine(CanFallRoutine());
    }

    public void StandUpComplete()
    {
        _controller.enabled = true;
    }

    public void ResetVelocityOnDead()
    {
        _yVelocity = 0;
        _dead = true;
        AnimationStateManager.Instance.SetIdleAnimFromDead();
        _falling = false;
        _canFall = false;
        StartCoroutine(CanFallRoutine());
    }

    IEnumerator CanFallRoutine()
    {
        yield return new WaitForSeconds(1.2f);
        _canFall = true;
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
        StartCoroutine(CanMovePlayerRoutine());
        _gravity = _startingGravity;
    }

    IEnumerator CanMovePlayerRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _canMove = true;
    }

    public void RollComplete()
    {
        _isRolling = false;
        _canMove = true;
    }

    public void GameComplete(GameObject confetti)
    {
        _controller.enabled = false;
        AnimationStateManager.Instance.SetVictoryAnimation();
        UIManager.Instance.DisplayGameCompleteText();
        UIManager.Instance.DisplayInputText();
        GameManager.Instance.GameOver();

        if (_coins == 30)
        {
            UIManager.Instance.DisplayCoinsCollectedText();
            confetti.SetActive(true);
        }
    }

    public void CanMovePlayer(bool move)
    {
        _canMove = move;
    }

}
