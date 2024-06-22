using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private List<AudioClip> sounds;


    public void PlaySound()
    {
        source.clip = sounds[Random.Range(0, sounds.Count)];
        source.Play();
    }

    public void PitchShift(float shift)
    {
        source.pitch = shift;
    }
}
