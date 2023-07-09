using System.Collections;
using UnityEngine;

public class SoundManager : PersistentSingleton<SoundManager>
{
    public AudioSource ThudSource;
    public AudioSource ClickSource;
    public AudioSource RoarSource;
    public AudioSource RubbleSource;
    public AudioSource ScoreSource;
    public float BaseRoarPitch = 1f;
    private IEnumerator DestroySourceAfterTime(AudioSource audioSource)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(audioSource);
    }
    public void PlayThud()
    {
        AudioSource ad = gameObject.AddComponent<AudioSource>();
        ad.clip = ThudSource.clip;
        ad.Play();
        StartCoroutine(DestroySourceAfterTime(ad));
    }

    public void PlayRoar()
    {
        RoarSource.pitch = 1.1f*BaseRoarPitch - Random.value*BaseRoarPitch / 5f;
        RoarSource.Play();
    }

    public void PlayRubble()
    {
        RubbleSource.pitch = 1.1f - Random.value / 5f;
        RubbleSource.Play();
    }

    public void PlayClick()
    {
        ClickSource.Play();
    }

    public void PlayScore()
    {
        ScoreSource.Play();
    }
}
