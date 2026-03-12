using UnityEngine;
using UnityUtils;

public class GlobalAudio : Singleton<GlobalAudio>
{
    [SerializeField] private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

    protected override void Awake()
    {
        audioSource = gameObject.GetOrAdd<AudioSource>();
        audioSource.playOnAwake = false; 
    }
    
    
    
    public void PlayOneShot(AudioClip clip, float volume = 1)
    {
        AudioSource.PlayOneShot(clip, volume);
    }
}