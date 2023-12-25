using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundScroller : MonoBehaviour
{
    public bool isScrollingDown;
    public float speed;
    Vector2 originalPosition;
    RectTransform rT;

    private void Start()
    {
        speed = 10f;
        rT = GetComponent<RectTransform>();
        originalPosition = rT.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(isScrollingDown)
        {
            rT.anchoredPosition -= new Vector2(0f, -5f) * speed * Time.deltaTime;
            if(rT.anchoredPosition.y > new Vector2(0f, 540f).y)
            {
                rT.anchoredPosition = originalPosition;
            }
        }
        else if(!isScrollingDown)
        {
            rT.anchoredPosition -= new Vector2(0f, 5f) * speed * Time.deltaTime;
            if (rT.anchoredPosition.y < new Vector2(0f, -590f).y)
            {
                rT.anchoredPosition = originalPosition;
            }
        }
    }
}
