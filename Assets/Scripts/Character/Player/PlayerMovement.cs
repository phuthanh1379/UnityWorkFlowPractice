using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    private CharacterController controller;
    private Vector3 velocity;

    [Header("Parameters")]
    [SerializeField] private bool isPlayerGrounded;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float speed = 2.0f;
    #endregion

    #region Unity Methods
    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    private void Update()
    {
        isPlayerGrounded = controller.isGrounded;
        if (isPlayerGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * speed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && isPlayerGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    #endregion
}