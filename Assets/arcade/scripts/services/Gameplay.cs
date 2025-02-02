using UnityEngine;
using UnityEngine.Events;

namespace iv.arcade.uberdriver
{
    [CreateAssetMenu(menuName = "iv/Arcade/Gameplay", order = 0)]
    public class Gameplay : ScriptableObject
    {
        [SerializeField] private Timer timer;

        public UnityEvent<bool> onGameStateChanged;
        public UnityEvent<bool> onPauseChanged;

        public bool IsPaused { get; private set; }
        public bool IsStarted { get; private set; }

        private float originalTimeScale;

        public void Pause()
        {
            if (IsPaused) return;

            IsPaused = true;

            originalTimeScale = Time.timeScale;
            // rough but should work for now
            Time.timeScale = 0;

            timer.Pause();

            onPauseChanged.Invoke(true);
        }

        public void Resume()
        {
            if (!IsPaused) return;

            IsPaused = false;

            Time.timeScale = originalTimeScale;
            timer.Unpause();

            onPauseChanged.Invoke(false);
        }

        public void StartGame()
        {
            IsStarted = true;
            Resume();
            timer.Restart();
            onGameStateChanged.Invoke(true);
        }

        public void EndGame()
        {
            IsStarted = false;
            Pause();
            onGameStateChanged.Invoke(false);
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void OnEnable()
        {
            Reset();
            timer.onTimeOver.AddListener(EndGame);
        }

        private void OnDisable()
        {
            timer.onTimeOver.RemoveListener(EndGame);
        }

        private void Reset()
        {
            IsStarted = false;
            Time.timeScale = 1f;
            Pause();
        }
    }
}