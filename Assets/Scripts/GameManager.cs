using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ObjectManager objectManager;
    public GameObject[] spawns;
    public float curSpawnDelay;
    public float maxSpawnDelay;

    public Text scoreText;
    public GameObject gameOverSet;
    public Image[] lifeIcon;
    public Image[] boomIcon;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;

    List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    string[] enemys;
    public int stage;
    public int maxStage;

    public AudioManager audioManager;

    bool infinityMode = false;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemys = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB"};
        StageStart();
    }

    void Update()
    {
        updateScoreText();
        updateLifeIcon();
        updateBoomIcon();
        SpawnEnemy();
    }

    public void StageStart()
    {
        stageAnim.SetTrigger("On");

        if (infinityMode)
        {
            stageAnim.GetComponent<Text>().text = "Infinity " + "\nStart";
            clearAnim.GetComponent<Text>().text = "Infinity " + "\nClear";

            spawnEnd = false;
            maxSpawnDelay = 10;
        }


        else
        {
            stageAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart";
            clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nClear";
            ReadSpawnFile();
        }

        fadeAnim.SetTrigger("Out");
    }
    public void StageEnd()
    {
        clearAnim.SetTrigger("On");
        stage++;
        fadeAnim.SetTrigger("In");

        if (stage > maxStage)
            infinityMode = true;
        
        Invoke("StageStart", 3.5f);
    }
    void ReadSpawnFile()
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        TextAsset textFile = Resources.Load("Stage" + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            if (line == null)
                break;
            
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(",")[0]);
            spawnData.type = line.Split(",")[1];
            spawnData.point = int.Parse(line.Split(",")[2]);

            spawnList.Add(spawnData);
        }

        stringReader.Close();
        maxSpawnDelay = spawnList[0].delay;

    }

    void SpawnEnemy()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay < maxSpawnDelay || spawnEnd)
            return;

        GameObject enemy;
        int enemyPoint;
        int enemyIndex;

        /*랜덤 소환*/
        if (infinityMode)
        {
            maxSpawnDelay = Random.Range(0f,3f);
            enemyPoint = Random.Range(0, 8);
            enemyIndex = Random.Range(0, 3);
            
        }

        /*메모장 소환*/
        else
        {
            enemyPoint = spawnList[spawnIndex].point;
            enemyIndex = 0;

            switch (spawnList[spawnIndex].type)
            {
                case "S":
                    enemyIndex = 0;
                    break;

                case "M":
                    enemyIndex = 1;
                    break;

                case "L":
                    enemyIndex = 2;
                    break;

                case "B":
                    enemyIndex = 3;
                    break;
            }

        }

        enemy = objectManager.makeObject(enemys[enemyIndex]);

        enemy.transform.position = spawns[enemyPoint].transform.position;
        enemy.transform.rotation = Quaternion.identity;

        Rigidbody2D enemyRigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.gameManager = this;
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;
        enemyLogic.audioManager = audioManager;
        
        if(enemyPoint == 4 || enemyPoint == 6)
        {
            enemyRigid.velocity = new Vector2(enemyLogic.speed, -1);
            enemy.transform.Rotate(Vector3.forward * 90);
        }
        else if(enemyPoint == 5 || enemyPoint == 7)
        {
            enemyRigid.velocity = new Vector2(-enemyLogic.speed, -1);
            enemy.transform.Rotate(Vector3.back * 90);
        }
        else
        {
            enemyRigid.velocity = Vector2.down * enemyLogic.speed;
        }

        if (enemyLogic.enemyName == "B")
            enemy.transform.position = new Vector3(0, spawns[enemyPoint].transform.position.y, 0);

        /*메모장 소환*/
        if (!infinityMode)
        {
            spawnIndex++;
            if (spawnIndex == spawnList.Count)
            {
                spawnEnd = true;
                return;
            }
            maxSpawnDelay = spawnList[spawnIndex].delay;
        }
        
        curSpawnDelay = 0;
    }

    public void reSpawnPlayer()
    {
        Invoke("reSpawnPlayerExe", 2f);
    }

    void reSpawnPlayerExe()
    {
        player.transform.position = player.initPosition;

        player.gameObject.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.makeObject("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        audioManager.PlayAudio("Explosion");

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);

    }

    // UI 관련 함수
    public void updateScoreText()
    {
        scoreText.text = string.Format("{0:n0}", player.score);
    }

    public void updateLifeIcon()
    {
        for (int i = 0; i < 3; i++)
        {
            lifeIcon[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < player.life; i++)
        {
            lifeIcon[i].color = new Color(1, 1, 1, 150f/255f);
        }
    }

    public void updateBoomIcon()
    {
        for (int i = 0; i < 3; i++)
        {
            boomIcon[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i <player.boom; i++)
        {
            boomIcon[i].color = new Color(1, 1, 1, 150f/255f);
        }
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }
}
