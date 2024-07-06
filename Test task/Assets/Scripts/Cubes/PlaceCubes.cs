using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCubes : MonoBehaviour
{
    [SerializeField] private GameObject pointToCheck;
    [SerializeField] private CubeComparer CubeComparer;
    private bool trueCubeStay;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractiveCube"))
        {
            GameObject cube = other.gameObject;
            cube.transform.position = transform.position;
            cube.transform.rotation = transform.rotation;
            cube.transform.SetParent(transform);

            Rigidbody cubeRigidbody = cube.GetComponent<Rigidbody>();
            if (cubeRigidbody != null)
            {
                cubeRigidbody.isKinematic = true;
            }

            CompareCube(cube);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractiveCube") && trueCubeStay)
        {
            trueCubeStay = false;
            StartCoroutine(CubeComparer.AddIdenticalCubes(trueCubeStay));
        }
    }
    private void CompareCube(GameObject cube)
    {
        if (cube.GetComponent<MeshRenderer>().material.color == pointToCheck.GetComponentInChildren<MeshRenderer>().material.color)
        {
            trueCubeStay = true;
            StartCoroutine(CubeComparer.AddIdenticalCubes(trueCubeStay));
        }
        else
        {
            Debug.Log("Не одинаковый материал");
        }
    }
}
