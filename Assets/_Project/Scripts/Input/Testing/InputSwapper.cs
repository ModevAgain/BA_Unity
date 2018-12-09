using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSwapper : MonoBehaviour {

    public Toggle GamepadToggle;
    public Toggle MKToggle;
    public Toggle TouchToggle;

    public BA.BA_InputMapper Mapper;

    private List<BA.BA_Input.BA_InputGroup> _groups;

    // Use this for initialization
    void Start () {

        _groups = new List<BA.BA_Input.BA_InputGroup>();

        GamepadToggle.onValueChanged.AddListener(SwapInputGamepad);
        MKToggle.onValueChanged.AddListener(SwapInputMK);
        TouchToggle.onValueChanged.AddListener(SwapInputTouch);

        SwapInputGamepad(true);
        SwapInputMK(true);
        SwapInputTouch(true);


    }

    private void SwapInputGamepad(bool active)
    {
        if (active)
        {
            if (_groups.Contains(BA.BA_Input.BA_InputGroup.GAMEPAD))
                return;
            else _groups.Add(BA.BA_Input.BA_InputGroup.GAMEPAD);
        }
        else
        {
            if (!_groups.Contains(BA.BA_Input.BA_InputGroup.GAMEPAD))
                return;
            else _groups.Remove(BA.BA_Input.BA_InputGroup.GAMEPAD);
        }

        Mapper.LoadContexts(_groups);

    }
    private void SwapInputMK(bool active)
    {
        if (active)
        {
            if (_groups.Contains(BA.BA_Input.BA_InputGroup.MOUSE_KEYBOARD))
                return;
            else _groups.Add(BA.BA_Input.BA_InputGroup.MOUSE_KEYBOARD);
        }
        else
        {
            if (!_groups.Contains(BA.BA_Input.BA_InputGroup.MOUSE_KEYBOARD))
                return;
            else _groups.Remove(BA.BA_Input.BA_InputGroup.MOUSE_KEYBOARD);
        }

        Mapper.LoadContexts(_groups);
    }
    private void SwapInputTouch(bool active)
    {
        if (active)
        {
            if (_groups.Contains(BA.BA_Input.BA_InputGroup.TOUCH))
                return;
            else _groups.Add(BA.BA_Input.BA_InputGroup.TOUCH);
        }
        else
        {
            if (!_groups.Contains(BA.BA_Input.BA_InputGroup.TOUCH))
                return;
            else _groups.Remove(BA.BA_Input.BA_InputGroup.TOUCH);
        }

        Mapper.LoadContexts(_groups);
    }


}
