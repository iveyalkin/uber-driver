using UnityEngine;

namespace iv.arcade.uberdriver.ui
{
    public class Windows : MonoBehaviour
    {
        [SerializeField] private Gameplay gameplay;

        [SerializeField] private GameObject container;
        [SerializeField] private GameObject pauseWindow;
        [SerializeField] private GameObject scoreWindow;
        [SerializeField] private GameObject creditsWindow;

        public void StartGame()
        {
            gameplay.StartGame();
        }

        public void ResumeGame()
        {
            gameplay.Resume();
        }

        public void ShowCredits()
        {
            pauseWindow.SetActive(false);
            creditsWindow.SetActive(true);
        }

        public void ShowScore()
        {
            pauseWindow.SetActive(false);
            scoreWindow.SetActive(true);
        }

        public void CloseSubwindow(GameObject window)
        {
            window.SetActive(false);
            pauseWindow.SetActive(true);
        }

        public void QuitGame()
        {
            gameplay.Quit();
        }

        private void OnEnable()
        {
            gameplay.onPauseChanged.AddListener(OnPauseChanged);
            gameplay.onGameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnDisable()
        {
            gameplay.onPauseChanged.RemoveListener(OnPauseChanged);
            gameplay.onGameStateChanged.RemoveListener(OnGameStateChanged);
        }

        private void OnGameStateChanged(bool isStart)
        {
            Debug.Log(isStart ? "Game started" : "Game ended");

            if (!isStart)
                ShowScore();
        }

        private void OnPauseChanged(bool isPaused)
        {
            // todo: use canvas enable/disable + graphics raycaster to disable window without rebuilding canvas
            container.SetActive(isPaused);
        }
    }
}