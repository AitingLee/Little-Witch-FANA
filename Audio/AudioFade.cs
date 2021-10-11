using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Audio;
public class AudioFade
{
    public static IEnumerator FadeOut(string soundVolume, float fadingTime, Func<float, float, float, float> Interpolate)
    {
        AudioMixer audioMixer = AudioManager.instance.musicMixer;
        float startVolume = 0;
        audioMixer.GetFloat($"{soundVolume}", out startVolume);
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            audioMixer.SetFloat($"{soundVolume}", Interpolate(startVolume, -80, t));
            yield return null;
        }

        audioMixer.SetFloat($"{soundVolume}", -80);
    }
    public static IEnumerator FadeIn(string soundVolume, float fadingTime, Func<float, float, float, float> Interpolate)
    {

        AudioMixer audioMixer = AudioManager.instance.musicMixer;
        audioMixer.SetFloat($"{soundVolume}", -80);
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            audioMixer.SetFloat($"{soundVolume}", Interpolate(-80, 0, t));
            yield return null;
        }


        audioMixer.SetFloat($"{soundVolume}", 0);
    }
}
