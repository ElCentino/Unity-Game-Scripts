using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    private Animator anim;
    private GameObject player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private bool playerInRange;
    private bool verified;
    private float timer;

    void  OnEnable()
    {
        //ScoreManager.HeatIncrease += Heat;
        //attackDamage += GameInstructions.ENEMY_DAMAGE_INCREASE;
    }

    void OnDisable()
    {
        //ScoreManager.HeatIncrease -= Heat;
    }

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player && !PowerSpawner.PLAYER_INVINCIBLE)
        {
            playerInRange = true;
            EnemyHealth.killstreakCounter = 0;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }

        if(attackDamage >= 25)
        {
            attackDamage = 25;
        }
    }

    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }

    public void Heat()
    {
        if(attackDamage <= 25 && IsVerified)
            attackDamage += GameInstructions.ENEMY_DAMAGE_INCREASE;
    }

    public bool IsVerified
    {
        get { return verified; }
        set { verified = value; }
    }
}
