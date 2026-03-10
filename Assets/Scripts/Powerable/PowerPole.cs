using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPole : PowerableBehaviour
{
    public GameObject glow;
    public float glowcounter = 0.1f;
    public ElectricalSource source;

    public AudioSource GlobalAudio;
    public AudioClip connect;
    public AudioClip disconnect;

    public void StartGlow()
    {
        if (source.powering)
        {
            glow.SetActive(true);
            glowcounter = 0.1f;
        }
    }

    protected override void OnPoweredOn()
    {
        StartGlow();
        GlobalAudio.PlayOneShot(connect, 0.7f);
    }

    protected override void OnPoweredOff()
    {
        GlobalAudio.PlayOneShot(disconnect, 0.7f);
    }

    void Update()
    {
        if (!IsPowered)
            glowcounter -= Time.deltaTime;

        if (glowcounter <= 0)
        {
            glow.SetActive(false);
            PowerOff(); 
        }
    }
}

