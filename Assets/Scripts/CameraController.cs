using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float screenshake = 0.1f;
    private Vector3 origPos;
    public static float shake = 0f;
    public float decreaseFactor = 1.0f;

    public static void AddShake(float shakeAmount)
    {
        shake = shakeAmount;
    }

    private void Start()
    {
        origPos = Camera.main.transform.localPosition;
    }

    private void Update()
    {
        if (shake > 0)
        {
            Camera.main.transform.localPosition = origPos + Random.insideUnitSphere * shake;
            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shake = 0.0f;
        }
    }
}
