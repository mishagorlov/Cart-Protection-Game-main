using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{

    bool checkingMouse, thrown;
    public float speed = 10f;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        checkingMouse = true;
        thrown = false;
        player = GetComponentInParent<Player>();
        this.GetComponent<BoxCollider2D>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;                                             //get mouse position                                     
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);             //Get object position and put it "on the screen" (same as mouse)                
        Vector3 offset = new Vector3(mousePos.x - screenPos.x, mousePos.y - screenPos.y);   //Check where the mouse is relative to the object 

        float angle = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;                      //Turn that into an angle and convert to degrees
        if(checkingMouse)
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);

        if (Input.GetMouseButtonDown(0))
        {
            if (player.currentHealth > 0 && !player.paused)
            {
                Fire();
                checkingMouse = false;
                if(player.characterInUse.specialAbility == SpecialAbility.AutoReequip)
                {
                    player.ReequipSpear();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (thrown)
            transform.position += transform.TransformDirection(Vector3.up) * speed * Time.deltaTime;
    }

    void Fire()
    {
        player.hasSpear = false;
        gameObject.transform.SetParent(null);
        thrown = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
        Invoke("Despawn", 5f);
        player.aS.PlayOneShot(player.throwSpearAudio);
    }

    void Despawn()
    {
        Destroy(this.gameObject);
    }
}
