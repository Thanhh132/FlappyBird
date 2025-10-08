using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pipes prefab;
    [SerializeField] private float spawnRate = 4f;
    [SerializeField] private float minHeight = -0.8f;
    [SerializeField] private float maxHeight = 2.8f;
    public float minVerticalGap = 0.5f;
    public float maxVerticalGap = 1f;

    void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), 3f, spawnRate);
    }

    private void Spawn()
    {
        Pipes pipes = Instantiate(prefab, transform.position, Quaternion.identity);
        float randomGap = Random.Range(minVerticalGap, maxVerticalGap);
        pipes.Setup(randomGap); 
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }
}
