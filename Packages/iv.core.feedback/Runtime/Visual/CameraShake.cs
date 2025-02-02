using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IV.Core.Feedback.Visual
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float maxAngle = 2f;
        [SerializeField] private float maxOffset = .5f;
        [SerializeField] private float traumaDecay = 1.3f;
        
        [SerializeField] private Feedback feedback;

        private float trauma;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector2 noiseOffset;

        private void Awake()
        {
            feedback.CameraShake = this;
        }

        private void Start()
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
            noiseOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
        }

        private void LateUpdate()
        {
            if (trauma <= 0f) return;

            trauma = Mathf.Max(trauma - traumaDecay * Time.deltaTime, 0);
            var shake = trauma * trauma; // square for more organic falloff

            // generate noise values for this frame
            var time = Time.time * 10f;
            var noise = new Vector3(
                Mathf.PerlinNoise(noiseOffset.x + time, 0f) - 0.5f,
                Mathf.PerlinNoise(noiseOffset.y + time, 0f) - 0.5f,
                Mathf.PerlinNoise(0f, time) - 0.5f
            );

            ApplyShake(noise, shake);
        }

        private void ApplyShake(Vector3 noise, float shake)
        {
            transform.localPosition = originalPosition + noise * (shake * maxOffset);
            transform.localRotation = originalRotation * Quaternion.Euler(noise * (shake * maxAngle));
        }

        public void InduceStress(float stress)
        {
            trauma = Mathf.Clamp01(trauma + stress);
        }

        public void Reset()
        {
            trauma = 0f;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }
}