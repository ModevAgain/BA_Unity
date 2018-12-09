using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

namespace BA
{
    /// <summary>
    /// 3rd level module for UI interction
    /// 
    /// <para>
    /// Generates frontend logic for processing on referenced BaseUIElements
    /// </para>
    /// </summary>

    [CreateAssetMenu(fileName = "InputReceiverUI", menuName = "Input/InputReceiverUI", order = 3)]
    public class BA_InputReceiverUI : ScriptableObject, IInitializable {

        public delegate void UIFunction();

        [Header("References")]
        public BA_InputMapper InputMapper;


        //UI Meta Functions
        public UIFunction BuildingMenu;
        //public UIFunction HideBuildingMenu;

        public UIFunction HighlightOption;
        public UIFunction DehighlightOption;
        public UIFunction SelectOption;


        private void ReceiveMouseInput(PointerEventData ped)
        {
            
            ped.pointerEnter?.GetComponent<BA_BaseUIElement>()?.OnPointerClick(ped);
            
        }

        private void ReceiveTouchInput(PointerEventData ped)
        {
            ped.pointerEnter?.GetComponent<BA_BaseUIElement>()?.OnPointerClick(ped);
        }

        private void ReceiveActionkeyInput()
        {
            BuildingMenu();
        }

        private void ReceiveDirectionalInput(Vector2 dir)
        {

        }

        #region Initializable
        public void Initialize()
        {
            #region Reset

            InputMapper.MouseInputUI = null;

            #endregion

            InputMapper.MouseInputUI += ReceiveMouseInput;
            InputMapper.TouchInputUI += ReceiveTouchInput;
            InputMapper.ActionKey += ReceiveActionkeyInput;

            _instance = Instance;
        }


        public void Deinitialize()
        {
            InputMapper.MouseInputUI -= ReceiveMouseInput;
            InputMapper.TouchInputUI -= ReceiveTouchInput;
            InputMapper.ActionKey -= ReceiveActionkeyInput;

            _instance = null;
        }

        #endregion

        static BA_InputReceiverUI _instance = null;
        public static BA_InputReceiverUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    List<BA_InputReceiverUI> sList = Resources.LoadAll<BA_InputReceiverUI>("").ToList();
                    _instance = (BA_InputReceiverUI) sList.Where((p) => p is BA_InputReceiverUI).FirstOrDefault();
                }
                return _instance;
            }
        }

        
    }


}
