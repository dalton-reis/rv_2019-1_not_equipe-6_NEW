
using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

[Serializable]
public struct ThunderEffect
{
    public AudioClip AudioClip;
    public float StartTime;
}

[RequireComponent(typeof(AudioSource))]
public class Lightning : MonoBehaviour
{
    public ParticleSystem Particle;
    public List<ThunderEffect> LightningAudios;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Particle.Play();

        float secondsToDestroy = Particle.main.duration;

        if (LightningAudios.Count > 0)
        {
            var thunderEffect = LightningAudios[Random.Range(0, LightningAudios.Count)];
            audioSource.clip = thunderEffect.AudioClip;
            audioSource.time = thunderEffect.StartTime;
            audioSource.Play();

            // Adjust the destroy time if needed
            float clipLengthToPlay = thunderEffect.AudioClip.length - thunderEffect.StartTime;
            if (clipLengthToPlay > secondsToDestroy)
                secondsToDestroy = clipLengthToPlay;
        }
        
        Destroy(gameObject, secondsToDestroy);
    }
}
