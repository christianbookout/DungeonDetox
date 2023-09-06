using UnityEngine;

public class Mop : MonoBehaviour
{
    public float maxHitAngle = 30f; // Adjust the maximum angle as needed.
    public float forceMagnitude = 10f; // Adjust the force magnitude in the Inspector.
    public float maxDistance = 5f; // Maximum distance for the second raycast.
    public LayerMask enemyLayer; // Assign the "enemy" layer in the Inspector.
    public float screenshake;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Get the mouse position in screen space.
            Vector3 mousePosition = Input.mousePosition;

            // Cast a ray from the camera to the mouse position.
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            // Calculate the angle between the camera's forward direction and the ray direction.
            float angleToMouse = Vector3.Angle(mainCamera.transform.forward, ray.direction);

            Debug.Log("Mouse Position (Screen Space): " + mousePosition);
            Debug.Log("Angle to Mouse: " + angleToMouse);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("Raycast hit something at: " + hit.point);

                // Calculate the intersection point between the ray from the camera and the ray to the hit point.
                Vector3 intersectionPoint = CalculateIntersectionPoint(ray.origin, ray.direction, hit.point, maxDistance);

                Debug.Log("Intersection Point: " + intersectionPoint);

                // Perform a second raycast from the player to the intersection point.
                RaycastHit secondHit;
                Vector3 playerPosition = transform.position;

                if (Physics.Raycast(playerPosition, intersectionPoint - playerPosition, out secondHit, maxDistance, enemyLayer))
                {
                    Debug.Log("Second raycast hit something at: " + secondHit.point);

                    // Check if the hit object in the second raycast is an enemy.
                    if (secondHit.collider.CompareTag("Enemy"))
                    {
                        // Apply force to the enemy in the direction of the second raycast with the adjustable force magnitude.
                        Rigidbody enemyRb = secondHit.collider.GetComponent<Rigidbody>();
                        if (enemyRb != null)
                        {
                            enemyRb.AddForce((intersectionPoint - playerPosition).normalized * forceMagnitude, ForceMode.Impulse);
                            Debug.Log("Applied Force to Enemy!");
                            CameraController.AddShake(screenshake);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
    }

    // Calculate the intersection point between two rays with a maximum distance for the second ray.
    private Vector3 CalculateIntersectionPoint(Vector3 origin1, Vector3 direction1, Vector3 origin2, float maxDistance)
    {
        // Calculate the direction of the second ray.
        Vector3 direction2 = origin2 - origin1;

        // Calculate the distance along the first ray where the two rays would intersect, clamped to the maximum distance.
        float t = Mathf.Clamp(Vector3.Dot(direction2, direction1) / Vector3.Dot(direction1, direction1), 0f, maxDistance);

        // Calculate the intersection point.
        Vector3 intersectionPoint = origin1 + t * direction1;

        return intersectionPoint;
    }
}
