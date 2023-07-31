using System;
using Main.Scripts.Helpers;
using Main.Scripts.Network.Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Main.Scripts.Player
{
    public class Movement : NetworkBehaviour
    {
        public UnityEvent<Vector2> onMoveForce;
        
        //variables
        [SerializeField] private float jumpForceUp = 5f;
        [SerializeField] private float jumpForceForward = 5f;
        [SerializeField] private bool isLocalPlayer = false;

        //input system
        private Vector2 _move;

        private bool _isGrounded = true;
        private Throttle _projectionThrottle = new Throttle();
        private Vector2 _startPosition = Vector2.zero;
        private float _normalizationFactor;

        private Rigidbody rb;
        private Projection _projection;
        
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            _projection = GetComponent<Projection>();
            _normalizationFactor = Math.Min(Screen.width / 4f, Screen.height / 4f);

            if (isLocalPlayer)
            {
                try
                {
                    GetComponent<ClientNetworkTransform>().enabled = false;
                    GetComponent<NetworkObject>().enabled = false;
                    GetComponent<NetworkRigidbody>().enabled = false;
                    rb.isKinematic = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            _isGrounded = true;
        }

        private void OnMove(InputValue value)
        {
            if (!_isGrounded) return;
            if (!IsOwner && !isLocalPlayer) return;

            _move = value.Get<Vector2>();
            onMoveForce?.Invoke(_move);
            if (_move.magnitude > 0.1f)
            {
                // _projectionThrottle.Run(DoTrajectory, 0.05f);
                DoTrajectory();
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
            _projection.CalculatePosition(transform1.position, jumpVelocity);
        }

        private Vector3 GetJumpVelocity()
        {
            var jumpVelocity = Vector3.zero;
            jumpVelocity += Vector3.forward * _move.y * jumpForceForward;
            jumpVelocity += Vector3.right * _move.x * jumpForceForward;
            jumpVelocity += Vector3.up * jumpForceUp;
            return jumpVelocity;
        }

        private void OnJump()
        {
            DoJump();
        }

        private void OnMoveByRelease(InputValue value)
        {
            if (!_isGrounded) return;
            if (!IsOwner && !isLocalPlayer) return;

            var pos = value.Get<Vector2>();
            if (pos != Vector2.zero)
            {
                if (_startPosition == Vector2.zero)
                {
                    _startPosition = value.Get<Vector2>();
                }
                else
                {
                    _move = (value.Get<Vector2>() - _startPosition) / _normalizationFactor;
                    onMoveForce?.Invoke(_move);
                    if (_move.magnitude > 1f) _move.Normalize();
                }

                if (_move.magnitude > 0.1f)
                {
                    _projectionThrottle.Run(DoTrajectory, 0.05f);
                }
                else
                {
                    _projection.HideTrajectory();
                }
            }
            else
            {
                if (_move.magnitude > 0.1f)
                {
                    DoJump();
                }

                _move = Vector2.zero;
                _startPosition = Vector2.zero;
            }
        }

        private void DoJump()
        {
            if (!IsOwner && !isLocalPlayer) return;

            if (_isGrounded)
            {
                _projection.HideTrajectory();
                _isGrounded = false;
                var jumpVelocity = GetJumpVelocity();

                // Project the jumpVelocity onto the plane defined by the Y-axis
                Vector3 projectedJumpVelocity = Vector3.ProjectOnPlane(jumpVelocity, Vector3.up);

                // Get the rotation quaternion to look in the direction of the projected jumpVelocity
                Quaternion rotation = Quaternion.LookRotation(projectedJumpVelocity, Vector3.up);

                rb.MoveRotation(rotation);
                rb.AddForce(jumpVelocity, ForceMode.Impulse);
                //rotate player in direction of jump but only on the x axis
            }
        }
    }
}