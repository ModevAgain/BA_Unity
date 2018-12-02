using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static BA.BA_Input;

namespace BA
{
    [CreateAssetMenu(fileName = "InputMapper", menuName = "Input/InputMapper", order = 0)]
    public class BA_InputMapper : ScriptableObject, IInitializable
    {

        [Header("Scene References")]
        private BA_InputModule _inputModule;

        [Header("Input References")]
        public BA_RawInput RawInput;
        public List<BA_InputContext> Contexts;

        #region Input Delegates

        public delegate void InputDelegate();
        public delegate void InputDelegateVector2(Vector2 v);
        public delegate void InputDelegateVector3(Vector3 v);
        public delegate void InputDelegatePointerEvent(PointerEventData ped);

        public InputDelegateVector2 MoveInputVector2;
        public InputDelegateVector3 MoveInputVector3;
        public InputDelegatePointerEvent MouseInputUI; 



        #endregion


        [SerializeField]
        private List<BA_InputContext> _activeContexts;

        #region Raycasting
        private GraphicRaycaster _gRaycaster;
        private List<RaycastResult> _raycastResults;
        private PointerEventData _ped;

        #endregion  

        private Vector3 _pointerPosition;
        private Vector2 _gamepadLeft;
        private Vector2 _gamepadRight;

        #region InputReceiver

        public void ReceiveInput(BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            if(type == BA_InputType.MOUSE_0_DOWN || type == BA_InputType.TOUCH_0_DOWN)
            {
                _ped = _inputModule.GetLastPointerEventData();

                _raycastResults.Clear();                

                _gRaycaster.Raycast(_ped, _raycastResults);

                //Move
                if (_raycastResults.Count == 0)
                {
                    MoveInputVector3(_pointerPosition);
                }
                //Delegate to UI Input Receiver
                else
                {
                    MouseInputUI(_ped);
                }
            }
        }

        public void ReceiveInputVector3(Vector3 vec, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            _pointerPosition = vec;
           
        }

        public void ReceiveInputTouch(Touch t, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            _pointerPosition = t.position;

        }

        public void ReceiveInputFloat(float f, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            if(type == BA_InputType.GAMEPAD_AXIS_LEFT_X)
            {
                _gamepadLeft.x = f;
            }
            else if(type == BA_InputType.GAMEPAD_AXIS_LEFT_Y)
            {
                _gamepadLeft.y = f;
            }
            else if (type == BA_InputType.GAMEPAD_AXIS_RIGHT_X)
            {
                _gamepadRight.x = f;
            }
            else if (type == BA_InputType.GAMEPAD_AXIS_RIGHT_Y)
            {
                _gamepadRight.y = f;
            }

            MoveInputVector2(_gamepadLeft);

        }

        private bool IsInputValid(BA_InputType type)
        {
            foreach (var context in _activeContexts)
            {
                foreach (var validType in context.AllowedInputs)
                {
                    if (validType == type)
                        return true;
                }
            }

            return false;
        }

        #endregion

        #region IInitializable

