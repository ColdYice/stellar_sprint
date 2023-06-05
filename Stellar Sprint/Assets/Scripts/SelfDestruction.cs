using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    private float destroyEnemySeconds = 5f;
    private float destroyOtherSeconds = 0.5f;

    void Start()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(SelfDestruct(destroyEnemySeconds));
        }
        else
        {
            StartCoroutine(SelfDestruct(destroyOtherSeconds));
        }
    }
    IEnumerator SelfDestruct(float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Destroy(gameObject);
    }
}
