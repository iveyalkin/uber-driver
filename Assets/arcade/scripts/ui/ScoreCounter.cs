using System;
using UnityEngine;
using UnityEngine.UI;

namespace iv.arcade.uberdriver.ui
{
    [RequireComponent(typeof(Text))]
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private Score score;
        [SerializeField] private Text text;

        private void OnEnable()
        {
            UpdateScore(score.CurrentScore);
            score.onScoreChanged.AddListener(UpdateScore);
        }

        private void OnDisable()
        {
            score.onScoreChanged.RemoveListener(UpdateScore);
        }

        private void UpdateScore(int newScore)
        {
            text.text = newScore.ToString();
        }
    }
}