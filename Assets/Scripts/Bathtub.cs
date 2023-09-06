using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bathtub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Cleaned!");
            Destroy(other.gameObject);
     
        }
    }
}
