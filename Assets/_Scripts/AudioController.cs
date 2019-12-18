using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioSource audioSource;
    public AudioClip buttonPress,token,keyboard,dragSuccess,dragFail,keyboardError,customerComplete,levelComplete,receiptComplete;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    public void PlayButtenPressSound()
    {
        audioSource.PlayOneShot(buttonPress);
    }

    public void PlayPanelInSound()
    {
        audioSource.PlayOneShot(buttonPress);
    }

    public void PlayPanelOutSount()
    {
        audioSource.PlayOneShot(buttonPress);
    }

    public void PlayTokenActSound()
    {
        audioSource.PlayOneShot(token);
    }

    public void PlayDragSuccessSound()
    {
        audioSource.PlayOneShot(dragSuccess);
    }

    public void PlayDragFailSound()
    {
        audioSource.PlayOneShot(dragFail);
    }

    public void PlayKeybordPressSound()
    {
        audioSource.PlayOneShot(keyboard);
    }

    public void PlayCashArrangementSound()
    {
        audioSource.PlayOneShot(buttonPress);
    }

    public void PlayCustomerCompleteSound()
    {
        audioSource.PlayOneShot(customerComplete);
    }

    public void PlayLevelCompleteSound()
    {
        audioSource.PlayOneShot(levelComplete);
    }

    public void PlayReceiptCompleteSound()
    {
        audioSource.PlayOneShot(receiptComplete);
    }
    public void PlayWrongKeyboardPressSound()
    {
        audioSource.PlayOneShot(keyboardError);
    }
}

