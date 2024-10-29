using UnityEngine;
using Unity.Netcode;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
                return;
            }


        }
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode switchViewInput = KeyCode.V;
        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;

        [SerializeField] private Transform transformCamera;
        public static bool isFirstPerson = false;
        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            InitializeTpCamera();
        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        protected virtual void Update()
        {
            InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
            if (Input.GetKeyDown(switchViewInput))
            {
                ToggleCameraView();
            }
        }
        
        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();

            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTargetThird(this.transform);
                    if (isFirstPerson)
                    {
                        tpCamera.InitFirst();
                    }
                    else
                    {
                        tpCamera.InitThird();
                    }
                }
            }
        }
        protected void ToggleCameraView()
        {
            if (isFirstPerson)
            {
                tpCamera.SetMainTargetThird(this.transform); // Switch to third-person
                isFirstPerson = false;
            }
            else
            {
                tpCamera.SetMainTargetFirst(transformCamera); // Switch to first-person
                isFirstPerson = true;
            }
        }


        protected virtual void InputHandle()
        {
            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
        }

        public virtual void MoveInput()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.z = Input.GetAxis(verticallInput);
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            if (isFirstPerson)
            {
                tpCamera.RotateCameraFirst(X, Y);

                // góc nhìn t1

                // Xoay nhân vật theo góc của camera
                Vector3 cameraForward = cameraMain.transform.forward;
                cameraForward.y = 0; // Đặt giá trị y = 0 để chỉ giữ lại hướng ngang
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                cc.transform.rotation = Quaternion.Slerp(cc.transform.rotation, targetRotation, Time.deltaTime * cc.rotationSpeed); // Cập nhật tốc độ xoay nếu cần
            }
            else
            {
                tpCamera.RotateCameraThird(X,Y);
            }
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
        }

        #endregion       
    }
}