using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberPickedItems : MonoBehaviour
{
    private int  wasPickedUp; 
    public string objectName;
    private void Update()
    {
        wasPickedUp = PlayerPrefs.GetInt(objectName + "wasPickedUp");

        if (wasPickedUp == 1)
        {
            Destroy(gameObject);
        }
    }
}
