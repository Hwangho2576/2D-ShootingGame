using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public ObjectManager objectManager;
    public float maxShotDelay;
    public float curShotDelay;
    public int followerDelay;
    public GameObject parent;
    Queue<Vector3> parentPos;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }
    void Update()
    {
        Follow();
        Fire();
    }
    void Follow()
    {
        if(!parentPos.Contains(parent.transform.position))
            parentPos.Enqueue(parent.transform.position);

        if (parentPos.Count > followerDelay)
            transform.position = parentPos.Dequeue();
        else if (parentPos.Count < followerDelay)
            transform.position = parent.transform.position;
    }

    void Fire()
    {
        curShotDelay += Time.deltaTime;

        if (curShotDelay < maxShotDelay)
            return;


        GameObject bulletC = objectManager.makeObject("FollowerBullet");
        bulletC.transform.position = transform.position;
        Rigidbody2D bulletCRigid = bulletC.GetComponent<Rigidbody2D>();
        bulletCRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }
}
