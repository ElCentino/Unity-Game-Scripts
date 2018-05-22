using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBridge : MonoBehaviour {

    private GameObject pauseMenu;
    private GameObject gameTitle;
    private Animator menuHUD;

    void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag(Tags.pauseMenu);
        gameTitle = GameObject.FindGameObjectWithTag(Tags.gameTitle);
        menuHUD = GameObject.FindGameObjectWithTag(Tags.menu).GetComponent<Animator>();
    }

	public void ReverseSelected()
    {
        if(!GameInstructions.IN_GAME)
        {
            pauseMenu.SetActive(true);
            menuHUD.enabled = false;
            menuHUD.gameObject.SetActive(false);
        }
        else
        {
            menuHUD.enabled = false;
            gameObject.SetActive(false);
            gameTitle.SetActive(false);
        }
    }
}
