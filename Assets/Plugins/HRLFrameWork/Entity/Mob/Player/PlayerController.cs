using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using HRL;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    public Player Owner;

    public float runSpeed;
    public float jumpSpeed;
    public float doulbJumpSpeed;
    public float climbSpeed;
    public float restoreTime;
    public float rushSpeed;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    public BoxCollider2D myFeet;

    public bool isRush;
    public bool isGround;
    public bool canDoubleJump;
    public bool isOneWayPlatform;
    [Title("漂浮")]
    public bool isFloat;
    public float floatingGravityScale = 0.5f;
    public float maxFallingSpeed = 1f;

    private bool isLadder;
    private bool isClimbing;

    private bool isJumping;
    private bool isFalling;
    private bool isDoubleJumping;
    private bool isDoubleFalling;

    private float playerGravity;
    [ShowInInspector, ReadOnly]
    private Vector2 move;

    public CapsuleCollider2D capsuleCollider;
    public ParticleSystem particleSystem;
    private AudioSource jumpSource;

    void Awake()
    {
        Owner = GetComponent<Player>();
        jumpSource = GetComponent<AudioSource>();
        RegisteInput();
    }

    #region Input
    public bool LockAttack = false;
    
    [Title("输入")]
    public InputData inputData_Up;
    public InputData inputData_Down;
    public InputData inputData_Left;
    public InputData inputData_Right;
    public InputData inputData_Attack;
    public InputData inputData_Attack_Far;

    private void RegisteInput()
    {
        inputData_Up.unityAction_Down = Jump;
        InputManager.Instance.RegisterInputAction(inputData_Up);

        inputData_Left.unityAction_Down = Input_Left_Press;
        inputData_Left.unityAction_Up = Input_Left_Release;
        InputManager.Instance.RegisterInputAction(inputData_Left, true);

        inputData_Right.unityAction_Down = Input_Right_Press;
        inputData_Right.unityAction_Up = Input_Right_Release;
        InputManager.Instance.RegisterInputAction(inputData_Right, true);

        inputData_Attack.unityAction_Down = Attack;
        inputData_Attack.unityAction_Up = Attack_Release;
        InputManager.Instance.RegisterInputAction(inputData_Attack, true);

        inputData_Attack_Far.unityAction_Down = Attack_Far;
        inputData_Attack_Far.unityAction_Up = Attack_Release_Far;
        InputManager.Instance.RegisterInputAction(inputData_Attack_Far, true);
    }

    private bool _LockInput()
    {
        return InputManager.Instance.mLockPlayerInputNum != 0;
    }

    private void Input_Left_Press()
    {
        if (_LockInput())
        {
            return;
        }
        move.x = -1f;
    }

    private void Input_Left_Release()
    {
        move.x = 0f;
    }

    private void Input_Right_Press()
    {
        if (_LockInput())
        {
            return;
        }
        move.x = 1f;
    }

    private void Input_Right_Release()
    {
        move.x = 0f;
    }

    private void Jump()
    {
        if (_LockInput())
        {
            return;
        }
        if (isGround)
        {
            jumpSource.Play();
            myAnim.SetBool("Jump", true);
            Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
            myRigidbody.velocity = Vector2.up * jumpVel;
            canDoubleJump = true;
            particleSystem.Play();
        }
        else
        {
            if (canDoubleJump)
            {
                jumpSource.Play();
                myAnim.SetBool("DoubleJump", true);
                Vector2 doubleJumpVel = new Vector2(0.0f, doulbJumpSpeed);
                myRigidbody.velocity = Vector2.up * doubleJumpVel;
                canDoubleJump = false;
                particleSystem.Play();
            }
        }
    }

    private void Attack()
    {
        if (_LockInput())
        {
            return;
        }
        if (LockAttack)
        {
            return;
        }
        foreach (Weapon weapon in Owner.weapons)
        {
            if (!weapon.weaponInfo.shoot)
            {
                weapon.Attack();
            }
        }
    }

    private void Attack_Far()
    {
        if (_LockInput())
        {
            return;
        }
        if (LockAttack)
        {
            return;
        }
        foreach (Weapon weapon in Owner.weapons)
        {
            if (weapon.weaponInfo.shoot)
            {
                weapon.Attack();
            }
        }
    }

    public void Attack_Release()
    {
        if (_LockInput())
        {
            return;
        }
        foreach (Weapon weapon in Owner.weapons)
        {
            if (!weapon.weaponInfo.shoot)
            {
                weapon.Attack_Release();
            }
        }
    }

    public void Attack_Release_Far()
    {
        if (_LockInput())
        {
            return;
        }
        foreach (Weapon weapon in Owner.weapons)
        {
            if (weapon.weaponInfo.shoot)
            {
                weapon.Attack_Release();
            }
        }
    }
    #endregion

    void OnEnable() { }

    void OnDisable() { }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        playerGravity = myRigidbody.gravityScale;
    }

    private void FixedUpdate()
    {
        CheckAirStatus();
        Flip();
        Run();
        CheckGrounded();
        CheckFallingSpeed();
        OneWayPlatformCheck();
    }

    void Update()
    {
        SwitchAnimation();
    }

    void Rush()
    {
        if (_LockInput())
        {
            return;
        }
        Time.timeScale = 1f;
        if (isRush)
        {
            return;
        }
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        worldPos.z = 0;
        var dir = (worldPos - transform.position);
        var subDir2 = new Vector2(dir.x, dir.y).normalized;
        myRigidbody.velocity = subDir2 * rushSpeed;
        isRush = true;
        canDoubleJump = true;
        capsuleCollider.isTrigger = true;
        myRigidbody.gravityScale = 0f;
        Invoke("RestorePlayerRush", restoreTime);
        //particleSystem.Play();
    }

    void RushReady()
    {
        if (isRush)
        {
            return;
        }
        Time.timeScale = 0.5f;
    }

    void RestorePlayerRush()
    {
        isRush = false;
        capsuleCollider.isTrigger = false;
        myRigidbody.gravityScale = 1f;
    }

    void CheckGrounded()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("Barrier")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("DestructibleLayer")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        isOneWayPlatform = myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
    }

    void CheckFallingSpeed()
    {
        if (!isGround && isFloat)
        {
            if (myRigidbody.velocity.y < -maxFallingSpeed)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -maxFallingSpeed);
            }
        }
    }

    void CheckLadder()
    {
        isLadder = myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        //Debug.Log("isLadder:" + isLadder);
    }

    void Flip()
    {
        bool plyerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(plyerHasXAxisSpeed)
        {
            if(myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                Owner.weapons_parent.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (myRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                Owner.weapons_parent.localRotation = Quaternion.Euler(0, -180, 0);
            }
        }
    }

    void Run()
    {
        if (_LockInput())
        {
            return;
        }
        if (!isRush)
        {
            float cur_run_speed = runSpeed;
            if (!isGround)
            {
                if (!isFloat)
                {
                    cur_run_speed = runSpeed * 0.5f;
                }
            }
            Vector2 playerVelocity = new Vector2(Mathf.Lerp(move.x * cur_run_speed, myRigidbody.velocity.x, 0.8f), myRigidbody.velocity.y);
            myRigidbody.velocity = playerVelocity;
            bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > 0.01f;
            myAnim.SetBool("Run", playerHasXAxisSpeed);
        }
    }

    void Climb()
    {
        if (_LockInput())
        {
            return;
        }
        float moveY = Input.GetAxis("Vertical");

        if(isClimbing)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveY * climbSpeed);
            canDoubleJump = false;
        }

        if (isLadder)        
        {            
            if (moveY > 0.5f || moveY < -0.5f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("DoubleJump", false);
                myAnim.SetBool("Climbing", true);
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveY * climbSpeed);
                myRigidbody.gravityScale = 0.0f;
            }
            else
            {
                if (isJumping || isFalling || isDoubleJumping || isDoubleFalling)
                {
                    myAnim.SetBool("Climbing", false);
                }
                else
                {
                    myAnim.SetBool("Climbing", false);
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0.0f);
                    
                }
            }
        }
        else
        {
            myAnim.SetBool("Climbing", false);
            myRigidbody.gravityScale = playerGravity;
        }

        if (isLadder && isGround)
        {
            myRigidbody.gravityScale = playerGravity;
        }

        //Debug.Log("myRigidbody.gravityScale:"+ myRigidbody.gravityScale);
    }

    void SwitchAnimation()
    {
        myAnim.SetBool("Idle", false);
        if (myAnim.GetBool("Jump"))
        {
            if(myRigidbody.velocity.y < 0.0f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("Fall", true);
            }
        }
        else if(isGround)
        {
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }
        else if(!isGround)
        {
            myAnim.SetBool("Fall", true);
            myAnim.SetBool("Idle", false);
        }

        if (myAnim.GetBool("DoubleJump"))
        {
            if (myRigidbody.velocity.y < 0.0f)
            {
                myAnim.SetBool("DoubleJump", false);
                myAnim.SetBool("DoubleFall", true);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("DoubleFall", false);
            myAnim.SetBool("Idle", true);
        }
        else if (!isGround)
        {
            myAnim.SetBool("Fall", true);
            myAnim.SetBool("Idle", false);
        }
    }

    void OneWayPlatformCheck()
    {
        if (isGround && capsuleCollider.isTrigger && !isRush)
        {
            capsuleCollider.isTrigger = false;
        }
        float moveY = Input.GetAxis("Vertical");
        if (isOneWayPlatform && moveY < -0.1f)
        {
            capsuleCollider.isTrigger = true;
            Invoke("RestorePlayerLayer", restoreTime);
        }
    }

    void RestorePlayerLayer()
    {
        capsuleCollider.isTrigger = false;
    }

    void CheckAirStatus()
    {
        isJumping = myAnim.GetBool("Jump");
        isFalling = myAnim.GetBool("Fall");
        isDoubleJumping = myAnim.GetBool("DoubleJump");
        isDoubleFalling = myAnim.GetBool("DoubleFall");
        isClimbing = myAnim.GetBool("Climbing");
        //Debug.Log("isJumping:" + isJumping);
        //Debug.Log("isFalling:" + isFalling);
        //Debug.Log("isDoubleJumping:" + isDoubleJumping);
        //Debug.Log("isDoubleFalling:" + isDoubleFalling);
        //Debug.Log("isClimbing:" + isClimbing);
    }
}
