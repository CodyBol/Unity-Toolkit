using System;
using ToolKit.Easing;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKit.UI
{
    /// <summary>
    /// WIP
    /// </summary>
    public class Modal : MonoBehaviour
    {
        [Header("Animation Settings")] [SerializeField]
        private AnimationCurve _defaultCurve = EasingCurve.Smooth();

        [SerializeField] private EasingType _openType = EasingType.Scale;
        [SerializeField] private EasingType _closeType = EasingType.Scale;
        [SerializeField] private float _defaultDuration = 1;
        
        private Vector3 _startPosition = Vector3.zero;

        [Header("Scrollbar Settings")] [SerializeField]
        private bool _useScrollBar;
        [SerializeField] private Transform _content;
        [SerializeField]private ScrollRect _scrollview;

        /// <summary>
        /// Resets scroll of scrollbar
        /// </summary>
        public void ResetScroll()
        {
            if (_useScrollBar)
            {
                _content.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// Closes modal with an animation curve
        /// </summary>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="action">runs when closed</param>
        public void Close(AnimationCurve curve, Action action = null)
        {
            transform.GetComponentInChildren<Button>().enabled = false;
            
            Action finished  = () =>
            {
                ResetScroll();
                if (_useScrollBar)
                {
                    _scrollview.enabled = false;
                }

                action?.Invoke();
                transform.localPosition = _startPosition;
                transform.localScale = Vector3.zero;
            };

            //TODO clean up this spaghetti code and make this a separate function that can be used in Open()
            if (_closeType == EasingType.SlideDown || _closeType == EasingType.SlideUp)
            {
                transform.localScale = new Vector3(1, 1, 1);
                float objectSize = (transform as RectTransform).rect.height;
                Vector3 target = new Vector3(transform.position.x,
                    _closeType == EasingType.SlideUp ? 0 - objectSize : Screen.height + objectSize,
                    transform.position.z);

                StartCoroutine(Ease.ToVector3(transform, target, curve, _defaultDuration, finished));
            }
            else if (_closeType == EasingType.SlideLeft || _closeType == EasingType.SlideRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                float objectSize = (transform as RectTransform).rect.width;
                Vector3 target = new Vector3(
                    _closeType == EasingType.SlideLeft ? 0 - objectSize : Screen.width + objectSize,
                    transform.position.y,
                    transform.position.z);

                StartCoroutine(Ease.ToVector3(transform, target, curve, _defaultDuration, finished));
            }
            else if (_closeType == EasingType.Scale)
            {
                StartCoroutine(Ease.OutScale(transform, curve, _defaultDuration, finished));
            }
            else if (_closeType == EasingType.Fade)
            {
                StartCoroutine(Ease.Fade(transform, true, curve, _defaultDuration, action));
            }
        }

        /// <summary>
        /// Close modal used for UI buttons
        /// </summary>
        public void CloseModal()
        {
            Close(_defaultCurve);
        }

        /// <summary>
        /// Close modal used for UI buttons with a finished action
        /// </summary>
        /// <param name="action">Run when finished</param>
        public void CloseModal(Action action)
        {
            Close(_defaultCurve, action);
        }

        /// <summary>
        /// Opens modal with an animation curve
        /// </summary>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="action">runs when opened</param>
        public void Open(AnimationCurve curve, Action action = null)
        { 
            Action finished = () =>
            {
                transform.GetComponentInChildren<Button>().enabled = true;
                action?.Invoke();
            };
            
            ResetScroll();
            if (_useScrollBar)
            {
                _scrollview.enabled = true;
            }

            //TODO clean up this spaghetti code and make this a separate function that can be used in Close()
            if (_openType == EasingType.SlideDown || _openType == EasingType.SlideUp)
            {
                transform.localScale = new Vector3(1, 1, 1);
                _startPosition = transform.position;
                float objectSize = (transform as RectTransform).rect.height;

                transform.position = new Vector3(transform.position.x,
                    _openType == EasingType.SlideUp ? 0 - objectSize : Screen.height + objectSize,
                    transform.position.z);

                StartCoroutine(Ease.ToVector3(transform, _startPosition, curve, _defaultDuration, finished));
            }
            else if (_openType == EasingType.SlideLeft || _openType == EasingType.SlideRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                _startPosition = transform.position;
                float objectSize = (transform as RectTransform).rect.width;

                transform.position = new Vector3(
                    _openType == EasingType.SlideLeft ? 0 - objectSize : Screen.width + objectSize,
                    transform.position.y,
                    transform.position.z);

                StartCoroutine(Ease.ToVector3(transform, _startPosition, curve, _defaultDuration, finished));
            }
            else if (_openType == EasingType.Scale)
            {
                StartCoroutine(Ease.InScale(transform, Vector3.one, curve, _defaultDuration, finished));
            }
            else if (_openType == EasingType.Fade)
            {
                StartCoroutine(Ease.Fade(transform, false, curve, _defaultDuration, finished));
            }
        }

        /// <summary>
        /// Open modal used for UI buttons
        /// </summary>
        public void OpenModal()
        {
            Open(_defaultCurve);
        }
    }
}