using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public string targetVolume;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AudioFade.FadeIn(targetVolume, 1.5f, Mathf.SmoothStep));
            StartCoroutine(AudioFade.FadeOut("MainVolume", 1.5f, Mathf.SmoothStep));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AudioFade.FadeIn("MainVolume", 1.5f, Mathf.SmoothStep));
            StartCoroutine(AudioFade.FadeOut(targetVolume, 1.5f, Mathf.SmoothStep));
        }
    }
}
