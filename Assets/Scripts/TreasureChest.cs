using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    SpriteRenderer sR;
    public Sprite treasureOpen, treasureFull, treasureClosed;
    public bool isEmpty;
    public float coolDown;
    Player player;
    AudioSource aS;
    public AudioClip treasureOpenAudio, treasureCloseAudio;

    public List<GameObject> currentTreasuresInScene = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        sR.sprite = treasureOpen;
        isEmpty = true;
        coolDown = 2f;
        player = GameObject.Find("Player").GetComponent<Player>();
        aS = GetComponent<AudioSource>();
        currentTreasuresInScene.Clear();
    }


    public void SetSprite(string type)
    {
        switch(type)
        {
            case "open":
                break;
            case "close":
                break;
            case "full":
                sR.sprite = treasureFull;
                isEmpty = false;
                Invoke("CloseChest", 1f);
                aS.PlayOneShot(treasureCloseAudio);
                break;
        }
    }

    void CloseChest()
    {
        sR.sprite = treasureClosed;
        Invoke("OpenChest", coolDown);
    }

    void OpenChest()
    {
        player.RandomUpgrade();
        player.ChangeScore(100, "Treasure Obtained");
        sR.sprite = treasureOpen;
        isEmpty = true;
        aS.PlayOneShot(treasureOpenAudio);
    }
}
