using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.CompanyName.GameName
{
    public class Move : MonoBehaviour
    {

        #region Variables

        public float speed = 4f;
        public float sprint = 7f;
        public float walk = 4f;
        public float gravity = -19.62f;
        public float jumpHeight = 2f;
        public float groundDistance = 0.4f;

        public Transform groundCheck;
        public LayerMask groundMask;

        public Camera mainCam;

        public Vector3 Drag;

        public AudioSource walkOne;
        public AudioSource walkTwo;

        private bool iAmGrounded;
        private bool iWantToPlayAudio;

        private float currentFOV;
        private float runFOVMultiplier = 1.28f;
        private float zoomFOV = 14f;
        private float peekAngle = 49f;
        private float horizontalInput;
        private float verticalInput;
        private float headBobMovementCounter;
        private float headBobIdleCounter;

        private CharacterController _controller;
        private Vector3 _velocity;
        private Vector3 moveDirection;
        private Vector3 cameraInitialPos;
        private Vector3 targetBobPosition;
        private Vector3 beforePeekPosition;

        private Quaternion beforePeekRotation;

        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        void Start()
        {
            currentFOV = mainCam.fieldOfView;
            _controller = GetComponent<CharacterController>();
            cameraInitialPos = mainCam.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            // Input Axes
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // Controls
            bool sprintingInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jumpingInput = Input.GetButtonDown("Jump");
            bool rightMouseZoomInput = Input.GetKey(KeyCode.Mouse1);
            bool crouchInput = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool peekLeftInput = Input.GetKey(KeyCode.Q);
            bool peekRightInput = Input.GetKey(KeyCode.E);


            // States
            bool iAmGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) && _velocity.y < 0;
            bool iWantToJump = jumpingInput && iAmGrounded;
            bool iWantToSprint = sprintingInput && iAmGrounded && verticalInput > 0;
            bool iWantToZoom = rightMouseZoomInput && iAmGrounded && !iWantToJump;
            bool iWantToCrouch = crouchInput && iAmGrounded;
            bool iWantToPeekLeft = peekLeftInput && iAmGrounded;
            bool iWantToPeekRight = peekRightInput && iAmGrounded;

            // Move in the pointed direction normalized
            moveDirection = new Vector3 (horizontalInput, 0, verticalInput).normalized;
            _controller.Move(transform.TransformDirection(moveDirection) * Time.smoothDeltaTime * speed);

            // Sprint
            if (iWantToSprint)
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, currentFOV * runFOVMultiplier, Time.smoothDeltaTime * 7f);
                speed = sprint;
            }
            else
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, currentFOV, Time.smoothDeltaTime * 7f);
                speed = walk;
            }

            // Gravity
            if (iAmGrounded)
            {
                _velocity.y = -2f;
            }

            _velocity.y += gravity * Time.smoothDeltaTime;

            // Jump
            if (iWantToJump)
            {
                _velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }

            // Right Mouse Zoom
            if (iWantToZoom)
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, zoomFOV, Time.smoothDeltaTime * 7f);
            }
            else
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, currentFOV, Time.smoothDeltaTime * 7f);
            }

            // Peek
            Vector3 peekPosition = new Vector3(1f, 0f, 0f);
            Quaternion peekRotation = Quaternion.Euler(0f, 0f, peekAngle);

            beforePeekPosition = transform.position;
            beforePeekRotation = transform.rotation;

            if (iWantToPeekLeft)
            {
                transform.position = Vector3.Lerp(beforePeekPosition, beforePeekPosition + peekPosition, 7f * Time.smoothDeltaTime);
                transform.rotation = Quaternion.Slerp(beforePeekRotation, beforePeekRotation * peekRotation, 7f * Time.smoothDeltaTime);
            }
            else if(iWantToPeekRight)
            {
                transform.position = Vector3.Lerp(beforePeekPosition, beforePeekPosition - peekPosition, 7f * Time.smoothDeltaTime);
                transform.rotation = Quaternion.Slerp(beforePeekRotation, beforePeekRotation * Quaternion.Inverse(peekRotation), 7f * Time.smoothDeltaTime);
            }
            else
            {
                transform.position = beforePeekPosition;
                transform.rotation = beforePeekRotation;
            }

            // Crouch
            Vector3 crouchValue = new Vector3(0f, 0.9f, 0f);
            Vector3 beforeCrouchPosition = mainCam.transform.localPosition;

            if (iWantToCrouch)
            {
                mainCam.transform.localPosition = Vector3.Lerp(beforeCrouchPosition, beforeCrouchPosition - crouchValue, 7f * Time.smoothDeltaTime);
            }
            else
            {
                mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, beforeCrouchPosition, 7f * Time.smoothDeltaTime);
            }

            // Drag
            _velocity.x /= 1 + Drag.x * Time.smoothDeltaTime;
            _velocity.y /= 1 + Drag.y * Time.smoothDeltaTime;
            _velocity.z /= 1 + Drag.z * Time.smoothDeltaTime;

            // Move with all the forces applied
            _controller.Move(_velocity * Time.smoothDeltaTime);

            // Head Bob
            if(horizontalInput == 0 && verticalInput == 0)
            {
                HeadBob(headBobIdleCounter, 0.14f, 0.07f);
                headBobIdleCounter += Time.smoothDeltaTime;
                mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, targetBobPosition, Time.smoothDeltaTime * 5f);
            }
            else if(iWantToSprint)
            {
                HeadBob(headBobMovementCounter, 0.28f, 0.21f);
                headBobMovementCounter += Time.smoothDeltaTime * 7f;
                mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, targetBobPosition, Time.smoothDeltaTime * 10f);
            }
            else
            {
                HeadBob(headBobMovementCounter, 0.14f, 0.14f);
                headBobMovementCounter += Time.smoothDeltaTime * 5f;
                mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, targetBobPosition, Time.smoothDeltaTime * 7f);
            }
        }

        #endregion

        #region Private Methods

        void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
        {
            targetBobPosition = cameraInitialPos + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf    .Sin(p_z * 2) * p_y_intensity, 0f);
        }

        #endregion
        
    }
}



