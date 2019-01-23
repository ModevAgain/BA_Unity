using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static BA.BA_Input;

namespace BA
{
    /// <summary>
    /// 2nd level module
    /// <para>
    /// Hooks on raw input events from 1st level module and processes it.
    /// Throws input events for 3rd level module depending on valid contexts.
    /// </para>
    /// </summary>

    [CreateAssetMenu(fileName = "InputMapper", menuName = "Input/InputMapper", order = 0)]
    public class BA_InputMapper : ScriptableObject, IInitializable
    {

        [Header("Scene References")]
        private BA_InputModule _inputModule;

        [Header("Input References")]
        public BA_RawInput RawInput;
        public List<BA_InputContext> Contexts;

        public List<BA_InputGroup> ValidInputGroups;

        #region Input Delegates

        public delegate void InputDelegate();
        public delegate void InputDelegateVector2(Vector2 v);
        public delegate void InputDelegateVector3(Vector3 v);
        public delegate void InputDelegatePointerEvent(PointerEventData ped);       

        public InputDelegateVector2 MoveInputVector2;
        public InputDelegateVector3 MoveInputVector3;
        public InputDelegatePointerEvent MouseInputUI;
        public InputDelegatePointerEvent TouchInputUI;
        public InputDelegate ActionKey;
        public InputDelegateVector2 ActionKey_2;
        public InputDelegateVector2 DirectionalInputRightStick;



        #endregion


        [SerializeField]
        private List<BA_InputContext> _activeContexts;

        #region Raycasting
        private GraphicRaycaster _gRaycaster;
        private List<RaycastResult> _raycastResults;
        private PointerEventData _ped;

        #endregion  

        private Vector2 _pointerPosition;
        [SerializeField]        
        private Vector2 _gamepadLeft;
        [SerializeField]
        private Vector2 _gamepadRight;

        private int _currentFingerID;

        #region InputReceiver

        public void ReceiveInput(BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            if(type == BA_InputType.MOUSE_0_DOWN)
            {

                int pointerID = -1;

                _ped = _inputModule.GetLastPointerEventDataCustom(pointerID);

                if (_ped == null)
                    Debug.Log("ped is null in " + type + " ==  " + pointerID);

                _raycastResults.Clear();

                _gRaycaster.Raycast(_ped, _raycastResults);

                //Move
                if (_raycastResults.Count == 0)
                {
                    MoveInputVector3(_pointerPosition);
                }
                //Delegate to UI Input Receiver
                else
                {
                        MouseInputUI(_ped);
                }
            }
            else if(type == BA_InputType.GAMEPAD_0_DOWN ||type == BA_InputType.KEYBOARD_0_DOWN)
            {
                ActionKey?.Invoke();
            }
            else if (type == BA_InputType.GAMEPAD_1_DOWN)
            {
                float gamepadToScreenPoint_x = (Screen.width / 2) + Screen.width * _gamepadRight.x;
                float gamepadToScreenPoint_y = (Screen.height / 2) + Screen.height * _gamepadRight.y;

                ActionKey_2?.Invoke(new Vector2(gamepadToScreenPoint_x, gamepadToScreenPoint_y));
            }
            else if(type == BA_InputType.KEYBOARD_1_DOWN)
            {
                ActionKey_2?.Invoke(_pointerPosition);
            }
            else if(type == BA_InputType.TOUCH_0_UP)
            {

                _ped = new PointerEventData(EventSystem.current);
                _ped.position = _pointerPosition;
                //_ped = _inputModule.GetLastPointerEventDataCustom(_currentFingerID);

                if (_ped == null)
                {
                    Debug.Log("ped is null in touch up");
                    Debug.Log("fingerID: " + _currentFingerID);
                }

                _raycastResults.Clear();

                _gRaycaster.Raycast(_ped, _raycastResults);

                foreach (var result in _raycastResults)
                {
                    if (result.gameObject.GetComponent<BA_BaseUIElement>() != null)
                        _ped.pointerEnter = result.gameObject;
                }

                if(_raycastResults.Count == 0)
                {
                    MoveInputVector3(_pointerPosition);
                }
                else
                {
                    TouchInputUI(_ped);
                }

            }
        }

        public void ReceiveInputVector2(Vector2 vec, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;            

            _pointerPosition = vec;
                      
        }

        Vector2 pPos;
        Vector2 mappedDirection;

