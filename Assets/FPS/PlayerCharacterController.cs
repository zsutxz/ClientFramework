using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private Camera m_Camera;
        [SerializeField] private InputHandler m_InputHandler;
        [SerializeField] private CharactorBaseProperty m_charactorProperty;
        private CharacterController m_Controller;
        public Vector3 characterVelocity { get; set; }
        public bool isGrounded { get; private set; }
        public bool hasJumpedThisFrame { get; private set; }
        public bool isCrouching { get; private set; }

        public float RotationMultiplier
        {
            get
            {
                //if (m_WeaponsManager.isAiming)
                //{
                //    return aimingRotationMultiplier;
                //}

                return 1f;
            }
        }

        private Vector3 m_GroundNormal;
        private Vector3 m_LatestImpactSpeed;
        private float m_LastTimeJumped = 0f;
        private float m_CameraVerticalAngle = 0f;
        private float m_TargetCharacterHeight;

        private void Start()
        {
            m_Controller = GetComponent<CharacterController>();
            SetCrouchingState(false, true);
            UpdateCharacterHeight(true);
        }

        private void Update()
        {
            hasJumpedThisFrame = false;
            GroundCheck();
            CroundCheck();
            UpdateCharacterHeight(false);
            HandleCharacterMovement();
        }


        private void GroundCheck()
        {
            //确保已在空中的地面检查距离非常小，以防止突然折断到地面  
            float chosenGroundCheckDistance = isGrounded ? (m_Controller.skinWidth + m_charactorProperty.groundCheckDistance) : m_charactorProperty.GroundCheckDistanceInAir;

            // 地面检查前复位
            isGrounded = false;
            m_GroundNormal = Vector3.up;

            // 只有在上次跳跃后距离地面很短的时候才去探测地面; 否则，在尝试跳跃之后，可能会立即摔到地上  
            if (Time.time >= m_LastTimeJumped + m_charactorProperty.JumpGroundingPreventionTime)
            {

                //如果接地，收集信息的地面正常与向下的胶囊cast代表字符胶囊  
                if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height), m_Controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, m_charactorProperty.groundCheckLayers, QueryTriggerInteraction.Ignore))
                {
                    //模向量
                    m_GroundNormal = hit.normal;

                    //只有当地面法线与角色向上的方向相同时，才认为这是有效的地面撞击  
                    //如果倾斜角度低于角色控制器的限制  
                    if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                        IsNormalUnderSlopeLimit(m_GroundNormal))
                    {
                        isGrounded = true;

                        //突然掉到地上
                        if (hit.distance > m_Controller.skinWidth)
                        {
                            m_Controller.Move(Vector3.down * hit.distance);
                        }
                    }
                }
            }
        }

        //蹲伏检测
        private void CroundCheck()
        {
            if (m_InputHandler.IsCround())
            {
                SetCrouchingState(!isCrouching, false);
            }
        }

        private void HandleCharacterMovement()
        {
            //使用输入速度绕其本地Y轴旋转变换  
            transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * m_charactorProperty.rotationSpeed * RotationMultiplier), 0f), Space.Self);

            // 添加垂直输入到相机的垂直角度  
            m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * m_charactorProperty.rotationSpeed * RotationMultiplier;

            // 限制相机的垂直角度为最小/最大  
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

            // 应用垂直角度作为一个局部旋转，使相机变换沿着它的右轴(使枢轴向上和向下)  
            m_Camera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);


            //角色运动处理
            bool isSprinting = m_InputHandler.IsSprinting();
            //如果在冲刺
            if (isSprinting)
            {
                isSprinting = SetCrouchingState(false, false);
            }

            float speedModifier = isSprinting ? m_charactorProperty.sprintSpeedModifier : 1f;

            // 根据角色的变换方向将移动输入转换为世界空间向量  
            Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

            //处理接地运动
            if (isGrounded)
            {
                // 计算所需的速度，从输入，最大速度，和当前的坡度  
                Vector3 targetVelocity = worldspaceMoveInput * m_charactorProperty.maxSpeedOnGround * speedModifier;
                // 蹲伏降低速度  
                if (isCrouching)
                    targetVelocity *= m_charactorProperty.maxSpeedCrouchedRatio;
                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;

                // 平滑插值之间的当前速度和目标速度基于加速度速度 
                characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, m_charactorProperty.movementSharpnessOnGround * Time.deltaTime);
                // 跳跃
                if (isGrounded && m_InputHandler.IsJump())
                {
                    // 强制蹲伏状态为假
                    if (SetCrouchingState(false, false))
                    {
                        // 首先消去速度的竖直分量
                        characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

                        //然后，向上添加jumpSpeed值
                        characterVelocity += Vector3.up * m_charactorProperty.jumpForce;

                        // play sound
                        // audioSource.PlayOneShot(jumpSFX);

                        //记录上次跳是因为需要防止短时间摔到地上  
                        m_LastTimeJumped = Time.time;
                        hasJumpedThisFrame = true;

                        // 强制接地为假
                        isGrounded = false;
                        m_GroundNormal = Vector3.up;
                    }
                }
            }
            // 处理空中移动
            else
            {
                // 增加空中速度
                characterVelocity += worldspaceMoveInput * m_charactorProperty.accelerationSpeedInAir * Time.deltaTime;

                //限制空气中速度到最大，但只能水平  
                float verticalVelocity = characterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, m_charactorProperty.maxSpeedInAir * speedModifier);
                characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                // 把重力作用到速度上
                characterVelocity += Vector3.down * m_charactorProperty.gravityDownForce * Time.deltaTime;
            }


            // 应用最终计算的速度值作为角色的移动  
            Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
            Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(m_Controller.height);
            m_Controller.Move(characterVelocity * Time.deltaTime);

            // 检测障碍物，相应地调整速度  
            m_LatestImpactSpeed = Vector3.zero;
            if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, m_Controller.radius, characterVelocity.normalized, out RaycastHit hit, characterVelocity.magnitude * Time.deltaTime, -1, QueryTriggerInteraction.Ignore))
            {
                // 记录上一次的撞击速度，因为坠落损伤逻辑可能需要它  
                m_LatestImpactSpeed = characterVelocity;
                characterVelocity = Vector3.ProjectOnPlane(characterVelocity, hit.normal);
            }
        }

        // 如果给定法线表示的斜率小于字符控制器的斜率限制，则返回true  
        private bool IsNormalUnderSlopeLimit(Vector3 normal)
        {
            return Vector3.Angle(transform.up, normal) <= m_Controller.slopeLimit;
        }

        //获取角色控制器胶囊的下半球的中心点  
        private Vector3 GetCapsuleBottomHemisphere()
        {
            return transform.position + (transform.up * m_Controller.radius);
        }

        //获取角色控制器胶囊的上半球的中心点      
        private Vector3 GetCapsuleTopHemisphere(float atHeight)
        {
            return transform.position + (transform.up * (atHeight - m_Controller.radius));
        }

        //获取与给定斜率相切的重定向方向  
        public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
        {
            Vector3 directionRight = Vector3.Cross(direction, transform.up);
            return Vector3.Cross(slopeNormal, directionRight).normalized;
        }

        private void UpdateCharacterHeight(bool force)
        {
            // 更新的高度 
            if (force)
            {
                m_Controller.height = m_TargetCharacterHeight;
                m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
                m_Camera.transform.localPosition = Vector3.up * m_TargetCharacterHeight * m_charactorProperty.cameraHeightRatio;
                //  m_Actor.aimPoint.transform.localPosition = m_Controller.center;
            }
            // 更新平滑高度
            else if (m_Controller.height != m_TargetCharacterHeight)
            {
                //调整胶囊的大小和相机的位置
                m_Controller.height = Mathf.Lerp(m_Controller.height, m_TargetCharacterHeight, m_charactorProperty.crouchingSharpness * Time.deltaTime);
                m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
                m_Camera.transform.localPosition = Vector3.Lerp(m_Camera.transform.localPosition, Vector3.up * m_TargetCharacterHeight * m_charactorProperty.cameraHeightRatio, m_charactorProperty.crouchingSharpness * Time.deltaTime);
                //   m_Actor.aimPoint.transform.localPosition = m_Controller.center;
            }
        }

        // 设置蹲伏状态，如果有阻碍返回false
        private bool SetCrouchingState(bool crouched, bool ignoreObstructions)
        {
            // 设置适当的高度
            if (crouched)
            {
                m_TargetCharacterHeight = m_charactorProperty.capsuleHeightCrouching;
            }
            else
            {
                // 检测到障碍物
                if (!ignoreObstructions)
                {
                    Collider[] standingOverlaps = Physics.OverlapCapsule(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_charactorProperty.capsuleHeightStanding), m_Controller.radius, -1, QueryTriggerInteraction.Ignore);

                    foreach (Collider c in standingOverlaps)
                    {
                        if (c != m_Controller)
                        {
                            return false;
                        }
                    }
                }

                m_TargetCharacterHeight = m_charactorProperty.capsuleHeightStanding;
            }

            //if (onStanceChanged != null)
            //{
            //    onStanceChanged.Invoke(crouched);
            //}

            isCrouching = crouched;
            return true;
        }
    }


}

