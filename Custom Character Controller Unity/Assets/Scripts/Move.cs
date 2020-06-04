using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.CompanyName.GameName
{
    public class Move : MonoBehaviour
    {

        #region Variables

        [Space]
        [Header("References")]
        public Camera theCamera;
        public Animator playerAnimations;
        public AudioSource walkAudioSource;
        public AudioClip walkAudioClip;

        [Space]
        [Header("Data")]
        public float speed = 4f;
        public float run = 7f;
        public float walk = 4f;

        [Space]
        [Header("External Forces")]
        public float gravity = -19.62f;
        public Vector3 Drag;

        [Space]
        [Header("Jump")]
        public float jumpHeight = 2.5f;
        public float groundDistance = 0.4f;

        public Transform groundCheck;
        public LayerMask groundMask;

        private bool iAmGrounded;
        private bool iWantToPlayFootsteps;

        private float currentFOV;
        private float runFOVMultiplier = 1.28f;
        private float zoomFOV = 14f;
        private float peekAngle = 14f;
        private float tiltAngle = 1.7f;

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
        private Quaternion beforeTiltRotation;

        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        private void Start()
        {
            playerAnimations.GetComponent<Animator>();
            currentFOV = theCamera.fieldOfView;
            _controller = GetComponent<CharacterController>();
            cameraInitialPos = theCamera.transform.localPosition;
        }

        // Update is called once per frame
        private void Update()
        {
            // Input Axes
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Controls
            bool runningInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jumpingInput = Input.GetButtonDown("Jump");
            bool rightMouseZoomInput = Input.GetKey(KeyCode.Mouse1);
            bool crouchInput = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool peekLeftInput = Input.GetKey(KeyCode.Q);
            bool peekRightInput = Input.GetKey(KeyCode.E);
            
            // States
            bool iAmGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) && _velocity.y < 0;
            bool iAmIdle = horizontalInput.Equals(0) && verticalInput.Equals(0);
            bool iWantToJump = jumpingInput && iAmGrounded;
            bool iWantToZoom = rightMouseZoomInput && iAmGrounded && !iWantToJump;
            bool iWantToCrouch = crouchInput && iAmGrounded;
            bool iWantToRun = runningInput && iAmGrounded && verticalInput > 0 && !iWantToCrouch;
            bool iWantToPeekLeft = peekLeftInput && iAmGrounded;
            bool iWantToPeekRight = peekRightInput && iAmGrounded;
            iWantToPlayFootsteps = iAmGrounded && !walkAudioSource.isPlaying && !iAmIdle;

            // Movement
            if (iAmIdle)
            {
                playerAnimations.SetBool("iWalk", false);
            }
            else
            {
                playerAnimations.SetBool("iWalk", true);
                playerAnimations.SetFloat("BlendX", horizontalInput);
                playerAnimations.SetFloat("BlendY", verticalInput);
            }


            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            _controller.Move(transform.TransformDirection(moveDirection) * (Time.smoothDeltaTime * speed));

            // Run
            if (iWantToRun)
            {
                playerAnimations.SetBool("iRun", true);
                theCamera.fieldOfView = Mathf.Lerp(theCamera.fieldOfView, currentFOV * runFOVMultiplier, Time.smoothDeltaTime * 7f);
                speed = run;
            }
            else
            {
                playerAnimations.SetBool("iRun", false);
                theCamera.fieldOfView = Mathf.Lerp(theCamera.fieldOfView, currentFOV, Time.smoothDeltaTime * 7f);
                speed = walk;
            }

            // Grounded Code
            if (iAmGrounded)
            {
                _velocity.y = -2f;
            }

            // Gravity
            _velocity.y += gravity * Time.smoothDeltaTime;

            // Jump
            if (iWantToJump)
            {
                _velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }

            // Right Mouse Zoom
            if (iWantToZoom)
            {
                theCamera.fieldOfView = Mathf.Lerp(theCamera.fieldOfView, zoomFOV, Time.smoothDeltaTime * 7f);
            }
            else
            {
                theCamera.fieldOfView = Mathf.Lerp(theCamera.fieldOfView, currentFOV, Time.smoothDeltaTime * 7f);
            }

            // Peek
            Vector3 peekPosition = new Vector3(0.37f, 0f, 0f);
            Quaternion peekRotation = Quaternion.Euler(0f, 0f, peekAngle);

            beforePeekPosition = theCamera.transform.localPosition;
            beforePeekRotation = theCamera.transform.localRotation;

            if (iWantToPeekLeft)
            {
                theCamera.transform.localPosition = Vector3.Lerp(beforePeekPosition, beforePeekPosition - peekPosition, 7f * Time.smoothDeltaTime);
                theCamera.transform.localRotation = Quaternion.Slerp(beforePeekRotation, beforePeekRotation * peekRotation, 7f * Time.smoothDeltaTime);
            }
            else if(iWantToPeekRight)
            {
                theCamera.transform.localPosition = Vector3.Lerp(beforePeekPosition, beforePeekPosition + peekPosition, 7f * Time.smoothDeltaTime);
                theCamera.transform.localRotation = Quaternion.Slerp(beforePeekRotation, beforePeekRotation * Quaternion.Inverse(peekRotation), 7f * Time.smoothDeltaTime);
            }
            else
            {
                theCamera.transform.localPosition = beforePeekPosition;
                theCamera.transform.localRotation = beforePeekRotation;
            }

            // Look back while running
            Quaternion lookBackRotation = Quaternion.Euler(0f, 160f, 0f);
            Quaternion beforeLookBackRotation = theCamera.transform.localRotation;


            if(iWantToRun && iWantToPeekLeft)
            {
                 theCamera.transform.localRotation = Quaternion.Slerp(beforePeekRotation, Quaternion.Inverse(lookBackRotation), Time.smoothDeltaTime * 35f);
            }
            else if(iWantToRun && iWantToPeekRight)
            {
                 theCamera.transform.localRotation = Quaternion.Slerp(beforePeekRotation, lookBackRotation, Time.smoothDeltaTime * 35f);
            }
            else
            {
                theCamera.transform.localRotation = Quaternion.Slerp(theCamera.transform.localRotation, beforePeekRotation, Time.smoothDeltaTime * 35f);
            }

            // Crouch
            Vector3 crouchValue = new Vector3(0f, 0.9f, -0.27f);
            Vector3 beforeCrouchPosition = theCamera.transform.localPosition;

            if (iWantToCrouch)
            {
                playerAnimations.SetBool("iCrouch", true);
                _controller.height = 0.1f;
                _controller.center = new Vector3(0f, 0.5f, 0f);
                theCamera.transform.localPosition = Vector3.Lerp(beforeCrouchPosition, beforeCrouchPosition - crouchValue, 7f * Time.smoothDeltaTime);
            }
            else
            {
                playerAnimations.SetBool("iCrouch", false);
                _controller.height = 1.9f;
                _controller.center = new Vector3(0f, 0.95f, 0f);
                theCamera.transform.localPosition = Vector3.Lerp(theCamera.transform.localPosition, beforeCrouchPosition, 7f * Time.smoothDeltaTime);
            }

            // Camera Tilt
            Quaternion tilt = Quaternion.Euler(0f, 0f, tiltAngle);
            beforeTiltRotation = theCamera.transform.localRotation;

            if (horizontalInput < 0)
            {
                theCamera.transform.localRotation = Quaternion.Slerp(beforeTiltRotation, beforeTiltRotation * tilt, 7f * Time.smoothDeltaTime);
            }
            else if (horizontalInput > 0)
            {
                theCamera.transform.localRotation = Quaternion.Slerp(beforeTiltRotation, beforeTiltRotation * Quaternion.Inverse(tilt), 7f * Time.smoothDeltaTime);
            }
            else
            {
                theCamera.transform.localRotation = beforeTiltRotation;
            }

            // Drag
            _velocity.x /= 1 + Drag.x * Time.smoothDeltaTime;
            _velocity.y /= 1 + Drag.y * Time.smoothDeltaTime;
            _velocity.z /= 1 + Drag.z * Time.smoothDeltaTime;

            // Applying all the forces on Movement
            _controller.Move(_velocity * Time.smoothDeltaTime);

            // Head Bob
            Vector3 runOffsetBob = new Vector3(0f, 0f, 0.17f);

            if (iAmIdle)
            {
                if (iWantToPeekLeft || iWantToPeekRight)
                {
                    HeadBob(headBobIdleCounter, 0.01f, 0.01f);
                }
                else
                {
                    HeadBob(headBobIdleCounter, 0.03f, 0.03f);
                }

                headBobIdleCounter += Time.smoothDeltaTime;
                theCamera.transform.localPosition = Vector3.Lerp(theCamera.transform.localPosition, targetBobPosition, Time.smoothDeltaTime * 5f);
            }
            else if (iWantToRun)
            {
                HeadBob(headBobMovementCounter, 0.14f, 0.21f);
                headBobMovementCounter += Time.smoothDeltaTime * 7f;
                theCamera.transform.localPosition = Vector3.Lerp(theCamera.transform.localPosition, targetBobPosition + runOffsetBob, Time.smoothDeltaTime * 10f);
            }
            else
            {
                HeadBob(headBobMovementCounter, 0.07f, 0.14f);
                headBobMovementCounter += Time.smoothDeltaTime * 5f;
                theCamera.transform.localPosition = Vector3.Lerp(theCamera.transform.localPosition, targetBobPosition, Time.smoothDeltaTime * 7f);
            }
        }

        #endregion

        #region Private Methods

        private void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
        {
            targetBobPosition = cameraInitialPos + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0f);
        }

        private void Footsteps()
        {
            if(iWantToPlayFootsteps)
            {
                walkAudioSource.PlayOneShot(walkAudioClip);
            }
        }
    
        #endregion

    }
}



