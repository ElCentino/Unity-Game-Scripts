using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DyingScreenController : MonoBehaviour {

    private Animator anim;
    private Image screen;
    private PlayerHealth playerHealth;
    private int PlayerHealthInt;

    public Color invincibilityColor;
    public Color currentColor;
    public float speed = 2f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        screen = GetComponent<Image>();
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
        PlayerHealthInt = Animator.StringToHash("PlayerHealth");
    }

    void Start()
    {
        currentColor = screen.color;
    }

    void Update()
    {
        anim.SetInteger(PlayerHealthInt, playerHealth.currentHealth);

        if(PowerSpawner.PLAYER_INVINCIBLE)
        {
            screen.color = invincibilityColor;
        } 
        else
        {
           if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.DyingScreenNormal"))
           {
               screen.color = currentColor;
           }          
        }
    }
}
