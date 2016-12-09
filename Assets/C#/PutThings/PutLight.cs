using UnityEngine;
using System.Collections;

public class PutLight : MonoBehaviour {
    public GameObject mainLight;
	// Use this for initialization
	void Start () {
        mainLight.transform.position = new Vector3(10f, 10f, 10f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
