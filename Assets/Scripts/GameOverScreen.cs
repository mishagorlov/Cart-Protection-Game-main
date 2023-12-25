using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    //Ghosts Defeated
    //Laps Completed
    //Treasure Collected

    public GameObject ghostsInfo, lapsInfo, treasureInfo, buttons;
    public AudioClip ghostsClip, lapsClip, treasureClip;
    AudioSource aS;


    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log("Game over active!");
        aS = GetComponent<AudioSource>();
        ghostsInfo.SetActive(false);
        lapsInfo.SetActive(false);
        treasureInfo.SetActive(false);
        buttons.SetActive(false);
        StartCoroutine(DisplayScores());
    }

    void ShowGhostInfo()
    {
        Debug.Log("Showing ghost info!");
        ghostsInfo.SetActive(true);
       // aS.PlayOneShot(ghostsClip);
    }

    void ShowLapInfo()
    {
        lapsInfo.SetActive(true);
     //  aS.PlayOneShot(lapsClip);
    }

    void ShowTreasureInfo()
    {
        treasureInfo.SetActive(true);
    //   aS.PlayOneShot(treasureClip);
    }

    void DisplayButtons()
    {
        buttons.SetActive(true);
    }

    IEnumerator DisplayScores()
    {
        Debug.Log("Coroutine Started");
        yield return new WaitForSeconds(1f);

        ShowGhostInfo();

        yield return new WaitForSeconds(1f);

        ShowLapInfo();

        yield return new WaitForSeconds(1f);

        ShowTreasureInfo();

        yield return new WaitForSeconds(1f);

        DisplayButtons();
    }
}
