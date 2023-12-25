using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cart : MonoBehaviour
{
    public float animationSpeed, animationSpeedMax;
    Animator animator;
    Player player;
    Spawner spawner;

    bool hasFuel, atStation, timerRunning;
    public GameObject fuelPool, sliderBg;

    float subtractTimer;
    public int completedLoops;

    public TextMeshProUGUI cartLapsText;
    float timerMax, currentTimer;

    Slider cartStopTimer;
    AudioSource aS;
    public AudioClip atStationAudio, cartRunningAudio, explodeAudio;

    void Start()
    {
        animationSpeed = 0.05f;
        animationSpeedMax = 2.5f;
        animator = GetComponent<Animator>();
        hasFuel = true;
        player = GameObject.Find("Player").GetComponent<Player>();
        subtractTimer = 0f;
        animator.speed = animationSpeed;
        completedLoops = 0;
        cartLapsText.text = completedLoops.ToString("00");
        timerMax = 5f;
        currentTimer = 0;
        cartStopTimer = GetComponentInChildren<Slider>();
        cartStopTimer.maxValue = timerMax;
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.currentHealth > 0)
        {
            if (!hasFuel || animator.speed == 0)
            {
                animator.speed = 0;
                subtractTimer += Time.deltaTime;
                currentTimer += Time.deltaTime;

                if(aS.isPlaying)
                {
                    aS.Stop();
                    aS.loop = false;
                }
                while (subtractTimer >= 1f)
                {
                    player.ChangeScore(-10, "Cart Stuck");
                    subtractTimer -= 1f;
                }

                if (currentTimer > timerMax)
                {
                    Explode();
                }

                if (!sliderBg.active)
                {
                    sliderBg.SetActive(true);
                }
            }

            if (animator.speed > 0)
            {
                currentTimer = 0;

                if (sliderBg.active)
                    sliderBg.SetActive(false);
                if(!aS.isPlaying)
                {
                    aS.loop = true;
                    aS.PlayOneShot(cartRunningAudio);
                }
            }

            if (atStation && !hasFuel && fuelPool.active)
            {
                hasFuel = true;
                fuelPool.SetActive(false);
                player.ChangeScore(200, "Lap Completed");
                subtractTimer = 0f;
                animator.speed = animationSpeed;
                currentTimer = 0f;
                atStation = false;
                if (spawner.ghostSpeed < spawner.ghostSpeedMax)
                    spawner.ghostSpeed += 0.1f;
            }

            cartStopTimer.value = currentTimer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Refuel")
        {
            hasFuel = false;
            atStation = true;
            completedLoops++;
            cartLapsText.text = completedLoops.ToString("00");
            animationSpeed += 0.002f;
            aS.PlayOneShot(atStationAudio);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ghost")
        {
            animator.speed = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ghost")
        {
            animator.speed = animationSpeed;
            currentTimer = 0f;
        }
    }

    void Explode()
    {
        aS.loop = false;
        aS.PlayOneShot(explodeAudio);
        player.ChangeScore(-10, "Cart Exploded");
        player.GameOver("Cart Exploded!");
        Destroy(gameObject);
    }

}
