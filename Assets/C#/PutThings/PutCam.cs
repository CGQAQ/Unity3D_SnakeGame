using UnityEngine;
using System.Collections;

public class PutCam : MonoBehaviour
{

    public GameObject mainCam;
    // Use this for initialization
    void Start()
    {
        mainCam.transform.position = new Vector3(10f, 10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            mainCam.transform.Rotate(new Vector3(-1,0,0), Space.Self);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            mainCam.transform.Rotate(new Vector3(1,0,0), Space.Self);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mainCam.transform.Rotate(new Vector3(0,-1,0), Space.Self);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mainCam.transform.Rotate(new Vector3(0,1,0), Space.Self);
        }
    }
}
