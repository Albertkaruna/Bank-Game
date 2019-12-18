using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {

    public static CharacterManager instance;

    public GameObject[] standingCharacters;
    public Transform[] sitPositions;
    public Transform tokens;
    public Transform sittingCharactersParent;
    public Transform standMachinePosition;
    public Transform counterCharPosition;
    public Transform door;
    public Transform levelCompletePanel;

    [HideInInspector]
    public List<int> chosenCharacters = new List<int>();
    public static bool letCharacter = false;

    private InGameUIManager ingameUIManager;
    [HideInInspector]
    public GameObject currentCharacter;
    private List<int> sitPositionHolder = new List<int>();
    private static int charCounter = 0;
    public static string currentKey = "";

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        ingameUIManager = Camera.main.GetComponent<InGameUIManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LetCharactersIn()
    {
        StartCoroutine(GetCharacter());
    }

    IEnumerator GetCharacter()
    {
        int _noOfChars = chosenCharacters.Count;
        yield return new WaitForEndOfFrame();

        if (_noOfChars == 1)
        {
            MoveCharacter(standingCharacters[chosenCharacters[0]]);
            ingameUIManager.SetOperations(PreGameUIManager.selectedLevel,0);
        }
        else if (_noOfChars == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                MoveCharacter(standingCharacters[chosenCharacters[i]]);
                ingameUIManager.SetOperations(PreGameUIManager.selectedLevel, i);
                letCharacter = false;
                yield return new WaitUntil(() => letCharacter==true);
            }
        }
        else if (_noOfChars == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                MoveCharacter(standingCharacters[chosenCharacters[i]]);
                ingameUIManager.SetOperations(PreGameUIManager.selectedLevel, i);
                letCharacter = false;
                yield return new WaitUntil(() => letCharacter == true);
            }
        }
        else if (_noOfChars == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                MoveCharacter(standingCharacters[chosenCharacters[i]]);
                ingameUIManager.SetOperations(PreGameUIManager.selectedLevel, i);
                letCharacter = false;
                yield return new WaitUntil(() => letCharacter == true);
            }
        }
    }

    void MoveCharacter(GameObject character)
    {
        currentCharacter = character;
        iTween.MoveTo(door.gameObject, iTween.Hash("x", -9.50f, "delay", 0.3f, "time", 1f));
        iTween.MoveTo(character, iTween.Hash("position", standMachinePosition.position, "delay",1f, "time", 2f));
        Invoke("CloseTheDoor", 1.5f);
    }

    public IEnumerator MoveCharacterReverse()
    {
        yield return new WaitForEndOfFrame();
        int _index = GetRandom();
        print("Random: " + _index);
        Vector3 pos = sitPositions[_index].transform.position;
        iTween.MoveTo(currentCharacter, iTween.Hash("position", pos, "delay", 1f, "time", 1f));
        yield return new WaitForSeconds(2.3f);
        GameObject go = sittingCharactersParent.Find(currentCharacter.name).gameObject;
        go.transform.position = pos;
        currentCharacter.transform.position = new Vector3(-20f, -1.64f, 0f);
        sitPositionHolder.Add(_index);
        yield return new WaitForSeconds(2);
        letCharacter = true;
        charCounter++;
        if(PreGameUIManager.instance.noOfCustomers==charCounter)
        {
            StartCoroutine(ingameUIManager.GetCounterRoom());
        }
    }

    public IEnumerator CheckIsOperationsComplete(string k,string v)
    {
        yield return new WaitForEndOfFrame();
        print(k + "    " + v);
        DragablesHandler.letReceipts = true;
    }

    public IEnumerator MoveCharToIdle()
    {
        yield return new WaitForEndOfFrame();
        currentCharacter.transform.localScale = Vector3.one;
        iTween.MoveTo(currentCharacter, iTween.Hash("position", new Vector3(-20f,-1.64f,0f), "delay", 1f, "time", 1f));
    }

    void CloseTheDoor()
    {
        iTween.MoveTo(door.gameObject, iTween.Hash("x", -7.63f, "delay", 0.3f, "time", 1f));
    }


    int GetRandom()
    {
        int _temp = Random.Range(0, 4);
        if (!sitPositionHolder.Contains(_temp))
        {
            return _temp;
        }
        else
        {
            return GetRandom();
        }
    }

    public IEnumerator GetCharacterToCounter()
    {
        int _noOfChars = chosenCharacters.Count;
        yield return new WaitForEndOfFrame();

        if (_noOfChars == 1)
        {
            StartCoroutine(MoveCharacterToCounter(standingCharacters[chosenCharacters[0]]));
        }
        else if (_noOfChars == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                StartCoroutine(MoveCharacterToCounter(standingCharacters[chosenCharacters[i]]));
                letCharacter = false;
                yield return new WaitUntil(() => letCharacter == true);
            }
        }
        else if (_noOfChars == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(MoveCharacterToCounter(standingCharacters[chosenCharacters[i]]));
                letCharacter = false;
                yield return new WaitUntil(() => letCharacter == true);
            }
        }
        else if (_noOfChars == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                StartCoroutine(MoveCharacterToCounter(standingCharacters[chosenCharacters[i]]));
                letCharacter = false;
                yield return new WaitUntil(() => letCharacter == true);
            }
        }
    }

    IEnumerator MoveCharacterToCounter(GameObject character)
    {
        yield return new WaitForSeconds(1f);
        currentCharacter = character;
        currentCharacter.transform.localScale = counterCharPosition.localScale;
        iTween.MoveTo(character, iTween.Hash("position", counterCharPosition.position,"time", 1f,"delay",0.3f));

        foreach (KeyValuePair<string, List<Toggle>> item in InGameUIManager.operations)
        {
            currentKey = item.Key;

            foreach (Toggle toggle in item.Value)
            {
                yield return new WaitForSeconds(1.3f);
                DragablesHandler.instance.dummyReceipts.Find(toggle.name).gameObject.SetActive(true);
                DragablesHandler.instance.AnimateHand(DragablesHandler.instance.dummyReceipts.Find(toggle.name).position, DragablesHandler.instance.monitor.position);
                DragablesHandler.letReceipts = false;
                yield return new WaitUntil(() => DragablesHandler.letReceipts == true);
            }
            StartCoroutine(MoveCharToIdle());
            InGameUIManager.operations.Remove(currentKey);
            letCharacter = true;
            break;
        }

        if(InGameUIManager.operations.Count==0)
        {
            StartCoroutine(LevelCompleted());
        }
    }

    public IEnumerator LevelCompleted()
    {
        yield return new WaitForSeconds(2f);
        ingameUIManager.counterRoom.SetActive(false);
        levelCompletePanel.gameObject.SetActive(true);
        ClearThings();

        RatingSystem.instance.LevelCompleted();
    }

    void ClearThings()
    {
        chosenCharacters.Clear();
        letCharacter = false;
        currentCharacter = null;
        sitPositionHolder.Clear();
        charCounter = 0;
        currentKey = "";

        ingameUIManager.ClearThingsInGame();
    }
}
