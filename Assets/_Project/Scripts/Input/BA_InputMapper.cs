using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BA.BA_Input;

namespace BA
{
    [CreateAssetMenu(fileName = "InputMapper", menuName = "Input/InputMapper", order = 0)]
    public class BA_InputMapper : ScriptableObject, IInitializable
    {

        public BA_RawInput RawInput;
        public List<BA_InputContext> Contexts;


        [SerializeField]
        private List<BA_InputContext> _activeContexts;

        #region InputReceiver

        public void ReceiveInput(BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            //Debug.Log("Received Input: " + type);
        }

        public void ReceiveInputVector3(Vector3 vec, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            //Debug.Log("Received InputVector3: " + vec + " | " + type);
        }

        public void ReceiveInputFloat(float f, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            //Debug.Log("Received InputFloat: " + f + " | " + type);
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
            //Mouse & Keyboard

            RawInput.Mouse_0_Down += () => ReceiveInput(BA_InputType.MOUSE_0_DOWN);
            RawInput.Mouse_0_Up += () => ReceiveInput(BA_InputType.MOUSE_0_UP);
            RawInput.Mouse_Position += (v) => ReceiveInputVector3(v,BA_InputType.MOUSE_0_POS);

            RawInput.Keyboard_0_Down += () => ReceiveInput(BA_InputType.KEYBOARD_0_DOWN);
            RawInput.Keyboard_0_Up += () => ReceiveInput(BA_InputType.KEYBOARD_0_UP);

            RawInput.Keyboard_1_Down += () => ReceiveInput(BA_InputType.KEYBOARD_1_DOWN);
            RawInput.Keyboard_1_Up += () => ReceiveInput(BA_InputType.KEYBOARD_1_UP);

            //Touch
            //TODO: implement Touch

            //Gamepad

            RawInput.Gamepad_0_Down += () => ReceiveInput(BA_InputType.GAMEPAD_0_DOWN);
            RawInput.Gamepad_0_Up += () => ReceiveInput(BA_InputType.GAMEPAD_0_UP);

            RawInput.Gamepad_1_Down += () => ReceiveInput(BA_InputType.GAMEPAD_1_DOWN);
            RawInput.Gamepad_1_Up += () => ReceiveInput(BA_InputType.GAMEPAD_1_UP);

            RawInput.Gamepad_Axis_Left_X += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_X);
            RawInput.Gamepad_Axis_Left_Y += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_Y);

            RawInput.Gamepad_Axis_Right_X += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_X);
            RawInput.Gamepad_Axis_Right_Y += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_Y);

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
            //TODO: implement Touch

            //Gamepad

            RawInput.Gamepad_0_Down -= () => ReceiveInput(BA_InputType.GAMEPAD_0_DOWN);
            RawInput.Gamepad_0_Up -= () => ReceiveInput(BA_InputType.GAMEPAD_0_UP);

            RawInput.Gamepad_1_Down -= () => ReceiveInput(BA_InputType.GAMEPAD_1_DOWN);
            RawInput.Gamepad_1_Up -= () => ReceiveInput(BA_InputType.GAMEPAD_1_UP);

            RawInput.Gamepad_Axis_Left_X -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_X);
            RawInput.Gamepad_Axis_Left_Y -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_Y);

            RawInput.Gamepad_Axis_Right_X -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_X);
            RawInput.Gamepad_Axis_Right_Y -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_Y);
        }


        #endregion
    }
}
