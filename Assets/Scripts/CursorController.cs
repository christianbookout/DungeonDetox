using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private Camera mainCamera;
    public float height;

    private void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
    }

    private void Update()
    {
        RaycastAndSetPosition();
    }

    private void RaycastAndSetPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(hit.point.x, height, hit.point.z);
        }
    }
}
