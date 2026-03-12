using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPole : PowerableBehaviour
{
    public GameObject glow;
    public float glowcounter = 0.1f;
    public ElectricalSource source;
    
    public AudioClip connect;
    public AudioClip disconnect;

    public void StartGlow()
    {
        glow.SetActive(true);
        glowcounter = 0.1f;
    }

    protected override void OnPoweredOn()
    {
        Debug.Log("Powered On");
        StartGlow();
        GlobalAudio.Instance.PlayOneShot(connect, 0.7f);
    }

    protected override void OnPoweredOff()
    {
        GlobalAudio.Instance.PlayOneShot(disconnect, 0.7f);
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

