using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gremlin : EnemyParentScript
{
    TreasureChest tC;
    Treasure treasureTarget;
    bool hadFirstTarget, escaping;

    // Start is called before the first frame update
    public override void Start()
    {
        hadFirstTarget = false;
        base.Start();
        tC = GameObject.Find("TreasureChest").GetComponent<TreasureChest>();
        GetClosestTreasure(tC.currentTreasuresInScene);
        escaping = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(target == null && hadFirstTarget)
        {
            if(tC.currentTreasuresInScene.Count > 0)
            {
                GetClosestTreasure(tC.currentTreasuresInScene);
            }
            else
            {
                target = GameObject.Find("Exit");
            }    
        }
    }

    GameObject GetClosestTreasure(List<GameObject> treasures)
    {
        Debug.Log("running check!");
        target = null;
        float closestDistanceSqe = Mathf.Infinity;
        Vector3 curPos = this.transform.position;
        for (int i = 0; i < treasures.Count; i++)
        {
            Vector3 directionToTarget = treasures[i].transform.position - curPos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqe && !treasures[i].GetComponent<Treasure>().isBeingTargeted)
            {
                closestDistanceSqe = dSqrToTarget;
                target = treasures[i].gameObject;
            }

        }
        hadFirstTarget = true;
        treasureTarget = target.GetComponent<Treasure>();
        treasureTarget.isBeingTargeted = true;
        return target;
    }

    public override void OnDestroy()
    {
        treasureTarget.isBeingTargeted = false;
        base.OnDestroy();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        base.OnTriggerEnter2D(collision);

        if(collision.gameObject.tag == "Treasure" && collision.gameObject == target)
        {
            collision.transform.SetParent(this.gameObject.transform);
            Debug.Log("Picked up Treasure!");
            collision.transform.position = this.transform.position;
            escaping = true;
            baseMoveSpeed = 2;
            target = GameObject.Find("Exit");//Start escape
        }
        if(collision.gameObject.name == "Exit" && escaping)
        {
            Debug.Log("I have reached the exit!");
            player.ChangeScore(-100, "Crab escaped with treasure!");
            Destroy(this.gameObject);
        }
    }
}
