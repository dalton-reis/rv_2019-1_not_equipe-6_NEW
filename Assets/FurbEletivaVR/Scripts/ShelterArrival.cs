
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Collections;

public class ShelterArrival : MonoBehaviour
{
    public TMP_Text CongratulationsBoardText;
    public List<ParticleSystem> ParticlesToPlay;
    public List<SimplePersonAnimator> peopleToAnimate;
    public List<Graphic> graphicsToEnableRaycast;

    public float MinTrigger = 0.5f;
    public float MaxTrigger = 0.8f;

    public float MinSecondsDelayFireworks = 1.0f;
    public float MaxSecondsDelayFireworks = 1.5f;

    public AudioClip FireworksLaunchAudio;
    public AudioClip FireworksExplodeAudio;

    private bool trigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (trigger == true)
            return;

        trigger = true;

        PeopleWave();
        Congratulations();

        // Enable the raycast on the graphics
        foreach (var graphic in graphicsToEnableRaycast)
            graphic.raycastTarget = true;
    }

    private void Congratulations()
    {
        var gameManager = GameManager.Instance;
        var ok = gameManager.Ok;
        float total = gameManager.Total;

        float okRatio = ok / total;

        if (okRatio < 0.5f)
        {
            // Saved a few people
            CongratulationsBoardText.text = "<color=#e03d1f><b>Poxa</b></color>\n" +
                $"Você só conseguiu salvar {ok} das {total} pessoas!";
        }
        else if (okRatio < 0.9f)
        {
            // Saved almost everyone
            CongratulationsBoardText.text = "<color=#e03d1f><b>Muito bom</b></color>\n" +
                $"Você conseguiu salvar {ok} das {total} pessoas!";
        }
        else if (okRatio < 1.1f)
        {
            // Saved everyone
            CongratulationsBoardText.text = "<color=#e03d1f><b>Parabéns!</b></color>\n" +
                $"Você salvou todas as {total} pessoas!";
        }

        StartCoroutine(StartEndlessFireworks());
    }

    private void PeopleWave()
    {
        foreach (var person in peopleToAnimate)
            StartCoroutine(person.Play(Random.Range(MinTrigger, MaxTrigger)));
    }

    private IEnumerator StartEndlessFireworks()
    {
        var fireworksCount = ParticlesToPlay.Count;

        if (fireworksCount == 0)
            yield break;

        while (true)
        {
            var fireworkToPlay = ParticlesToPlay[Random.Range(0, fireworksCount)];
            var fireworkToPlayAudioSource = fireworkToPlay.gameObject.GetComponent<AudioSource>();

            fireworkToPlay.Play();

            // Play Launch
            fireworkToPlayAudioSource.PlayOneShot(FireworksLaunchAudio);
            // Wait
            yield return new WaitForSeconds(FireworksLaunchAudio.length);
            // Play Explosion
            fireworkToPlayAudioSource.PlayOneShot(FireworksExplodeAudio);

            var fireworksSecondsDelay = Random.Range(MinSecondsDelayFireworks, MaxSecondsDelayFireworks);

            // Wait before playing again
            yield return new WaitForSeconds(FireworksLaunchAudio.length + fireworksSecondsDelay);
        }
    }
}
