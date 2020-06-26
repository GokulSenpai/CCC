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
    public float mouseSensitivity = 200f;
    [Range(0.01f,1f)]
    [Tooltip( "1 = Normal Mouse Movement" )]
    public float smoothMouseFactor = 0.07f;

    public AnimationCurve mouseCurve;

    private IEnumerator _lookPlay;
    
    private Player _playerRef;

    private float _smoothMouse = 0f;
    
    private float _cameraXRotation = 0f;
    [HideInInspector] public float playerYRotation = 0f;

    #endregion

    #region Function Callbacks

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
        playerYRotation += horizontalMouseInput;

        // Vertical Clamp
        _cameraXRotation = Mathf.Clamp(_cameraXRotation, _playerRef.mouseClampMin, _playerRef.mouseClampMax);
        
        SmoothMouse(smoothMouseFactor);
    }

    private void SmoothMouse(float smoothMouseValue)
    {
        // Evaluates a value from the public animator curve
            _smoothMouse = mouseCurve.Evaluate(smoothMouseValue);

            // Takes current local rotation of camera and rotation to move to
            var localRotation = theCamera.localRotation;
            Quaternion currentRotation = localRotation;
            Quaternion desiredRotation = Quaternion.Euler(_cameraXRotation, localRotation.y, localRotation.z);

            // Smoothing with generated evaluate value
            localRotation = Quaternion.SlerpUnclamped(currentRotation, desiredRotation, _smoothMouse);
            theCamera.localRotation = localRotation;

            // Takes current rotation of player and rotation to move to 
            var rotation = thePlayer.rotation;
            Quaternion currentPlayerRotation = rotation;
            Quaternion desiredPlayerRotation = Quaternion.Euler(rotation.x, playerYRotation, rotation.z);

            // Smoothing with generated evaluate value
            rotation = Quaternion.SlerpUnclamped(currentPlayerRotation, desiredPlayerRotation, _smoothMouse);
            thePlayer.rotation = rotation;
    }
    #endregion
    
}