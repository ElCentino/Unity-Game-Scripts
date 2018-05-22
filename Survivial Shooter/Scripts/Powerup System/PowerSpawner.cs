using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerSpawner : MonoBehaviour {

    private GameObject player;
    private GameObject enemyManager;
    private GameObject pointer;
    private Animator HUDAnim;
    private MeshRenderer meshR;
    private CapsuleCollider col;
    private AudioSource audioSource;
    private Light cLight;

    private PlayerShooting shooting;
    private PlayerMovement movement;
    private PlayerHealth health;

    private Text powerUpText;
    private Slider healthSlider;

    public Transform[] spawnLocation;
    public float timer = 0f;
    [Range(0, 30)]
    public float timeBeforeSpawn = 30f;
    [Range(0, 15)]
    public float effectsTimerMax = 15f;
    [Range(0, 30)]
    public float activeTimer = 10f;
    public float fallOffTime = 0f;
    [Range(0, 4)]
    public float dSpeed;
    [Range(0, 0.08f)]
    public float dFirerate;
    [Range(0, 0.7f)]
    public float negativeSpeed;

    private float countDown;
    private float broker;
    private bool callOnce;
    private bool powerupActive;
    private float effectsTimerReset = 15f;
    private float playerSpeedReset;
    private float weaponFirreateReset;
    private int playerDamageReset;
    private int index;

    public Text notificator;

    public AudioClip healthFx;
    public AudioClip speedFx;

    public Material lineMat;
    public Light lineMatLight;
    public Color[] lineColors = new Color[2];

    private string status = "dead";
    private string PLabel = "";

    public bool FirerateOnly;
    public bool HealthOnly;
    public bool InvincibilityOnly;
    public bool SpeedOnly;
    public bool DamageOnly;
    public bool ScoreMOnly;

    public static bool PLAYER_INVINCIBLE = false;

    private enum Status : int
    {
        Firerate = 0,
        Speed = 1,
        Health = 2,
        Invincibility = 3,
        DamageBoost = 4,
        ScoreMultiplier = 5
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        enemyManager = GameObject.FindGameObjectWithTag(Tags.enemyManager);
        HUDAnim = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<Animator>();

        meshR = GetComponent<MeshRenderer>();
        col = GetComponent<CapsuleCollider>();
        cLight = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();

        shooting = player.GetComponentInChildren<PlayerShooting>();
        movement = player.GetComponent<PlayerMovement>();
        health = player.GetComponent<PlayerHealth>();

        powerUpText = GameObject.FindGameObjectWithTag(Tags.powerText).GetComponent<Text>();
        healthSlider = GameObject.FindGameObjectWithTag(Tags.HPSlider).GetComponent<Slider>();
        pointer = GameObject.FindGameObjectWithTag(Tags.pointer);

        lineMat.color = lineColors[0];
    }

    void Start()
    {
        if(GameInstructions.DevMode) enemyManager.SetActive(!GameInstructions.DevMode);

        DisableProperties();
        countDown = activeTimer;

        broker = 1f;
        effectsTimerReset = effectsTimerMax;

        playerSpeedReset = movement.speed;
        playerDamageReset = shooting.Damage;
        weaponFirreateReset = shooting.timeBetweenBullets;
    }

    void Mechanics()
    {
        //Alive Timer Initialization
        timer += Mathf.Abs(Time.deltaTime * broker);

        if(timer >= timeBeforeSpawn)
        {
            SpawnPowerUP();
          
            countDown -= Time.deltaTime;
            notificator.text = (!powerupActive) ? "Time Left\n" + Mathf.Round(countDown).ToString() : 
                (index != (int)Status.Health) ? PLabel + "\n" + Mathf.Round(effectsTimerMax).ToString() : "Health Restored";

            if(timer >= timeBeforeSpawn + (activeTimer - fallOffTime))
            {
                timer = 0f;
                DisableProperties();
                notificator.enabled = false;
                countDown = activeTimer;
                fallOffTime = 0f;
            }
            else if(powerupActive)
            {
                fallOffTime += Time.deltaTime;
                effectsTimerMax -= Time.deltaTime;
                broker = 0f;

                if(effectsTimerMax <= broker)
                {
                    broker = 1;
                    notificator.enabled = false;
                    status = "Disabled";
                    effectsTimerMax = effectsTimerReset;
                    powerupActive = false;
                }
            }
        }
        else
        {
            callOnce = true;
        }
    }

    void DecidePowerup()
    {
        if(!FirerateOnly && !InvincibilityOnly && !SpeedOnly && !HealthOnly && !ScoreMOnly && !DamageOnly)
        {
            index = Random.Range(0, 6);

            switch (index)
            {
                case (int)Status.Firerate:
                    PLabel = "Fire rate Boost";
                    StartCoroutine(WeaponBoost(PLabel));
                    break;

                case (int)Status.Health:
                    PLabel = "Health Restored";
                    HealthRestore(PLabel);
                    break;

                case (int)Status.Speed:
                    PLabel = "Speed Boost";
                    StartCoroutine(SpeedBoost(PLabel));
                    break;

                case (int)Status.Invincibility:
                    PLabel = "Invincibility";
                    StartCoroutine(Invincibility(PLabel));
                    break;

                case (int)Status.DamageBoost:
                    PLabel = "Damage Boost";
                    StartCoroutine(DamageBoost(PLabel));
                    break;

                case (int)Status.ScoreMultiplier:
                    int r = Random.Range(1, 6);
                    PLabel = index.ToString() + "x Score Multiplier";
                    StartCoroutine(ScoreMultiplier(PLabel, r));
                    break;
            }
        }
        else
        {
            if(FirerateOnly)
            {
                HealthOnly = false;
                SpeedOnly = false;
                InvincibilityOnly = false;
                ScoreMOnly = false;
                DamageOnly = false;

                PLabel = "Fire rate boost";
                StartCoroutine(WeaponBoost(PLabel));
            }
            else if(HealthOnly)
            {
                FirerateOnly = false;
                SpeedOnly = false;
                InvincibilityOnly = false;
                ScoreMOnly = false;
                DamageOnly = false;

                PLabel = "Health Restored";
                HealthRestore(PLabel);
            }
            else if(SpeedOnly)
            {
                FirerateOnly = false;
                HealthOnly = false;
                SpeedOnly = false;
                InvincibilityOnly = false;
                ScoreMOnly = false;
                DamageOnly = false;

                PLabel = "Speed Boost";
                StartCoroutine(SpeedBoost(PLabel));
            } 
            else if(InvincibilityOnly)
            {
                FirerateOnly = false;
                HealthOnly = false;
                SpeedOnly = false;
                ScoreMOnly = false;
                DamageOnly = false;

                PLabel = "Invincibility";
                StartCoroutine(Invincibility(PLabel));
            }
            else if(ScoreMOnly)
            {
                FirerateOnly = false;
                HealthOnly = false;
                SpeedOnly = false;
                InvincibilityOnly = false;
                DamageOnly = false;

                int r = Random.Range(2, 6);
                PLabel = r + "x Score Multiplier";
                StartCoroutine(ScoreMultiplier(PLabel, r));
            } 
            else if(DamageOnly)
            {
                FirerateOnly = false;
                HealthOnly = false;
                SpeedOnly = false;
                InvincibilityOnly = false;
                ScoreMOnly = false;

                PLabel = "Damage Boost";
                StartCoroutine(DamageBoost(PLabel));
            }
        }       
    }

    IEnumerator SpeedBoost(string boost)
    {
        /*Speed is greatly increased 
         * for 15 seconds whilst 
         * damage is reduced slightly.
         */

        audioSource.clip = speedFx;
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        DisplayText("Speed Boost");
        movement.speed += dSpeed;
        shooting.Damage -= 3;
        while(effectsTimerMax >= 1)
        {
            yield return null;
        }

        movement.speed = playerSpeedReset;
        shooting.Damage = playerDamageReset;

        audioSource.clip = speedFx;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void HealthRestore(string boost)
    {
        //Health is restored.

        audioSource.clip = healthFx;
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        DisplayText(boost);

        health.currentHealth = 100;
        healthSlider.value = 100;
    }

    IEnumerator WeaponBoost(string boost)
    {
        /*Weapons fire rate is greatly 
         * increase whilst speed is 
         * slightly reduced.
         */

        audioSource.clip = healthFx;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        DisplayText(boost);

        shooting.timeBetweenBullets -= dFirerate;
        movement.speed -= negativeSpeed;

        while (effectsTimerMax >= 1)
        {
            yield return null;
        }

        shooting.timeBetweenBullets = weaponFirreateReset;
        movement.speed = playerSpeedReset;

        audioSource.clip = healthFx;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    IEnumerator Invincibility(string boost)
    {
        //Health is frozen for 15 seconds.

        DisplayText(boost);

        int currentHealth = health.currentHealth;

        PowerSpawner.PLAYER_INVINCIBLE = true;
       
        while(effectsTimerMax >= 1)
        {
            health.currentHealth = currentHealth;
            healthSlider.value = health.currentHealth;

            health.ChangeHealthColor();
            yield return null;
        }

        health.ChangeHealthColor();

        PowerSpawner.PLAYER_INVINCIBLE = false;
    }

    IEnumerator ScoreMultiplier(string boost, int multiplier)
    {
        GameInstructions.SCORE_MULTIPLIER = multiplier;

        DisplayText(boost);

        while (effectsTimerMax >= 1)
        {          
            yield return null;
        }

        GameInstructions.SCORE_MULTIPLIER = 1;
    }

    IEnumerator DamageBoost(string boost)
    {
        shooting.Damage = shooting.Damage * 50;

        audioSource.clip = speedFx;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        lineMat.color = lineColors[1];

        DisplayText(boost);

        while (effectsTimerMax >= 1)
        {
            yield return null;
        }

        lineMat.color = lineColors[0];

        audioSource.clip = speedFx;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        shooting.Damage = playerDamageReset;
    }

    void SpawnPowerUP()
    {
        int pos = Random.Range(0, spawnLocation.Length);

        if(callOnce) transform.position = spawnLocation[pos].position;

        EnableProperties();
    }

    public void DisplayText(string text)
    {
        HUDAnim.SetTrigger("PowerUpTaken");
        powerUpText.text = text;
    }

    void EnableProperties()
    {
        if(callOnce)
        {
            meshR.enabled = true;
            cLight.enabled = true;
            col.enabled = true;
        }

        if (meshR.enabled && cLight.enabled && col.enabled && callOnce)
        {
            notificator.enabled = true;
            DisplayText("Powerup Available");
            pointer.SetActive(true);
            callOnce = false;
        }
    }

    void DisableProperties()
    {
        meshR.enabled = false;
        cLight.enabled = false;
        col.enabled = false;
        pointer.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player && GameInstructions.IN_GAME)
        {
            status = "Enabled";
            DecidePowerup();
            DisableProperties();
        }
    }

    void Update()
    {
        if (status.Equals("Enabled"))
            powerupActive = true;
        else
            powerupActive = false;

        Mechanics();

        if(!powerupActive)
            health.ResetHealthColor();
    }
}