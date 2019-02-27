using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    [CreateAssetMenu(fileName = "InputRegulator", menuName = "Input/InputRegulator", order = 4)]
    public class BA_ContextRegulator : ScriptableObject
    {

        public BA_InputMapper InputMapper;

        public void Building_Activated()
        {
            InputMapper.UnloadContext(BA_Input.BA_InputGroup.MOUSE_KEYBOARD, "SHOOT");
            InputMapper.UnloadContext(BA_Input.BA_InputGroup.GAMEPAD, "SHOOT");
            InputMapper.UnloadContext(BA_Input.BA_InputGroup.TOUCH, "SHOOT");
        }

        public void Building_Deactivated()
        {
            //InputMapper.LoadContext(BA_Input.BA_InputGroup.MOUSE_KEYBOARD, BA_InputContext.BA_ContextType.SHOOT);
            //InputMapper.LoadContext(BA_Input.BA_InputGroup.GAMEPAD, BA_InputContext.BA_ContextType.SHOOT);
            //InputMapper.LoadContext(BA_Input.BA_InputGroup.TOUCH, BA_InputContext.BA_ContextType.SHOOT);
            InputMapper.LoadContext(BA_Input.BA_InputGroup.MOUSE_KEYBOARD, "SHOOT");
            InputMapper.LoadContext(BA_Input.BA_InputGroup.GAMEPAD, "SHOOT");
            InputMapper.LoadContext(BA_Input.BA_InputGroup.TOUCH, "SHOOT");
        }
    }
}
