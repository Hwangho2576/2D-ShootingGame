using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioFire;
    public AudioSource audioExplosion;
    public AudioSource audioBGM;
    public AudioSource audioItem;
    public AudioSource audioBoom;
    public AudioSource audioHit;


    public void PlayAudio(string type)
    {
        switch(type)
        {
            case "Fire":
                audioFire.Play();
                break;
            case "Explosion":
                audioExplosion.Play();
                break;
            case "Item":
                audioItem.Play();
                break;
            case "Boom":
                audioBoom.Play();
                break;

            case "Hit":
                audioHit.Play();
                break;

        }
    }

    public void StopAudio(string type)
    {
        switch (type)
        {
            case "Fire":
                audioFire.Stop();
                break;
            case "Explosion":
                audioExplosion.Stop();
                break;
            case "Item":
                audioItem.Stop();
                break;
            case "Boom":
                audioBoom.Stop();
                break;
            case "Hit":
                audioHit.Stop();
                break;

        }
    }
}
