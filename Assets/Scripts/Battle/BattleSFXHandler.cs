using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSFXHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip confirmSFX;
    [SerializeField]
    private AudioClip backSFX;

    public void PlayConfirmSFX()
    {
        sfxSource.PlayOneShot(confirmSFX);
    }

    public void PlayBackSFX()
    {
        sfxSource.PlayOneShot(backSFX);
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }
}
