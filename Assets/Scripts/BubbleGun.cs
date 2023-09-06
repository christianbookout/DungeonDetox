using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BubbleGun : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Slider ammoSlider;
    public float shootForce = 0.1f;
    public float maxOffset = 0.2f;
    public float bubbleLifetime = 1f;
    public float bubbleLifetimeVariability = 0.3f;
    public int maxAmmo = 20;
    public int ammo = 20;
    public float fireCooldown = 0.01f;
    private float nextFireTime = 0f;
    public float screenshake = 0.3f;
    public float height = 1f;
    private AudioSource bubbleFire;
    private PlayerController player;

    private void Start()
    {
        bubbleFire = GetComponent<AudioSource>();
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {

        if (IsInsideBathtub() && Input.GetMouseButton(0))
        {
            if (ammo < maxAmmo) ammo++;
        }
        else if (Input.GetMouseButtonDown(0) && ammo > 0)
        {
            FireBubble();
            bubbleFire.Play();
            CameraController.AddShake(screenshake);
            ammo--;
            nextFireTime = Time.time + fireCooldown;
        }
        if (Time.time > nextFireTime)
        {
            player.speed = PlayerController.MAX_SPEED;
        } else
        {
            player.speed = PlayerController.SHOOT_SPEED;
        }
        ammoSlider.value = ammo / (float)maxAmmo;
    }

    bool IsInsideBathtub()
    {
        int bathtubLayer = LayerMask.NameToLayer("Bathtub");

        // Use the gun's collider bounds for the OverlapBox
        Collider gunCollider = GetComponent<Collider>();

        Collider[] overlappingColliders = Physics.OverlapBox(gunCollider.bounds.center, gunCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Bathtub"));

        foreach (Collider col in overlappingColliders)
        {
            if (col.gameObject.layer == bathtubLayer)
            {
                return true;
            }
        }
        return false;
    }

    void FireBubble()
    {
        var pos = new Vector3(transform.position.x, height, transform.position.z);
        GameObject bubble = Instantiate(bubblePrefab, pos, Quaternion.identity);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000f);
        }

        Vector3 directionToTarget = (targetPoint - pos).normalized;

        Vector3 randomOffset = new(Random.Range(-maxOffset, maxOffset), 0, Random.Range(-maxOffset, maxOffset));
        Vector3 shootDirection = directionToTarget + randomOffset;
        shootDirection.y = 0;

        Rigidbody rb = bubble.GetComponent<Rigidbody>();
        rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);

        var lifetime = Random.Range(bubbleLifetime - bubbleLifetimeVariability, bubbleLifetime + bubbleLifetimeVariability);
        Destroy(bubble, lifetime);
    }
}
