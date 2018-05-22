using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Select : MonoBehaviour {

    private Color survivalDefaultColor;
    private Color normalDefaultColor;
    private Color gray = new Color(0, 255, 50, 0.5f);
    private Color red = new Color(255, 0, 0, 0.5f);

    private Image survivalImage;
    private Image normalImage;

    private GameObject menuHUD;
    private GameObject pauseMenu;
    private GameObject player;
    private Animator anim;
    private Light mainLight;
    private GameInstructions GI;
    private TimeClass timer;

    private string currentButtonName;

    public float speed = 2f;

    void Start()
    {
        survivalDefaultColor = GameObject.FindGameObjectWithTag(Tags.survival).GetComponent<Image>().color;
        normalDefaultColor = GameObject.FindGameObjectWithTag(Tags.normal).GetComponent<Image>().color;
        survivalImage = GameObject.FindGameObjectWithTag(Tags.survival).GetComponent<Image>();
        normalImage = GameObject.FindGameObjectWithTag(Tags.normal).GetComponent<Image>();
        timer = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<TimeClass>();

        menuHUD = GameObject.FindGameObjectWithTag(Tags.menu);
        player = GameObject.FindGameObjectWithTag(Tags.player);
        pauseMenu = GameObject.FindGameObjectWithTag(Tags.pauseMenu);
        mainLight = GameObject.FindGameObjectWithTag(Tags.mainLight).GetComponentInChildren<Light>();
        anim = menuHUD.GetComponent<Animator>();
        GI = GetComponent<GameInstructions>();
    }

    public void MouseEnterEvent()
    {
        if (currentButtonName == Literals.Buttons.BUTTON_SURVIVAL)
        {
            StartCoroutine(Literals.ButtonState.SURVIVAL_ENTER);
        } 
        else
        {
            StartCoroutine(Literals.ButtonState.NORMAL_ENTER);
        }
    }

    public void MouseExitEvent()
    {
        if (currentButtonName == Literals.Buttons.BUTTON_SURVIVAL)
        {
            StartCoroutine(Literals.ButtonState.SURVIVAL_EXIT);
        }   
        else
        {
            StartCoroutine(Literals.ButtonState.NORMAL_EXIT);
        }
    }

    public void getName(string name)
    {
        currentButtonName = name;
    }

    IEnumerator SurvivalEnter()
    {
        while (survivalImage.color != gray)
        {
            survivalImage.color = Color.Lerp(survivalImage.color, gray, speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator SurvivalExit()
    {
        StopCoroutine(Literals.ButtonState.SURVIVAL_ENTER);

        while(survivalImage.color != survivalDefaultColor)
        {
            survivalImage.color = Color.Lerp(survivalImage.color, survivalDefaultColor, speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator NormalEnter()
    {
        while(normalImage.color != red)
        {
            normalImage.color = Color.Lerp(normalImage.color, red, speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator NormalExit()
    {
        StopCoroutine(Literals.ButtonState.NORMAL_ENTER);
        while(normalImage.color != normalDefaultColor)
        {
            normalImage.color = Color.Lerp(normalImage.color, normalDefaultColor, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void ChooseMode(string button)
    {
        if(button.Equals(Literals.Buttons.BUTTON_SURVIVAL))
        {
            GameInstructions.GAME_MODE = Literals.GAME_MODES.SURVIVAL_GAME_MODE;
        }
        else if(button.Equals(Literals.Buttons.BUTTON_NORMAL))
        {
            GameInstructions.GAME_MODE = Literals.GAME_MODES.NORNAL_GAME_MODE;
            timer.Hour = 0;
            timer.Minute = 0;
            timer.Seconds = 0;
            TimeClass.timeScale = GameInstructions.RESET_TIME[3];

            mainLight.intensity = Mathf.Lerp(mainLight.intensity, 0.45f, speed * Time.deltaTime);

            Transform[] playersBody = player.GetComponentsInChildren<Transform>();

            foreach (Transform part in playersBody)
            {
                if (part.gameObject.layer == LayerMask.NameToLayer("Selectables"))
                {
                    part.gameObject.SetActive(false);
                }
            }
        }
        else if(button.Equals(Literals.Buttons.BUTTON_HEAT))
        {
            GameInstructions.GAME_MODE = Literals.GAME_MODES.HEAT_GAME_MODE;
            timer.Hour = 0;
            timer.Minute = 0;
            timer.Seconds = 0;
            TimeClass.timeScale = GameInstructions.RESET_TIME[3];

            mainLight.intensity = 0f;         
        }

        GI.ReactivateStarters();
        GameInstructions.IN_GAME = true;
        anim.SetTrigger("Selected");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !GameInstructions.IN_GAME && !pauseMenu.activeInHierarchy)
        {
            anim.SetTrigger("Escaped");
        }
    }
}