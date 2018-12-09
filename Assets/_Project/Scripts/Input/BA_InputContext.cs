using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BA.BA_Input;

namespace BA
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

        public BA_ContextType Type;
        public BA_InputGroup Group;
        public List<BA_InputType> AllowedInputs;



        public enum BA_ContextType
        {
            MOVE,
            SHOOT,
            BUILD,
        }

    }
}
