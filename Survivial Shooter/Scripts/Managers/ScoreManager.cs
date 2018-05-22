using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int score;

    private PowerSpawner powerup;
    private EnemyAttack[] enemyAttack;
    private EnemyManager[] enemyManager;
    private Text text;

    private int currentLevel;
    private int nextLevel;
    private int heat;
    private float resetTime;
    private bool levelUP;
    private bool heatIncrease;
    private GameObject[] enemies;
    private NavMeshAgent[] enemyNavMesh;

    public Text levelText;
    public float animSpeed = 5f;
    public float animFadeTime = 4f;
    public float scoreLevelChange = 300f;
    public float dTF = 0.5f;
    public int heatLevel = 0;

    public float highTextAlpha;
    public float lowTextAlpha;

    public delegate void HeatUp();
    public static event HeatUp HeatIncrease;

    public static int HEATLEVELBOOST = 0;

    public Text bestTime, bestTimeNormal, bestScoreNormal, bestScoreHeat, LKS, LKSNormal, HZK, HZKNormal, topLevel, topHeat;

    void Awake ()
    {
        text = GetComponent <Text> ();
        powerup = GameObject.FindGameObjectWithTag(Tags.powerUP).GetComponent<PowerSpawner>();
        enemyManager = GameObject.FindGameObjectWithTag(Tags.enemyManager).GetComponents<EnemyManager>();
        scoreLevelChange = transform.parent.GetComponent<GameInstructions>().ScoreLevelChange;
    }

    void Start()
    {
        score = 0;
        currentLevel = 0;
        heat = 1;
        nextLevel = currentLevel + 1;
    }


    void Update ()
    {
        text.text = "Score: " + score;

        resetTime += Time.deltaTime;

        if(GameInstructions.GAME_MODE == Literals.GAME_MODES.HEAT_GAME_MODE)
        {
            if ((int)resetTime == 59)
            {
                GameInstructions.UpdateEnemyHeat();
            }
            else if ((int)resetTime == 60)
            {
                resetTime = 0;
                IncreaseHeat();
            }

            levelText.text = "Heat " + heat.ToString();

            GameInstructions.ENEMY_HEAT = heat;
        }  
        else
        {
            if (score / nextLevel >= scoreLevelChange)
            {
                levelUP = true;

                if (powerup != null)
                    powerup.DisplayText("Level UP");

                LevelUP();
            }

            levelText.text = "Level " + nextLevel.ToString();
        }      
    }

    void LevelUP()
    {
        if(levelUP)
        {
            if(HEATLEVELBOOST < 150)
            {
                HEATLEVELBOOST += 20;
            }
            
            nextLevel++;
            foreach(EnemyManager em in enemyManager)
            {
                if(em.spawnTime > 1.3f)
                    em.spawnTime -= 0.3f;
            }

            if(GameInstructions.GAME_MODE == Literals.GAME_MODES.SURVIVAL_GAME_MODE)
            {
                TimeClass.timeScale += dTF;
            }

            GetEnemyStatus();
            AddLevelStatus();

            levelUP = false;
        }     
    }

    void GetEnemyStatus()
    {
        enemies = GameObject.FindGameObjectsWithTag(Tags.enemy);
        enemyNavMesh = new NavMeshAgent[enemies.Length];
        enemyAttack = new EnemyAttack[enemies.Length];

        for (int i = 0; i < enemyNavMesh.Length && i < enemyAttack.Length; i++)
        {
            if (enemies[i].activeInHierarchy)
            {
                enemyNavMesh[i] = enemies[i].GetComponent<NavMeshAgent>();
                enemyAttack[i] = enemies[i].GetComponent<EnemyAttack>();
            }    
        }
    }

    void AddLevelStatus()
    {
        foreach(NavMeshAgent nav in enemyNavMesh)
        {
            nav.speed = nav.speed + 1;
        }

        foreach(EnemyAttack enemyAttck in enemyAttack)
        {
            enemyAttck.attackDamage += 1;
        }
    }

    void IncreaseHeat()
    {
        powerup.DisplayText("Heat Increased");

        foreach (EnemyManager enemyMGMT in enemyManager)
        {
            if(enemyMGMT.spawnTime >= 1.1)
            {
                enemyMGMT.spawnTime -= GameInstructions.timeBtwSpawn;
            }          
        }    

        if(GameInstructions.DEFAULT_ENEMY_SPEED <= 8)
        {
            GameInstructions.DEFAULT_ENEMY_SPEED += GameInstructions.ENEMY_SPEED_INCREASE;
        }

        enemies = GameObject.FindGameObjectsWithTag(Tags.enemy);

        foreach(GameObject enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.startingHealth += 10;
        }

        HEATLEVELBOOST += 100;

        heat++;

        if(HeatIncrease != null)
        {
            HeatIncrease();
        }
    }

    public void SetHighScores()
    {
        if(PlayerPrefs.HasKey(Literals.Saves.First_SAVE))
        {
            GameInstructions.LoadSaves();

            bestTime.text = GameInstructions.HighScoresValues.bestTime;
            bestTimeNormal.text = GameInstructions.HighScoresValues.bestTimeNormal;
            bestScoreNormal.text = GameInstructions.HighScoresValues.bestScoreNormal.ToString();
            bestScoreHeat.text = GameInstructions.HighScoresValues.bestScoreHeat.ToString();
            LKS.text = GameInstructions.HighScoresValues.longestKS.ToString();
            LKSNormal.text = GameInstructions.HighScoresValues.longestKSNormal.ToString();
            HZK.text = GameInstructions.HighScoresValues.HZK.ToString();
            HZKNormal.text = GameInstructions.HighScoresValues.HZKNormal.ToString();
            topLevel.text = GameInstructions.HighScoresValues.topLevel.ToString();
            topHeat.text = GameInstructions.HighScoresValues.topHeat.ToString();
        }    
    }

    public void ResetAll()
    {
        bestTime.text = "00:00";
        bestTimeNormal.text = "00:00";
        bestScoreNormal.text = "0";
        bestScoreHeat.text = "0";
        LKS.text = "0";
        LKSNormal.text = "0";
        HZK.text = "0";
        HZKNormal.text = "0";
        topLevel.text = "0";
        topHeat.text = "0";
    }

    public int Level
    {
        get { return nextLevel; }
    }

    public int Heat
    {
        get { return heat; }
    }

    public int Score
    {
        get { return score; }
    }
}
