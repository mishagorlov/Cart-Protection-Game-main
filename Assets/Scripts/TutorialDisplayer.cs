using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialDisplayer : MonoBehaviour
{

    public TextMeshProUGUI tutorialTitle, tutorialText, currentPageUI;
    public Image tutorialImage;

    public GameObject[] tutorialImages;
    public string[] tutorialTitles;
    public string[] tutorialDescription;

    public GameObject leftArrow, rightArrow;
    int currentPage;
    // Start is called before the first frame update
    void Start()
    {
        currentPage = 0;
        tutorialTitle.text = tutorialTitles[0];
        tutorialText.text = tutorialDescription[0];
        ImageDisplayer(0);
        leftArrow.SetActive(false);
        PageNumber();
    }

    public void ChangePage(bool positive)
    {
        if (positive)
        {
            if(currentPage < (tutorialDescription.Length -1))
            {
                currentPage++;
                if (!leftArrow.active)
                    leftArrow.SetActive(true);
                if(currentPage == tutorialDescription.Length -1)
                {
                    rightArrow.SetActive(false);
                }
                else
                {
                    if (!rightArrow.active)
                        rightArrow.SetActive(true);
                }
            }
        }
        else
        {
            if(currentPage > 0)
            {
                currentPage--;
                if (!rightArrow.active)
                    rightArrow.SetActive(true);

                if(currentPage == 0)
                {
                    leftArrow.SetActive(false);
                }
                else
                {
                    if(!leftArrow.active)
                    {
                        leftArrow.SetActive(true);
                    }
                }
            }
        }

        DisplayPage(currentPage);
        ImageDisplayer(currentPage);
        PageNumber();
    }
    void PageNumber()
    {
        currentPageUI.text = (currentPage + 1).ToString() + "/" + tutorialDescription.Length.ToString();
    }

    void DisplayPage(int page)
    {
        tutorialText.text = tutorialDescription[page];

        //Pages 0 - 1: Movement
        if(currentPage >= 0 && currentPage < 2)
        {
            tutorialTitle.text = tutorialTitles[0];
        }
        //pages 2 - 5: Cart
        else if(currentPage >= 2 && currentPage < 6)
        {
            tutorialTitle.text = tutorialTitles[1];
        }
        //pages 6 - 7: Ghosts
        else if(currentPage >= 6 && currentPage < 8)
        {
            tutorialTitle.text = tutorialTitles[2];
        }
        //pages 8 - 9: Spears
        else if(currentPage >= 8 && currentPage < 10)
        {
            tutorialTitle.text = tutorialTitles[3];
        }
        //page 10: health
        else if(currentPage == 10)
        {
            tutorialTitle.text = tutorialTitles[4];
        }
        //page 11 - 12: graves
        else if(currentPage > 10 && currentPage < 13)
        {
            tutorialTitle.text = tutorialTitles[5];
        }
        //pages 13 - 14: treasure
        else if(currentPage >= 13 && currentPage < 15)
        {
            tutorialTitle.text = tutorialTitles[6];
        }
        //page 15: levels
        else if(currentPage == 15)
        {
            tutorialTitle.text = tutorialTitles[7];
        }
        //page 16 - 17: summary
        else if(currentPage > 15)
        {
            tutorialTitle.text = tutorialTitles[8];
        }
    }

    void ImageDisplayer(int number)
    {
        for (int i = 0; i < tutorialImages.Length - 1; i++)
        {
            if(i == number)
            {
                tutorialImages[i].SetActive(true);
            }
            else
            {
                tutorialImages[i].SetActive(false);
            }
        }
    }
}
