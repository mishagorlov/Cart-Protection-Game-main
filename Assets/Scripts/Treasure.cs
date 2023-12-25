using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    TreasureChest tC;
    public bool isBeingTargeted;

    private void Start()
    {
        isBeingTargeted = false;
        Debug.Log("Treasure reporting in!");
        tC = GameObject.Find("TreasureChest").GetComponent<TreasureChest>();
        tC.currentTreasuresInScene.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        tC.currentTreasuresInScene.Remove(this.gameObject);
    }
}
