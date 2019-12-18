using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragablesHandler : MonoBehaviour {

    public static DragablesHandler instance;

    public LayerMask RayHitLayers;
    public Transform enterDetailsPanel;
    public Transform dummyReceipts;
    public Transform originalReceipts;
    public GameObject keyboardErrorMsg;
    public GameObject newCashParent;
    public GameObject oldCashParent;
    public Transform hand;

    public Transform monitor;
    public Text monitorInputField;
    public Text monitorTitleTxt;
    [SerializeField]
    private GameObject cash;
    public GameObject shareReceipt;
    public Animator stamp;


    private RaycastHit2D rayHit;
    private bool canPick=false;
    private Collider2D col1, col2;
    private Vector2 initPosition;
    private bool once = true;
    private Transform currentReceipt;
    private static string title, receiptName;
    private static int operationsCount = 0;

    private string firstValue=null, endValue=null;
    private string rayName;
    private int noOfCash = 0;

    public static bool letReceipts = true;
    private int cashCount = 0;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
       
    }

    // Update is called once per frame
    void Update()
    {    
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (canPick)
        {
            rayHit.transform.position = mousePos;
        }
        rayHit = Physics2D.Raycast(mousePos, Vector2.zero, 1f, RayHitLayers);
  
        if (Input.GetMouseButtonDown(0) && rayHit.collider != null && once)
        {
            initPosition = rayHit.transform.position;
            rayName = rayHit.collider.name;

            if (rayName == "Token_0"|| rayName == "Token_1"|| rayName == "Token_2"|| rayName == "Token_3")
            {
                canPick = true;
                once = false;
                col1 = rayHit.collider;
                col2 = CharacterManager.instance.currentCharacter.GetComponent<Collider2D>();
            }
            else if (rayName == "Price Bonds" || rayName == "Money Transfer" || rayName == "Share Purchase" || rayName == "Currency Echange")
            {
                canPick = true;
                once = false;
                col1 = rayHit.collider;
                col2 = monitor.GetComponent<Collider2D>();
            }else if(rayName=="1"|| rayName == "2" || rayName == "3" || rayName == "4" || rayName == "5" || rayName == "6" || rayName == "7")
            {
                canPick = true;
                once = false;
                col1 = rayHit.collider;
                col2 = oldCashParent.transform.Find(rayName+rayName).GetComponent<Collider2D>();
            }else if(rayName=="Cash")
            {
                canPick = true;
                once = false;
                col1 = rayHit.collider;
                col2 = CharacterManager.instance.currentCharacter.GetComponent<Collider2D>();
            }
            else if (rayName == "StampReceipt")
            {
                canPick = true;
                once = false;
                col1 = rayHit.collider;
                col2 = CharacterManager.instance.currentCharacter.GetComponent<Collider2D>();
            }
        }

        if (canPick)
        {
            if (Physics2D.IsTouching(col1, col2) && Input.GetMouseButtonUp(0))
            {
                AudioController.instance.PlayDragSuccessSound();
                col1.gameObject.SetActive(false);
                canPick = false;
                once = true;
                HideHand();
                if (rayName == "Token_0" || rayName == "Token_1" || rayName == "Token_2" || rayName == "Token_3")
                {
                    StartCoroutine(CharacterManager.instance.MoveCharacterReverse());
                    col1.transform.localPosition = Vector3.zero;
                    canPick = false;
                    once = true;

                    RatingSystem.instance.ShowNewStarEffect();
                }
                else if (rayName == "Price Bonds" || rayName == "Money Transfer" || rayName == "Share Purchase" || rayName == "Currency Echange")
                {
                    col1.gameObject.SetActive(false);
                    col1.transform.localPosition = Vector3.zero;
                    canPick = false;
                    once = true;
                    ActivateReceipt(rayHit.collider.gameObject);
                }else if(rayName== "StampReceipt")
                {
                    RatingSystem.instance.ShowStarEffect();
                    col1.transform.gameObject.SetActive(false);
                    col1.transform.position = initPosition;
                    canPick = false;
                    once = true;
                    //StartCoroutine(CharacterManager.instance.MoveCharToIdle());
                    StartCoroutine(CharacterManager.instance.CheckIsOperationsComplete(CharacterManager.currentKey, currentReceipt.name));
                }
                else if (rayName == "1" || rayName == "2" || rayName == "3" || rayName == "4" || rayName == "5" || rayName == "6" || rayName == "7")
                {
                    col1.gameObject.SetActive(false);
                    col1.transform.localPosition = Vector3.zero;
                    canPick = false;
                    once = true;
                    cashCount++;
                    newCashParent.transform.GetChild(cashCount).GetComponent<Collider2D>().enabled = true;
                
                    if(cashCount<noOfCash)
                    {
                        string _nm = newCashParent.transform.GetChild(cashCount).name;
                        AnimateHand(newCashParent.transform.GetChild(cashCount).position, oldCashParent.transform.Find(_nm + _nm).position);
                    }

                    if (cashCount>=noOfCash)
                    {
                        cashCount = 0;
                        noOfCash = 0;
                        print("Cash arranged");
                       
                        if (currentReceipt.name != "Share Purchase")
                        {
                            cash.SetActive(true);
                            InGameUIManager.instance.cashArrangement.SetActive(false);
                            cash.GetComponent<Collider2D>().enabled = true;
                            StartCoroutine(AddDelay(cash.transform.position, CharacterManager.instance.currentCharacter.transform.position, 0.1f));
                        }
                        else
                        {
                            InGameUIManager.instance.cashArrangement.SetActive(false);
                            shareReceipt.SetActive(true);
                            StartCoroutine(AddDelay(shareReceipt.transform.position, CharacterManager.instance.currentCharacter.transform.position, 1f));
                            stamp.SetBool("stamp", true);
                            Invoke("StampBackAnim", 1f);
                        }
                    }
                }else if(rayName=="Cash")
                {
                    RatingSystem.instance.ShowStarEffect();
                    cash.GetComponent<Collider2D>().enabled = false;
                    col1.transform.gameObject.SetActive(false);
                    col1.transform.position =new  Vector3(0f, -3.15f,0f);
                    canPick = false;
                    once = true;
                    //StartCoroutine(CharacterManager.instance.MoveCharToIdle());
                    StartCoroutine(CharacterManager.instance.CheckIsOperationsComplete(CharacterManager.currentKey, currentReceipt.name));
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (rayHit.collider != null)
                {
                    AudioController.instance.PlayDragFailSound();
                    col1.transform.position = initPosition;
                    canPick = false;
                    once = true;
                }
            } 
        }
    }

    IEnumerator AddDelay(Vector3 pos1,Vector3 pos2,float delay)
    {        
        yield return new WaitForSeconds(delay);
        AnimateHand(pos1, pos2);
    }

    void StampBackAnim()
    {
        stamp.SetBool("stamp", false);
    }

    void ActivateReceipt(GameObject rayhitObject)
    {
        enterDetailsPanel.gameObject.SetActive(true);

        for (int i = 0; i < originalReceipts.childCount; i++)
        {
            originalReceipts.GetChild(i).gameObject.SetActive(false);
        }

        switch (rayhitObject.name)
        {
            case "Price Bonds":
                originalReceipts.GetChild(0).gameObject.SetActive(true);
                currentReceipt = originalReceipts.GetChild(0);
                DecideWhatToEnter("Serial Number");
                GeneratePricezBond();
                break;
            case "Money Transfer":
                originalReceipts.GetChild(1).gameObject.SetActive(true);
                currentReceipt = originalReceipts.GetChild(1);
                DecideWhatToEnter("Transfer Amount");
                GenerateMoneyTransfer();
                break;
            case "Share Purchase":
                originalReceipts.GetChild(2).gameObject.SetActive(true);
                currentReceipt = originalReceipts.GetChild(2);
                DecideWhatToEnter("Company");
                GenerateSharePurchase(rayhitObject.name.Substring(0,3));
                break;
            case "Currency Exchange":
                originalReceipts.GetChild(3).gameObject.SetActive(true);
                currentReceipt = originalReceipts.GetChild(3);
                DecideWhatToEnter("Rate");
                GenerateCurrencyExchange();
                break;

            default:
                Debug.Log("Nothing matches.");
                break;
        }
    }

    public void GeneratePricezBond()
    {
        int faceValue = Random.Range(250, 500);
        int serialNumber = Random.Range(30500, 65000);
        originalReceipts.GetChild(0).GetChild(0).transform.Find("Face Value").GetComponent<Text>().text = faceValue.ToString();
        originalReceipts.GetChild(0).GetChild(0).transform.Find("Serial Number").GetComponent<Text>().text = serialNumber.ToString();
    }


    public void GenerateMoneyTransfer()
    {
        int phNumber = Random.Range(36586, 45250);
        int transferAmount = Random.Range(50, 250);
        int transactionFee = 2;
        int total = transferAmount + transactionFee;
        int receiverPhNumber = Random.Range(45251, 65985);
        originalReceipts.GetChild(1).GetChild(0).transform.Find("Phone Number").GetComponent<Text>().text = phNumber.ToString();
        originalReceipts.GetChild(1).GetChild(0).transform.Find("Transfer Amount").GetComponent<Text>().text = transferAmount.ToString();
        originalReceipts.GetChild(1).GetChild(0).transform.Find("Transfer Fee").GetComponent<Text>().text = transactionFee.ToString();
        originalReceipts.GetChild(1).GetChild(0).transform.Find("Total").GetComponent<Text>().text = total.ToString();
        originalReceipts.GetChild(1).GetChild(0).transform.Find("Other Phone Number").GetComponent<Text>().text = receiverPhNumber.ToString();
    }

    public void GenerateSharePurchase(string genre)
    {
        string[] menNames = new string[] { "JOHN", "ADAM", "BOB", "STEVE", "TYLER", "JACK" };
        string[] womenNames = new string[] { "MARLA", "KATY", "JULIE", "IGGY", "SIA", "RITA" };
        string name = "";
        string[] companies = new string[] { "DENS", "DIGI", "KARN", "FRAN", "CRITZ", "MYND" };
        int total = Random.Range(350, 500);
        string company = companies[Random.Range(0, companies.Length - 1)];
        if (genre == "Boy")
        {
            name = menNames[Random.Range(0, (menNames.Length - 1))];
        }
        else
        {
            name = womenNames[Random.Range(0, (womenNames.Length - 1))];
        }
       originalReceipts.GetChild(2).GetChild(0).transform.Find("Name").GetComponent<Text>().text = name;
       originalReceipts.GetChild(2).GetChild(0).transform.Find("Company").GetComponent<Text>().text = company;
       originalReceipts.GetChild(2).GetChild(0).transform.Find("Total").GetComponent<Text>().text = total.ToString();
    }

    public void GenerateCurrencyExchange()
    {
        int amount = Random.Range(250, 500);
        float rate = Random.Range(50.55f, 65.23f);
        float totalAmount = amount * rate;
        originalReceipts.GetChild(3).GetChild(0).transform.Find("Amount").GetComponent<Text>().text = amount.ToString();
        originalReceipts.GetChild(3).GetChild(0).transform.Find("Rate").GetComponent<Text>().text = rate.ToString();
        originalReceipts.GetChild(3).GetChild(0).transform.Find("TotalAmount").GetComponent<Text>().text = totalAmount.ToString();
    }

    public void EnterDataToMonitor(string c)
    {
        string value = "";

        switch (receiptName)
        {
            case "Price Bonds":
                if (title == "Face Value")
                {
                    value = originalReceipts.GetChild(0).GetChild(0).transform.Find("Face Value").GetComponent<Text>().text;
                }
                else if (title == "Serial Number")
                {
                    value = value = originalReceipts.GetChild(0).GetChild(0).transform.Find("Serial Number").GetComponent<Text>().text;
                }
                break;
            case "Money Transfer":
                if (title == "Phone Number")
                {
                    value = originalReceipts.GetChild(1).GetChild(0).transform.Find("Phone Number").GetComponent<Text>().text;
                }
                else if (title == "Transfer Amount")
                {
                    value = originalReceipts.GetChild(1).GetChild(0).transform.Find("Transfer Amount").GetComponent<Text>().text;
                }
                else if (title == "Transfer Fee")
                {
                    value = originalReceipts.GetChild(1).GetChild(0).transform.Find("Transfer Fee").GetComponent<Text>().text;
                }
                else if (title == "Total")
                {
                    value = originalReceipts.GetChild(1).GetChild(0).transform.Find("Total").GetComponent<Text>().text;
                }
                else if (title == "Other Phone Number")
                {
                    value = originalReceipts.GetChild(1).GetChild(0).transform.Find("Other Phone Number").GetComponent<Text>().text;
                }
                break;
            case "Share Purchase":
                if (title == "Name")
                {
                    value = originalReceipts.GetChild(2).GetChild(0).transform.Find("Name").GetComponent<Text>().text;
                }
                else if (title == "Company")
                {
                    value = originalReceipts.GetChild(2).GetChild(0).transform.Find("Company").GetComponent<Text>().text;
                }
                else if (title == "Total")
                {
                    value = originalReceipts.GetChild(2).GetChild(0).transform.Find("Total").GetComponent<Text>().text;
                }
                break;
            case "Currency Exchange":
                if (title == "Amount")
                {
                    value = originalReceipts.GetChild(3).GetChild(0).transform.Find("Amount").GetComponent<Text>().text;
                }
                else if (title == "Rate")
                {
                    value = originalReceipts.GetChild(3).GetChild(0).transform.Find("Rate").GetComponent<Text>().text;
                }
                else if (title == "TotalAmount")
                {
                    value = originalReceipts.GetChild(3).GetChild(0).transform.Find("TotalAmount").GetComponent<Text>().text;
                }
                break;
            default:
                Debug.Log("Nothing Mathes");
                break;
        }
        firstValue = value;
        string oldValue = monitorInputField.text;
        if (value.StartsWith(oldValue + c))
        {
            monitorInputField.text = oldValue + c;
            if(value==oldValue+c)
            {
                AudioController.instance.PlayKeybordPressSound();
                endValue = oldValue + c;
            }
        }
        else
        {
            keyboardErrorMsg.SetActive(true);
            AudioController.instance.PlayWrongKeyboardPressSound();
            Invoke("HideKeyboardErrorMsg", 1.5f);
        }
    }

    void HideKeyboardErrorMsg()
    {
        keyboardErrorMsg.SetActive(false);
    }

    public void GetKeyboardKeys(GameObject key)
    {
       if (key.name!="BACKSPACE" && key.name!="ENTER" && key.name!="SPACE")
        {
            EnterDataToMonitor(key.name);
             AudioController.instance.PlayKeybordPressSound();

        }else if(key.name=="ENTER")
        {
            AudioController.instance.PlayKeybordPressSound();
            //print(firstValue + "    " + endValue);
            if (firstValue==endValue && firstValue!=null&& endValue != null)
            {
                operationsCount++;
                monitorInputField.text = "";
                firstValue = null;
                endValue = null;

                if (currentReceipt.name=="Price Bonds")
                {
                    DecideWhatToEnter("Face Value");
                    if(operationsCount==2)
                    {
                        enterDetailsPanel.gameObject.SetActive(false);
                        cash.GetComponent<Collider2D>().enabled = false;
                        cash.SetActive(true);
                        iTween.MoveFrom(cash, iTween.Hash("y", 1f, "delay", 0.5f, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
                        StartCoroutine(DoSomeWithCash());
                        operationsCount = 0;
                    }
                }else if(currentReceipt.name=="Money Transfer")
                {
                    DecideWhatToEnter("Phone Number");
                    if (operationsCount == 2)
                    {
                        DecideWhatToEnter("Total");
                    }
                    else if (operationsCount == 3)
                    {
                        DecideWhatToEnter("Other Phone Number");
                    }
                    else if (operationsCount == 4)
                    {
                        DecideWhatToEnter("Transfer Fee");
                    }
                    else if (operationsCount == 5)
                    {
                        enterDetailsPanel.gameObject.SetActive(false);
                        cash.SetActive(true);
                        iTween.MoveFrom(cash, iTween.Hash("y", 1f, "delay", 0.5f, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
                        StartCoroutine(DoSomeWithCash());
                        operationsCount = 0;
                    }

                    print("money transfer");
                }
                else if (currentReceipt.name == "Share Purchase")
                {
                    DecideWhatToEnter("Name");
                    if(operationsCount==2)
                    {
                        DecideWhatToEnter("Total");
                    }else if(operationsCount==3)
                    {
                        enterDetailsPanel.gameObject.SetActive(false);
                        cash.SetActive(true);
                        iTween.MoveFrom(cash, iTween.Hash("y", 1f, "delay", 0.5f, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
                        StartCoroutine(DoSomeWithCash());
                        operationsCount = 0;
                    }
                    print("Share purchase");
                }
                else if (currentReceipt.name == "Currency Exchange")
                {
                    DecideWhatToEnter("Amount");
                    if (operationsCount == 2)
                    {
                        DecideWhatToEnter("TotalAmount");
                    }
                    else if (operationsCount == 3)
                    {
                        enterDetailsPanel.gameObject.SetActive(false);
                        cash.SetActive(true);
                        iTween.MoveFrom(cash, iTween.Hash("y", 1f, "delay", 0.5f, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
                        StartCoroutine(DoSomeWithCash());
                        operationsCount = 0;
                    }

                    print("currency exchange");
                }
            }
        }
        else
        {
            AudioController.instance.PlayKeybordPressSound();
        }
    }

    IEnumerator DoSomeWithCash()
    {
        yield return new WaitForSeconds(2);
        cash.SetActive(false);
        InGameUIManager.instance.GetCashArrangement();
    }

    void DecideWhatToEnter(string _name)
    {
        monitorTitleTxt.text = "Enter the " + _name;
        title = _name;
        receiptName = currentReceipt.name;
    }

    public void EnableNewCash()
    {
        noOfCash = Random.Range(3, 6);

        for (int i = 0; i < 7; i++)
        {
            newCashParent.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
        }

        for (int i = 0; i < 7; i++)
        {
            int _r = Random.Range(0, 7);
            newCashParent.transform.GetChild(_r).SetSiblingIndex(i);           
        }

        for (int i = 0; i < noOfCash; i++)
        {
            newCashParent.transform.GetChild(i).gameObject.SetActive(true);
            newCashParent.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = (12-i);
        }
        newCashParent.transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
        string _nm = newCashParent.transform.GetChild(0).name;
        AnimateHand(newCashParent.transform.GetChild(0).position, oldCashParent.transform.Find(_nm + _nm).position);
    }

    public void AnimateHand(Vector3 _from,Vector3 _to)
    {
        print("Animate Hand");
        hand.gameObject.SetActive(true);
        hand.position = _from;
        iTween.MoveTo(hand.gameObject, iTween.Hash("position", _to, "time", 1f, "looptype", iTween.LoopType.loop));
    }

    public void HideHand()
    {
        print("Hide Animate Hand");
        Destroy(hand.GetComponent<iTween>());
        hand.gameObject.SetActive(false);
        hand.position = Vector3.zero;
    }
}
