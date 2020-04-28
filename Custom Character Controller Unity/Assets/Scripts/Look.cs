using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.CompanyName.GameName
{
    public class Look : MonoBehaviour
    {

        #region Variables

        public float mouseSensitivity = 200f;
        public float smoothRotateSpeed = 7f;

        float cameraXRotation = 0f;
        float playerYRotation = 0f;

        public Transform theCamera;
        public Transform thePlayer;

        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            // Mouse Input
            float horizontalMouseInput = Input.GetAxis("Mouse X") * mouseSensitivity * Time.smoothDeltaTime;
            float verticalMouseInput = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.smoothDeltaTime;

            // Rotation subtracted and added every frame based on mouse input
            cameraXRotation -= verticalMouseInput;
            playerYRotation += horizontalMouseInput;

            // Vertical Clamp
            cameraXRotation = Mathf.Clamp(cameraXRotation, -90f, 90f);

        }

        // LateUpdate is called after all Update functions have been called
        void LateUpdate()
        {
            // Takes current local rotation of camera and rotation to move to
            Quaternion currentRotation = theCamera.localRotation;
            Quaternion desiredRotation = Quaternion.Euler(cameraXRotation, theCamera.localRotation.y, theCamera.localRotation.z);

            // Smoothing
            theCamera.localRotation = Quaternion.Slerp(currentRotation, desiredRotation, Time.smoothDeltaTime * smoothRotateSpeed);

            // Takes current rotation of player and rotation to move to 
            Quaternion currentPlayerRotation = thePlayer.rotation;
            Quaternion desiredPlayerRotation = Quaternion.Euler(thePlayer.rotation.x, playerYRotation, thePlayer.rotation.z);

            // Smoothing
            thePlayer.rotation = Quaternion.Slerp(currentPlayerRotation, desiredPlayerRotation, Time.smoothDeltaTime * smoothRotateSpeed);
    
        }

        #endregion

    }
}
