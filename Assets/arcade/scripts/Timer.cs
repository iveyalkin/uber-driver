using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace iv.arcade.uberdriver
{
    [CreateAssetMenu(menuName = "iv/Arcade/Timer", order = 0)]
    public class Timer : ScriptableObject
    {
        [SerializeField]
        [Min(1)]
        private int roundSeconds = 60;

        private int elapsedSeconds;

        public bool IsPause { get; private set; } = true;

        private CancellationTokenSource resetToken;

        public UnityEvent<float> onTimeChanged;
        public UnityEvent onTimeOver;

        public float RemainingTime => roundSeconds - elapsedSeconds;

        public void Restart()
        {
            Stop();

            resetToken = new CancellationTokenSource();
            IsPause = false;

            StartTimer(resetToken.Token);
        }

        public void Stop()
        {
            DisposeToken();
            Reset();
        }

        public void Pause()
        {
            IsPause = true;
        }

        private async void StartTimer(CancellationToken cancellationToken)
        {
            try
            {
                while (RemainingTime > 0)
                {
                    await Awaitable.WaitForSecondsAsync(1f, cancellationToken);

                    if (!IsPause)
                        Tick();
                }

                onTimeOver.Invoke();
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Timer cancelled");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Tick()
        {
            elapsedSeconds++;

            onTimeChanged.Invoke(RemainingTime);
        }

        private void Reset()
        {
            elapsedSeconds = 0;
        }

        private void OnEnable()
        {
            Reset();
        }

        private void OnDisable()
        {
            DisposeToken();
        }

        private void DisposeToken()
        {
            if (resetToken == null) return;

            resetToken.Cancel();
            resetToken.Dispose();
            resetToken = null;
        }
    }
}