        public void Initialize()
        {

            #region RESET
            RawInput.Mouse_0_Down = null;
            RawInput.Mouse_0_Up = null;
            RawInput.Mouse_Position = null;

            RawInput.Keyboard_0_Down = null;
            RawInput.Keyboard_0_Up = null;
            RawInput.Keyboard_1_Down = null;
            RawInput.Keyboard_1_Up = null;

            RawInput.Touch_0 = null;
            RawInput.Touch_0_Down = null;
            RawInput.Touch_0_Up = null;
            RawInput.Touch_1 = null;
            RawInput.Touch_1_Down = null;
            RawInput.Touch_1_Up = null;

            RawInput.Gamepad_0_Down = null;
            RawInput.Gamepad_0_Up = null;
            RawInput.Gamepad_1_Down = null;
            RawInput.Gamepad_1_Up = null;

            RawInput.Gamepad_Axis_Left_X = null;
            RawInput.Gamepad_Axis_Left_Y = null;

            RawInput.Gamepad_Axis_Right_X = null;
            RawInput.Gamepad_Axis_Right_Y = null;

            #endregion

            //Mouse & Keyboard

            RawInput.Mouse_0_Down += () => ReceiveInput(BA_InputType.MOUSE_0_DOWN);
            RawInput.Mouse_0_Up += () => ReceiveInput(BA_InputType.MOUSE_0_UP);
            RawInput.Mouse_Position += (v) => ReceiveInputVector3(v,BA_InputType.MOUSE_0_POS);

            RawInput.Keyboard_0_Down += () => ReceiveInput(BA_InputType.KEYBOARD_0_DOWN);
            RawInput.Keyboard_0_Up += () => ReceiveInput(BA_InputType.KEYBOARD_0_UP);

            RawInput.Keyboard_1_Down += () => ReceiveInput(BA_InputType.KEYBOARD_1_DOWN);
            RawInput.Keyboard_1_Up += () => ReceiveInput(BA_InputType.KEYBOARD_1_UP);

            //Touch
           
            RawInput.Touch_0 += (t) => ReceiveInputTouch(t, BA_InputType.TOUCH_0);
            RawInput.Touch_0_Down += () => ReceiveInput(BA_InputType.TOUCH_0_DOWN);
            RawInput.Touch_0_Up += () => ReceiveInput(BA_InputType.TOUCH_0_UP);

            //Gamepad

            RawInput.Gamepad_0_Down += () => ReceiveInput(BA_InputType.GAMEPAD_0_DOWN);
            RawInput.Gamepad_0_Up += () => ReceiveInput(BA_InputType.GAMEPAD_0_UP);

            RawInput.Gamepad_1_Down += () => ReceiveInput(BA_InputType.GAMEPAD_1_DOWN);
            RawInput.Gamepad_1_Up += () => ReceiveInput(BA_InputType.GAMEPAD_1_UP);

            _gamepadLeft = Vector2.zero;
            _gamepadRight = Vector2.zero;

            RawInput.Gamepad_Axis_Left_X += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_X);
            RawInput.Gamepad_Axis_Left_Y += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_Y);

            RawInput.Gamepad_Axis_Right_X += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_X);
            RawInput.Gamepad_Axis_Right_Y += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_Y);


            //Raycasting
            _gRaycaster = FindObjectOfType<GraphicRaycaster>();
            _raycastResults = new List<RaycastResult>();
            _ped = new PointerEventData(EventSystem.current);

            //InputModule
            _inputModule = FindObjectOfType<BA_InputModule>();

        }


        public void Deinitialize()
        {
            //Mouse & Keyboard

            RawInput.Mouse_0_Down -= () => ReceiveInput(BA_InputType.MOUSE_0_DOWN);
            RawInput.Mouse_0_Up -= () => ReceiveInput(BA_InputType.MOUSE_0_UP);
            RawInput.Mouse_Position -= (v) => ReceiveInputVector3(v, BA_InputType.MOUSE_0_POS);

            RawInput.Keyboard_0_Down -= () => ReceiveInput(BA_InputType.KEYBOARD_0_DOWN);
            RawInput.Keyboard_0_Up -= () => ReceiveInput(BA_InputType.KEYBOARD_0_UP);

            RawInput.Keyboard_1_Down -= () => ReceiveInput(BA_InputType.KEYBOARD_1_DOWN);
            RawInput.Keyboard_1_Up -= () => ReceiveInput(BA_InputType.KEYBOARD_1_UP);

            //Touch
            RawInput.Touch_0_Down -= () => ReceiveInput(BA_InputType.TOUCH_0_DOWN);
            RawInput.Touch_0_Up -= () => ReceiveInput(BA_InputType.TOUCH_0_UP);
            RawInput.Touch_0 -= (t) => ReceiveInputTouch(t, BA_InputType.TOUCH_0);

            //Gamepad

            RawInput.Gamepad_0_Down -= () => ReceiveInput(BA_InputType.GAMEPAD_0_DOWN);
            RawInput.Gamepad_0_Up -= () => ReceiveInput(BA_InputType.GAMEPAD_0_UP);

            RawInput.Gamepad_1_Down -= () => ReceiveInput(BA_InputType.GAMEPAD_1_DOWN);
            RawInput.Gamepad_1_Up -= () => ReceiveInput(BA_InputType.GAMEPAD_1_UP);

            RawInput.Gamepad_Axis_Left_X -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_X);
            RawInput.Gamepad_Axis_Left_Y -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_Y);

            RawInput.Gamepad_Axis_Right_X -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_X);
            RawInput.Gamepad_Axis_Right_Y -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_Y);


            //Raycasting
            _raycastResults.Clear();
        }


        #endregion
    }
}
