using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField]
    private AudioSource bgmSource;
    [SerializeField]
    private AudioClip[] bgmClips;
    private int currentBgmIndex;
    private Coroutine bgmCheckCoroutine;

    [Header("UI SFX")]
    [SerializeField]
    private AudioSource uiSfxSource;

    private void Awake()
    {
        if (bgmSource != null)
        {
            bgmSource.playOnAwake = false;
        }

        if (uiSfxSource != null)
        {
            uiSfxSource.playOnAwake = false;
        }
    }

    private void OnEnable()
    {
        GameServices.Register(this);
        bgmCheckCoroutine = StartCoroutine(CoCheckBGMRoutine());
        GameEvents.OnReturnMainScene += PlayBGMIfNeeded;
    }

    private void OnDisable()
    {
        GameServices.Unregister(this);

        if (bgmCheckCoroutine != null)
        {
            StopCoroutine(bgmCheckCoroutine);
            bgmCheckCoroutine = null;
        }

        GameEvents.OnReturnMainScene -= PlayBGMIfNeeded;
    }

    private IEnumerator CoCheckBGMRoutine()
    {
        while (true)
        {
            PlayBGMIfNeeded();
            yield return new WaitForSeconds(2f);
        }
    }

    private void PlayBGMIfNeeded()
    {
        if (bgmClips.Length <= 0)
        {
            Debug.Log("Assign any BGM!!!");
            return;
        }

        if (!bgmSource.isPlaying)
        {
            PlayRandomBGM();
        }
    }

    [ContextMenu("Play Random BGM")]
    public void PlayRandomBGM()
    {
        currentBgmIndex = Random.Range(0, bgmClips.Length);
        PlayBGM(currentBgmIndex);
    }

    private void PlayBGM(int bgmToPlay)
    {
        if (bgmClips.Length <= 0)
        {
            Debug.Log("Assign any BGM!!!");
            return;
        }

        bgmSource.Stop();

        currentBgmIndex = bgmToPlay;
        bgmSource.clip = bgmClips[bgmToPlay];
        bgmSource.Play();
    }

    public void PlayUISFX(AudioClip clip)
    {
        if (clip == null || uiSfxSource == null)
        {
            return;
        }

        uiSfxSource.PlayOneShot(clip);
    }
}
