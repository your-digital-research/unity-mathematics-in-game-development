using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public abstract class CameraButton : MonoBehaviour
    {
        #region PRIVATE_VARIABLES

        private EventTrigger _eventTrigger;

        #endregion

        #region MONO

        private void Awake()
        {
            GetComponents();
        }

        private void Start()
        {
            Init();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        #endregion

        #region ABSTRACT_FUNCTIONS

        protected abstract void OnPointerDown(PointerEventData pointerEventData);
        protected abstract void OnPointerUp(PointerEventData pointerEventData);

        #endregion

        #region PRIVATE_FUNCTIONS

        private void GetComponents()
        {
            _eventTrigger = GetComponent<EventTrigger>();
        }

        private void Init()
        {
            AddListeners();
        }

        private void AddListeners()
        {
            // Add a callback to the PointerClick event
            EventTrigger.Entry pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            EventTrigger.Entry pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };

            pointerDown.callback.AddListener((data) => OnPointerDown((PointerEventData)data));
            pointerUp.callback.AddListener((data) => OnPointerUp((PointerEventData)data));

            _eventTrigger.triggers.Add(pointerDown);
            _eventTrigger.triggers.Add(pointerUp);
        }

        private void RemoveListeners()
        {
            _eventTrigger.triggers.Clear();
        }

        #endregion
    }
}