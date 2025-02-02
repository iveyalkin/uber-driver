using UnityEngine;
using IV.Core.Feedback.Visual;

namespace IV.Core.Feedback
{
    [CreateAssetMenu(menuName = "iv/Feedback")]
    public class Feedback : ScriptableObject
    {
        // private CameraShake shaker;

        [Header("Feedback Settings")]
        [SerializeField] private float collisionShakeIntensity = 0.4f;
        [SerializeField] private float explosionShakeIntensity = 0.8f;
        [SerializeField] private float driftShakeIntensity = 0.2f;

        private CameraShake cameraShake;

        public CameraShake CameraShake
        {
            get => cameraShake;
            set => cameraShake = value;
        }

        public void TriggerCollision(float impactVelocity)
        {
            var intensity = Mathf.Clamp01(impactVelocity / 10f) * collisionShakeIntensity;
            CameraShake.InduceStress(intensity);

            Handheld.Vibrate();
        }

        public void TriggerExplosion(float proximity)
        {
            var intensity = Mathf.Clamp01(1f - proximity) * explosionShakeIntensity;
            CameraShake.InduceStress(intensity);

            Handheld.Vibrate();
        }

        public void TriggerDrift(float intensity)
        {
            CameraShake.InduceStress(intensity * driftShakeIntensity);

            // Could add subtle continuous haptics here if platform supports it
        }
    }
}
