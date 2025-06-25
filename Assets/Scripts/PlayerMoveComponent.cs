using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveComponent : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float gravity = -20f;

    [SerializeField]
    private float jumpForce = 3f;

    private float jumpCount = 0;

    private float yVelocity = 0;

    private CharacterController controller;
    #endregion

    #region Unity Functions
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = ARAVRInput.GetAxis("Horizontal");
        float vertical = ARAVRInput.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Camera.main.transform.TransformDirection(direction);

        yVelocity += gravity * Time.deltaTime;
        if(controller.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
        }
        // if(controller.collisionFlags == CollisionFlags.Below)
        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch) && jumpCount < 1)
        {
            yVelocity += jumpForce;
        }
        direction.y = yVelocity;

        controller.Move(direction * speed * Time.deltaTime);
    }
    #endregion
}