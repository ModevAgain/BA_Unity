using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputSystem.BA_Input;
using NaughtyAttributes;

namespace InputSystem
{
    /// <summary>
    /// 2nd level assistant module
    /// 
    /// <para>
    /// Generates input contexts for validating incoming inputs.
    /// Assists 2nd level module InputMapper.
    /// </para>
    /// </summary>

    [CreateAssetMenu(fileName = "InputContext", menuName = "Input/InputContext", order = 2)]
    public class BA_InputContext : ScriptableObject
    {
        [Dropdown("BA_ContextTypes")]
        public string ContextType;
        public BA_InputGroup Group;
        public List<BA_InputType> AllowedInputs;




        public enum BA_ContextType
        {
            MOVE,
            SHOOT,
            BUILD,
        }

        #region ContextType

        public static DropdownList<string> BA_ContextTypes = new DropdownList<string>()
        { 
            { "MOVE", "MOVE" },
            { "SHOOT", "SHOOT"},
            { "BUILD", "BUILD"}
        };

        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]        
        public string NewContext;
        
        [Button("Add new Context")]
        public void AddNewContext()
        {
            BA_ContextTypes.Add(NewContext, NewContext);
            NewContext = "Success!";
        }

        
        [Button("Reset to Default Contexts")]
        public void ResetContexts()
        {
            BA_ContextTypes = new DropdownList<string>()
            {
                { "MOVE", "MOVE" },
                { "SHOOT", "SHOOT"},
                { "BUILD", "BUILD"}
            };

            NewContext = "";
        }

        #endregion  
    }
}
