using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUtils : MonoBehaviour
{
    public ParticleSystem[] particleSystems = new ParticleSystem[0];
    public AudioSource[] audioSources = new AudioSource[0];
    public Sprite[] sprites = new Sprite[0];

    public void playParticleSystem(int id)
    {
        try
        {
            particleSystems[id].Play();
        } catch { Debug.LogError("Particle System with id: " + id + "isn't set."); }
    }

    public void playAudio(int id)
    {
        try
        {
            audioSources[id].PlayOneShot(audioSources[id].clip);
        }
        catch { Debug.LogError("AudioSource with id: " + id + "isn't set."); }
    }

    public void SwapSprite(int id)
    {
        try
        {
            GetComponent<SpriteRenderer>().sprite = sprites[id];
        }
        catch { Debug.LogError("Sprite with id: " + id + "isn't set."); }
    }
}