        public void ReceiveInputSwipe(Vector2 start, Vector2 end, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            //Dirty QuickFix: Get PlayerPosToScreenPoint for directions origin
            if (type == BA_InputType.TOUCH_0_SWIPE)
            {

                Vector2 direction = (end - start);/*.normalized*/
                //Debug.Log("start: " + start + " |   end: " + end + " |  direction not normalized: " + (end - start));
                //Vector2 mappedDirection = new Vector2(Screen.width / 2f + (Screen.width/2) * direction.x, Screen.height / 2f + (Screen.height/2) * direction.y);

                pPos = Camera.main.WorldToScreenPoint(DataPipe.instance.PlayerReferences.transform.position);

                mappedDirection = new Vector2((pPos.x) + direction.x, (pPos.y) + direction.y);

                //Debug.Log("direction: " + direction + " | mappedDirectionToPosition: " + mappedDirection);

                //Debug.Log("screenRay: " + direction.normalized);

                //GameObject.FindGameObjectWithTag("DebugUI2").GetComponent<RectTransform>().position = end;
                //GameObject.FindGameObjectWithTag("DebugUI").GetComponent<RectTransform>().position = mappedDirection;

                ActionKey_2.Invoke(mappedDirection);
            }
        }

        public void ReceiveInputTouch(Touch t, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            _pointerPosition = t.position;
            _currentFingerID = t.fingerId;
            
            //Debug.Log("fingerID" + t.fingerId);

        }

        public void ReceiveInputFloat(float f, BA_InputType type)
        {
            if (!IsInputValid(type))
                return;

            if(type == BA_InputType.GAMEPAD_AXIS_LEFT_X)
            {
                _gamepadLeft.x = f;
            }
            else if(type == BA_InputType.GAMEPAD_AXIS_LEFT_Y)
            {
                _gamepadLeft.y = f;
            }
            else if (type == BA_InputType.GAMEPAD_AXIS_RIGHT_X)
            {
                _gamepadRight.x = f;
            }
            else if (type == BA_InputType.GAMEPAD_AXIS_RIGHT_Y)
            {
                _gamepadRight.y = f;
            }

            MoveInputVector2(_gamepadLeft);
            if(_gamepadRight != Vector2.zero)
                DirectionalInputRightStick(_gamepadRight);

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

        #region ContextManaging

        public void LoadContexts(List<BA_InputGroup> validInputGroups)
        {
            _activeContexts = Contexts.Where((context) => validInputGroups.Contains(context.Group)).ToList();
            ValidInputGroups = validInputGroups;
        }

        public void LoadContext(BA_InputGroup g, BA_InputContext.BA_ContextType t)
        {
            BA_InputContext context = Contexts.Where((c) => c.Group == g && c.Type == t).FirstOrDefault();

            if (context == null)
                return;
            if (_activeContexts.Contains(context))
                return;
            if (!ValidInputGroups.Contains(g))
                return;

            _activeContexts.Add(context);
        }

        public void UnloadContext(BA_InputGroup g, BA_InputContext.BA_ContextType t)
        {
            BA_InputContext context = Contexts.Where((c) => c.Group == g && c.Type == t).FirstOrDefault();

            if (context == null)
                return;
            if (!_activeContexts.Contains(context))
                return;

            _activeContexts.Remove(context);
        }

        #endregion

        #region IInitializable

        public void Initialize()
        {

            #region RESET
            RawInput.Mouse_0_Down = null;
            RawInput.Mouse_0_Up = null;
            RawInput.Mouse_Position = null;

            RawInput.Keyboard_0_Down = null;
            RawInput.Keyboard_0_Up = null;
            RawInput.Keyboard_1_Down = null;
            RawInput.Keyboard_1_Up = null;

            RawInput.Touch_0 = null;
            RawInput.Touch_0_Down = null;
            RawInput.Touch_0_Up = null;
            RawInput.Touch_1 = null;
            RawInput.Touch_1_Down = null;
            RawInput.Touch_1_Up = null;

            RawInput.Gamepad_0_Down = null;
            RawInput.Gamepad_0_Up = null;
            RawInput.Gamepad_1_Down = null;
            RawInput.Gamepad_1_Up = null;

            RawInput.Gamepad_Axis_Left_X = null;
            RawInput.Gamepad_Axis_Left_Y = null;

            RawInput.Gamepad_Axis_Right_X = null;
            RawInput.Gamepad_Axis_Right_Y = null;

            #endregion

            //Mouse & Keyboard

            RawInput.Mouse_0_Down += () => ReceiveInput(BA_InputType.MOUSE_0_DOWN);
            RawInput.Mouse_0_Up += () => ReceiveInput(BA_InputType.MOUSE_0_UP);
            RawInput.Mouse_Position += (v) => ReceiveInputVector2(v,BA_InputType.MOUSE_0_POS);

            RawInput.Keyboard_0_Down += () => ReceiveInput(BA_InputType.KEYBOARD_0_DOWN);
            RawInput.Keyboard_0_Up += () => ReceiveInput(BA_InputType.KEYBOARD_0_UP);

            RawInput.Keyboard_1_Down += () => ReceiveInput(BA_InputType.KEYBOARD_1_DOWN);
            RawInput.Keyboard_1_Up += () => ReceiveInput(BA_InputType.KEYBOARD_1_UP);

            //Touch
           
            RawInput.Touch_0 += (t) => ReceiveInputTouch(t, BA_InputType.TOUCH_0);
            RawInput.Touch_0_Down += () => ReceiveInput(BA_InputType.TOUCH_0_DOWN);
            RawInput.Touch_0_Up += () => ReceiveInput(BA_InputType.TOUCH_0_UP);
            RawInput.Swipe_0 += (s,e) => ReceiveInputSwipe(s, e, BA_InputType.TOUCH_0_SWIPE);

            //Gamepad

            RawInput.Gamepad_0_Down += () => ReceiveInput(BA_InputType.GAMEPAD_0_DOWN);
            RawInput.Gamepad_0_Up += () => ReceiveInput(BA_InputType.GAMEPAD_0_UP);

            RawInput.Gamepad_1_Down += () => ReceiveInput(BA_InputType.GAMEPAD_1_DOWN);
            RawInput.Gamepad_1_Up += () => ReceiveInput(BA_InputType.GAMEPAD_1_UP);

            _gamepadLeft = Vector2.zero;
            _gamepadRight = Vector2.zero;

            RawInput.Gamepad_Axis_Left_X += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_X);
            RawInput.Gamepad_Axis_Left_Y += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_Y);

