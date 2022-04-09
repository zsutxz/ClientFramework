using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace FPS
{
    /// <summary>
    /// ���볣��
    /// </summary>
    public class InputConstants
    {
        /// <summary>   ��Ұ��תX </summary>
        public const string Axis_MouseX = "Mouse X";
        /// <summary>  ��Ұ��תY </summary>
        public const string Axis_MouseY = "Mouse Y";
        /// <summary>  ˮƽ�� </summary>
        public const string Axis_Horizontal = "Horizontal";
        /// <summary>  ��ֱ�� </summary>
        public const string Axis_Vertical = "Vertical";
        /// <summary>  ��Ծ�� </summary>
        public const string Key_Jump = "Space";
        /// <summary>  �׷��� </summary>
        public const string Key_Crouch = "C";
        /// <summary>  ��̼� </summary>
        public const string Key_Sprint = "LeftShift";
    }

    [Serializable]
    public class InputHandler
    {

        /// <summary>   ��Ұ��ת������ </summary>
        public float rotateSensitivity = 1.0f;
        /// <summary>   ��ת��Ұ��תX�� </summary>
        public bool invertRotateX;
        /// <summary>   ��ת��Ұ��תY�� </summary>
        public bool invertRotateY;


        /// <summary>
        /// ��ȡ�ƶ���
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
            //���ƶ����������������Ϊ1������Խ����ƶ����ܳ������������ƶ��ٶ�  
            _vec = Vector3.ClampMagnitude(_vec, 1);
            return _vec;
        }

        /// <summary>
        /// �����
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
        /// �Ƿ���Ծ
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


