using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameUIManager : MonoBehaviour {

    public static PreGameUIManager instance;

    public GameObject bg, startingBG,levelSelectionPanel,levelInfoPanel,operationSelectionPanel;
    public static int selectedLevel;
    public GameObject loadingScreen;

    [SerializeField]
    private Text levelNo, targetCoin, totalCustomer;
    
    public GameObject[] operations;

    private List<int> coinCount = new List<int>() { 5, 6, 7, 8 };
    private List<int> customerCount = new List<int>() { 1, 2, 3, 4 };

    [HideInInspector]
    public int noOfCustomers = 0;
    [HideInInspector]
    public int noOfCoins = 0;

    private string levelNoStr, targetCoinStr, totalCustomerStr;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        bg.SetActive(true);
        startingBG.SetActive(true);

        levelNoStr = levelNo.text;
        targetCoinStr = targetCoin.text;
        totalCustomerStr = totalCustomer.text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayButton()
    {
        AudioController.instance.PlayButtenPressSound();
        startingBG.SetActive(false);
        levelSelectionPanel.SetActive(true);
    }



    public void SelectLevel(GameObject go)
    {
        AudioController.instance.PlayButtenPressSound();
        selectedLevel = int.Parse(go.name);
        LoadLevel(selectedLevel);
    }

    public void LoadLevel(int levelNo)
    {
        levelSelectionPanel.SetActive(false);
        levelInfoPanel.SetActive(true);
        RatingSystem.instance.StartCountingSeconds();
        SetTarget();
        print("Selected Level: " + selectedLevel);
    }

    public void OKInfoPanel()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(WaitAndPlay());
    }

    IEnumerator WaitAndPlay()
    {
        yield return new WaitForSeconds(3f);
        RatingSystem.instance.ShowNewStarEffect();
        loadingScreen.SetActive(false);
        AudioController.instance.PlayButtenPressSound();
        levelInfoPanel.SetActive(false);
        bg.SetActive(false);
        operationSelectionPanel.SetActive(true);

        CharacterManager.instance.LetCharactersIn();
    }


    void SetTarget()
    {
        levelNo.text = levelNoStr+" "+ selectedLevel.ToString();


        if (selectedLevel == 1 || selectedLevel == 2 || selectedLevel == 4)
        {
            noOfCustomers = customerCount[0];
            noOfCoins = 5;
        }
        else if (selectedLevel == 3 || selectedLevel == 5 || selectedLevel == 7)
        {
            noOfCustomers = customerCount[1];
            noOfCoins = 7;
        }
        else if (selectedLevel == 6 || selectedLevel == 8 || selectedLevel == 10)
        {
            noOfCustomers = customerCount[2];
            noOfCoins = 10;
        }
        else if (selectedLevel == 9 || selectedLevel == 11 || selectedLevel == 12)
        {
            noOfCustomers = customerCount[3];
            noOfCoins = 15;
        }

        totalCustomer.text = totalCustomerStr+" "+noOfCustomers.ToString();
        targetCoin.text = targetCoinStr+" "+noOfCoins.ToString();
        GetRandomCharacters();
    }

    void GetRandomCharacters()
    {
        CharacterManager.instance.chosenCharacters.Clear();
        print("Noofcustomers: " + noOfCustomers);
        if(noOfCustomers==1)
        {
            CharacterManager.instance.chosenCharacters.Add(GetRandom());
        }
        else if (noOfCustomers == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                CharacterManager.instance.chosenCharacters.Add(GetRandom());
            }
        }
        else if (noOfCustomers == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                CharacterManager.instance.chosenCharacters.Add(GetRandom());
            }
        }
        else if (noOfCustomers == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                CharacterManager.instance.chosenCharacters.Add(GetRandom());
            }
        }       
    }

    int GetRandom()
    {
        int _temp = Random.Range(0, 4);
        print(_temp);
        if(!CharacterManager.instance.chosenCharacters.Contains(_temp))
        {
            return _temp;
        }
        else
        {
            return GetRandom();
        }
    }


}
