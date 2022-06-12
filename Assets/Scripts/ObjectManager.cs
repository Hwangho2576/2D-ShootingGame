using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject playerBulletPrefabA;
    public GameObject playerBulletPrefabB;
    public GameObject enemyPrefabS;
    public GameObject enemyPrefabM;
    public GameObject enemyPrefabL;
    public GameObject enemyPrefabB;
    public GameObject enemyBulletPrefabA;
    public GameObject enemyBulletPrefabB;
    public GameObject enemyBulletPrefabC;
    public GameObject enemyBulletPrefabD;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject followerBulletPrefab;
    public GameObject explosionPrefab;

    GameObject[] playerBulletA;
    GameObject[] playerBulletB;
    GameObject[] enemyS;
    GameObject[] enemyM;
    GameObject[] enemyL;
    GameObject[] enemyB;
    GameObject[] enemyBulletA;
    GameObject[] enemyBulletB;
    GameObject[] enemyBulletC;
    GameObject[] enemyBulletD;
    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;
    GameObject[] followerBullet;
    GameObject[] explosion;

    GameObject[] targetPool;

    void Awake()
    {
        playerBulletA = new GameObject[100];
        playerBulletB = new GameObject[100];
        enemyS = new GameObject[100];
        enemyM = new GameObject[100];
        enemyL = new GameObject[100];
        enemyB = new GameObject[2];
        enemyBulletA = new GameObject[100];
        enemyBulletB = new GameObject[100];
        enemyBulletC = new GameObject[100];
        enemyBulletD = new GameObject[100];
        itemCoin = new GameObject[30];
        itemPower = new GameObject[20];
        itemBoom = new GameObject[20];
        followerBullet = new GameObject[50];
        explosion = new GameObject[50];

        Generate();
    }


    void Generate()
    {
        for(int i = 0; i < playerBulletA.Length; i++)
        {
            playerBulletA[i] = Instantiate(playerBulletPrefabA);
            playerBulletA[i].SetActive(false);
        }

        for(int i = 0; i <playerBulletB.Length; i++)
        {
            playerBulletB[i] = Instantiate(playerBulletPrefabB);
            playerBulletB[i].SetActive(false);
        }
        for (int i = 0; i < enemyS.Length; i++)
        {
            enemyS[i] = Instantiate(enemyPrefabS);
            enemyS[i].SetActive(false);
        }
        for (int i = 0; i < enemyM.Length; i++)
        {
            enemyM[i] = Instantiate(enemyPrefabM);
            enemyM[i].SetActive(false);
        }
        for (int i = 0; i < enemyL.Length; i++)
        {
            enemyL[i] = Instantiate(enemyPrefabL);
            enemyL[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletA.Length; i++)
        {
            enemyBulletA[i] = Instantiate(enemyBulletPrefabA);
            enemyBulletA[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletB.Length; i++)
        {
            enemyBulletB[i] = Instantiate(enemyBulletPrefabB);
            enemyBulletB[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletC.Length; i++)
        {
            enemyBulletC[i] = Instantiate(enemyBulletPrefabC);
            enemyBulletC[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletD.Length; i++)
        {
            enemyBulletD[i] = Instantiate(enemyBulletPrefabD);
            enemyBulletD[i].SetActive(false);
        }

        for (int i = 0; i < itemCoin.Length; i++)
        {
            itemCoin[i] = Instantiate(itemCoinPrefab);
            itemCoin[i].SetActive(false);
        }

        for (int i = 0; i < itemBoom.Length; i++)
        {
            itemBoom[i] = Instantiate(itemBoomPrefab);
            itemBoom[i].SetActive(false);
        }

        for (int i = 0; i < itemPower.Length; i++)
        {
            itemPower[i] = Instantiate(itemPowerPrefab);
            itemPower[i].SetActive(false);
        }

        for (int i = 0; i < followerBullet.Length; i++)
        {
            followerBullet[i] = Instantiate(followerBulletPrefab);
            followerBullet[i].SetActive(false);
        }

        for (int i = 0; i < enemyB.Length; i++)
        {
            enemyB[i] = Instantiate(enemyPrefabB);
            enemyB[i].SetActive(false);
        }

        for (int i = 0; i < enemyB.Length; i++)
        {
            enemyB[i] = Instantiate(enemyPrefabB);
            enemyB[i].SetActive(false);
        }

        for(int i = 0; i < explosion.Length; i++)
        {
            explosion[i] = Instantiate(explosionPrefab);
            explosion[i].SetActive(false);
        }
    }
   
    public GameObject makeObject(string objName)
    {
        switch(objName)
        {
            case "PlayerBulletA":
                targetPool = playerBulletA;
                break;

            case "PlayerBulletB":
                targetPool = playerBulletB;
                break;

            case "EnemyS":
                targetPool = enemyS;
                break;

            case "EnemyM":
                targetPool = enemyM;
                break;

            case "EnemyL":
                targetPool = enemyL;
                break;

            case "EnemyBulletA":
                targetPool = enemyBulletA;
                break;

            case "EnemyBulletB":
                targetPool = enemyBulletB;
                break;
            
            case "EnemyBulletC":
                targetPool = enemyBulletC;
                break;

            case "EnemyBulletD":
                targetPool = enemyBulletD;
                break;

            case "ItemCoin":
                targetPool = itemCoin;
                break;

            case "ItemPower":
                targetPool = itemPower;
                break;

            case "ItemBoom":
                targetPool = itemBoom;
                break;

            case "FollowerBullet":
                targetPool = followerBullet;
                break;

            case "EnemyB":
                targetPool = enemyB;
                break;

            case "Explosion":
                targetPool = explosion;
                break;

        }


        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }

    public GameObject[] GetObjectPool(string poolName)
    {

        switch (poolName)
        {
            case "PlayerBulletA":
                targetPool = playerBulletA;
                break;

            case "PlayerBulletB":
                targetPool = playerBulletB;
                break;

            case "EnemyS":
                targetPool = enemyS;
                break;

            case "EnemyM":
                targetPool = enemyM;
                break;

            case "EnemyL":
                targetPool = enemyL;
                break;

            case "EnemyBulletA":
                targetPool = enemyBulletA;
                break;

            case "EnemyBulletB":
                targetPool = enemyBulletB;
                break;

            case "EnemyBulletC":
                targetPool = enemyBulletC;
                break;

            case "EnemyBulletD":
                targetPool = enemyBulletD;
                break;

            case "ItemCoin":
                targetPool = itemCoin;
                break;

            case "ItemPower":
                targetPool = itemPower;
                break;

            case "ItemBoom":
                targetPool = itemBoom;
                break;

            case "FollowerBullet":
                targetPool = followerBullet;
                break;

            case "EnemyB":
                targetPool = enemyB;
                break;

            case "Explosion":
                targetPool = explosion;
                break;
        }

        return targetPool;
    }
}
