using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BA
{
    [System.Serializable]
    public class BA_Input
    {

        public BA_InputType Type;
        public BA_InputGroup Group;

        public enum BA_InputType
        {
            MOUSE_0_DOWN,
            MOUSE_0_UP,
            MOUSE_0_POS,

            KEYBOARD_0_DOWN,
            KEYBOARD_0_UP,
            KEYBOARD_1_DOWN,
            KEYBOARD_1_UP,

            TOUCH_0_DOWN,
            TOUCH_0_UP,
            TOUCH_0,

            TOUCH_1_DOWN,
            TOUCH_1_UP,
            TOUCH_1,

            GAMEPAD_AXIS_LEFT_X,
            GAMEPAD_AXIS_LEFT_Y,
            GAMEPAD_AXIS_RIGHT_X,
            GAMEPAD_AXIS_RIGHT_Y,

            GAMEPAD_0_DOWN,
            GAMEPAD_0_UP,
            GAMEPAD_1_DOWN,
            GAMEPAD_1_UP,

        }

        public enum BA_InputGroup
        {
            MOUSE_KEYBOARD,
            TOUCH,
            GAMEPAD
        }        
    }
}
