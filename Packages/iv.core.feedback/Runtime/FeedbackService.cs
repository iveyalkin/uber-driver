using IV.Core.Feedback.Sfx;
using IV.Core.Feedback.Visual;
using UnityEngine;

namespace IV.Core.Feedback
{
    [CreateAssetMenu(menuName = "iv/Feedback")]
    public class Feedback : ScriptableObject
    {
        [SerializeField] private float collisionShakeIntensity = 0.4f;
        [SerializeField] private float explosionShakeIntensity = 0.8f;
        [SerializeField] private float driftShakeIntensity = 0.2f;

        private CameraShake cameraShake;
        private SfxPool sfxPool;

        [SerializeField] private SfxCue collisionCue;
        [SerializeField] private SfxCue pickupCue;

        public CameraShake CameraShake
        {
            get => cameraShake;
            set => cameraShake = value;
        }
        
        public SfxPool SfxPool
        {
            get => sfxPool;
            set => sfxPool = value;
        }

        public void TriggerCollision(float impactVelocity)
        {
            var intensity = Mathf.Clamp01(impactVelocity / 10f) * collisionShakeIntensity;
            CameraShake.InduceStress(intensity);

            SfxPool.Play(collisionCue);

            //Handheld.Vibrate();
        }

        public void TriggerExplosion(float proximity)
        {
            var intensity = Mathf.Clamp01(1f - proximity) * explosionShakeIntensity;
            CameraShake.InduceStress(intensity);

            //Handheld.Vibrate();
        }

        public void TriggerDrift(float intensity)
        {
            CameraShake.InduceStress(intensity * driftShakeIntensity);

            // Could add subtle continuous haptics here if platform supports it
        }
        
        public void TriggerPickup()
        {
            SfxPool.Play(pickupCue);

            //Handheld.Vibrate();
        }
    }
}
