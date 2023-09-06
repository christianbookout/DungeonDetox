using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    public float speed = 10f;
    public static readonly float MAX_SPEED = 12.5f;
    public static readonly float SHOOT_SPEED = 7.5f;
    public float sensitivity = 100f;
    public Sprite southWest;
    public Sprite west;
    public Sprite south;
    public Sprite northWest;
    public Sprite north;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool walking;

    void Start()
    {
        speed = MAX_SPEED;
        characterController = GetComponent<CharacterController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (transform.position.y > 1.1)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, 1f, transform.position.z);
            Vector3 displacement = targetPosition - characterController.transform.position;

            // Ensure we're not moving the character vertically due to gravity or other forces
            // If you're handling gravity manually, you may want to add that displacement as well.
            // Here, we just want to move to the target position.
            displacement.y = 1f - characterController.transform.position.y;

            // Use the Move method
            characterController.Move(displacement);
        }
        Move();
        FixSprite();
    }

    void FixSprite()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        Vector3 directionToMouse = (mouseWorldPosition - transform.position).normalized;

        Vector3 playerForwardHorizontal = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 mouseDirectionHorizontal = new Vector3(directionToMouse.x, 0f, directionToMouse.z).normalized;

        float dot = playerForwardHorizontal.x * mouseDirectionHorizontal.x + playerForwardHorizontal.z * mouseDirectionHorizontal.z;
        float det = playerForwardHorizontal.x * mouseDirectionHorizontal.z - playerForwardHorizontal.z * mouseDirectionHorizontal.x;
        float angle = Mathf.Atan2(det, dot) * Mathf.Rad2Deg;

        spriteRenderer.flipX = false;
        animator.SetLayerWeight(0, 1);
        float walkingNum = 0f;

        // warning: absolutely terrible spaghetti code below
        if (walking)
        {
            walkingNum = 1f;
            animator.enabled = true;
            animator.SetLayerWeight(0, 0);
        } else
        {
            animator.enabled = false;
        }
        if (angle >= -22.5f && angle < 22.5f)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
            animator.SetLayerWeight(3, 0);
            animator.SetLayerWeight(4, 0);
            animator.SetLayerWeight(5, walkingNum);
            spriteRenderer.sprite = north;
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
            animator.SetLayerWeight(3, 0);
            animator.SetLayerWeight(4, walkingNum);
            animator.SetLayerWeight(5, 0);
            spriteRenderer.sprite = northWest;
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, walkingNum);
            animator.SetLayerWeight(3, 0);
            animator.SetLayerWeight(4, 0);
            animator.SetLayerWeight(5, 0);
            spriteRenderer.sprite = west;
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            animator.SetLayerWeight(1, walkingNum);
            animator.SetLayerWeight(2, 0);
            animator.SetLayerWeight(3, 0);
            animator.SetLayerWeight(4, 0);
            animator.SetLayerWeight(5, 0);
            spriteRenderer.sprite = southWest;
        }
        else if (angle >= 157.5f || angle < -157.5f)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
            animator.SetLayerWeight(3, walkingNum);
            animator.SetLayerWeight(4, 0);
            animator.SetLayerWeight(5, 0);
            spriteRenderer.sprite = south;
        }
        else if (angle >= -157.5f && angle < -112.5f)
        {
            animator.SetLayerWeight(1, walkingNum);
            animator.SetLayerWeight(2, 0);
            animator.SetLayerWeight(3, 0);
            animator.SetLayerWeight(4, 0);
            animator.SetLayerWeight(5, 0);
            spriteRenderer.sprite = southWest;
            spriteRenderer.flipX = true;
        }
        else if (angle >= -112.5f && angle < -67.5f)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, walkingNum);
            animator.SetLayerWeight(3, 0);
            animator.SetLayerWeight(4, 0);
            animator.SetLayerWeight(5, 0);
            spriteRenderer.sprite = west;
            spriteRenderer.flipX = true;
        }
        else if (angle >= -67.5f && angle < -22.5f)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
            animator.SetLayerWeight(3, 0);
            animator.SetLayerWeight(4, walkingNum);
            animator.SetLayerWeight(5, 0);
            spriteRenderer.sprite = northWest;
            spriteRenderer.flipX = true;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(ray, out float hitDistance))
        {
            return ray.GetPoint(hitDistance);
        }

        return Vector3.zero;
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = (transform.forward * moveVertical) + (transform.right * moveHorizontal);
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        walking = movement.magnitude > 0;

        characterController.Move(speed * Time.deltaTime * movement);
    }
}
