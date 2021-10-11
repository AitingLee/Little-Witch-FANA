using UnityEngine.Audio;
using System;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public AudioSource teleportSound, deadSound, shopSound, equippedSound, buttonSound, portalSound, reviveSound, iceSkill;
    public AudioSource normalAttack, novaLight, novaStrong, projectile1, projectile2, acid, lightingExplosion, projectile3, lightingStrike, slice, chickAttack, frostBlast, frostImpact, stump, flap, headHit, tail, bite, fireSpray;


    private static AudioManager _instance;
    public static AudioManager instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            StartCoroutine(AudioFade.FadeIn("MenuVolume", 1.5f, Mathf.SmoothStep));
            DontDestroyOnLoad(gameObject);
        }
    }

    public AudioMixer mainMixer, musicMixer;
    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSoundEffectVolume(float volume)
    {
        mainMixer.SetFloat("SoundEffectVolume", volume);
    }
}
