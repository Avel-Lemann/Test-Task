using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPosition;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Host создан!!!");
            PhotonNetwork.Instantiate("Host", Vector3.zero, Quaternion.identity);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Player создан!!!");
            PhotonNetwork.Instantiate(player.name, spawnPosition.position, Quaternion.identity);
        }
    }
}
