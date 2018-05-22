using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillStreakManager : MonoBehaviour {

    private float highAlpha;
    private float lowAlpha;
    private float textAlpha;
    private float timer;

    private static int hightestKillStreak = 0;

    private Text killstreak;

    public float speed = 3f;

    void Awake()
    {
        killstreak = GetComponent<Text>();
    }

    void Start()
    {
        highAlpha = 1f;
        lowAlpha = 0f;
    }

    void Update()
    {
        if(EnemyHealth.killstreakCounter >= 5)
        {
            FadeIn();
            killstreak.text = "KillStreak : " + EnemyHealth.killstreakCounter.ToString();
        }
        else
        {
            FadeOut();
        }

        UpdateKillStreak();
    }

    void FadeIn()
    {
        Color newColor = killstreak.color;
        newColor.a = Mathf.Lerp(killstreak.color.a, highAlpha, speed * Time.deltaTime);
        killstreak.color = newColor;
    }

    void FadeOut()
    {
        Color newColor = killstreak.color;
        newColor.a = Mathf.Lerp(killstreak.color.a, lowAlpha, speed * Time.deltaTime);
        killstreak.color = newColor;
    }

    public static int KillStreak
    {
        get
        {
            return hightestKillStreak;
        }

        set { hightestKillStreak = value; }
    }

    public static void UpdateKillStreak()
    {
        int currentKS = EnemyHealth.killstreakCounter;

        if(currentKS >= hightestKillStreak)
        {
            hightestKillStreak = currentKS;
        }
    }
}
