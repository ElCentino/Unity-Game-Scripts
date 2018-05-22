using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private Text killCounter;
    private PowerSpawner powerup;
    private ScoreManager scoreManager;
    private TimeClass timer;

    private int bestScoreHeat, bestScoreNormal, bestLevel, bestHeat;
    private string bestTimeHeat, bestTimeNormal;

    private bool heatMode, normalMode;

    private Animator dyingScreenAnim;

    public Text build;

    void Awake()
    {
        killCounter = GameObject.FindGameObjectWithTag(Tags.killCounter).GetComponent<Text>();
        powerup = GameObject.FindGameObjectWithTag(Tags.powerUP).GetComponent<PowerSpawner>();
        dyingScreenAnim = GameObject.FindGameObjectWithTag(Tags.dyingScreen).GetComponent<Animator>();
        timer = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<TimeClass>();
        scoreManager = GetComponentInChildren<ScoreManager>();
    }

    void Start()
    {
        GameInstructions.LoadSaves();
        bestScoreHeat = GameInstructions.HighScoresValues.bestScoreHeat;
        bestScoreNormal = GameInstructions.HighScoresValues.bestScoreNormal;
        bestTimeHeat = GameInstructions.HighScoresValues.bestTime;
        bestTimeNormal = GameInstructions.HighScoresValues.bestTimeNormal;
        bestHeat = GameInstructions.HighScoresValues.topHeat;
        bestLevel = GameInstructions.HighScoresValues.topLevel;
    }

    void Update()
    {
        heatMode = GameInstructions.GAME_MODE == Literals.GAME_MODES.HEAT_GAME_MODE;
        normalMode = GameInstructions.GAME_MODE == Literals.GAME_MODES.NORNAL_GAME_MODE;

        GameBuildSeial();
        ZomKillsReporter();
        HighScoreUpdate();
        BestTimeUpdate();
        BestHeatLevel();
        
        if(dyingScreenAnim != null && dyingScreenAnim.gameObject.activeInHierarchy)
            dyingScreenAnim.SetBool("Invincibility", PowerSpawner.PLAYER_INVINCIBLE);
    }

    void HighScoreUpdate()
    {
        if(heatMode)
        {
            if(scoreManager.Score > bestScoreHeat)
            {
                CallText("New Highscore!!!");
                bestScoreHeat = scoreManager.Score * 50;
            }
        } 
        else if(normalMode)
        {
            if(scoreManager.Score > bestScoreNormal)
            {
                CallText("New Highscore!!!");
                bestScoreNormal = scoreManager.Score * 50;
            }
        }
    }

    void BestTimeUpdate()
    {
        if(heatMode)
        {
            if(timer.BCompareTime(timer.GetTime(), bestTimeHeat))
            {
                CallText("New Best Time!!!");
                bestTimeHeat = "9999999:9999999";
            }
        }
        else if(normalMode)
        {
            if(timer.BCompareTime(timer.GetTime(), bestTimeNormal))
            {
                CallText("New Best Time!!!");
                bestTimeNormal = "9999999:9999999";
            }
        }
    }

    void ZomKillsReporter()
    {
        if (killCounter.gameObject.activeInHierarchy) killCounter.text = "Zombies Killed\n" + EnemyHealth.enemiesKilledCounter;
    }

    void BestHeatLevel()
    {
        if(heatMode)
        {
            if(scoreManager.Heat > bestHeat)
            {
                CallText("New Heat Peek!!!!");
                bestHeat = scoreManager.Heat * 50;
            }
        } 
        else if(normalMode)
        {
            if (scoreManager.Level > bestLevel)
            {
                CallText("New Heat Peek!!!!");
                bestLevel = scoreManager.Level * 50;
            }
        }
    }

    void GameBuildSeial()
    {
        if(build.gameObject.activeInHierarchy)
            build.text = "Build 2018.2.10.11.11";
    }

    void CallText(string txt)
    {
      powerup.DisplayText(txt);
    }
}
