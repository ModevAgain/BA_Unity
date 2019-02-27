using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InterfaceTest", menuName = "Testing/Interface", order = 1 )]
public class InterfaceTesting : ScriptableObject, IInitializable /*IUpdatable*/ {

    public InputSystem.BA_RawInput RawInput;

    public void Deinitialize()
    {
        //Debug.Log("Test: Deinitialized");
    }

    public void Initialize()
    {
        RawInput.Mouse_0_Down += MouseUpTest;

        //Debug.Log("RawInput.Mouse_0_Down InvocationCOunt: " + RawInput.Mouse_0_Down.GetInvocationList().Length);

        //Debug.Log("Test: Initialized");
    }

    //void IUpdatable.Update()
    //{
    //   // Debug.Log("Test: Updated");
    //}


    #region DelegateTest

    private void MouseUpTest()
    {
        //Debug.Log("Test: MouseUp");
    }

    #endregion
}
