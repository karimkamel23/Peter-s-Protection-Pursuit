using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        instance = this;
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        SetVolumeOnStart();
    }

    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _newVolume)
    {
        ChangeSourceVolume(1, "soundVolume", _newVolume, soundSource);

    }

    public void ChangeMusicVolume(float _newVolume)
    {
        ChangeSourceVolume(0.15f, "musicVolume", _newVolume, musicSource);

    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float newVolume, AudioSource source)
    {
        source.volume = newVolume * baseVolume;
        PlayerPrefs.SetFloat(volumeName, newVolume);
    }

    public float GetCurrentSoundVolume()
    {
        return PlayerPrefs.GetFloat("soundVolume", 1);
    }

    public float GetCurrentMusicVolume()
    {
        return PlayerPrefs.GetFloat("musicVolume", 1);
    }

    private void SetVolumeOnStart()
    {
        soundSource.volume = GetCurrentSoundVolume() * 1;
        musicSource.volume = GetCurrentMusicVolume() * 0.15f;
    }
}
