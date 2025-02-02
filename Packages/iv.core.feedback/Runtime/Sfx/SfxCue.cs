using UnityEngine;

namespace IV.Core.Feedback.Sfx
{
    [CreateAssetMenu(menuName = "iv/Sfx/Cue", order = 0)]
    public class SfxCue : ScriptableObject
    {
        public AudioClip clip;
    }
}