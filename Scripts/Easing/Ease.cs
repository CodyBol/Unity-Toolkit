using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKit.Easing
{
    public class Ease
    {
        /// <summary>
        /// Moves a transform over to a position with the use of an animation curve for smooth movement
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="endPosition">Target position</param>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="duration">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <param name="ignoreZ">If the z should be changed as well</param>
        /// <returns></returns>
        public static IEnumerator ToVector3(Transform transform, Vector3 endPosition, AnimationCurve curve,
            float duration, Action finished = null, bool ignoreZ = false)
        {
            if (ignoreZ)
            {
                endPosition.z = transform.position.z;
            }

            float startTime = duration;
            Vector3 startPosition = transform.position;
            Vector3 distance = endPosition - startPosition;

            while (duration > 0 && transform != null)
            {
                duration -= Time.deltaTime;

                float strength = curve.Evaluate((startTime - duration) / startTime);
                transform.position = startPosition + distance * strength;
                yield return null;
            }

            transform.position = endPosition;

            Finished(finished);
        }
        
        /// <summary>
        /// Moves a transform over to a position local with the use of an animation curve for smooth movement
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="endPosition">Target local position</param>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="duration">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <param name="ignoreZ">If the z should be changed as well</param>
        /// <returns></returns>
        public static IEnumerator ToVector3Local(Transform transform, Vector3 endPosition, AnimationCurve curve,
            float duration, Action finished = null, bool ignoreZ = false)
        {
            if (ignoreZ)
            {
                endPosition.z = transform.localPosition.z;
            }

            float startTime = duration;
            Vector3 startPosition = transform.localPosition;
            Vector3 distance = endPosition - startPosition;

            while (duration > 0 && transform != null)
            {
                duration -= Time.deltaTime;

                float strength = curve.Evaluate((startTime - duration) / startTime);
                transform.localPosition = startPosition + distance * strength;
                yield return null;
            }

            transform.localPosition = endPosition;

            Finished(finished);
        }
        
        /// <summary>
        /// Adds a local amount to the current transform position
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="addition">The amount that is locally added to the transform</param>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="duration">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <param name="ignoreZ">If the z should be changed as well</param>
        /// <returns></returns>
        public static IEnumerator AddVector3Local(Transform transform, Vector3 addition, AnimationCurve curve,
            float duration, Action finished = null, bool ignoreZ = false)
        {
            Vector3 endPosition = transform.localPosition + addition;
            if (ignoreZ)
            {
                endPosition.z = transform.localPosition.z;
            }

            float startTime = duration;
            Vector3 startPosition = transform.localPosition;
            Vector3 distance = endPosition - startPosition;

            while (duration > 0 && transform != null)
            {
                duration -= Time.deltaTime;

                float strength = curve.Evaluate((startTime - duration) / startTime);
                transform.localPosition = startPosition + distance * strength;
                yield return null;
            }

            transform.localPosition = endPosition;

            Finished(finished);
        }

        /// <summary>
        /// Rotates the transform to a certain degrees
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="endRotation">Target rotation</param>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="duration">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <returns></returns>
        public static IEnumerator ToDegrees(Transform transform, Vector3 endRotation, AnimationCurve curve,
            float duration, Action finished = null)
        {
            float startTime = duration;
            Vector3 startRotation = transform.eulerAngles;
            Vector3 distance = endRotation - startRotation;

            while (duration > 0)
            {
                duration -= Time.deltaTime;

                float strength = curve.Evaluate((startTime - duration) / startTime);
                transform.eulerAngles = startRotation + distance * strength;
                yield return null;
            }

            transform.eulerAngles = endRotation;

            Finished(finished);
        }
        
        /// <summary>
        /// Rotates the front of a transform towards a target gameobject
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="target">Target (gameobject) that is going to be rotated at</param>
        /// <param name="speed">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <returns></returns>
        public static IEnumerator ToTarget(Transform transform, GameObject target,
            float speed, Action finished = null)
        {
            yield return ToTarget(transform, target.transform.position, speed, finished);
        }
        
        /// <summary>
        /// Rotates the front of a transform towards a target position
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="target">Target that is going to be rotated at</param>
        /// <param name="speed">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <returns></returns>
        public static IEnumerator ToTarget(Transform transform, Vector3 target,
            float speed, Action finished = null)
        {
            
            Quaternion prev = new Quaternion(-1, -1, -1, -1);
            while (transform != null && prev != transform.rotation)
            {
                prev = transform.rotation;
                
                Vector3 targetDirection = target - transform.position;

                float singleStep = speed * Time.deltaTime;

                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                transform.rotation = Quaternion.LookRotation(newDirection);
                yield return null;

            }

            Finished(finished);
        }

        /// <summary>
        /// Scales in a transform
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="endScale">Scale it will be after the ease</param>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="duration">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <returns></returns>
        public static IEnumerator InScale(Transform transform, Vector3 endScale, AnimationCurve curve, float duration,
            Action finished = null)
        {
            float startTime = duration;
            Vector3 startScale = transform.localScale;
            Vector3 scaleDifference = endScale - startScale;

            while (duration > 0)
            {
                duration -= Time.deltaTime;

                float strength = curve.Evaluate((startTime - duration) / startTime);
                if (transform.localScale != startScale + scaleDifference * strength)
                {
                    transform.localScale = startScale + scaleDifference * strength;
                }
                yield return null;
            }

            transform.localScale = endScale;

            Finished(finished);
        }

        /// <summary>
        /// Scales out a transform to Vector3.zero scale
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="duration">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <returns></returns>
        public static IEnumerator OutScale(Transform transform, AnimationCurve curve, float duration,
            Action finished = null)
        {
            float startTime = duration;
            Vector3 startScale = transform.localScale;
            Vector3 scaleDifference = Vector3.zero - startScale;

            while (duration > 0)
            {
                duration -= Time.deltaTime;

                float strength = curve.Evaluate((startTime - duration) / startTime);
                transform.localScale = startScale + scaleDifference * strength;
                yield return null;
            }

            transform.localScale = Vector3.zero;

            Finished(finished);
        }

        /// <summary>
        /// [UI only] This fades out the opacity of all text and images in a transform and it's children
        /// </summary>
        /// <param name="transform">Subjected transform</param>
        /// <param name="toDark">If it should fade in or out</param>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="duration">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <returns></returns>
        public static IEnumerator Fade(Transform transform, bool toDark, AnimationCurve curve, float duration,
            Action finished = null)
        {
            transform.localScale = Vector3.one;
            
            //gets all the children
            Image[] images = transform.GetComponentsInChildren<Image>();
            Text[] texts = transform.GetComponentsInChildren<Text>();
            
            //enables interaction
            if (!toDark)
            {
                foreach (var image in images)
                {
                    image.enabled = true;
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                }

                foreach (var text in texts)
                {
                    text.enabled = true;
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                }
            }

            float startTime = duration;

            while (duration > 0)
            {
                duration -= Time.deltaTime;

                foreach (var image in images)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b,
                        toDark ? 1 - curve.Evaluate((startTime - duration) / startTime) : curve.Evaluate((startTime - duration) / startTime));
                }

                foreach (var text in texts)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b,
                        toDark ? 1 - curve.Evaluate((startTime - duration) / startTime) : curve.Evaluate((startTime - duration) / startTime));
                }

                yield return null;
            }

            //disables interaction
            if (toDark)
            {
                foreach (var image in images)
                {
                    image.enabled = false;
                }

                foreach (var text in texts)
                {
                    text.enabled = false;
                }
            }

            Finished(finished);
        }
        
        /// <summary>
        /// This is used on a circle sprite to create a radial loading animation
        /// </summary>
        /// <param name="image">The circle sprite</param>
        /// <param name="curve">Animation curve to customize the movement</param>
        /// <param name="duration">How long it should last</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <returns></returns>
        public static IEnumerator RadialLoad(Image image, AnimationCurve curve, float duration,
            Action finished = null)
        {
            float startTime = duration;
            float startScale = 1;
            float scaleDifference = 0 - 1;

            while (duration > 0)
            {
                duration -= Time.deltaTime;

                float strength = curve.Evaluate((startTime - duration) / startTime);
                image.fillAmount = startScale + scaleDifference * strength;
                yield return null;
            }

            image.fillAmount = 0;

            Finished(finished);
        }

        /// <summary>
        /// Old code this should be replaced in every method with action?.Invoke();
        /// </summary>
        /// <param name="action">Action that runs when a co routine is finished</param>
        private static void Finished(Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }
        
        /// <summary>
        /// Just a timer that waits x seconds
        /// </summary>
        /// <param name="duration">How long it should wait</param>
        /// <param name="loop">Action that runs every frame while waiting</param>
        /// <param name="finished">Action that runs when finished</param>
        /// <returns></returns>
        public static IEnumerator Wait(float duration, Action loop = null, Action finished = null)
        {
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                loop?.Invoke();
                yield return null;
            }

            finished?.Invoke();
        }
    }
}