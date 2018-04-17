using UnityEngine;

namespace Networking
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ConfigurableJoint))]
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private float lookSensitivity = 3;
        [SerializeField]
        private float thrusterForce = 1000f;
        [SerializeField]
        private float thrusterFuelBurnSpeed = 0.5f;
        [SerializeField]
        private float thrusterFuelRegenSpeed = 0.2f;
        [HideInInspector]
        public float thrusterFuelAmount;
        [HideInInspector]
        public float maxThrusterFuel = 1f;

        public float GetThrusterFuelAmount()
        {
            return thrusterFuelAmount;
        }
        [SerializeField]
        private LayerMask environmentMask;

        [Header("Spring Settings")]
        [SerializeField]
        private float jointSpring = 20f;
        [SerializeField]
        private float jointMaxForce = 40f;

        [SerializeField]
        private Animator anim;

        private PlayerMotor motor;
        private ConfigurableJoint joint;

        private void Start()
        {
            motor = GetComponent<PlayerMotor>();
            joint = GetComponent<ConfigurableJoint>();
            anim = GetComponent<Animator>();
            SetJointSettings(jointSpring);

            thrusterFuelAmount = maxThrusterFuel;
        }

        private void Update()
        {
            // setting target position for spring
            // this makes the physics act when it comes to applying gravity when flying over
            RaycastHit _hit;
            if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f,environmentMask))
            {
                joint.targetPosition = new Vector3(0, -_hit.point.y, 0);
            }
            else
            {
                joint.targetPosition = new Vector3(0, 0, 0);
            }

            float _xMove = Input.GetAxis("Horizontal");
            float _zMove = Input.GetAxis("Vertical");
            // apply movement
            Vector3 _moveHorizontal = transform.right * _xMove;
            Vector3 _moveVertical = transform.forward * _zMove;
            Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

            // animate movement
            anim.SetFloat("ForwardVelocity", _zMove);

            motor.Move(_velocity);
            // apply rotation
            float _yRot = Input.GetAxisRaw("Mouse X");
            Vector3 _rotation = new Vector3(0, _yRot, 0) * lookSensitivity;
            motor.Rotate(_rotation);
            // apply camera rotation
            float _xRot = Input.GetAxisRaw("Mouse Y");
            float _camRotationX = _xRot * lookSensitivity;

            motor.RotateCamera(_camRotationX);

            // calculate thruster force
            Vector3 _thrusterForce = Vector3.zero;
            if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
            {
                thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

                if (thrusterFuelAmount >= 0.01f)
                {
                    _thrusterForce = Vector3.up * thrusterForce;
                    SetJointSettings(0f);
                }
            }
            else
            {
                thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
                SetJointSettings(jointSpring);
            }

            thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

            // apply thruster force
            motor.ApplyThruster(_thrusterForce);
        }

        private void SetJointSettings(float _jointSpring)
        {
            joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
        }
    }
}