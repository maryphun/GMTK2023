using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMapping : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float bgmSound = 1.0f;
    [SerializeField] private float seSound = 1.0f;

    [Header("TrackManager")] 
    [SerializeField] private TrackManager trackMng;

    private void Start()
    {
        AudioManager.Instance.SetMusicVolume(bgmSound);
        AudioManager.Instance.SetSEMasterVolume(seSound);
        StartCoroutine(GamePlay());
    }

    IEnumerator GamePlay()
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySFX("starting");
        yield return new WaitForSeconds(2.0f);
        AudioManager.Instance.PlaySFX("3");
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySFX("2");
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySFX("1");
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySFX("start");
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlayMusicWithFade("8bit-act10_stage05", 4.0f);

        trackMng.StartTrack();
    }
}
