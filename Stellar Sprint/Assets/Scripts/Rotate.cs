using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 2f;

    void Update()
    {
        transform.Rotate(0, 0, -360 * rotationSpeed * Time.deltaTime);
    }
}
