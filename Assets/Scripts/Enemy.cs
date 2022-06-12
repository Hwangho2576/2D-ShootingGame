using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public GameManager gameManager;
    public ObjectManager objectManager;
    public AudioManager audioManager;
    public Sprite[] sprites;
    public Player player;
    public int speed;
    public int initHealth;
    public float curShotDelay;
    public float maxShotDelay;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    int curHealth;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyName == "B")
            anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        curHealth = initHealth;

        if(enemyName == "B")
        {
            Invoke("Stop", 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyName == "B")
            return;

        Fire();
    }

    void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        curPatternCount = 0;
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }

    }

    void FireFoward()
    {
        if (curHealth <= 0) return;

        GameObject bulletL = objectManager.makeObject("EnemyBulletC");
        GameObject bulletLL = objectManager.makeObject("EnemyBulletC");
        GameObject bulletR = objectManager.makeObject("EnemyBulletC");
        GameObject bulletRR = objectManager.makeObject("EnemyBulletC");

        bulletL.transform.position = transform.position + Vector3.left * 0.2f;
        bulletLL.transform.position = transform.position + Vector3.left * 0.6f;
        bulletR.transform.position = transform.position + Vector3.right * 0.2f;
        bulletRR.transform.position = transform.position + Vector3.right * 0.6f;

        Rigidbody2D bulletLRigid = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D bulletLLRigid = bulletLL.GetComponent<Rigidbody2D>();
        Rigidbody2D bulletRRigid = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D bulletRRRigid = bulletRR.GetComponent<Rigidbody2D>();

        bulletLRigid.AddForce(Vector2.down * 5, ForceMode2D.Impulse);
        bulletLLRigid.AddForce(Vector2.down * 5, ForceMode2D.Impulse);
        bulletRRigid.AddForce(Vector2.down * 5, ForceMode2D.Impulse);
        bulletRRRigid.AddForce(Vector2.down * 5, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireFoward", 2);
        else 
            Invoke("Think", 3);
    }

    void FireShot()
    {
        if (curHealth <= 0) return;

        for (int i = 0; i < 5; i++)
        {
            GameObject bulletS = objectManager.makeObject("EnemyBulletD");
            bulletS.transform.position = transform.position;
            Rigidbody2D bulletSRigid = bulletS.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 randVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += randVec;
            bulletSRigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }
        
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 2);
        else
            Invoke("Think", 3);
    }

    void FireArc()
    {
        if (curHealth <= 0) return;

        GameObject bulletS = objectManager.makeObject("EnemyBulletD");
        bulletS.transform.position = transform.position;
        bulletS.transform.rotation = Quaternion.identity;

        Rigidbody2D bulletSRigid = bulletS.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / maxPatternCount[patternIndex]), -1);
        bulletSRigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }

    void FireAround()
    {
        if (curHealth <= 0) return;

        int bulletCountA = 50;
        int bulletCountB = 40;
        int bulletCount = curPatternCount % 2 == 0 ? bulletCountA : bulletCountB;

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bulletS = objectManager.makeObject("EnemyBulletA");
            bulletS.transform.position = transform.position;
            bulletS.transform.rotation = Quaternion.identity;

            Rigidbody2D bulletSRigid = bulletS.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
            bulletSRigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * i / bulletCount + Vector3.forward * 90;
            bulletS.transform.Rotate(rotVec);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.7f);
        else
            Invoke("Think", 3);
    }

    void Fire()
    {
        curShotDelay += Time.deltaTime;

        if (curShotDelay < maxShotDelay)
            return;

        switch (enemyName)
        {
            case "S":
                GameObject bulletS = objectManager.makeObject("EnemyBulletA");
                bulletS.transform.position = transform.position;
                Rigidbody2D bulletSRigid = bulletS.GetComponent<Rigidbody2D>();
                Vector3 dirVec = player.transform.position - transform.position;
                bulletSRigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
                break;

            case "L":
                GameObject bulletL = objectManager.makeObject("EnemyBulletB");
                GameObject bulletR = objectManager.makeObject("EnemyBulletB");

                bulletL.transform.position = transform.position + Vector3.left * 0.3f;
                bulletR.transform.position = transform.position + Vector3.right * 0.3f;

                Rigidbody2D bulletLRigid = bulletL.GetComponent<Rigidbody2D>();
                Rigidbody2D bulletRRigid = bulletR.GetComponent<Rigidbody2D>();

                Vector3 dirVecL = player.transform.position - transform.position + Vector3.left * 0.3f;
                Vector3 dirVecR = player.transform.position - transform.position + Vector3.right * 0.3f;

                bulletLRigid.AddForce(dirVecL.normalized * 5, ForceMode2D.Impulse);
                bulletRRigid.AddForce(dirVecL.normalized * 5, ForceMode2D.Impulse);
                break;
        }

        curShotDelay = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "BorderBullet" && enemyName != "B")
        {
            gameObject.SetActive(false);
        }
        if(collision.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnDamaged(bullet.dmg);
            collision.gameObject.SetActive(false);
        }
    }

    public void OnDamaged(int dmg)
    {
        if (curHealth <= 0)
            return;

        curHealth -= dmg;

        audioManager.PlayAudio("Hit");

        if (enemyName == "B")
        {
            anim.SetTrigger("OnDamaged");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        if (curHealth <= 0)
        {
            int itemRatio = Random.Range(0, 10);

            if(itemRatio < 4)
            {
                GameObject itemCoin = objectManager.makeObject("ItemCoin");
                itemCoin.transform.position = transform.position;
                itemCoin.SetActive(true);
            }
            else if(itemRatio < 7)
            {
                GameObject itemPower = objectManager.makeObject("ItemPower");
                itemPower.transform.position = transform.position;
                itemPower.SetActive(true);
            }
            else if(itemRatio < 9)
            {
                GameObject itemBoom = objectManager.makeObject("ItemBoom");
                itemBoom.transform.position = transform.position;
                itemBoom.SetActive(true);
            }

            player.score += 100;

            if (enemyName == "B")
            {
                CancelInvoke("Think");
                gameManager.StageEnd();
            }
            gameManager.CallExplosion(transform.position, enemyName);
            gameObject.SetActive(false);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }
}
