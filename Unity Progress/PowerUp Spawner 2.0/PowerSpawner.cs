using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerSpawner : MonoBehaviour {

    private GameObject player;
    private GameObject enemyManager;
    private Animator HUDAnim;
    private MeshRenderer meshR;
    private CapsuleCollider col;
    private Light cLight;

    private PlayerShooting shooting;
    private PlayerMovement movement;
    private PlayerHealth health;

    private Text powerUpText;
    private Slider healthSlider;

    public float timer = 0f;
    public float timerMax = 15f;
    public float aliveTimer = 0f;
    public float aliveTimerMax = 30f;
    public float effectsTimerMax = 15f;
    public float timeRange = 10f;
    public Transform[] spawnLocation;
    public float dSpeed;
    public float dFirerate;
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

    private string status = "dead";
    private string PLabel = "";

    private enum Status : int
    {
        Firerate = 0,
        Speed = 1,
        Health = 2,
        Invincibility = 3
    }

    void Awake()
    {
        if(this.enabled)
        {
            player = GameObject.FindGameObjectWithTag(Tags.player);
            enemyManager = GameObject.FindGameObjectWithTag(Tags.enemyManager);
            HUDAnim = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<Animator>();

            meshR = GetComponent<MeshRenderer>();
            col = GetComponent<CapsuleCollider>();
            cLight = GetComponentInChildren<Light>();

            shooting = player.GetComponentInChildren<PlayerShooting>();
            movement = player.GetComponent<PlayerMovement>();
            health = player.GetComponent<PlayerHealth>();

            powerUpText = GameObject.FindGameObjectWithTag(Tags.powerText).GetComponent<Text>();
            healthSlider = GameObject.FindGameObjectWithTag(Tags.HPSlider).GetComponent<Slider>();
        }    
    }

    void Start()
    {
        if(GameInstructions.DevMode) enemyManager.SetActive(!GameInstructions.DevMode);

        DisableProperties();
        countDown = timeRange;

        broker = 1f;
        effectsTimerReset = effectsTimerMax;

        playerSpeedReset = movement.speed;
        playerDamageReset = shooting.Damage;
        weaponFirreateReset = shooting.timeBetweenBullets;
    }

    void Mechanics()
    {
        //Alive Timer Initialization
        aliveTimer += Time.deltaTime * broker;

        if(aliveTimer >= aliveTimerMax)
        {
            SpawnPowerUP();

            
            countDown -= Time.deltaTime;
            notificator.text = (!powerupActive) ? "Time Left\n" + Mathf.Round(countDown).ToString() : 
                (index != (int)Status.Health) ? PLabel + "\n" + Mathf.Round(effectsTimerMax).ToString() : "Health Restored";

            if(aliveTimer >= aliveTimerMax + timeRange)
            {
                aliveTimer = 0f;
                DisableProperties();
                notificator.enabled = false;
                countDown = timeRange;
            }
            else if(powerupActive)
            {
                broker = 0f;
                effectsTimerMax -= Time.deltaTime;

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
        index = Random.Range(1, 4);

        switch(index)
        {
            case (int)Status.Firerate:
                PLabel = "Fire rate boost";
                StartCoroutine(WeaponBoost(PLabel));
                break;
            

            case (int) Status.Health:
                PLabel = "Health Restored";
                HealthRestore(PLabel);
                break;

            case (int)Status.Speed:
                PLabel = "Speed Boost";
                StartCoroutine(SpeedBoost(PLabel));
                break;

            case (int) Status.Invincibility:
                PLabel = "Invincibility";
                StartCoroutine(Invincibility(PLabel));
                break;
        }


    }

    IEnumerator SpeedBoost(string boost)
    {
        //Speed is greatly increased for 15 seconds whilst damage is reduced slightly.
        DisplayText("Speed Boost");
        movement.speed += dSpeed;
        shooting.Damage -= 3;
        while(effectsTimerMax >= 1)
        {
            yield return null;
        }

        movement.speed = playerSpeedReset;
        shooting.Damage = playerDamageReset;
    }

    void HealthRestore(string boost)
    {
        //Health is restored.
        DisplayText(boost);

        health.currentHealth = 100;
        healthSlider.value = 100;
    }

    IEnumerator WeaponBoost(string boost)
    {
        //Weapons fire rate is greatly increase whilst speed is slightly reduced.
        DisplayText(boost);

        shooting.timeBetweenBullets -= dFirerate;
        movement.speed -= negativeSpeed;

        while (effectsTimerMax >= 1)
        {
            Debug.Log("working");
            yield return null;
        }


        shooting.timeBetweenBullets = weaponFirreateReset;
        movement.speed = playerSpeedReset;
    }

    IEnumerator Invincibility(string boost)
    {
        //Health is frozen for 15 seconds.
        DisplayText(boost);

        int currentHealth = health.currentHealth;
       

        while(effectsTimerMax >= 1)
        {
            health.currentHealth = currentHealth;
            healthSlider.value = health.currentHealth;

            health.ChangeHealthColor();
            yield return null;
        }

        health.ChangeHealthColor();
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
            callOnce = false;
        }
    }

    void DisableProperties()
    {
        meshR.enabled = false;
        cLight.enabled = false;
        col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
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
