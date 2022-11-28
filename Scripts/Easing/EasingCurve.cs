using UnityEngine;

namespace ToolKit.Easing
{
    /// <summary>
    /// Premade curves so you don't need to make a serializeable field for in the inspector
    /// </summary>
    public class EasingCurve : AnimationCurve
    {
        /// <summary>
        /// Linear line from (0,0) to (1,1)
        /// </summary>
        /// <returns>Animation curve</returns>
        public static AnimationCurve Linear()
        {
            AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

            return curve;
        }
        
        /// <summary>
        /// rounded line that has a slight curve at the start and end for a smooth transition
        /// </summary>
        /// <returns>Animation curve</returns>
        public static AnimationCurve Smooth()
        {
            AnimationCurve curve = EaseInOut(0, 0, 1, 1);

            return curve;
        }
        
        /// <summary>
        /// rounded line that has a slow start and a smooth end
        /// </summary>
        /// <returns>Animation curve</returns>
        public static AnimationCurve LowSmooth(float end = 0.5f)
        {
            AnimationCurve curve = EaseInOut(0, 0, 1, end);

            return curve;
        }
        
        /// <summary>
        /// rounded line that has a slow start and bounces a few times out
        /// </summary>
        /// <returns>Animation curve</returns>
        public static AnimationCurve BounceOut()
        {
            AnimationCurve curve = EaseInOut(0, 0, 1, 1);
            curve.AddKey(0.25f, 0.9f);
            curve.AddKey(0.60f, 0.75f);

            return curve;
        }
        
        /// <summary>
        /// rounded line that has a smooth start and linear out
        /// </summary>
        /// <returns>Animation curve</returns>
        public static AnimationCurve SlowToSteep()
        {
            AnimationCurve curve = Linear(0, 0, 1, 1);
            curve.AddKey(0.75f, 0.2f);

            
            Keyframe[] keys = curve.keys;

            Keyframe key = curve.keys[1];
            key.inTangent = 0;
            key.outTangent = 0;
            keys[1] = key;
            
            curve.keys = keys;


            return curve;
        }
    }
}