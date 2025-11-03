using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public Transform topPipe;
    public Transform bottomPipe;
    public float gap = 3f;

    [SerializeField] private float speed = 2f;
    private float destroyX = -8f;

    void Update()
    {
        Moving();
    }

    public void Setup(float gapValue)
    {
        gap = gapValue;
        topPipe.position += Vector3.up * gap / 2;
        bottomPipe.position += Vector3.down * gap / 2;
    }

    private void Moving()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
