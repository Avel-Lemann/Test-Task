using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloseRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject host;

    private void Start()
    {
        StartCoroutine(ServerActivityCheck());
    }

    private IEnumerator ServerActivityCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            host = GameObject.FindWithTag("Host");
            if (host == null)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel("Lobbu");
            }
        }
    }
}