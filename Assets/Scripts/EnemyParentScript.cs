using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentScript : MonoBehaviour
{
    public float baseMoveSpeed;
    public int pointValue;
    protected GameObject target;
    protected Player player;
    protected Spawner spawner;
    public GameObject grave;

    public virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }
    public virtual void Update()
    {
        if (player.currentHealth > 0)
        {
            if (target != null)
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, (baseMoveSpeed * spawner.ghostSpeed * Time.deltaTime));
            else
                target = null;
        }
    }

    public virtual void OnDestroy()
    {
        
        spawner.UpdateUI();
        player.ChangeScore(pointValue, "Ghost Killed");
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spear")
        {
            player.GetComponent<AudioSource>().PlayOneShot(player.ghostKilledAudio);
            player.ChangeScore(pointValue, "Enemy Killed");
            if(transform.childCount > 0)
            {
                Debug.Log("I have a treasure!!!!");
                Transform t = transform.GetChild(0);
                t.SetParent(null);
            }
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
