using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float smoothRotateSpeed = 5f;

    float xRotation = 0f;
    float yRotation = 0f;

    public Transform playerBody;

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
    }

    // LateUpdate is called after all Update functions have been called.
    void LateUpdate()
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion desiredRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        transform.rotation = Quaternion.Slerp(currentRotation, desiredRotation, Time.smoothDeltaTime * smoothRotateSpeed);

        Quaternion currentPlayerRotation = playerBody.transform.rotation;
        Quaternion desiredPlayerRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        // Add or remove xRotation here to see a cool effect.

        playerBody.transform.rotation = Quaternion.Slerp(currentPlayerRotation, desiredPlayerRotation, Time.smoothDeltaTime * smoothRotateSpeed);
        // playerBody.transform.rotation = Quaternion.Euler(0f, playerBody.transform.rotation.y, playerBody.transform.rotation.z);
    }
}
