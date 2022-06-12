using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public float speed;
    public GameObject[] elements;
    public int startIndex;
    public int endIndex;
    float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if(elements[endIndex].transform.position.y < -viewHeight)
        {
            elements[endIndex].transform.localPosition = elements[startIndex].transform.localPosition + Vector3.up * viewHeight;

            startIndex = endIndex;
            endIndex--;
            if (endIndex < 0) endIndex = elements.Length - 1;

        }

    }
}
