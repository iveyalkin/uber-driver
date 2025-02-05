using System;
using UnityEngine;
using UnityEngine.UI;

namespace iv.arcade.uberdriver.ui
{
    [RequireComponent(typeof(Text))]
    public partial class TimerCounter : MonoBehaviour
    {
        private const string timerFormat = "mm':'ss";

        [SerializeField] private Timer timer;

        private void OnEnable()
        {
            UpdateTimer(timer.RemainingTime);
            timer.onTimeChanged.AddListener(UpdateTimer);
        }

        private void OnDisable()
        {
            timer.onTimeChanged.RemoveListener(UpdateTimer);
        }

        private void UpdateTimer(float time)
        {
            text.text = TimeSpan.FromSeconds(time).ToString(timerFormat);
        }
    }
}