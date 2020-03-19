using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Adds Rigidbody component automatically to the object this script is attached to 
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float gravity = 9.8f;
    public float jumpHeight = 3f;
    public float moveSpeed = 14f;

    public GameObject cameraForRotation;

    new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = cameraForRotation.transform.right * horizontalInput + cameraForRotation.transform.forward * verticalInput;


        rigidbody.MovePosition(moveDirection * moveSpeed * Time.deltaTime);
        //rigidbody.MovePosition(cameraForRotation.transform.localPosition + moveDirection * Time.deltaTime * moveSpeed);

        //Jump
        //if(Input.GetButtonDown("Jump"))
        //{
        //    rigidbody.AddForce(transform.up * Mathf.Sqrt(2 * gravity * jumpHeight), ForceMode.VelocityChange);
        //}
    }
}
