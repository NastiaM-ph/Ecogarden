using UnityEngine;

public class CreatureAmbientSound : MonoBehaviour
{

    [Header("Sound")]
    [SerializeField] private AudioClip ambientSound;
    [Header("Timing")]
    [SerializeField] private float minDelay = 3f;
    [SerializeField] private float maxDelay = 8f;

    [Header("Volume")]
    [SerializeField] private float volume = 0.5f;

    private AudioSource audioSource;


    void Start()

    {

        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        audioSource.loop = false;

        audioSource.volume = volume;

        ScheduleNextSound();
    }


    void ScheduleNextSound()

    {

        float delay = Random.Range(minDelay, maxDelay);

        Invoke(nameof(PlayAmbientSound), delay);

    }

    void PlayAmbientSound()
    {

        if (ambientSound != null)
        {

            audioSource.PlayOneShot(ambientSound);
        }


        ScheduleNextSound();
    }
}

 