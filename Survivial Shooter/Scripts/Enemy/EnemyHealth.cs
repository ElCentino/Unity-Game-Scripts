using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public int timeValue = 10;
    public AudioClip deathClip;

    public static int enemiesKilledCounter = 0;
    public static int killstreakCounter = 0;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    TimeClass timer;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        timer = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<TimeClass>();

        currentHealth = startingHealth;
    }

    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
            enemiesKilledCounter++;
            killstreakCounter++;
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }

    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        ScoreManager.score += (scoreValue * GameInstructions.SCORE_MULTIPLIER) + ScoreManager.HEATLEVELBOOST;

        if (GameInstructions.GAME_MODE == Literals.GAME_MODES.SURVIVAL_GAME_MODE)
        {
            timer.Seconds += timeValue;
            timer.UpdateBonusTime();
        }
        Destroy (gameObject, 2f);
    }
}
