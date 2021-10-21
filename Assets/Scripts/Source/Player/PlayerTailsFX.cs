using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTailsFX : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> tails;

    public void SetTailsLength(float length = 2f, float speedMultiplier = 1f)
    {
        foreach (var tail in tails)
        {
            var module = tail.velocityOverLifetime;
            module.x = -Mathf.Abs(length);

            var main = tail.main;
            main.simulationSpeed = speedMultiplier;
        }
    }
}
