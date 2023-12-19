using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bridsChirping;
    [SerializeField] private AudioClip failedSound;
    [SerializeField] private int audio_;
   
    public void PlayBirdsChirpingSound()
    {
        audio_ = PlayerPrefs.GetInt("Audio", 1);
        if (audio_==1)
        {
        audioSource.Stop();
        audioSource.clip = bridsChirping;
        Invoke("PlayAudioSource", 0.2f);
        }
    }
    private void PlayAudioSource()
    {
        audioSource.Play();
    }
    public void PlayFailedSound()
    {
        audio_ = PlayerPrefs.GetInt("Audio", 1);
        if (audio_ == 1)
        {
            audioSource.Stop();
            audioSource.clip = failedSound;
            Invoke("PlayAudioSource", 0.2f);
        }
    }
}
