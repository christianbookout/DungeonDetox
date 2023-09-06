using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float wanderSpeed = 3.0f; // Speed at which the enemy moves.
    public float wanderTime = 5.0f; // Time the enemy spends wandering before moving towards a target.
    public float changeDirectionInterval = 2.0f; // Time interval to change the wandering direction.
    public string targetObjectName = "Target"; // The specific name of target objects in the scene.
    public int maxHealth = 10;
    public int health;
    public GameObject bubbleVisual;

    private Transform currentTarget = null;
    private float wanderTimer;
    private float directionChangeTimer;
    private Vector3 wanderDirection;
    private List<Transform> targetObjects = new List<Transform>(); // List of target objects to choose from.
    private bool isMoving = true;
    private Animator animator;

    void Update()
    {
        if (!isMoving) return;
        if (health <= 0)
        {
            isMoving = false;
            StartCoroutine(FloatToTub());
            return;
        }
        if (currentTarget == null)
        {
            // Wander around if there's no current target.
            Wander();
        }
        else
        {
            // Move towards the current target.
            MoveTowardsTarget();
        }
    }

    private void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();

        // Initialize the timers and direction.
        wanderTimer = wanderTime;
        directionChangeTimer = 0f;
        ChooseRandomWanderDirection();

        // Populate the targetObjects list with objects in the scene that match the specific name.
        PopulateTargetObjectsList();
    }

    IEnumerator FloatToTub()
    {
        var collider = GetComponent<Collider>();
        var rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        collider.enabled = false;
        var bubVis = Instantiate(bubbleVisual);
        bubVis.transform.position = transform.position;
        bubVis.transform.parent = transform;
        float floatSpeed = 4f;
        float duration = 1f;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float moveSpeed = 10f;
        GameObject bathtub = GameObject.FindGameObjectWithTag("Bathtub");

        var transPos = new Vector3(transform.position.x, 0, transform.position.z);
        var bathPos = new Vector3(bathtub.transform.position.x, 0, bathtub.transform.position.z);

        do
        {
            Vector3 moveDir = (bathPos - transPos).normalized;
            moveDir.y = 0;
            float distanceToMoveThisFrame = moveSpeed * Time.deltaTime;
            float actualDistanceToTarget = Vector3.Distance(transPos, bathPos);

            if (distanceToMoveThisFrame > actualDistanceToTarget)
            {
                transform.position = new Vector3(bathPos.x, transform.position.y, bathPos.z);
            }
            else
            {
                transform.Translate(moveDir * distanceToMoveThisFrame);
            }
            transPos = new Vector3(transform.position.x, 0, transform.position.z);
            yield return null;
        } while (Vector3.Distance(transPos, bathPos) > 1f);

        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        collider.enabled = true;
        collider.isTrigger = true;
        Destroy(bubVis);
    }


    void Wander()
    {
        wanderTimer -= Time.deltaTime;
        directionChangeTimer -= Time.deltaTime;

        if (wanderTimer <= 0)
        {
            // Time to select the closest target.
            currentTarget = FindClosestTarget();
        }
        else
        {
            // Continue wandering.
            if (directionChangeTimer <= 0)
            {
                ChooseRandomWanderDirection();
                directionChangeTimer = changeDirectionInterval;
            }

            var dir = wanderDirection.normalized;
            if (dir.z <= 0)
            {
                animator.SetLayerWeight(0, 0);
                animator.SetLayerWeight(1, 1);
                animator.SetLayerWeight(2, 0);
            } else
            {
                animator.SetLayerWeight(0, 0);
                animator.SetLayerWeight(1, 0);
                animator.SetLayerWeight(2, 1);
            }
            Vector3 movement = Time.deltaTime * wanderSpeed * dir;
            transform.Translate(movement);
        }
    }

    void MoveTowardsTarget()
    {
        if (currentTarget != null)
        {
            // Move towards the current target.
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            transform.Translate(direction * wanderSpeed * Time.deltaTime);
        }
    }

    void PopulateTargetObjectsList()
    {
        // Find all GameObjects with the specific name in the scene and add them to the targetObjects list.
        GameObject[] objectsWithSpecificName = GameObject.FindGameObjectsWithTag(targetObjectName);

        targetObjects.Clear();

        foreach (GameObject obj in objectsWithSpecificName)
        {
            targetObjects.Add(obj.transform);
        }
    }

    Transform FindClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform target in targetObjects)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget; // Return the closest target found.
    }

    void ChooseRandomWanderDirection()
    {
        // Choose a random direction on the xz plane.
        wanderDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }
}
