using UnityEngine;
using UnityEngine.UI;


class RatingSystem : MonoBehaviour
{
    public static RatingSystem instance;

    public Transform levelsContainer;
    public GameObject starEffect, cashEffect,newStarEffect;
    public static int totalCoins=0;

    private int seconds;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UnlockAndRating();
    }

    public void UnlockAndRating()
    {
        for (int i = 1; i < 13; i++)
        {
            int _rating = GetRating(i.ToString());
            ApplyRatingToAll(_rating, i);
            UnlockLevels(i.ToString(), _rating);
        }
    }

    void ApplyRatingToAll(int rating,int index)
    {
        Transform stars=levelsContainer.GetChild(index - 1).Find("Starts");
        if (rating == 1)
        {
            stars.GetChild(0).gameObject.SetActive(true);
        }
        else if (rating == 2)
        {
            stars.GetChild(0).gameObject.SetActive(true);
            stars.GetChild(1).gameObject.SetActive(true);
        }
        else if (rating == 3)
        {
            stars.GetChild(0).gameObject.SetActive(true);
            stars.GetChild(1).gameObject.SetActive(true);
            stars.GetChild(2).gameObject.SetActive(true);
        }
    }

    void UnlockLevels(string levelNo, int rating)
    {

        if (rating > 0)
        {
            print("Unlock: " + levelNo + "   " + rating);
            if(levelNo!="1")
                levelsContainer.Find(levelNo).Find("Lock").GetComponent<Image>().enabled = false;
            levelsContainer.Find(levelNo).GetComponent<Button>().interactable = true;
        }

        if(rating==-1)
        {
            levelsContainer.Find(levelNo).Find("Lock").GetComponent<Image>().enabled = false;
            levelsContainer.Find(levelNo).GetComponent<Button>().interactable = true;
        }
    }

    public void StartCountingSeconds()
    {
        seconds = 0;
        InvokeRepeating("CountSeconds", 1, 1);
    }

    void CountSeconds()
    {
        seconds++;

        //if (seconds >= 300)
        //{
        //    seconds = 0;
        //    // Stop the process and let the customer go
        //}
    }

    void Update()
    {

    }

    public void LevelCompleted()
    {
        RatingSystem.instance.ShowCashEffect();

        if (seconds >= 180)
        {
            for (int i = 0; i < 2; i++)
            {
                CharacterManager.instance.levelCompletePanel.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
            CharacterManager.instance.levelCompletePanel.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(false);
            LevelCompletedBadly();
        }
        else if (seconds >= 240)
        {
            CharacterManager.instance.levelCompletePanel.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            CharacterManager.instance.levelCompletePanel.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
            CharacterManager.instance.levelCompletePanel.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(false);
            LevelCompletedBadly();
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                CharacterManager.instance.levelCompletePanel.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
            PlayerPrefs.SetInt(PreGameUIManager.selectedLevel.ToString(), 3);
            PlayerPrefs.SetInt((PreGameUIManager.selectedLevel + 1).ToString(), -1);
        }

        int _totalCoins = GetTotalCoins() + PreGameUIManager.instance.noOfCoins;
        SetTotalCoins(_totalCoins);

        CharacterManager.instance.levelCompletePanel.GetChild(0).Find("CoinsEarned").GetComponent<Text>().text = "Coins   Earned   :"+ PreGameUIManager.instance.noOfCoins.ToString();
        CharacterManager.instance.levelCompletePanel.GetChild(0).Find("TotalCoins").GetComponent<Text>().text = "Total Coins        : "+ _totalCoins.ToString();
        CancelInvoke("CountSeconds");
    }

    public void LevelCompletedBadly()
    {
        PlayerPrefs.SetInt(PreGameUIManager.selectedLevel.ToString(), 2);
    }

    public void LevelFailed()
    {
        CancelInvoke("CountSeconds");
        PlayerPrefs.SetInt(PreGameUIManager.selectedLevel.ToString(), 0);
    }

    int GetRating(string levelNo)
    {
        return PlayerPrefs.GetInt(levelNo, 0);
    }

    public void SetTotalCoins(int totalCoins)
    {
        PlayerPrefs.SetInt("totalcoins", totalCoins);
    }

    public int GetTotalCoins()
    {
        return PlayerPrefs.GetInt("totalcoins");
    }

    public void ShowStarEffect()
    {
        Destroy(Instantiate(starEffect), 3f);
        AudioController.instance.PlayReceiptCompleteSound();
    }

    public void ShowCashEffect()
    {
        Destroy(Instantiate(cashEffect), 3f);
        AudioController.instance.PlayLevelCompleteSound();
    }

    public void ShowNewStarEffect()
    {
        Destroy(Instantiate(newStarEffect), 5f);
        AudioController.instance.PlayReceiptCompleteSound();
    }
}


