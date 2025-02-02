using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace iv.arcade.uberdriver
{
    [CreateAssetMenu(menuName = "iv/Arcade/Score", order = 0)]
    public class Score : ScriptableObject
    {
        public int CurrentScore { get; private set; }

        public UnityEvent<int> onScoreChanged;

        private void OnEnable() => ResetScore();

        public void AddCurrentScore(int value)
        {
            CurrentScore += Mathf.Abs(value);
            onScoreChanged.Invoke(CurrentScore);
        }

        public void ResetScore()
        {
            CurrentScore = 0;
            onScoreChanged.Invoke(CurrentScore);
        }

        public void DeductScore(int value)
        {
            CurrentScore = Mathf.Max(0, CurrentScore - Mathf.Abs(value));
            onScoreChanged.Invoke(CurrentScore);
        }
    }
}