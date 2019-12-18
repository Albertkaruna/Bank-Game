using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Renamer : MonoBehaviour {

    [SerializeField]
    private int startWith = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = startWith; i <=transform.childCount; i++)
        {
            transform.GetChild(i - 1).name = i.ToString();
            transform.GetChild(i - 1).GetComponentInChildren<Text>().text = i.ToString();
        }
	}
}
