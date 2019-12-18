using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour {

    public static InGameUIManager instance;

    public GameObject counterRoom, cashArrangement,handWithAnim;
    [SerializeField]
    private Toggle[] operationToggles;


    public static Dictionary<string, List<Toggle>> operations = new Dictionary<string, List<Toggle>>();

    private int tokenCounter = 0;
    private static bool once = true;
    public static string cKey = "";
    public bool canToggleOperation = false,lemmino=false;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetOperations(int selectedLevel,int charCount)
    {
        List<Toggle> chosenOperations = new List<Toggle>();

        print(selectedLevel);

        if (selectedLevel == 1 || selectedLevel == 2 || selectedLevel == 4)
        {
            int _temp = Random.Range(0, 2);
            chosenOperations.Add(operationToggles[_temp]);
            operations.Add("One", chosenOperations);
            cKey = "One";
        }
        else if (selectedLevel == 3 || selectedLevel == 5 || selectedLevel == 7)
        {
            if(charCount==0)
            {
                chosenOperations.Add(operationToggles[0]);
                chosenOperations.Add(operationToggles[2]);
                operations.Add("One", chosenOperations);
                cKey = "One";
            }
            else
            {
                chosenOperations.Add(operationToggles[1]);
                operations.Add("Two", chosenOperations);
                cKey = "Two";
            }

        }
        else if (selectedLevel == 6|| selectedLevel == 8 || selectedLevel == 10)
        {
            if (charCount == 0)
            {
                chosenOperations.Add(operationToggles[0]);
                operations.Add("One", chosenOperations);
                cKey = "One";
            }
            else if (charCount == 1)
            {
                chosenOperations.Add(operationToggles[1]);
                chosenOperations.Add(operationToggles[2]);
                operations.Add("Two", chosenOperations);
                cKey = "Two";
            }
            else
            {
                chosenOperations.Add(operationToggles[0]);
                chosenOperations.Add(operationToggles[2]);
                chosenOperations.Add(operationToggles[3]);
                operations.Add("Three", chosenOperations);
                cKey = "Three";
            }
        }
        else if (selectedLevel == 9 || selectedLevel == 11 || selectedLevel == 12)
        {
            if (charCount == 0)
            {
                chosenOperations.Add(operationToggles[0]);
                chosenOperations.Add(operationToggles[1]);
                operations.Add("One", chosenOperations);
                cKey = "One";
            }
            else if (charCount == 1)
            {
                chosenOperations.Add(operationToggles[3]);
                operations.Add("Two", chosenOperations);
                cKey = "Two";
            }
            else if(charCount==2)
            {
                chosenOperations.Add(operationToggles[0]);
                chosenOperations.Add(operationToggles[2]);
                chosenOperations.Add(operationToggles[3]);
                operations.Add("Three", chosenOperations);
                cKey = "Three";
            }
            else
            {
                chosenOperations.Add(operationToggles[0]);
                chosenOperations.Add(operationToggles[1]);
                chosenOperations.Add(operationToggles[2]);
                operations.Add("Four", chosenOperations);
                cKey = "Four";
            }
        }

        StartCoroutine(CallEnableOperations());

    }


    IEnumerator CallEnableOperations()
    {
        bool once = true;

        foreach (KeyValuePair<string, List<Toggle>> item in operations)
        {
            List<Toggle> temp = new List<Toggle>();
            foreach (Toggle toggle in item.Value)
            {
                temp.Add(toggle);
            }
            foreach (Toggle toggle in item.Value)
            {
                if (item.Key == cKey)
                {
                    if (once)
                    {
                        StartCoroutine(EnableOperations(toggle.name, 2f));
                        once = false;
                    }
                    else
                    {
                        StartCoroutine(EnableOperations(toggle.name, 0.3f));
                    }

                    temp.Remove(toggle);
                    canToggleOperation = false;
                    if (temp.Count <= 0)
                    {
                        lemmino = true;
                        once = true;
                    }
                    yield return new WaitUntil(() => canToggleOperation == true);
                    print("Toggle: " + toggle.name);
                }
            }
        }
    }


    IEnumerator EnableOperations(string _opName,float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        handWithAnim.SetActive(true);
        switch (_opName)
        {
            case "Price Bonds":
                operationToggles[0].interactable=true;
                handWithAnim.transform.position = PreGameUIManager.instance.operations[0].transform.position;
                break;
            case "Money Transfer":
                operationToggles[1].interactable = true;
                handWithAnim.transform.position = PreGameUIManager.instance.operations[1].transform.position;
                break;
            case "Share Purchase":
                operationToggles[2].interactable = true;
                handWithAnim.transform.position = PreGameUIManager.instance.operations[2].transform.position;
                break;
            case "Currency Exchange":
                operationToggles[3].interactable = true;
                handWithAnim.transform.position = PreGameUIManager.instance.operations[3].transform.position;
                break;

            default:
                Debug.Log("Nothing matches.");
                break;
        }
    }

    public void OperationsToggleChange(Toggle toggle)
    {
        toggle.interactable = false;
        handWithAnim.SetActive(false);
        canToggleOperation = true;
        handWithAnim.transform.position = Vector3.zero;
        print(lemmino);
        if(lemmino && once && !operationToggles[0].interactable && !operationToggles[1].interactable && !operationToggles[2].interactable && !operationToggles[3].interactable)
        {
            lemmino = false;
            once = false;
            print("All toggeled: "+toggle.name);
            foreach (Toggle item in operationToggles)
            {
                item.isOn = false;
            }
            GenerateToken();
        }
    }

    void GenerateToken()
    {
        AudioController.instance.PlayTokenActSound();
        CharacterManager.instance.tokens.GetChild(tokenCounter).gameObject.SetActive(true);
        tokenCounter++;
        once = true;
        DragablesHandler.instance.AnimateHand(CharacterManager.instance.tokens.GetChild(tokenCounter).position,CharacterManager.instance.currentCharacter.transform.position);
    }

    public IEnumerator GetCounterRoom()
    {
        yield return new WaitForEndOfFrame();
        counterRoom.SetActive(true);
        for (int i = 0; i < CharacterManager.instance.sittingCharactersParent.childCount; i++)
        {
            CharacterManager.instance.sittingCharactersParent.GetChild(i).position = new Vector3(-30f, -2.22f,0f);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(CharacterManager.instance.GetCharacterToCounter());
    }

    public void GetCashArrangement()
    {
        cashArrangement.SetActive(true);
        DragablesHandler.instance.EnableNewCash();
    }

    public void ClearThingsInGame()
    {
        tokenCounter = 0;
        once = true;
        cKey = "";

        int r = Random.Range(0,4);

        for (int i = 0; i < 4; i++)
        {
            int _r = Random.Range(0, 4);
            CharacterManager.instance.tokens.GetChild(_r).SetSiblingIndex(i);
        }
    }
}
