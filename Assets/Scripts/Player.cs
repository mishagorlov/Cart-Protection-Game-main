using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;

    float _Horizontal, _Vertical, _MoveLimiter, maxTilt;
    Rigidbody2D rb2d;
    
    public Slider gatheringSlider;
    float gatheringTimeMax, currentGatheringTime;

    public bool hasSpear, isHoldingTreasure;
    public GameObject spearPrefab, waterFountain, waterPool;
    WeaponRack weaponRack;
    int currentTask;
    GameObject currentGrave, cart;
    public int maxHealth, currentHealth, highScore;
    bool canTakeDamage;

    int currentScore, treasureChance;
    public TextMeshProUGUI scoreText, upgradeText, finalTreasureText, finalGhostsText, finalLapsText, finalScoreText, finalDeathReason, highScoreText, ghostsHigh, lapsHigh, treasureHigh, scoreLogText;
    public GameObject image1, image2, image3, treasure, treasurePrefab, gameOverPanel;

    float upgradeTextTimerMax, upgradeTextTimer;
    Spawner spawner;
    public int totalTreasureFound, totalGhostsKilled, ghostsHighScore, lapsHighScore, treasureHighScore;

    string scoreLog;
    public SpriteRenderer character;
    public AudioSource aS;
    public AudioClip throwSpearAudio, equipSpearAudio, pickupTreasureAudio, waterTapAudio, ghostKilledAudio, healthUpAudio, damageAudio;

    float coolDownAbilityTimer;
    bool canAbilityBeUsed;

    bool dodging;
    float dodgeSpeedMultiplier;

    public Character characterInUse;

    public bool paused;

    Animator characterTiltAnimator;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        characterInUse = CharacterSelector.instance.characterClasses[CharacterSelector.instance.currentCharacterSelected];
    }

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        aS = GetComponent<AudioSource>();
        character.sprite = characterInUse.characterSprite;
        speed = 8f;
        gatheringTimeMax = 1f;
        currentTask = 0;
        _MoveLimiter = 0.7f;
        rb2d = GetComponent<Rigidbody2D>();
        hasSpear = true;
        weaponRack = GameObject.Find("WeaponRack").GetComponent<WeaponRack>();
        gatheringSlider.maxValue = gatheringTimeMax;
        currentGrave = null;
        isHoldingTreasure = false;
        maxHealth = 3;
        ChangeHealth(maxHealth);
        Time.timeScale = 1;
        currentScore = 0;
        treasure.SetActive(false);
        cart = GameObject.Find("Cart");
        treasureChance = 6;
        upgradeTextTimerMax = 2f;
        upgradeTextTimer = 0f;
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        totalTreasureFound = 0;
        Cursor.visible = false;
        coolDownAbilityTimer = 0f;
        canAbilityBeUsed = true;
        dodgeSpeedMultiplier = 1;
        canTakeDamage = true;
        characterTiltAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _Horizontal = Input.GetAxisRaw("Horizontal");
        _Vertical = Input.GetAxisRaw("Vertical");

        if (currentTask > 0)
        {
            if (currentGatheringTime < gatheringTimeMax)
            {
                currentGatheringTime += Time.deltaTime * characterInUse.craftSpeedModifier;
            }


            if (currentGatheringTime >= gatheringTimeMax)
            {
                CompleteTask(currentTask);
                currentGatheringTime = 0;
            }
        }

        gatheringSlider.value = currentGatheringTime;

        if(currentHealth <= 0)
        {
            GameOver("Killed by ghosts!");
        }
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (upgradeTextTimer > 0)
            upgradeTextTimer -= Time.deltaTime;

        if (upgradeTextTimer <= 0)
            upgradeText.text = "";

        if(Input.GetKeyDown(KeyCode.LeftShift) && canAbilityBeUsed)
        {
            UseAbility();
        }

        if(!canAbilityBeUsed)
        {
            coolDownAbilityTimer += Time.deltaTime;
            if(coolDownAbilityTimer >= characterInUse.specialAbilityCooldown)
            {
                canAbilityBeUsed = true;
                coolDownAbilityTimer = 0;
                dodging = false;
                Debug.Log("Ability Ready!");
            }
        }
        if(dodging)
        {
            dodgeSpeedMultiplier = 2f;
            canTakeDamage = false;
        }
        else
        {
            dodgeSpeedMultiplier = 1f;
            canTakeDamage = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Pause(false);
                paused = false;
            }
            else if (!paused)
            {
                Pause(true);
                paused = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_Horizontal != 0 && _Vertical != 0)
        {
            _Horizontal *= _MoveLimiter;
            _Vertical *= _MoveLimiter;
        }
        if(_Horizontal > 0)
        {
            characterTiltAnimator.SetTrigger("isTiltingRight");
        }
        else if(_Horizontal < 0) 
        {
            characterTiltAnimator.SetTrigger("isTiltingLeft");
        }
        else if(_Horizontal == 0)
        {
            characterTiltAnimator.SetTrigger("isIdle");
        }
        rb2d.velocity = new Vector2(_Horizontal * speed, _Vertical * speed * dodgeSpeedMultiplier) * CharacterSelector.instance.characterClasses[CharacterSelector.instance.currentCharacterSelected].speed;
    }

    void Pause(bool pausing)
    {
        if (pausing)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            Time.timeScale = 0f;
            cart.GetComponent<AudioSource>().Stop();
            Cursor.visible = true;
        }
        else
        {
            SceneManager.UnloadSceneAsync(2);
            if(SceneManager.GetSceneByName("OptionsMenu").isLoaded)
                SceneManager.UnloadSceneAsync(3);
            Time.timeScale = 1f;
            cart.GetComponent<AudioSource>().Play();
            Cursor.visible = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "WeaponRack" && !hasSpear)
        {
            ReequipSpear();
        }

        if (collision.gameObject.name == "Anvil")
        {
            if (weaponRack.currentSpearCount < 3)
            {
                EnableSlider();
                currentTask = 1;
                collision.gameObject.GetComponent<AudioSource>().Play();
            }
        }

        if (collision.gameObject.name == "AddWater")
        {
            if (!waterPool.active)
            {
                EnableSlider();
                currentTask = 2;
                aS.loop = true;
                aS.PlayOneShot(waterTapAudio);
            }
        }

        if(collision.gameObject.name == "TreasureChest")
        {
            if(isHoldingTreasure && collision.gameObject.GetComponent<TreasureChest>().isEmpty)
            {
                isHoldingTreasure = false;
                collision.gameObject.GetComponent<TreasureChest>().SetSprite("full");
                treasure.SetActive(false);
                totalTreasureFound++;
            }
        }

        if (collision.gameObject.tag == "Treasure")
        {
            if (!isHoldingTreasure)
            {
                isHoldingTreasure = true;
                Destroy(collision.gameObject);
                treasure.SetActive(true);
                aS.PlayOneShot(pickupTreasureAudio);
            }
        }
    }

    public void RandomUpgrade()
    {
        int upgrade = Random.Range(0, 5);

        switch(upgrade)
        {
            case 0:
                speed += 0.2f;
                BottomText("Faster Movement!");
                //Movement Speed
                break;
            case 1:
                gatheringTimeMax -= 0.1f;
                BottomText("Faster Crafting!");
                gatheringSlider.maxValue = gatheringTimeMax;
                //Crafting Speed
                break;
            case 2:
                spawner.spawnDelayMin += 0.2f;
                spawner.spawnDelayMax += 0.2f;
                BottomText("Less Ghosts!");
                //Ghost spawn delay
                break;
            case 3:
                cart.GetComponent<Cart>().animationSpeed -= 0.005f;
                BottomText("Slower Cart!");
                //Slow down cart
                break;
            case 4:
                if(treasureChance > 4)
                {
                    treasureChance--;
                    BottomText("More Treasure!");
                }
                else if (treasureChance <= 4)
                {
                    BottomText("Max Treasure Rank!");
                    ChangeScore(25, "Max Treasure Found");
                }
                //Treasure is more common
                break;
        }
    }

    public void ChangeScore(int amount, string reason)
    {
        float scoreWithMultiplier = amount * characterInUse.scoreModifier;
        int scoreToAdd = Mathf.RoundToInt(scoreWithMultiplier);
        currentScore += scoreToAdd;
        scoreText.text = currentScore.ToString("0000");
        scoreLog = scoreLog+ reason + ": " + amount + "\n";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ghost" && !dodging)
        {
            Debug.Log("Lose health");
            ChangeHealth(-1);
            Destroy(collision.gameObject);
            ChangeScore(-20, "Attacked by Ghost");
            aS.PlayOneShot(damageAudio);
        }
    }
    void EnableSlider()
    {
        gatheringSlider.gameObject.SetActive(true);
    }

    void CompleteTask(int taskType)
    {
        switch(taskType)
        {
            case 0:
                break;
            case 1:
                if (characterInUse.specialAbility != SpecialAbility.AutoReequip || characterInUse.specialAbility == SpecialAbility.AutoReequip && hasSpear)
                {
                    weaponRack.ChangeSpearAmount(1);

                }
                else if(characterInUse.specialAbility == SpecialAbility.AutoReequip && !hasSpear) 
                {
                    EquipSpear();
                }
                break;
            case 2:
                AddWater();
                break;
            case 3:
                GraveRobbed();
                break;
        }
        currentTask = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Anvil")
        {
            ResetTasks();
            collision.gameObject.GetComponent<AudioSource>().Stop();
        }
        if(collision.gameObject.name == "AddWater")
        {
            ResetTasks();
            aS.loop = false;
            aS.Stop();
        }
        if(collision.gameObject.tag == "Grave")
        {
            ResetTasks();
            collision.gameObject.GetComponent<AudioSource>().Stop();
            currentGrave = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Anvil")
        {
            if (weaponRack.currentSpearCount < 3)
            {
                gatheringSlider.gameObject.SetActive(true);
                currentTask = 1;
            }
        }

        if (collision.gameObject.tag == "Grave")
        {
            EnableSlider();
            currentTask = 3;
            currentGrave = collision.gameObject;
            if(!collision.gameObject.GetComponent<AudioSource>().isPlaying)
                collision.gameObject.GetComponent<AudioSource>().Play();
        }

        if (collision.gameObject.name == "WeaponRack")
            {
                ReequipSpear();
            }
        if (collision.gameObject.tag == "Grave")
        {
            currentGrave = collision.gameObject;
        }
    }

    public void ReequipSpear()
    {
        if (weaponRack.currentSpearCount > 0 && !hasSpear)
        {
            weaponRack.ChangeSpearAmount(-1);
            EquipSpear();
        }
    }

    void EquipSpear()
    {
        hasSpear = true;
        Vector3 spawnPosition = gameObject.transform.position;
        spawnPosition.x += 0.4f;
        Instantiate(spearPrefab, spawnPosition, Quaternion.identity, this.gameObject.transform);
    }

    void AddWater()
    {
        if (!waterFountain.active && !waterPool.active)
        {
            waterFountain.gameObject.SetActive(true);
            StartCoroutine(WaterFountain());
        }
    }

    void ResetTasks()
    {
        currentTask = 0;
        gatheringSlider.gameObject.SetActive(false);
    }

    IEnumerator WaterFountain()
    {
        yield return new WaitForSeconds(1f);
        waterPool.SetActive(true);
        waterFountain.SetActive(false);
    }

    void GraveRobbed()
    {
        if (currentGrave != null)
        {
            Destroy(currentGrave);
        }

        currentGrave = null;

        // int reward = Random.Range(0, 11);
        //DELETE BELOW TEST LINE
        int reward = treasureChance + 1;

        if (reward >= 0 && reward <= 1)
        {
            if (currentHealth < maxHealth)
            {
                BottomText("Health Up!");
                ChangeHealth(1);
            }
            else if(currentHealth == maxHealth)
            {
                ChangeScore(25, "Max Health Score Up");
                BottomText("Score Up!");
            }
        }
        else if (reward > 1 && reward < treasureChance)
        {
            BottomText("Nothing Found!");
        }
        else if (reward > treasureChance)
        {
            aS.PlayOneShot(pickupTreasureAudio);
            if (!isHoldingTreasure)
            {
                BottomText("Treasure Found!");
                treasure.SetActive(true);
                isHoldingTreasure = true;
            }
            else
            {
                Instantiate(treasurePrefab, this.transform.position, Quaternion.identity);
                BottomText("Treasure Dropped!");
            }
        }
    }

    public void BottomText(string text)
    {
        upgradeText.text = text;
        upgradeTextTimer = upgradeTextTimerMax;
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0 && !canTakeDamage)
        {
            return;
        }
        currentHealth += amount;

        switch (currentHealth)
        {
            case 0:
                image1.SetActive(false);
                image2.SetActive(false);
                image3.SetActive(false);
                break;
            case 1:
                image1.SetActive(true);
                image2.SetActive(false);
                image3.SetActive(false);
                break;
            case 2:
                image1.SetActive(true);
                image2.SetActive(true);
                image3.SetActive(false);
                break;
            case 3:
                image1.SetActive(true);
                image2.SetActive(true);
                image3.SetActive(true);
                break;
        }
        if (amount > 0)
            aS.PlayOneShot(healthUpAudio);
    }

    public void GameOver(string reason)
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        // Time.timeScale = 0;
        finalGhostsText.text = "Defeated: " + totalGhostsKilled.ToString("00");
        HighScoreCalc("TotalGhosts", totalGhostsKilled, ghostsHighScore, ghostsHigh, false);
        
        finalLapsText.text = "Laps: " + cart.GetComponent<Cart>().completedLoops.ToString("00");
        HighScoreCalc("TotalLaps", cart.GetComponent<Cart>().completedLoops, lapsHighScore, lapsHigh, false);

        finalTreasureText.text = "Looted: " + totalTreasureFound.ToString("00");
        HighScoreCalc("TotalTreasure", totalTreasureFound, treasureHighScore, treasureHigh, false);

        gameOverPanel.SetActive(true);
        // scoreLogText.text = scoreLog;
        finalScoreText.text = "Final Score: " + currentScore.ToString("0000");
        Cursor.visible = true;
        finalDeathReason.text = reason;

        HighScoreCalc("Hiscore", currentScore, highScore, highScoreText, true);
        
        if(PlayerPrefs.HasKey("TotalGames"))
        {
            int gamesPlayed = PlayerPrefs.GetInt("TotalGames");
            PlayerPrefs.SetInt("TotalGames", (gamesPlayed++));
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("TotalGames", 1);
            PlayerPrefs.Save();
        }
    }

    void HighScoreCalc(string PrefKey, int value, int highscorevalue, TextMeshProUGUI textfield, bool fourDigit)
    {
        if (PlayerPrefs.HasKey(PrefKey))
        {
            if (value > PlayerPrefs.GetInt(PrefKey))
            {
                value = highscorevalue;
                PlayerPrefs.SetInt(PrefKey, highscorevalue);
                PlayerPrefs.Save();
                textfield.text = "New Highscore!";
            }
            else if (value <= PlayerPrefs.GetInt(PrefKey))
            {
                if(fourDigit)
                    textfield.text = "Highscore: " + PlayerPrefs.GetInt(PrefKey).ToString("0000");
                else
                    textfield.text = "Highscore: " + PlayerPrefs.GetInt(PrefKey).ToString("00");
            }
        }
        else
        {
            if (value > highscorevalue)
            {
                highscorevalue = value;
                PlayerPrefs.SetInt(PrefKey, highscorevalue);
                PlayerPrefs.Save();
                textfield.text = "New Highscore!";
            }
        }
    }

    void UseAbility()
    {
        canAbilityBeUsed = false;

        switch (characterInUse.specialAbility)
        {
            case SpecialAbility.PauseUnpauseCart:
                Debug.Log("Pause Unpause!");
                if (cart.GetComponent<Animator>().speed > 0)
                    cart.GetComponent<Animator>().speed = 0;
                else
                {
                    cart.GetComponent<Animator>().speed = cart.GetComponent<Cart>().animationSpeed;
                    Debug.Log("Resetting speed to!" + cart.GetComponent<Cart>().animationSpeed);
                }  
                break;
            case SpecialAbility.None:
                break;
            case SpecialAbility.Dodge:
                dodging = true;
                break;
        }
    }

}

