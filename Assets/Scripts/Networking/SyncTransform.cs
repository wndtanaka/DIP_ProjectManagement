using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

namespace Networking
{
    public class SyncTransform : NetworkBehaviour
    {
        public float lerpSpeed = 10f;

        // Threshold for when to send commands
        public float positionThreshold = 0.5f;
        public float rotationThreshold = 5f;

        [SyncVar] Vector3 syncPosition;
        [SyncVar] Quaternion syncRotation;

        private Rigidbody rigid;
        private Vector3 lastPosition;
        private Quaternion lastRotation;

        private void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            TransmitPosition();
            LerpPosition();

            TransmitRotation();
            LerpRotation();
        }

        void LerpPosition()
        {
            if (!isLocalPlayer)
            {
                rigid.position = Vector3.Lerp(rigid.position, syncPosition, Time.deltaTime * lerpSpeed);
            }
        }
        void LerpRotation()
        {
            if (!isLocalPlayer)
            {
                rigid.rotation = Quaternion.Lerp(rigid.rotation, syncRotation, Time.deltaTime * lerpSpeed);
            }
        }
        [Command]
        void CmdSendPositionToServer(Vector3 position)
        {
            syncPosition = position;
        }
        [Command]
        void CmdSendRotationToServer(Quaternion rotation)
        {
            syncRotation = rotation;
        }
        [ClientCallback]
        void TransmitPosition()
        {
            if (isLocalPlayer && Vector3.Distance(rigid.position, lastPosition) > positionThreshold)
            {
                CmdSendPositionToServer(rigid.position);
                lastPosition = rigid.position;
            }
        }
        [ClientCallback]
        void TransmitRotation()
        {
            if (isLocalPlayer && Quaternion.Angle(rigid.rotation, lastRotation) > rotationThreshold)
            {
                CmdSendRotationToServer(rigid.rotation);
                lastRotation = rigid.rotation;
            }
        }
    }
}