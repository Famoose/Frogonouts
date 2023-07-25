using System;
using System.Collections;
using System.Collections.Generic;
using Main.Scripts;
using Main.Scripts.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

//physic based movement class
//this class jumps and moves the player based on rigidbody 3d physics
public class Movement : MonoBehaviour
{
    //variables
    [SerializeField] private float jumpForceUp = 5f;
    [SerializeField] private float jumpForceForward = 5f;
    [SerializeField] private Vector3 _projectionOffset = Vector3.zero;

    [SerializeField] private Ball _jumpBall;
    
    //input system
    private Vector2 move;

    private Rigidbody rb;
    private Projection _projection;
    public bool isGrounded;
    private Throttle _projectionThrottle = new Throttle();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _projection = GetComponent<Projection>();
    }

    private void OnCollisionEnter(Collision other)
    {
        //todo: check if collision is ground
        isGrounded = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnMove(InputValue value)
    {
        if(!isGrounded) return;
        move = value.Get<Vector2>();
        if (move.magnitude > 0.1f)
        {
            _projectionThrottle.Run(DoTrajectory, 0.05f);
            //DoTrajectory();
        }
        else
        {
            _projection.HideTrajectory();
        }
    }
    
    private void DoTrajectory()
    {
        var transform1 = this.transform;
        //make a jump force based on the jump force variable player forward and up vector
        var jumpVelocity = GetJumpVelocity();
        _projection.SimulateTrajectory(_jumpBall, transform1.position + _projectionOffset, jumpVelocity);
    }

    private Vector3 GetJumpVelocity()
    {
        var transform1 = this.transform;
        var jumpVelocity = Vector3.zero;
        jumpVelocity += transform1.forward * move.y * jumpForceForward;
        jumpVelocity += transform1.right * move.x * jumpForceForward;
        jumpVelocity += transform1.up * jumpForceUp;
        return jumpVelocity;

    }

    private void OnJump()
    {
        if (isGrounded)
        {
            _projection.HideTrajectory();
            isGrounded = false;
            var jumpVelocity = GetJumpVelocity();
            //get move and set rotation
            var rot = Quaternion.LookRotation(jumpVelocity);
            rb.MoveRotation(new Quaternion(0, rot.y, 0, 0));
            rb.AddForce(jumpVelocity, ForceMode.Impulse);
        }
    }
}
