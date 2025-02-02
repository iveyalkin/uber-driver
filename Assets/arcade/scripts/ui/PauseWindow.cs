using UnityEngine;

namespace iv.arcade.uberdriver.ui
{
    public class PauseWindow : MonoBehaviour
    {
        [SerializeField] private Gameplay gameplay;

        [SerializeField] private GameObject resumeButton;

        private void OnEnable()
        {
            resumeButton.SetActive(gameplay.IsStarted);
        }
    }
}