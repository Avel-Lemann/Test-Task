using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRandomize : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject cubePrefab;
    [SerializeField, Range(0f, 1f)] private float grayProbability = 0.5f;
    [SerializeField] private Vector3 cubeSize = new Vector3(0.5f, 0.5f, 0.5f);

    [Header("Spawn Settings")]
    [SerializeField] private float spacing = 2f;
    [SerializeField] private int numCubesPerLine = 5;
    [SerializeField] private GameObject[] SpawnParent;
    private int numberParent = 0;

    void Start()
    {
        SpawnCubesInLine(Vector3.left * spacing, numCubesPerLine);
        SpawnCubesInLine(Vector3.zero, numCubesPerLine);
        SpawnCubesInLine(Vector3.right * spacing, numCubesPerLine);
    }

    void SpawnCubesInLine(Vector3 offset, int count)
    {  
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = transform.position + offset + i * Vector3.forward * spacing;
            GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity, SpawnParent[numberParent].transform);
            numberParent++;

            MeshRenderer renderer = newCube.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Color color = Random.value < grayProbability ? Color.gray : Color.white;
                renderer.material.color = color;
            }
            newCube.transform.localScale = cubeSize;

        }
    }
}
