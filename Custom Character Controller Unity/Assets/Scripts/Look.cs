using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    #region Variables

    [Space]
    [Header("References")]
    public Transform theCamera;
    public Transform thePlayer;

    [Space]
    [Header("Mouse Data")]
    public float mouseSensitivity = 100f;
    public float smoothRotateSpeed = 5f;

    public bool smoothMouse = true;

    private Player _playerRef;

    private float _cameraXRotation = 0f;
    [HideInInspector] public float _playerYRotation = 0f;

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        _playerRef = thePlayer.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Mouse Input
        float horizontalMouseInput = Input.GetAxis("Mouse X") * mouseSensitivity * Time.smoothDeltaTime;
        float verticalMouseInput = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.smoothDeltaTime;

        // Rotation subtracted and added every frame based on mouse input
        _cameraXRotation -= verticalMouseInput;
        _playerYRotation += horizontalMouseInput;

        // Vertical Clamp
        _cameraXRotation = Mathf.Clamp(_cameraXRotation, _playerRef.peekClampMin, _playerRef.peekClampMax);

        if (smoothMouse)
        {
            SmoothMouse();
        }
        else
        {
            NormalMouse();
        }
    }
    
    private void SmoothMouse()
    {
        // Takes current local rotation of camera and rotation to move to
        var localRotation = theCamera.localRotation;
        Quaternion currentRotation = localRotation;
        Quaternion desiredRotation = Quaternion.Euler(_cameraXRotation, localRotation.y, localRotation.z);

        // Smoothing
        localRotation = Quaternion.SlerpUnclamped(currentRotation, desiredRotation, Time.smoothDeltaTime * smoothRotateSpeed);
        theCamera.localRotation = localRotation;

        // Takes current rotation of player and rotation to move to 
        var rotation = thePlayer.rotation;
        Quaternion currentPlayerRotation = rotation;
        Quaternion desiredPlayerRotation = Quaternion.Euler(rotation.x, _playerYRotation, rotation.z);

        // Smoothing
        rotation = Quaternion.SlerpUnclamped(currentPlayerRotation, desiredPlayerRotation, Time.smoothDeltaTime * smoothRotateSpeed);
        thePlayer.rotation = rotation;
    }

    private void NormalMouse()
    {
        // Takes current local rotation of camera and rotation to move to
        var localRotation = theCamera.localRotation;
        theCamera.localRotation = Quaternion.Euler(_cameraXRotation, localRotation.y, localRotation.z);

        // Takes current rotation of player and rotation to move to 
        var rotation = thePlayer.rotation;
        thePlayer.rotation = Quaternion.Euler(rotation.x, _playerYRotation, rotation.z);
        
    }
    #endregion
    
}