using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;
    public Camera Camera;
    public int speed = 5;

    //camera
    public float mouseSensitivity = 100f;
    float xrotation = 0f;

    void Start()
    {
       controller = GetComponent<CharacterController>();
       Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        //Player Movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);




        //Camera
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;


        transform.Rotate(Vector3.up * mouseX);
        xrotation -= mouseY;
        xrotation = Mathf.Clamp(xrotation, -90f, 90f);
        
        Camera.transform.localRotation = Quaternion.Euler(xrotation, 0f, 0f);

    }


}
   

