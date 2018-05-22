using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public int maxHealth = 100;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip; 
    public AudioSource backgroundMusic;
    public float flashSpeed = 5f;
    public float speed = 3f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim, healthAnim;
    AudioSource playerAudio;
    public Material playerMaterial;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;

    public Image invincibiltyBar;

    bool isDead;
    bool damaged;
    bool callOnce;

    public bool freezePlayerHealth;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        healthAnim = GameObject.FindGameObjectWithTag("HUD-Heart").GetComponentInChildren<Animator>();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;

        playerMaterial.color = Color.white;
    }

    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;

        healthSlider.value = currentHealth;

        if (healthAnim != null && healthAnim.gameObject.activeInHierarchy)
            healthAnim.SetInteger("PlayerHealth", currentHealth);

        if(GameInstructions.TIME_UP && !isDead)
        {
            TimeClass.timeScale = 0;     
        }

        if (freezePlayerHealth)
            currentHealth = 100;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    public void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;

        if(backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }

        TimeClass.timeScale = 0;
    }

    public void ChangeHealthColor()
    {
        Color temp = invincibiltyBar.color;
        temp.a = Mathf.Lerp(temp.a, 1f, Time.deltaTime * speed);       
        invincibiltyBar.color = temp;
        playerMaterial.color = Color.Lerp(playerMaterial.color, Color.yellow, speed * Time.deltaTime);
    }

    public void ResetHealthColor()
    {
        Color temp = invincibiltyBar.color;
        temp.a = Mathf.Lerp(temp.a, 0f, Time.deltaTime * speed);
        invincibiltyBar.color = temp;
        playerMaterial.color = Color.Lerp(playerMaterial.color, Color.white, speed * Time.deltaTime);
    }


    public void RestartLevel ()
    {
        GameInstructions.SaveGame();
        GameInstructions.ResetInstructions();
        SceneManager.LoadScene(0);
    }

    public bool IsDead
    {
        get { return isDead; }
    }
}
