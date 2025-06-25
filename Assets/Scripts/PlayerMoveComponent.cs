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


        //카메라 앞쪽에서 케릭터가 총을 드는 모션 및 수류탄을 던지는 모션을 작성
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; //좌우회전시 카메라를 따라가게 만들었지만 상하회전시는 안되게 막음
        transform.forward = cameraForward.normalized;
    }
    #endregion
}