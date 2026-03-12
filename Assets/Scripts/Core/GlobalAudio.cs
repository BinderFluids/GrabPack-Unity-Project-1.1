using UnityEngine;
using UnityUtils;

public class GlobalAudio : PersistentSingleton<GlobalAudio>
{
    [SerializeField] private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = gameObject.GetOrAdd<AudioSource>();
        audioSource.playOnAwake = false; 
    }
    
    
    
    public void PlayOneShot(AudioClip clip, float volume = 1)
    {
        AudioSource.PlayOneShot(clip, volume);
    }
}