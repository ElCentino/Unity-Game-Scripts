using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour {

    private GameObject player;
    private GameObject pointer;
    private Text powerUpText;
    private Slider healthSlider;
    private Animator HUDAnim;
    private PlayerShooting shooting;
    private PlayerMovement movement;
    private PlayerHealth health;
    private PowerSpawner ps;

    private float timer;
    private float fireRate;
    private float playerSpeed;
    private float activetimer;
    private bool inUse;

    public int addedHealth = 20;
    public float addedFirerate = 0.1f;
    public float addedSpeed = 2f;
    public float timerMax = 5f;
    public float maxActiveTime = 30f;
    public float maxDestructionTime = 5f;
    public float speed = 4f;

    public static bool alive = false;

    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shooting = player.GetComponentInChildren<PlayerShooting>();
        movement = player.GetComponent<PlayerMovement>();
        health = player.GetComponent<PlayerHealth>();
        powerUpText = GameObject.FindGameObjectWithTag("PText").GetComponent<Text>();
        healthSlider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        HUDAnim = GameObject.FindGameObjectWithTag("HUD").GetComponent<Animator>();
        ps = GameObject.FindGameObjectWithTag("GameController").GetComponent<PowerSpawner>();

        pointer = GameObject.FindGameObjectWithTag("Pointer");
        pointer.GetComponent<MeshRenderer>().enabled = true;

        fireRate = shooting.timeBetweenBullets;
        playerSpeed = movement.speed;
        activetimer = 0f;
        alive = true;
    }

    void Update()
    {
        activetimer += Time.deltaTime;

        if(inUse)
        {
            if (activetimer >= maxActiveTime)
            {
                activetimer = 0f;
                inUse = false;
                HidePowerUP();
            }
        }   
        else
        {
            if(activetimer >= maxDestructionTime)
            {
                activetimer = 0f;
                alive = false;  
                DestroyPowerUP();
            }
        }

        RotatePointer();
    }

    void RotatePointer()
    {
        GameObject currentPowerUp = GameObject.FindGameObjectWithTag("PowerUp");
        GameObject director = GameObject.FindGameObjectWithTag("director");

        Quaternion pointerRotation = director.transform.localRotation;
        Vector3 powerUpRelLocation = currentPowerUp.transform.position - director.transform.position;

        Quaternion lookPos = Quaternion.LookRotation(powerUpRelLocation);

        director.transform.LookAt(currentPowerUp.transform);

        director .transform.localRotation = Quaternion.Lerp(pointerRotation, lookPos, speed * Time.deltaTime);
    }

    void DecideProfile()
    {
        StopAllCoroutines();

        int index = Random.Range(0, 3);

        switch(index)
        {
            case 0:
                BoostHealth();
                break;
            case 1:
                StartCoroutine("BoostFireRate");
                break;
            case 2:
                StartCoroutine("BoostMovementSpeed");
                break;
        }
    }

    IEnumerator BoostMovementSpeed()
    {
        DisplayPowerUp("Speed Boost");

        float currentSpeed = movement.speed;
        float newSpeed = currentSpeed + addedSpeed;

        timer = 0f;

        while(timer <= timerMax)
        {
            timer += Time.deltaTime;
            movement.speed = newSpeed;
            yield return null;
        }

        movement.speed = playerSpeed;
    }

    IEnumerator BoostFireRate()
    {
        DisplayPowerUp("Fire Rate Increased");

        timer = 0f;

        while(timer <= timerMax)
        {
            timer += Time.deltaTime;
            shooting.timeBetweenBullets = addedFirerate;
            yield return null;
        }

        shooting.timeBetweenBullets = fireRate;
    }

    void BoostHealth()
    {
        DisplayPowerUp("Health Boost");

        int maxHealth = 100;

        if(health.currentHealth >= maxHealth)
        {
            return;
        }

        int currentHealth = maxHealth - health.currentHealth;
        currentHealth += addedHealth;

        if(currentHealth >= maxHealth)
        {
            health.currentHealth = maxHealth;
        }
        else
        {
            health.currentHealth = (int)currentHealth;
        }

        healthSlider.value = currentHealth;
    }

    void DisplayPowerUp(string text)
    {
        powerUpText.text = text;

        HUDAnim.SetTrigger(Animator.StringToHash("PowerUpTaken"));
    }

    void HidePowerUP()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<Light>().enabled = false;
        transform.Find("point").GetComponent<Light>().enabled = false;
    }

    void DestroyPowerUP()
    { 
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            inUse = true;
            pointer.GetComponent<MeshRenderer>().enabled = false;
            DecideProfile();
            HidePowerUP();
        }
    }
}
