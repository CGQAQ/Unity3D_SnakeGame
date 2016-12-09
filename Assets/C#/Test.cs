using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class Test : MonoBehaviour {

    public Button b;

	// Use this for initialization
	void Start () {
        b.GetComponent<Button>().onClick.AddListener(click);
	}
	
    public void click()
    {
        print("hello");
    }

	// Update is called once per frame
	void Update () {

	}
}
