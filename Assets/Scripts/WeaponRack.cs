using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRack : MonoBehaviour
{

    public int currentSpearCount;
    int maxSpearCount;

    public GameObject image1, image2, image3;

    private void Start()
    {
        maxSpearCount = 3;
        currentSpearCount = maxSpearCount;
    }


    public void ChangeSpearAmount(int amount)
    {
        currentSpearCount += amount;

        if (currentSpearCount > maxSpearCount)
            currentSpearCount = maxSpearCount;

        if (currentSpearCount < 0)
            currentSpearCount = 0;

        switch(currentSpearCount)
        {
            case 0:
                image1.SetActive(false);
                image2.SetActive(false);
                image3.SetActive(false);
                break;
            case 1:
                image1.SetActive(true);
                image2.SetActive(false);
                image3.SetActive(false);
                break;
            case 2:
                image1.SetActive(true);
                image2.SetActive(true);
                image3.SetActive(false);
                break;
            case 3:
                image1.SetActive(true);
                image2.SetActive(true);
                image3.SetActive(true);
                break;
        }
    }
}
