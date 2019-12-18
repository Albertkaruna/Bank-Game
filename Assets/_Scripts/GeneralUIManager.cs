using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUIManager : MonoBehaviour {

    public GameObject exitPanel;
    public Sprite bgOn, bgOff;
    private bool bgToggle=true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            exitPanel.SetActive(true);
        }
	}

    public void Yes()
    {
        Application.Quit();
    }

    public void No()
    {
        exitPanel.SetActive(false);
    }

    public void Restart()
    {
        AudioController.instance.PlayButtenPressSound();
        PreGameUIManager.instance.LoadLevel(PreGameUIManager.selectedLevel);
        CharacterManager.instance.levelCompletePanel.gameObject.SetActive(false);
        // Restart the same level
    }

    public void Home()
    {
        AudioController.instance.PlayButtenPressSound();
        PreGameUIManager.instance.startingBG.SetActive(true);
        CharacterManager.instance.levelCompletePanel.gameObject.SetActive(false);
        RatingSystem.instance.UnlockAndRating();
    }

    public void Next()
    {
        AudioController.instance.PlayButtenPressSound();
        PreGameUIManager.instance.levelSelectionPanel.SetActive(true);
        CharacterManager.instance.levelCompletePanel.gameObject.SetActive(false);
        RatingSystem.instance.UnlockAndRating();
    }

    public void BGSoundBtn(Button bgSoundBtn)
    {
        bgToggle = !bgToggle;
        AudioController.instance.audioSource.enabled = bgToggle;
        if (bgToggle)
        {
            Camera.main.GetComponent<AudioSource>().Play();
            bgSoundBtn.GetComponent<Image>().sprite = bgOn;
        }
        else
        {
            Camera.main.GetComponent<AudioSource>().Pause();
            bgSoundBtn.GetComponent<Image>().sprite = bgOff;
        }
    }

    //public void SoundEffectBtn(Button soundEffectBtn)
    //{
    //    effectToggle = !effectToggle;
    //    AudioController.instance.audioSource.enabled = effectToggle;
    //    if (effectToggle)
    //    {
    //        soundEffectBtn.GetComponent<Image>().sprite = effectOn;
    //    }
    //    else
    //    {
    //        soundEffectBtn.GetComponent<Image>().sprite = effectOff;
    //    }
    //}
}
