using UnityEngine;
public class SoundManager : MonoBehaviour

{
    public static SoundManager Instance { get; private set; }


    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip creatureSound;
    [SerializeField] private AudioSource clickAudioSource;

    private bool isMuted = false;


    void Awake()

    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

    }

    void Start()

    {
        PlayMusic();

    }

    public void PlayMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)

        {
            musicSource.Play();
        }
    }

    public void PlayClick()
    {
        if (clickAudioSource != null && clickSound != null)
        {
            clickAudioSource.PlayOneShot(clickSound);
        }
        else if (sfxSource != null && clickSound != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
    }

    public void StopClick()
    {
        if (clickAudioSource != null && clickAudioSource.isPlaying)
        {
            clickAudioSource.Stop();
        }
    }

    public void PlayCreatureSound()
    {
        if (sfxSource != null && creatureSound != null)
        {
            sfxSource.PlayOneShot(creatureSound);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        if (sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else if (clickAudioSource != null)
        {
            clickAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("[SoundManager] Cannot play SFX: sfxSource is not assigned!");
        }
    }
    private bool isMusicMuted = false;
    private bool isSFXMuted = false;

    public void ToggleMusicMute()
    {
        isMusicMuted = !isMusicMuted;
        if (musicSource != null)
        {
            musicSource.mute = isMusicMuted;
        }
        Debug.Log($"[SoundManager] Music Muted: {isMusicMuted}");
    }

    public void SetMusicMuted(bool mute)
    {
        isMusicMuted = mute;
        if (musicSource != null)
        {
            musicSource.mute = isMusicMuted;
        }
    }

    public bool IsMusicMuted() => isMusicMuted;

    public void ToggleSFXMute()
    {
        isSFXMuted = !isSFXMuted;
        if (sfxSource != null) sfxSource.mute = isSFXMuted;
        if (clickAudioSource != null) clickAudioSource.mute = isSFXMuted;
        Debug.Log($"[SoundManager] SFX Muted: {isSFXMuted}");
    }

    public void SetSFXMuted(bool mute)
    {
        isSFXMuted = mute;
        if (sfxSource != null) sfxSource.mute = isSFXMuted;
        if (clickAudioSource != null) clickAudioSource.mute = isSFXMuted;
    }

    public bool IsSFXMuted() => isSFXMuted;

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null) musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null) sfxSource.volume = Mathf.Clamp01(volume);
        if (clickAudioSource != null) clickAudioSource.volume = Mathf.Clamp01(volume);
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

    public bool IsMuted() => isMuted;
}
 


 