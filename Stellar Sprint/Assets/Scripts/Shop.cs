using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{    
    // Должен быть таким же как число уровня
    public string objectName;

    public int price;    
    public GameObject block;
    public Text objectPrice;
    private int fuels, hasAccess;

    private void Update()
    {
        fuels = PlayerPrefs.GetInt("fuels");
        objectPrice.text = (fuels + "/" + price).ToString();
    }

    private void Awake()
    {
        AccessUpdate();
    }

    void AccessUpdate()
    {
        hasAccess = PlayerPrefs.GetInt(objectName + "Access");

        if (hasAccess == 1)
        {
            block.gameObject.SetActive(false);
            objectPrice.gameObject.SetActive(false);
        }

    }

    public void Buy()
    {
        int fuels = PlayerPrefs.GetInt("fuels");

        if (hasAccess == 0)
        {
            if (fuels >= price)
            {
                PlayerPrefs.SetInt(objectName + "Access", 1);
                PlayerPrefs.SetInt("fuels", fuels - price);
                AccessUpdate();
            }
        }
        else
        {
            string levelNum = objectName.Remove(0,5);
            SceneManager.LoadScene(int.Parse(levelNum));
        }
    }
}
