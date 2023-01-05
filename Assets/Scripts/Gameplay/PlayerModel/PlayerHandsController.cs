using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
    [SerializeField] private Transform handHolder;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private ParticleSystem swordParticles;
    public Transform handBone;

    private PlayerStateMachine playerStateMachine;
    private PlayerInputManager playerInput;
   


    private Vector2 attackDirection = Vector2.up;
    private Direction currentDirection;
    private Vector2 smoothDirection = Vector2.zero;
    private enum Direction
    {
        Up,     //0
        Right,  //1
        Left,   //2
        Down    //3
    }

    private float blendSpeed = 5.5f;

    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerInput = playerStateMachine.PlayerInput;
        currentDirection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = playerInput.movementInput;
        AnimationDirection(moveDirection);
        GetAttackDirection(moveDirection);
    }

    private void AnimationDirection(Vector2 direction)
    {
        smoothDirection.x = Mathf.Lerp(smoothDirection.x, direction.x, blendSpeed * Time.deltaTime);
        smoothDirection.y = Mathf.Lerp(smoothDirection.y, direction.y, blendSpeed * Time.deltaTime);
        handAnimator.SetFloat("DirectionX", smoothDirection.x);
        handAnimator.SetFloat("DirectionZ", smoothDirection.y);
    }
    private void GetAttackDirection(Vector2 direction)
    {
        float x = direction.x;
        float y = direction.y;
        // UP
        if (x == 0 && y == 0)
        {
            return;
        }
        if (x > 0.5)
        {
            attackDirection = Vector2.right;
            currentDirection = Direction.Right;
        }
        else if (x < -0.5)
        {
            attackDirection = Vector2.left;
            currentDirection = Direction.Left;
        }
        else if (y > 0.85)
        {
            attackDirection = Vector2.up;
            currentDirection = Direction.Up;
        }
        else if (y < 0.85)
        {
            attackDirection = Vector2.down;
            currentDirection = Direction.Down;
        }
        
    }

    private void Bobbing()
    {

    }

    public void SwapWeapons()
    {
        playerInventory.SwapHeldWeapon();
    }
    public void PlayAttackAnim()
    {
        StartCoroutine(SwordTrails());
        switch (currentDirection)
        {
            case Direction.Up:
                handAnimator.SetTrigger("AttackUp");
                break;
            case Direction.Right:
                handAnimator.SetTrigger("AttackRight");
                break;
            case Direction.Left:
                handAnimator.SetTrigger("AttackLeft");
                break;
            case Direction.Down:
                handAnimator.SetTrigger("AttackDown");
                break;
        }
    }

    public IEnumerator SwordTrails()
    {
        if (swordParticles == null)
        {
            yield break;
        }
        yield return new WaitForSeconds(0.2f);
        swordParticles.Play();
        yield return new WaitForSeconds(0.12f);
        swordParticles.Stop();
    }


    public void SetCrouchAnimation(bool mode)
    {
        handAnimator.SetBool("isCrouching", mode);
    }

    public void SetSprintAnimation(bool mode)
    {
        handAnimator.SetBool("isSprinting", mode);
    }
 
    public void SetFallAnimation(bool mode)
    {
        handAnimator.SetBool("isFalling", mode);
    }

    public void PlayParryAnimation()
    {
        switch (currentDirection)
        {
            case Direction.Up:
                handAnimator.SetTrigger("ParryUp");
                break;
            case Direction.Right:
                handAnimator.SetTrigger("ParryRight");
                break;
            case Direction.Left:
                handAnimator.SetTrigger("ParryLeft");
                break;
            case Direction.Down:
                handAnimator.SetTrigger("ParryDown");
                break;
        }
    }

    public void SetBlockAnimation(bool mode)
    {
        handAnimator.SetBool("isBlocking", mode);
    }
}
