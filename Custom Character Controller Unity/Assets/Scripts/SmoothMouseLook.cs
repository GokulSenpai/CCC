using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float smoothRotateSpeed = 5f;

    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Quaternion currentRotation = transform.localRotation;
        Quaternion desiredRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        transform.localRotation = Quaternion.Slerp(currentRotation, desiredRotation, Time.deltaTime * smoothRotateSpeed);
    }
}
