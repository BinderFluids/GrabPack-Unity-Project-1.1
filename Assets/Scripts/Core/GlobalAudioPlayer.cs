using UnityEngine;

public class GlobalAudioPlayer : MonoBehaviour
{
    public void PlaySound(AudioClip clip, float volume)
    {
        GlobalAudio.Instance.PlayOneShot(clip, volume);
    }
    public void PlaySound(AudioClip clip)
    {
        PlaySound(clip, 1);
    }
}