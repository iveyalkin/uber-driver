using System;
using UnityEngine;
using UnityEngine.Audio;

namespace IV.Core.Feedback.Sfx
{
    public class SfxPool : MonoBehaviour
    {
        private AudioSource sharedSource;

        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField] private Feedback feedback;

        public void Play(SfxCue cue)
        {
            sharedSource.PlayOneShot(cue.clip);
        }

        private void Awake()
        {
            feedback.SfxPool = this;
        }

        private void OnEnable()
        {
            if (sharedSource == null)
            {
                sharedSource = new GameObject("SfxSource").AddComponent<AudioSource>();
                sharedSource.outputAudioMixerGroup = mixerGroup;
                DontDestroyOnLoad(sharedSource.gameObject);
            }
            else
            {
                Debug.LogWarning("SfxPool: sharedSource is NOT null");
            }
        }

        private void OnDisable()
        {
            if (sharedSource != null)
            {
                Destroy(sharedSource.gameObject);
            }
            else
            {
                Debug.LogWarning("SfxPool: sharedSource IS null");
            }
        }
    }
}