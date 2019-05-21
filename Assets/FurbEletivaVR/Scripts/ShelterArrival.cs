
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using Random = UnityEngine.Random;
using UnityEngine.UI;

public class ShelterArrival : MonoBehaviour
{
    public TMP_Text CongratulationsBoardText;
    public List<ParticleSystem> ParticlesToPlay;
    public List<SimplePersonAnimator> peopleToAnimate;
    public List<Graphic> graphicsToEnableRaycast;

    public float MinTrigger = 0.5f;
    public float MaxTrigger = 0.8f;

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
        CongratulationsBoardText.text = "<color=#e03d1f><b>Parabéns!</b></color>\n" +
            $"Você salvou {ok} das {ok + gameManager.Fail} pessoas!";

        foreach (var particle in ParticlesToPlay)
            particle.Play(true);
    }

    private void PeopleWave()
    {
        foreach (var person in peopleToAnimate)
            StartCoroutine(person.Play(Random.Range(MinTrigger, MaxTrigger)));
    }
}
