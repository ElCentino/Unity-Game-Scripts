using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    private GameObject pauseMenu;
    private Animator menuHUD;
    private Color bgColor;

    public Image backgroundImage;
    public Button highscoresButton;
    public GameObject optionPane;
    public GameObject exitPane;
    public GameObject mainPane;
    public bool hovered = false;
    public float speed = 4f;

    void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag(Tags.pauseMenu);
        menuHUD = GameObject.FindGameObjectWithTag(Tags.menu).GetComponent<Animator>();
        
    }

    void Start()
    {
        bgColor = backgroundImage.color;
        backgroundImage.color = Color.clear;
        menuHUD.enabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && GameInstructions.IN_GAME)
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
            Time.timeScale = pauseMenu.activeInHierarchy ? 0 : 1;
            backgroundImage.color = Color.Lerp(backgroundImage.color, bgColor, speed);

            if(Time.timeScale == 0)
            {
                StopCoroutine("BackgroundFade");
                StartCoroutine("BackgroundFade");
            }

            if(!mainPane.activeInHierarchy && (exitPane.activeInHierarchy || optionPane.activeInHierarchy))
            {
                exitPane.SetActive(false);
                optionPane.SetActive(false);
                mainPane.SetActive(true);
            }

            hovered = false;
        }

        if(GameInstructions.IN_GAME)
        {
            highscoresButton.image.color = Color.gray;
            highscoresButton.enabled = false;
        }

        backgroundImage.color = Color.Lerp(backgroundImage.color, bgColor, speed * Time.deltaTime);
    }

    public void Play()
    {
        if(GameInstructions.IN_GAME)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;

            hovered = false;

            StopCoroutine("BackgroundFade");
            StartCoroutine("BackgroundFade");           
        }
        else
        {           
            if(!menuHUD.gameObject.activeInHierarchy)
            {
                menuHUD.gameObject.SetActive(true);
            }
            else
            {
                menuHUD.enabled = true;
            }

            menuHUD.enabled = true;
            menuHUD.SetTrigger("Restart");
            pauseMenu.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator BackgroundFade()
    {
        while(backgroundImage.color != Color.clear)
        {
            backgroundImage.color = Color.Lerp(backgroundImage.color, Color.clear, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void HoverEnter()
    {
        hovered = true;
    }

    public void HoverExit()
    {
        hovered = false;
    }
}
