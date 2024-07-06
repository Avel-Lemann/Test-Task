using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float verticalRotationLimit = 80.0f;
    [SerializeField] private Camera cam;

    [Header("Hand Settings")]
    [SerializeField] private float handRange = 5;
    [SerializeField] private GameObject hand;
    private GameObject holdCube;
    private bool cubeInHand;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float verticalRotation = 0.0f;

    private PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(view.IsMine)
        {
            cam.gameObject.SetActive(true);
        }
        else
        {
            cam.gameObject.SetActive(false);
        }

        if (view.IsMine)
        {
            CameraRotation();
            Movement();
        }
        TakeCube();
    }

    private void TakeCube()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!cubeInHand)
            {
                Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, handRange))
                {
                    if (hit.collider.CompareTag("InteractiveCube"))
                    {
                        // Получаем PhotonView куба
                        PhotonView cubeView = hit.collider.GetComponent<PhotonView>();
                        holdCube = hit.collider.gameObject;
                        // Вызываем RPC метод для поднятия куба на всех клиентах
                        view.RPC("PickUpCubeRPC", RpcTarget.AllBuffered, cubeView.ViewID);

                        cubeInHand = true;
                    }
                }
            }
            else if (cubeInHand)
            {
                // Вызываем RPC метод для опускания куба на всех клиентах
                view.RPC("DropCubeRPC", RpcTarget.AllBuffered, holdCube.GetComponent<PhotonView>().ViewID);

                cubeInHand = false;
            }
        }
    }

    [PunRPC]
    private void PickUpCubeRPC(int viewID)
    {
        PhotonView cubeView = PhotonView.Find(viewID);
        GameObject cube = cubeView.gameObject;

        // Выполняем действия на сервере
        cube.GetComponent<Rigidbody>().isKinematic = true;
        cube.transform.parent = hand.transform;
        cube.transform.position = hand.transform.position;

        // Синхронизируем действия на всех клиентах
        view.RPC("SyncPickUpCubeRPC", RpcTarget.AllBuffered, viewID);
    }

    [PunRPC]
    private void SyncPickUpCubeRPC(int viewID)
    {
        PhotonView cubeView = PhotonView.Find(viewID);
        GameObject cube = cubeView.gameObject;

        // Выполняем действия на всех клиентах
        cube.GetComponent<Rigidbody>().isKinematic = true;
        cube.transform.parent = hand.transform;
        cube.transform.position = hand.transform.position;
    }

    [PunRPC]
    private void DropCubeRPC(int viewID)
    {
        PhotonView cubeView = PhotonView.Find(viewID);
        GameObject cube = cubeView.gameObject;

        // Выполняем действия на сервере
        cube.GetComponent<Rigidbody>().isKinematic = false;
        cube.transform.parent = null;

        // Синхронизируем действия на всех клиентах
        view.RPC("SyncDropCubeRPC", RpcTarget.AllBuffered, viewID);
    }

    [PunRPC]
    private void SyncDropCubeRPC(int viewID)
    {
        PhotonView cubeView = PhotonView.Find(viewID);
        GameObject cube = cubeView.gameObject;

        // Выполняем действия на всех клиентах
        cube.GetComponent<Rigidbody>().isKinematic = false;
        cube.transform.parent = null;
    }


    private void Movement()
    {
        if (controller.isGrounded)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
    private void CameraRotation()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        transform.Rotate(0, horizontalRotation, 0);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
