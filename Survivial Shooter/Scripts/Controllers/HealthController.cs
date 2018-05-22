using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthController : MonoBehaviour {

    public Slider healthSlider;
    public AudioSource backgroundMusic;
    public Color orange;
    public Image slider;
    public float speed = 2;   

    Text healthText;
    Color textColor, sliderColor;
    float lowVolume, highVolume;

    private AudioReverbZone reverbZone;
    private PlayerHealth playerHealth;
    private AudioSource heartBeat;

    void Awake()
    {
        healthText = GetComponent<Text>();
        textColor = healthText.color;
        sliderColor = slider.color;
        heartBeat = GetComponent<AudioSource>();
        reverbZone = GameObject.FindGameObjectWithTag(Tags.reverbZone).GetComponent<AudioReverbZone>();
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
    }

    void Start() 
    {
        highVolume = backgroundMusic.volume;
        lowVolume = backgroundMusic.volume / 2;
    }


    void Update()
    {
        healthText.text = "" + healthSlider.value.ToString();

        int health = (int)healthSlider.value;

        if(health <= 25)
        {
            healthText.color = Color.Lerp(healthText.color, Color.red, speed * Time.deltaTime);
            slider.color = Color.Lerp(slider.color, Color.red, speed * Time.deltaTime);
            backgroundMusic.volume = Mathf.Lerp(backgroundMusic.volume, lowVolume, speed * Time.deltaTime);

            if(playerHealth.IsDead)
            {
                heartBeat.volume = Mathf.Lerp(heartBeat.volume, 0, speed * Time.deltaTime);
            }
            else
            {
                heartBeat.volume = Mathf.Lerp(heartBeat.volume, highVolume + 1.5f, speed * Time.deltaTime);
            }
          
            reverbZone.enabled = true;
        }
        else if(health <= 50)
        {
            healthText.color = Color.Lerp(healthText.color, orange, speed * Time.deltaTime);
            slider.color = Color.Lerp(slider.color, orange, speed * Time.deltaTime);
        }
        else
        {
            healthText.color = textColor;
            slider.color = Color.Lerp(slider.color, sliderColor, speed * Time.deltaTime);
            backgroundMusic.volume = Mathf.Lerp(backgroundMusic.volume, highVolume, speed * Time.deltaTime);
            heartBeat.volume = Mathf.Lerp(heartBeat.volume, 0, speed * Time.deltaTime);
            reverbZone.enabled = false;
        }
    }
}