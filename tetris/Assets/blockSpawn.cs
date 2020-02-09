using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockSpawn : MonoBehaviour
{
    public GameObject[] Blocks;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBlocks();
    }

    // Update is called once per frame
    public void SpawnNewBlocks()
    {
        Instantiate(Blocks[Random.Range(0, Blocks.Length)], transform.position, Quaternion.identity);
    }
}
