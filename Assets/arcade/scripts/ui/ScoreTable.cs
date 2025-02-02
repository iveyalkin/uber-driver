using UnityEngine;
using UnityEngine.UI;

namespace iv.arcade.uberdriver.ui
{
    public class ScoreTable : MonoBehaviour
    {
        [SerializeField] private Score score;

        [SerializeField] private Text scoreText;

        private void OnEnable()
        {
            scoreText.text = $"Score: {score.CurrentScore}";
        }
    }
}