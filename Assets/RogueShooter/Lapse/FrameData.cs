using System.Collections.Generic;
using UnityEngine;

namespace NeonEscape
{
    [System.Serializable]
    public class FrameData
    {
        public float recordedTime; // just Time.time (time since start of app)

        public Vector3 position;
        public Quaternion rotation;
        public bool isActive = true;

        // animation played in this frame
        public int animationStateHash;
        public float animationNormalizedTime;

        public List<ICommand> actions = new List<ICommand>();

        public FrameData(Vector3 pos, Quaternion rot, int animHash, float animTime, bool active)
        {
            recordedTime = Time.fixedTime;
            position = pos;
            rotation = rot;
            animationStateHash = animHash;
            animationNormalizedTime = animTime;
            isActive = active;
            actions = new List<ICommand>(); // Start with an empty list
        }
        public FrameData(bool active)
        {
            recordedTime = Time.time;
            isActive = active;
        }
    }
}
