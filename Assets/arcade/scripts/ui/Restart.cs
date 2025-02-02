using UnityEngine;

namespace iv.arcade.uberdriver.ui
{
    public class Restart : MonoBehaviour
    {
        [SerializeField] private Timer timer;

        public void RestartGame()
        {
            timer.Restart();
        }
    }
}