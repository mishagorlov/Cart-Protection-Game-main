using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{

    public int totalGhostsSpawned, currentGhostsAlive, maxGhostsAtATime, GhostsPerLevel, ghostsInLevel, remainingGhosts;
    public GameObject doorOpen, doorClosed;
    public GameObject ghostPrefab, gremlinPrefab;
    public float spawnRadius;
    Player player;
    public float spawnDelayMin, spawnDelayMax, currentSpawnTimer, spawnDelay, ghostSpeed, ghostSpeedMax;

    float coolDownTimerMax, coolDownTimer;
    bool spawning;

    public TextMeshProUGUI ghostsLeftInLevelText;
    public Slider coolDownSlider;
    AudioSource aS;

    public AudioClip doorOpenAudio, doorCloseAudio, ghostsSpawnAudio;

    TreasureChest tC;

    void Start()
    {
        aS = GetComponent<AudioSource>();
        tC = GameObject.Find("TreasureChest").GetComponent<TreasureChest>();
        totalGhostsSpawned = 0;
        maxGhostsAtATime = 6;
        GhostsPerLevel = 10;
        remainingGhosts = GhostsPerLevel;
        spawnDelay = NewSpawnDelay(spawnDelayMin, spawnDelayMax);
        player = GameObject.Find("Player").GetComponent<Player>();
        spawning = true;
        coolDownTimerMax = 20f;
        ghostsInLevel = 0;
        ghostSpeedMax = 2f;
        ghostSpeed = 1;
        doorClosed.SetActive(true);
        doorOpen.SetActive(false);
        coolDownSlider.maxValue = coolDownTimerMax;
        coolDownSlider.value = 0;
        coolDownSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player.currentHealth > 0)
        {
            if (spawning)
            {
                if (currentSpawnTimer < spawnDelay)
                {
                    currentSpawnTimer += Time.deltaTime;
                }
                else if (currentSpawnTimer >= spawnDelay)
                {
                    if (currentGhostsAlive < maxGhostsAtATime && ghostsInLevel < GhostsPerLevel)
                    {
                        if (SpawnGremlin())
                            SpawnEnemy(gremlinPrefab);
                        else
                            SpawnEnemy(ghostPrefab);
                    }
                    else
                    {
                        if (currentGhostsAlive == 0)
                        {
                            CooldownNextLevel();
                        }
                    }
                }

            }
            else if (!spawning)
            {
                if (coolDownTimer < coolDownTimerMax)
                {
                    coolDownTimer += Time.deltaTime;

                    coolDownSlider.value = coolDownTimer;
                }
                else if (coolDownTimer >= coolDownTimerMax)
                {
                    NextLevel();
                }
            }
        }
    }


    bool SpawnGremlin()
    {
        bool noAvailable = true;

        foreach (GameObject t in tC.currentTreasuresInScene)
        {
            if (t.GetComponent<Treasure>().isBeingTargeted == false)
            {
                noAvailable = false;
                break;
            }
        }

        if (noAvailable)
        {
            Debug.Log("Not spawning gremlin, all treasures are being targeted at the moment");
            return false;
        }
        else
        {
            Debug.Log("There is at least one treasure available, spawning Gremlin");
            return true;
        }
    }
    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    float NewSpawnDelay(float a, float b)
    {
        float c = Random.Range(a, b);
        return c;
    }

    void SpawnEnemy(GameObject enemyToSpawn)
    {
        aS.PlayOneShot(ghostsSpawnAudio);
        Vector3 pos = RandomCircle(this.transform.position, spawnRadius);
        Instantiate(enemyToSpawn, pos, Quaternion.identity);

        if (enemyToSpawn == ghostPrefab)
        {
            totalGhostsSpawned++;
            currentGhostsAlive++;
            ghostsInLevel++;
        }
        spawnDelay = NewSpawnDelay(spawnDelayMin, spawnDelayMax);
        
        currentSpawnTimer = 0f;
        if(ghostsInLevel == GhostsPerLevel)
        {
            if(currentGhostsAlive == 0)
                CooldownNextLevel();
        }
    }

    void CooldownNextLevel()
    {
        spawning = false;
        coolDownTimer = 0f;
        player.BottomText("Cooldown to Next Level!");
        doorOpen.SetActive(true);
        doorClosed.SetActive(false);
        aS.PlayOneShot(doorOpenAudio);
        coolDownSlider.gameObject.SetActive(true);
    }

    void NextLevel()
    {
        ghostsInLevel = 0;
        maxGhostsAtATime++;
        GhostsPerLevel += (GhostsPerLevel / 3);
        remainingGhosts = GhostsPerLevel;
        spawning = true;

        if(player.currentHealth < player.maxHealth)
        {
            player.ChangeHealth(1);
            player.BottomText("Next Level, +1 Health");
        }
        else if(player.currentHealth == player.maxHealth)
        {
            player.BottomText("Next Level!");
        }
        UpdateUI();
        doorOpen.SetActive(false);
        doorClosed.SetActive(true);
        aS.PlayOneShot(doorCloseAudio);
        spawnDelayMin -= 0.3f;
        spawnDelayMax -= 0.3f;
        coolDownSlider.gameObject.SetActive(false);
        coolDownSlider.value = coolDownSlider.maxValue;
    }

    public void UpdateUI()
    {
        ghostsLeftInLevelText.text = remainingGhosts.ToString("00") ;
    }
}

