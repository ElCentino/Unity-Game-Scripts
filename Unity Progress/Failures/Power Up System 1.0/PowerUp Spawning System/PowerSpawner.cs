using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSpawner : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject powerUp;
    public Text powerUpText;
    public int enemiesKilledBeforeSpawn = 10;
    public GameObject pointer;
    public float speed = 2f;

    private Animator anim;
    private bool spawn;
    private GameObject currentPowerUp;

    void Awake()
    {
        anim = GameObject.FindGameObjectWithTag("HUD").GetComponent<Animator>();
        spawn = false;
    }

	void Update()
    {

        //bool powerUpAlive = GameObject.FindGameObjectWithTag("PowerUp").activeSelf;

        spawn = (EnemyHealth.enemiesKilledCounter >= enemiesKilledBeforeSpawn)? true : false;

        if(spawn)
        {
            DisplayText("Power Up is available");
            Spawn();
            EnemyHealth.enemiesKilledCounter = 0;
        }
    }

    void DisplayText(string message)
    {
        powerUpText.text = message;
        anim.SetTrigger(Animator.StringToHash("PowerUpTaken"));
    }

    void Spawn()
    {
        spawn = false;

        int index = Random.Range(0, spawnPoints.Length);

        Instantiate(powerUp, spawnPoints[index].position, spawnPoints[index].rotation);
    }

    void EnablePointer()
    {
        //pointer.SetActive(true);

        
    }
}
