using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.CompanyName.GameName
{
    public class Move : MonoBehaviour
    {

        #region Variables

        public float speed = 7f;
        public float sprint = 10f;
        public float gravity = -9.81f;
        public float jumpHeight = 3f;
        public float groundDistance = 0.4f;

        public Transform groundCheck;
        public LayerMask groundMask;

        public Camera mainCam;

        public Vector3 Drag;

        private bool iAmGrounded;

        private float walkFOV;
        private float runFOVMultiplier = 1.28f;
        private float horizontalInput;
        private float verticalInput;

        private CharacterController _controller;
        private Vector3 _velocity;
        private Vector3 moveDirection;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            walkFOV = mainCam.fieldOfView;
            _controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            // Input Axes
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Controls
            bool sprintingInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jumpingInput = Input.GetButtonDown("Jump");

            // States
            bool iAmGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            bool iWantToJump = jumpingInput && iAmGrounded;
            bool iWantToSprint = sprintingInput && iAmGrounded && verticalInput > 0;

            // Move in the pointed direction
            moveDirection = (transform.right * horizontalInput + transform.forward * verticalInput).normalized;
            _controller.Move(moveDirection * Time.smoothDeltaTime * speed);

            // Sprint
            if (iWantToSprint)
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, walkFOV * runFOVMultiplier, Time.smoothDeltaTime * 7f);
                speed = sprint;
            }
            else
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, walkFOV, Time.smoothDeltaTime * 7f);
                speed = 6.0f;
            }

            // Gravity
            if (iAmGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += gravity * Time.smoothDeltaTime;

            // Jump
            if (iWantToJump)
            {
                _velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }

            // Drag
            _velocity.x /= 1 + Drag.x * Time.smoothDeltaTime;
            _velocity.y /= 1 + Drag.y * Time.smoothDeltaTime;
            _velocity.z /= 1 + Drag.z * Time.smoothDeltaTime;

            // Move with all the forces applied
            _controller.Move(_velocity * Time.smoothDeltaTime);
        }
    }
}



