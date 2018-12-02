using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BA
{
    [CreateAssetMenu(fileName = "InputReceiverUI", menuName = "Input/InputReceiverUI", order = 3)]
    public class BA_InputReceiverUI : ScriptableObject, IInitializable {

        [Header("References")]
        public BA_InputMapper InputMapper;





        private void ReceiveMouseInput(PointerEventData ped)
        {
            Debug.Log(ped.pointerEnter);
            
        }

        public void Initialize()
        {
            #region Reset

            InputMapper.MouseInputUI = null;

            #endregion

            InputMapper.MouseInputUI += ReceiveMouseInput;
        }

        public void Deinitialize()
        {
            InputMapper.MouseInputUI -= ReceiveMouseInput;
        }
    }


}
