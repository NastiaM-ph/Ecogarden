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

   /* public void PlayClick()
    {
        if (sfxSource != null && clickSound != null)

        {
            sfxSource.PlayOneShot(clickSound);
        }
    }*/
    public void PlayClick()
{
    if (clickAudioSource != null && clickSound != null)
    {
        clickAudioSource.clip = clickSound;
        clickAudioSource.loop = true;
        clickAudioSource.Play();
    }
}
public void StopClick()
{
    if (clickAudioSource != null)
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
        if (sfxSource != null && clip != null)

        {
            sfxSource.PlayOneShot(clip);
        }
    }
      public void ToggleMute()

    {

        isMuted = !isMuted;

        AudioListener.volume = isMuted ? 0f : 1f;

    }
    public bool IsMuted()

    {

        return isMuted;
    }

}
 


 