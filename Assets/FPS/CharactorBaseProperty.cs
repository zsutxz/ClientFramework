using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace FPS 
{

    /// <summary> 角色基本属性 </summary>
    [Serializable]
    public class CharactorBaseProperty
    {
        /// <summary> 向下的重力 </summary>
        public float gravityDownForce = 20f;
        /// <summary> 地面检测层s </summary>
        public LayerMask groundCheckLayers = -1;
        /// <summary> 地面检测距离 </summary>
        public float groundCheckDistance = 0.05f;
        /// <summary> 在地面最大速度 </summary>
        public float maxSpeedOnGround = 10f;
        /// <summary> 移动在斜面 </summary>
        public float movementSharpnessOnGround = 15;
        /// <summary> 最大速度蜷缩比 </summary>
        [Range(0, 1)]
        public float maxSpeedCrouchedRatio = 0.5f;
        /// <summary> 空中最大速度 </summary>
        public float maxSpeedInAir = 10f;
        /// <summary> 空中速度 </summary>
        public float accelerationSpeedInAir = 25f;
        /// <summary> 冲刺速度修正 </summary>
        public float sprintSpeedModifier = 2f;
        /// <summary> 移动摄像机的旋转速度 </summary>
        public float rotationSpeed = 200f;
        /// <summary> 瞄准时旋转速度倍增器 </summary>
        public float aimingRotationMultiplier = 0.4f;
        /// <summary> 跳跃时向上施加的力 </summary>
        public float jumpForce = 9f;
        /// <summary> 摄像机所处的角色高度的比例(0-1)   </summary>
        public float cameraHeightRatio = 0.9f;

        /// <summary>  站立时的高度 </summary>
        public float capsuleHeightStanding = 1.8f;
        /// <summary>  蹲伏时的高度 </summary>
        public float capsuleHeightCrouching = 0.9f;
        /// <summary>  蹲伏转换速度 </summary>
        public float crouchingSharpness = 10f;
   
        private float m_jumpGroundingPreventionTime = 0.2f;
        /// <summary> 接地预防时间 </summary>
        public float JumpGroundingPreventionTime => m_jumpGroundingPreventionTime;

        private float m_groundCheckDistanceInAir = 0.07f;
        /// <summary> 空中检测地面距离 </summary>
        public float GroundCheckDistanceInAir => m_groundCheckDistanceInAir;
    }

}

