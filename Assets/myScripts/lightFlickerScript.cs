using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lightFlickerScript : MonoBehaviour
{
    private Light light;
    public float minIntensity = .5f;
    public float maxIntensity = 5f;
    public float flickerSpeed = 0.1f;

    private void Start()
    {
        light = GetComponent<Light>();

        InvokeRepeating("Flicker", 0f, flickerSpeed);
    }

    private void Flicker()
    {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        light.intensity = randomIntensity;
    }
}
