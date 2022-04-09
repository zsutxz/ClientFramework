using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace FPS
{
    /// <summary>
    /// 输入常量
    /// </summary>
    public class InputConstants
    {
        /// <summary>   视野旋转X </summary>
        public const string Axis_MouseX = "Mouse X";
        /// <summary>  视野旋转Y </summary>
        public const string Axis_MouseY = "Mouse Y";
        /// <summary>  水平轴 </summary>
        public const string Axis_Horizontal = "Horizontal";
        /// <summary>  垂直轴 </summary>
        public const string Axis_Vertical = "Vertical";
        /// <summary>  跳跃键 </summary>
        public const string Key_Jump = "Space";
        /// <summary>  蹲伏键 </summary>
        public const string Key_Crouch = "C";
        /// <summary>  冲刺键 </summary>
        public const string Key_Sprint = "LeftShift";
    }

    [Serializable]
    public class InputHandler
    {

        /// <summary>   视野旋转灵敏度 </summary>
        public float rotateSensitivity = 1.0f;
        /// <summary>   反转视野旋转X轴 </summary>
        public bool invertRotateX;
        /// <summary>   反转视野旋转Y轴 </summary>
        public bool invertRotateY;


        /// <summary>
        /// 获取移动轴
        /// </summary>
        /// <param name="pAxisName"></param>
        /// <returns></returns>
        public float GetMoveAxis(string pAxisName)
        {
            return Input.GetAxisRaw(pAxisName);
        }

        public float GetLookInputsHorizontal()
        {
            return GetMouseOrStickLookAxis(InputConstants.Axis_MouseX, invertRotateX);
        }

        public float GetLookInputsVertical()
        {
            return GetMouseOrStickLookAxis(InputConstants.Axis_MouseY, invertRotateY);
        }

        public Vector3 GetMoveInput()
        {
            Vector3 _vec = new Vector3(Input.GetAxisRaw(InputConstants.Axis_Horizontal), 0, Input.GetAxisRaw(InputConstants.Axis_Vertical));
            //将移动输入的最大幅度限制为1，否则对角线移动可能超过定义的最大移动速度  
            _vec = Vector3.ClampMagnitude(_vec, 1);
            return _vec;
        }

        /// <summary>
        /// 冲刺中
        /// </summary>
        /// <returns></returns>
        public bool IsSprinting()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        private float GetMouseOrStickLookAxis(string pMouseName,bool pInvert)
        {
            float _i = Input.GetAxisRaw(pMouseName);
            if (pInvert)
                _i *= -1f;
            _i *= rotateSensitivity;
            _i *= 0.01f;
            return _i;
        }

        /// <summary>
        /// 是否跳跃
        /// </summary>
        /// <returns></returns>
        public bool IsJump()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public bool IsCround()
        {
            return Input.GetKeyDown(KeyCode.C);
        }
    }
}


