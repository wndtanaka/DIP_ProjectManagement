using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;
        [SerializeField]
        private float cameraRotationLimit = 60f;

        private Vector3 velocity = Vector3.zero;
        private Vector3 rotation = Vector3.zero;
        private float cameraRotationX = 0;
        private float currentCameraRotationX = 0f;
        private Vector3 thrusterForce = Vector3.zero;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 _velocity)
        {
            velocity = _velocity;
        }

        public void Rotate(Vector3 _rotation)
        {
            rotation = _rotation;
        }
        public void RotateCamera(float _camRotation)
        {
            cameraRotationX = _camRotation;
        }

        public void ApplyThruster(Vector3 _thrusterForce)
        {
            thrusterForce = _thrusterForce;
        }

        private void FixedUpdate()
        {
            PerformMovement();
            PerformRotation();
        }

        void PerformMovement()
        {
            if (velocity != Vector3.zero)
            {
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            }
            if (thrusterForce != Vector3.zero)
            {
                rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
        void PerformRotation()
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
            if (cam != null)
            {// set our rotation and clamp it
                currentCameraRotationX -= cameraRotationX;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

                // apply our rotation to the transform of our camera
                cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
            }
        }
    }
}