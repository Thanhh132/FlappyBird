using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawn : MonoBehaviour
{
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float minHeight = -1f;
    [SerializeField] private float maxHeight = 1f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), 3f, spawnRate);
    }

    private void Spawn()
    {
        float spawnYPosition = Random.Range(minHeight, maxHeight);
        Instantiate(pipePrefab, new Vector3(transform.position.x, spawnYPosition, 0), Quaternion.identity);
    }
}
