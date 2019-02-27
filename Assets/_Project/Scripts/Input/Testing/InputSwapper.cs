using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSwapper : MonoBehaviour {

    public GameData GameData;

    public Toggle GamepadToggle;
    public Toggle MKToggle;
    public Toggle TouchToggle;

    public InputSystem.BA_InputMapper Mapper;

    private List<InputSystem.BA_Input.BA_InputGroup> _groups;

    // Use this for initialization
    void Start () {

        _groups = new List<InputSystem.BA_Input.BA_InputGroup>();

        GamepadToggle.onValueChanged.AddListener(SwapInputGamepad);
        MKToggle.onValueChanged.AddListener(SwapInputMK);
        TouchToggle.onValueChanged.AddListener(SwapInputTouch);

        SwapInputGamepad(GamepadToggle.isOn);
        SwapInputMK(MKToggle.isOn);
        SwapInputTouch(TouchToggle.isOn);


    }

    private void SwapInputGamepad(bool active)
    {
        GameData.Gamepad = active;
        if (active)
        {
            if (_groups.Contains(InputSystem.BA_Input.BA_InputGroup.GAMEPAD))
                return;
            else _groups.Add(InputSystem.BA_Input.BA_InputGroup.GAMEPAD);
        }
        else
        {
            if (!_groups.Contains(InputSystem.BA_Input.BA_InputGroup.GAMEPAD))
                return;
            else _groups.Remove(InputSystem.BA_Input.BA_InputGroup.GAMEPAD);
        }

        Mapper.LoadContexts(_groups);


    }
    private void SwapInputMK(bool active)
    {
        GameData.MK = active;
        if (active)
        {
            if (_groups.Contains(InputSystem.BA_Input.BA_InputGroup.MOUSE_KEYBOARD))
                return;
            else _groups.Add(InputSystem.BA_Input.BA_InputGroup.MOUSE_KEYBOARD);
        }
        else
        {
            if (!_groups.Contains(InputSystem.BA_Input.BA_InputGroup.MOUSE_KEYBOARD))
                return;
            else _groups.Remove(InputSystem.BA_Input.BA_InputGroup.MOUSE_KEYBOARD);
        }

        Mapper.LoadContexts(_groups);

    }
    private void SwapInputTouch(bool active)
    {
        GameData.Touch = active;
        if (active)
        {
            if (_groups.Contains(InputSystem.BA_Input.BA_InputGroup.TOUCH))
                return;
            else _groups.Add(InputSystem.BA_Input.BA_InputGroup.TOUCH);
        }
        else
        {
            if (!_groups.Contains(InputSystem.BA_Input.BA_InputGroup.TOUCH))
                return;
            else _groups.Remove(InputSystem.BA_Input.BA_InputGroup.TOUCH);
        }

        Mapper.LoadContexts(_groups);

    }


}
