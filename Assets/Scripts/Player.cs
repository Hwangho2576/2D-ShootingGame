using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public ObjectManager objectManager;
    public GameObject boomEffect;
    public GameObject[] follower;
    public int power;
    public int maxPower;
    public int boom;
    public int maxBoom;
    public int speed;
    public int life;
    public int score;
    public Vector3 initPosition;
    public float curShotDelay;
    public float maxShotDelay;

    Animator anim;
    SpriteRenderer spriteRenderer;
    bool isTouchTop;
    bool isTouchLeft;
    bool isTouchRight;
    bool isTouchBottom;
    bool isBoomTime;
    bool isDamaged;
    bool isRespawnTime;

    public AudioManager audioManager;

    bool[] joyControl = new bool[9];
    bool isControl;
    bool isButtonB;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initPosition = transform.position;
    }


    private void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 3);
    }
    void Update()
    {
        Move();
        Fire();
        Boom();
    }
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }


        if ((h == 1 && isTouchRight) || (h == -1 && isTouchLeft) || !isControl)
            h = 0;

        if ((v == 1 && isTouchTop) || (v == -1 && isTouchBottom) || !isControl)
            v = 0;

        Vector3 curPosition = transform.position;

        transform.position = curPosition + new Vector3(h, v, 0) * speed * Time.deltaTime;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")
            || joyControl[0] || joyControl[2] || joyControl[3] 
            || joyControl[5] || joyControl[6] || joyControl[8])
            anim.SetInteger("Input", (int)h);
    }

    public void JoyPanel(int type)
    {
        for(int i = 0; i <9; i++)
        {
            joyControl[i] = i == type;
        }
    }

    public void JoyDown()
    {
        isControl = true;
    }
    public void JoyUp()
    {
        isControl = false;
    }

    public void ButtonDownB()
    {
        isButtonB = true;
    }

    private void Fire()
    {
        curShotDelay += Time.deltaTime;

        if (curShotDelay < maxShotDelay)
            return;

        audioManager.PlayAudio("Fire");

        switch (power)
        {
            case 1:
                GameObject bulletC = objectManager.makeObject("PlayerBulletA");
                bulletC.transform.position = transform.position;
                Rigidbody2D bulletCRigid = bulletC.GetComponent<Rigidbody2D>();
                bulletCRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                break;

            case 2:
                GameObject bulletL = objectManager.makeObject("PlayerBulletA");
                bulletL.transform.position = transform.position - new Vector3(-0.15f, 0, 0);
                Rigidbody2D bulletLRigid = bulletL.GetComponent<Rigidbody2D>();
                bulletLRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletR = objectManager.makeObject("PlayerBulletA");
                bulletR.transform.position = transform.position - new Vector3(0.15f, 0, 0);
                Rigidbody2D bulletRRigid = bulletR.GetComponent<Rigidbody2D>();
                bulletRRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                break;

            case 3:
            case 4:
            case 5:
            case 6:

                GameObject bulletCC = objectManager.makeObject("PlayerBulletB");
                bulletCC.transform.position = transform.position;
                Rigidbody2D bulletCCRigid = bulletCC.GetComponent<Rigidbody2D>();
                bulletCCRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletLL = objectManager.makeObject("PlayerBulletA");
                bulletLL.transform.position = transform.position - new Vector3(-0.25f, 0, 0);
                Rigidbody2D bulletLLRigid = bulletLL.GetComponent<Rigidbody2D>();
                bulletLLRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletRR = objectManager.makeObject("PlayerBulletA");
                bulletRR.transform.position = transform.position - new Vector3(0.25f, 0, 0);
                Rigidbody2D bulletRRRigid = bulletRR.GetComponent<Rigidbody2D>();
                bulletRRRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }
        curShotDelay = 0;
    }
    void OnDmaged()
    {
        if (isDamaged == true)
            return;

        if (isRespawnTime == true)
            return;

        isDamaged = true;
        life--;

        gameManager.CallExplosion(transform.position, "P");

        if (life > 0)
            gameManager.reSpawnPlayer();
        else
            gameManager.GameOver();

        gameObject.SetActive(false);

        power = 1;

        for(int i = 0; i<follower.Length;i++)
        {
            if (follower[i].activeSelf)
                follower[i].SetActive(false);
        }

        isDamaged = false;
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }

    }

    void Boom()
    {
        //if (!Input.GetButton("Fire1"))
        //    return;

        if (!isButtonB) return;
        isButtonB = false;

        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boomEffect.SetActive(true);
        audioManager.PlayAudio("Boom");
        Invoke("OffBoomEffect", 3);

        boom--;

        isBoomTime = true;

        GameObject[] enemySPool = objectManager.GetObjectPool("EnemyS");
        GameObject[] enemyMPool = objectManager.GetObjectPool("EnemyM");
        GameObject[] enemyLPool = objectManager.GetObjectPool("EnemyL");
        GameObject[] enemyBPool = objectManager.GetObjectPool("EnemyB");
        GameObject[] enemyBulletAPool = objectManager.GetObjectPool("EnemyBulletA");
        GameObject[] enemyBulletBPool = objectManager.GetObjectPool("EnemyBulletB");
        GameObject[] enemyBulletCPool = objectManager.GetObjectPool("EnemyBulletC");
        GameObject[] enemyBulletDPool = objectManager.GetObjectPool("EnemyBulletD");

        for (int i = 0; i < enemySPool.Length; i++)
        {
            if (enemySPool[i].activeSelf)
            {
                Enemy enemySLogic = enemySPool[i].GetComponent<Enemy>();
                enemySLogic.OnDamaged(1000);
            }
        }
        for (int i = 0; i < enemyMPool.Length; i++)
        {
            if (enemyMPool[i].activeSelf)
            {
                Enemy enemyMLogic = enemyMPool[i].GetComponent<Enemy>();
                enemyMLogic.OnDamaged(1000);
            }
        }
        for (int i = 0; i < enemyLPool.Length; i++)
        {
            if (enemyLPool[i].activeSelf)
            {
                Enemy enemyLLogic = enemyLPool[i].GetComponent<Enemy>();
                enemyLLogic.OnDamaged(1000);
            }
        }
        for (int i = 0; i < enemyBPool.Length; i++)
        {
            if (enemyBPool[i].activeSelf)
            {
                Enemy enemyBLogic = enemyBPool[i].GetComponent<Enemy>();
                enemyBLogic.OnDamaged(1000);
            }
        }

        for (int i = 0; i < enemyBulletAPool.Length; i++)
        {
            if (enemyBulletAPool[i].activeSelf)
            {
                enemyBulletAPool[i].SetActive(false);
            }
        }
        for (int i = 0; i < enemyBulletBPool.Length; i++)
        {
            if (enemyBulletBPool[i].activeSelf)
            {
                enemyBulletBPool[i].SetActive(false);
            }
        }
        for (int i = 0; i < enemyBulletCPool.Length; i++)
        {
            if (enemyBulletCPool[i].activeSelf)
            {
                enemyBulletCPool[i].SetActive(false);
            }
        }
        for (int i = 0; i < enemyBulletDPool.Length; i++)
        {
            if (enemyBulletDPool[i].activeSelf)
            {
                enemyBulletDPool[i].SetActive(false);
            }
        }
    }
    void OffBoomEffect()
    {
        audioManager.StopAudio("Boom");
        isBoomTime = false;
        boomEffect.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Border")
        {
            switch(collision.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
            }    
        }

        else if(collision.tag == "EnemyBullet" || collision.tag == "Enemy")
        {
            OnDmaged();
        }

        else if(collision.tag == "Item")
        {
            Item itemLogic = collision.GetComponent<Item>();

            audioManager.PlayAudio("Item");

            switch(itemLogic.type)
            {
                case "Boom":
                    if (boom == maxBoom)
                        score += 500;
                    else 
                        boom++;

                    break;
                case "Coin":
                    score += 500;

                    break;
                case "Power":
                    if (power == maxPower)
                        score += 500;
                    else
                    {
                        power++;
                        if (power == 4) follower[0].SetActive(true);
                        if (power == 5) follower[1].SetActive(true);
                        if (power == 6) follower[2].SetActive(true);
                    }
                    break;
            }

            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            switch (collision.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
            }
        }
    }
}