            RawInput.Gamepad_Axis_Right_X += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_X);
            RawInput.Gamepad_Axis_Right_Y += (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_Y);


            //Raycasting
            _gRaycaster = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<GraphicRaycaster>();
            _raycastResults = new List<RaycastResult>();
            _ped = new PointerEventData(EventSystem.current);

            //InputModule
            _inputModule = FindObjectOfType<BA_InputModule>();

        }


        public void Deinitialize()
        {
            //Mouse & Keyboard

            RawInput.Mouse_0_Down -= () => ReceiveInput(BA_InputType.MOUSE_0_DOWN);
            RawInput.Mouse_0_Up -= () => ReceiveInput(BA_InputType.MOUSE_0_UP);
            RawInput.Mouse_Position -= (v) => ReceiveInputVector2(v, BA_InputType.MOUSE_0_POS);

            RawInput.Keyboard_0_Down -= () => ReceiveInput(BA_InputType.KEYBOARD_0_DOWN);
            RawInput.Keyboard_0_Up -= () => ReceiveInput(BA_InputType.KEYBOARD_0_UP);

            RawInput.Keyboard_1_Down -= () => ReceiveInput(BA_InputType.KEYBOARD_1_DOWN);
            RawInput.Keyboard_1_Up -= () => ReceiveInput(BA_InputType.KEYBOARD_1_UP);

            //Touch
            RawInput.Touch_0_Down -= () => ReceiveInput(BA_InputType.TOUCH_0_DOWN);
            RawInput.Touch_0_Up -= () => ReceiveInput(BA_InputType.TOUCH_0_UP);
            RawInput.Touch_0 -= (t) => ReceiveInputTouch(t, BA_InputType.TOUCH_0);
            RawInput.Swipe_0 -= (s,e) => ReceiveInputSwipe(s, e, BA_InputType.TOUCH_0_SWIPE);

            //Gamepad

            RawInput.Gamepad_0_Down -= () => ReceiveInput(BA_InputType.GAMEPAD_0_DOWN);
            RawInput.Gamepad_0_Up -= () => ReceiveInput(BA_InputType.GAMEPAD_0_UP);

            RawInput.Gamepad_1_Down -= () => ReceiveInput(BA_InputType.GAMEPAD_1_DOWN);
            RawInput.Gamepad_1_Up -= () => ReceiveInput(BA_InputType.GAMEPAD_1_UP);

            RawInput.Gamepad_Axis_Left_X -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_X);
            RawInput.Gamepad_Axis_Left_Y -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_LEFT_Y);

            RawInput.Gamepad_Axis_Right_X -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_X);
            RawInput.Gamepad_Axis_Right_Y -= (f) => ReceiveInputFloat(f, BA_InputType.GAMEPAD_AXIS_RIGHT_Y);


            //Raycasting
            _raycastResults.Clear();
        }


        #endregion
    }
}
