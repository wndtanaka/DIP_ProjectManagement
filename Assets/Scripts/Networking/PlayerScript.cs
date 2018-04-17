using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerScript : NetworkBehaviour
    {
        public float movementSpeed = 10f;
        public float rotationSpeed = 5f;
        public float jumpHeight = 5f;
        private bool isGrounded = false;
        private Rigidbody rigid;


        // Use this for initialization
        void Start()
        {
            rigid = GetComponent<Rigidbody>();

            // get audio listener from camera
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            // get camera
            Camera camera = GetComponentInChildren<Camera>();

            if (isLocalPlayer)
            {
                // enable everything
                camera.enabled = true;
                audioListener.enabled = true;
            }
            else
            {
                // disable everything
                camera.enabled = false;
                audioListener.enabled = false;
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (isLocalPlayer)
            {
                HandleInput();
            }
        }

        void Move(KeyCode _key)
        {
            Vector3 position = rigid.position;
            Quaternion rotation = rigid.rotation;
            switch (_key)
            {
                case KeyCode.W:
                    position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case KeyCode.S:
                    position += -transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case KeyCode.A:
                    rotation *= Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
                    break;
                case KeyCode.D:
                    rotation *= Quaternion.AngleAxis(rotationSpeed, Vector3.up);
                    break;
                case KeyCode.Space:
                    if (isGrounded)
                    {
                        rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                        isGrounded = false;
                    }
                    break;
            }
            rigid.MovePosition(position);
            rigid.MoveRotation(rotation);
        }

        void HandleInput()
        {
            KeyCode[] keys =
            {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D,
            KeyCode.Space
        };

            foreach (var key in keys)
            {
                if (Input.GetKey(key))
                {
                    Move(key);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            isGrounded = true;
        }
    }
}