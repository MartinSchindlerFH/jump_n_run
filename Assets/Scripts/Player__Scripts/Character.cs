using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Character : MonoBehaviour
{
    private bool isJumping = false;
    private float jumpCooldownTimer;
    private CharacterController controller;
    private InputAction moveAction;
    private InputAction jumpAction;
    [SerializeField]
    private float jumpCooldown;
    //We set gravity lower than in real live as it is more fun!
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float characterSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float dampening;
    [SerializeField]
    private Transform cameraTransform;
    private Vector3 characterMovement;
    private Vector3 jumpVelocity;
    private Vector3 characterGravity;

    [SerializeField] private AudioClip WalkingSound;
    [SerializeField] private AudioClip JumpingSound;
    private AudioSource audioSorce;

    private Animator animator;
    void Start()
    {
        this.controller = this.GetComponent<CharacterController>();
        this.moveAction = InputSystem.actions.FindAction("Move");
        this.jumpAction = InputSystem.actions.FindAction("Jump");
        this.jumpCooldownTimer = 0.0f;

        animator = GetComponent<Animator>();
    }

    private void Awake()    // Scene Laden
    {
        audioSorce = GetComponent<AudioSource>();
        audioSorce.loop = true;
        audioSorce.playOnAwake = false;
    }
    void HandleJumping()
    {
        if (this.controller.isGrounded && this.isJumping && this.jumpCooldownTimer <= 0.0f)
        {
            this.jumpVelocity = Vector3.zero;
            this.isJumping = false;
        }
        if (this.controller.isGrounded && !this.isJumping && this.jumpAction.WasPressedThisFrame())
        {
            this.characterGravity = Vector3.zero;
            this.jumpVelocity = Vector3.zero;
            this.jumpVelocity.y = this.jumpSpeed;
            this.jumpCooldownTimer = this.jumpCooldown;
            this.isJumping = true;
        }
        if (this.jumpVelocity.y > 0.0f)
        {
            this.jumpVelocity.y -= Time.fixedDeltaTime;
        }
        else
        {
            this.jumpVelocity = Vector3.zero;
        }
        this.jumpCooldownTimer -= Time.fixedDeltaTime;
    }
    void FixedUpdate()
    {
        this.HandleJumping();
        var inputMovement = this.moveAction.ReadValue<Vector2>();
        var inputRightDirection = this.cameraTransform.right;
        var inputForwardDirection = this.cameraTransform.forward;
        inputRightDirection.y = 0.0f;
        inputForwardDirection.y = 0.0f;
        inputRightDirection.Normalize();
        inputForwardDirection.Normalize();
        //Since we do not use the physics system, we have to simulate gravity ourselves
        if (this.controller.isGrounded)
        {
            this.characterGravity.y = 0.0f;
        }
        this.characterGravity.y += this.gravity * Time.fixedDeltaTime;
        this.characterMovement += this.characterGravity * Time.fixedDeltaTime;
        this.characterMovement += this.jumpVelocity * Time.fixedDeltaTime;
        this.characterMovement += inputRightDirection * inputMovement.x * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement += inputForwardDirection * inputMovement.y * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement *= (1 - this.dampening);
        Vector3 characterForward = this.characterMovement;
        characterForward.y = 0.0f;
        if (characterForward.sqrMagnitude > 0.0f && characterForward != Vector3.zero)
        {
            this.transform.forward = characterForward.normalized;
        }

        var platformVelocity = new Vector3(0, 0, 0);
        if (controller.isGrounded) platformVelocity = GetPlatformVelocity(controller.transform.position, characterForward) ;
        var combinedMovement = this.characterMovement + platformVelocity * Time.fixedDeltaTime;
        this.controller.Move(combinedMovement);
        this.controller.Move(this.characterMovement);

        this.SetAnimationState(inputMovement);
    }

    private Vector3 GetPlatformVelocity(Vector3 position, Vector3 movement)
    {
        
        if (Physics.Raycast(position, Vector3.down * 2f, out RaycastHit hitinfo, 10, 10))
        {
            Debug.DrawRay(position, Vector3.down * hitinfo.distance, Color.yellow);
            Debug.Log("Did Hit");
            Debug.Log(hitinfo.collider.gameObject.GetComponent<MovingPlatform>().GetVelocity());
            return hitinfo.collider.gameObject.GetComponent<MovingPlatform>().GetVelocity();
        }
        Debug.DrawRay(position, Vector3.down, Color.white);
        Debug.Log("No Hit");
        return new Vector3(0, 0, 0);
    }

    void SetAnimationState(Vector2 inputMovement)
    {
        if (this.isJumping)
        {
            audioSorce.clip = JumpingSound;
            audioSorce.Play();
        }
        else { 
            if(audioSorce.clip == JumpingSound)
            {
                audioSorce.Stop();
            }
        }

        if(inputMovement != Vector2.zero)
        {
            audioSorce.clip = WalkingSound;
            audioSorce.Play();
        }
        else
        {
            if(inputMovement == Vector2.zero)
            {
                audioSorce.Stop();
            }
        }
        
        this.animator.SetBool("IsJumping", this.isJumping);
        this.animator.SetBool("IsRunning", inputMovement != Vector2.zero);
        this.animator.SetFloat("MovementForward", inputMovement.magnitude);

    }
}