using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameInstructions : MonoBehaviour
{
    public static float timeBtwSpawn = 0.1f;
    public static bool DevMode = false;
    public static int[] RESET_TIME = {0, 3, 0, 1};
    public static string GAME_MODE = "SELECT";
    public static float DEFAULT_ENEMY_SPEED = 4f;
    public static float ENEMY_SPEED_INCREASE = 0.6f;
    public static int ENEMY_DAMAGE_INCREASE = 2;
    public static int ENEMY_HEAT = 1;
    public static bool IN_GAME = false;
    public static int SCORE_MULTIPLIER = 1;

    public static class HighScoresValues
    {
        public static string bestTime = "00:00";
        public static string bestTimeNormal = "00:00";
        public static int bestScoreNormal = 0;
        public static int bestScoreHeat = 0;
        public static int longestKS = 0;
        public static int longestKSNormal = 0;
        public static int HZK = 0;
        public static int HZKNormal = 0;
        public static int topLevel = 0;
        public static int topHeat = 0;
    }

    public bool developerMode;
    public GameObject devNotificator;
    public float ScoreLevelChange = 300f;

    int starterLayer = 0;

    private GameObject enemyManager;
    private PowerSpawner powerSpawner;
    private PlayerHealth playerHealth;
    private TimeClass timer;
    private EnemyManager[] em;
    private static ScoreManager scoreManager;
    private static GameObject[] enemies;
    private ArrayList dcUIs = new ArrayList();

    public static bool TIME_UP = false;

    private static TimeClass lastTimer;

    void OnEnable()
    {
        ScoreManager.HeatIncrease += UpdateEnemyDamage;
    }

    void OnDisable()
    {
        ScoreManager.HeatIncrease -= UpdateEnemyDamage;
    }

    void Awake()
    {
        DevMode = developerMode;

        starterLayer = LayerMask.NameToLayer("Starter");
        timer = GetComponent<TimeClass>();
        lastTimer = GetComponent<TimeClass>();
        enemyManager = GameObject.FindGameObjectWithTag(Tags.enemyManager);
        em = enemyManager.GetComponents<EnemyManager>();
        powerSpawner = GameObject.FindGameObjectWithTag(Tags.powerUP).GetComponent<PowerSpawner>();
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
        enemies = GameObject.FindGameObjectsWithTag(Tags.enemy);
        scoreManager = GetComponentInChildren<ScoreManager>();

        EnableEnemies(false);
        timer.enabled = false;
    }

    void Start()
    {

        devNotificator.SetActive(DevMode);
        
        RectTransform[] starters = GetComponentsInChildren<RectTransform>();

        foreach (RectTransform start in starters)
        {
            if (start.gameObject.layer == starterLayer)
            {
                start.gameObject.SetActive(false);
                dcUIs.Add(start);
            }         
        }

        timer.Hour = RESET_TIME[0];
        timer.Minute = RESET_TIME[1];
        timer.Seconds = RESET_TIME[2];
        TimeClass.timeScale = RESET_TIME[3];
    }

    public void ReactivateStarters()
    {
        foreach (object objs in dcUIs) 
        {
            if(objs is RectTransform)
            {
                RectTransform newObj = (RectTransform)objs;
                newObj.gameObject.SetActive(true);
            }
        }

        EnableEnemies(!DevMode);
        powerSpawner.enabled = true;
        timer.enabled = true;

        GetComponent<Animator>().SetTrigger("Selected");
    }

    void EnableEnemies(bool des)
    {
        foreach (EnemyManager enemyMGMT in em)
        {
            enemyMGMT.enabled = des;
        }
    }

    void ResetTimer()
    {
        if (GameInstructions.TIME_UP && !playerHealth.IsDead)
        {
            playerHealth.currentHealth = 0;

            GameInstructions.TIME_UP = false;

            TimeClass.timeScale = 0;
            timer.Hour = GameInstructions.RESET_TIME[0];
            timer.Minute = GameInstructions.RESET_TIME[1];
            timer.Seconds = GameInstructions.RESET_TIME[2];

            playerHealth.Death();
        }
    }

    public static void UpdateEnemyHeat()
    {
        enemies = GameObject.FindGameObjectsWithTag(Tags.enemy);

        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy)
            {
                EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack>();
                if (enemyAttack.IsVerified == false)
                {
                    enemyAttack.IsVerified = true;
                }

                enemyAttack.Heat();
            }
        }      
    }

    private void UpdateEnemyDamage()
    {
        GameInstructions.ENEMY_DAMAGE_INCREASE += 1;
    }

    public static void LoadSaves()
    {
        GameInstructions.HighScoresValues.bestTime = PlayerPrefs.GetString(Literals.Saves.BEST_TIME_HEAT);
        GameInstructions.HighScoresValues.bestTimeNormal = PlayerPrefs.GetString(Literals.Saves.BEST_TIME_NORMAL);
        GameInstructions.HighScoresValues.bestScoreNormal = PlayerPrefs.GetInt(Literals.Saves.BEST_SCORE_NORMAL);
        GameInstructions.HighScoresValues.bestScoreHeat = PlayerPrefs.GetInt(Literals.Saves.BEST_SCORE_HEAT);
        GameInstructions.HighScoresValues.longestKS = PlayerPrefs.GetInt(Literals.Saves.LONGEST_KILLSTREAK);
        GameInstructions.HighScoresValues.longestKSNormal = PlayerPrefs.GetInt(Literals.Saves.LONGEST_KILLSTREAK_NORMAL);
        GameInstructions.HighScoresValues.HZK = PlayerPrefs.GetInt(Literals.Saves.HIGHEST_ZOMBIES_KILLED);
        GameInstructions.HighScoresValues.HZKNormal = PlayerPrefs.GetInt(Literals.Saves.HIGHEST_ZOMBIES_KILLED_NORMAL);
        GameInstructions.HighScoresValues.topLevel = PlayerPrefs.GetInt(Literals.Saves.TOP_LEVEL);
        GameInstructions.HighScoresValues.topHeat = PlayerPrefs.GetInt(Literals.Saves.TOP_HEAT);
    }

    public static void SaveGame()
    {
        PlayerPrefs.SetInt(Literals.Saves.First_SAVE, 1);

        if(GameInstructions.GAME_MODE == Literals.GAME_MODES.NORNAL_GAME_MODE)  
        {   
            if(scoreManager.Score >= PlayerPrefs.GetInt(Literals.Saves.BEST_SCORE_NORMAL))
            {
                PlayerPrefs.SetInt(Literals.Saves.BEST_SCORE_NORMAL, scoreManager.Score);
            }

            PlayerPrefs.SetString(Literals.Saves.BEST_TIME_NORMAL, lastTimer.CompareTime(lastTimer.GetTime(), PlayerPrefs.GetString(Literals.Saves.BEST_TIME_NORMAL)));

            if (KillStreakManager.KillStreak >= PlayerPrefs.GetInt(Literals.Saves.LONGEST_KILLSTREAK_NORMAL))
            {
                PlayerPrefs.SetInt(Literals.Saves.LONGEST_KILLSTREAK_NORMAL, KillStreakManager.KillStreak);
            }

            if (EnemyHealth.enemiesKilledCounter >= PlayerPrefs.GetInt(Literals.Saves.HIGHEST_ZOMBIES_KILLED_NORMAL))
            {
                PlayerPrefs.SetInt(Literals.Saves.HIGHEST_ZOMBIES_KILLED_NORMAL, EnemyHealth.enemiesKilledCounter);
            }
            
            if(scoreManager.Level >= PlayerPrefs.GetInt(Literals.Saves.TOP_LEVEL))
            {
                PlayerPrefs.SetInt(Literals.Saves.TOP_LEVEL, scoreManager.Level);
            }           
        }          
        else
        {
            if(scoreManager.Score >= PlayerPrefs.GetInt(Literals.Saves.BEST_SCORE_HEAT))
            {
                PlayerPrefs.SetInt(Literals.Saves.BEST_SCORE_HEAT, scoreManager.Score);
            }

            PlayerPrefs.SetString(Literals.Saves.BEST_TIME_HEAT, lastTimer.CompareTime(lastTimer.GetTime(), PlayerPrefs.GetString(Literals.Saves.BEST_TIME_HEAT)));

            if (KillStreakManager.KillStreak >= PlayerPrefs.GetInt(Literals.Saves.LONGEST_KILLSTREAK))
            {
                PlayerPrefs.SetInt(Literals.Saves.LONGEST_KILLSTREAK, KillStreakManager.KillStreak);
            }

            if (EnemyHealth.enemiesKilledCounter >= PlayerPrefs.GetInt(Literals.Saves.HIGHEST_ZOMBIES_KILLED))
            {
                PlayerPrefs.SetInt(Literals.Saves.HIGHEST_ZOMBIES_KILLED, EnemyHealth.enemiesKilledCounter);
            }

            if (scoreManager.Heat >= PlayerPrefs.GetInt(Literals.Saves.TOP_HEAT))
            {
                PlayerPrefs.SetInt(Literals.Saves.TOP_HEAT, scoreManager.Heat);
            } 
        }

        PlayerPrefs.Save();
    }

    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        scoreManager.ResetAll();
    }

    public static void ResetInstructions()
    {
        EnemyHealth.enemiesKilledCounter = 0;
        GameInstructions.DEFAULT_ENEMY_SPEED = 3f;
        GameInstructions.ENEMY_SPEED_INCREASE = 0.6f;
        GameInstructions.ENEMY_DAMAGE_INCREASE = 2;
        GameInstructions.ENEMY_HEAT = 1;
        GameInstructions.IN_GAME = false;
        ScoreManager.HEATLEVELBOOST = 0;
    }
}