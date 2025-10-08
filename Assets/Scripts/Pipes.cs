using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public Transform topPipe;
    public Transform bottomPipe;
    public float gap = 3f;
    [SerializeField] private float speed = 2f;

    private float destroyX = -8f; // vị trí x để hủy pipe

    void Start()
    {
        // Có thể bỏ leftEdge nếu không dùng nữa
    }

    public void Setup(float gapValue)
    {
        gap = gapValue;
        topPipe.position += Vector3.up * gap / 2;
        bottomPipe.position += Vector3.down * gap / 2;
    }

    void Update()
    {
        Moving();
    }

    private void Moving()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Nếu Pipe đi qua bên trái màn hình thì hủy
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
