using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemyParentScript
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        target = GameObject.Find("Cart");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        player.totalGhostsKilled++;
        spawner.currentGhostsAlive--;
        spawner.remainingGhosts--;

        //float randomChance = Random.Range(0f, 10f);
        float randomChance = 6;
        if (randomChance > 5)
        {
            if (this.gameObject.scene.isLoaded)
                Instantiate(grave, this.transform.position, Quaternion.identity);
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
