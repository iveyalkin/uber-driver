using System;
using UnityEngine;
using UnityEngine.Audio;

namespace iv.arcade.uberdriver
{
    [CreateAssetMenu(menuName = "iv/Arcade/Audio", order = 0)]
    public class Audio : ScriptableObject
    {
        private readonly int muteDb = -80;

        [SerializeField] private AudioMixer audioMixer;

        [SerializeField] private MixerParameters mixerParameters = new()
        {
            gameplayVolumeParameter = "gameplay-volume",
        };

        public void UnmuteGameplay()
        {
            audioMixer.ClearFloat(mixerParameters.gameplayVolumeParameter);
        }

        public void MuteGameplay()
        {
            audioMixer.SetFloat(mixerParameters.gameplayVolumeParameter, muteDb);
        }

        [Serializable]
        private struct MixerParameters
        {
            public string gameplayVolumeParameter;
        }
    }
}