using Temp;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace Code.UI.View
{
    public class JoystickView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private RectTransform _joystickBackgroundRect;
        [SerializeField] private OnScreenStick _onScreenJoystick;
        
        [Header("Hiding")]
        [SerializeField] private Image _joystickImage;
        [SerializeField] private Image _knobImage;

        private void Awake()
        {
            SetActiveJoystick(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            ExecuteEvents.dragHandler(_onScreenJoystick, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetActiveJoystick(true);
            
            _joystickBackgroundRect.anchoredPosition = eventData.position / UITag.SelfRect.localScale;

            ExecuteEvents.pointerDownHandler(_onScreenJoystick, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetActiveJoystick(false);
            
            ExecuteEvents.pointerUpHandler(_onScreenJoystick, eventData);
        }

        private void SetActiveJoystick(bool value)
        {
            _joystickImage.enabled = value;
            _knobImage.enabled = value;
        }
    }
}