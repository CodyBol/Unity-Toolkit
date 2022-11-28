using System;
using System.Collections;
using System.Numerics;
using ToolKit.Easing;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace ToolKit.Camera
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Current;
        private Coroutine _routine;

        [SerializeField] private float _moveduration;
        [SerializeField] private AnimationCurve _moveCurve = EasingCurve.Smooth();

        public void Awake() => Current = this;

        /// <summary>
        /// Starts a coroutine that shakes the screen.
        /// </summary>
        /// <param name="duration">How long should it shake</param>
        /// <param name="curve">How the shake changes over the duration</param>
        /// <param name="strengthModifier">Amplifies the values of the curve</param>
        /// <param name="finished">Runs when the shake is over</param>
        public void ShakeScreen(float duration, AnimationCurve curve, float strengthModifier = 1,
            Action finished = null)
        {
            _routine = StartCoroutine(Shake(duration, curve, strengthModifier));
        }

        /// <summary>
        /// Starts a coroutine that moves this camera towards another position with a customizable curve
        /// </summary>
        /// <param name="target">The end position</param>
        /// <param name="finished">Runs when the target is reached</param>
        /// <param name="ignoreZ">Set if the z should be ignored (needed for 3d games)</param>
        public void MoveTowards(Vector3 target, Action finished = null, bool ignoreZ = false)
        {
            StartCoroutine(Ease.ToVector3(transform,
                new Vector3(target.x, target.y, transform.position.z),
                _moveCurve, _moveduration, finished, ignoreZ));
        }

        /// <summary>
        /// This runs when the ShakeScreen method is called
        /// </summary>
        private IEnumerator Shake(float duration, AnimationCurve curve, float strengthModifier = 1,
            Action finished = null)
        {
            Vector3 startPosition = transform.localPosition;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(elapsedTime / duration) * strengthModifier;
                transform.localPosition = startPosition + Random.insideUnitSphere * strength;
                yield return null;
            }

            transform.localPosition = startPosition;
            _routine = null;
            finished?.Invoke();
        }

        public bool RoutineFinished()
        {
            return _routine == null;
        }
    }
}