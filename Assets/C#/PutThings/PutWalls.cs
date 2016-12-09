using UnityEngine;
using System.Collections;

public class PutWalls : MonoBehaviour
{
    public GameObject wallX, wallY, wallZ;

    // Use this for initialization
    void Start()
    {
        wallX.transform.position = new Vector3(9.5f, 9.5f, -1);
        wallY.transform.position = new Vector3(9.5f, -1f, 0f);
        wallZ.transform.position = new Vector3(-1f, 9.5f, 0f);
        GameObject wallXs = GameObject.Instantiate(wallX);
        GameObject wallYs = GameObject.Instantiate(wallY);
        GameObject wallZs = GameObject.Instantiate(wallZ);
        wallXs.name = "CenterXs";
        wallYs.name = "CenterYs";
        wallZs.name = "CenterZs";
        wallXs.transform.position = new Vector3(9.5f, 9.5f, 20f);
        wallYs.transform.position = new Vector3(9.5f, 20f, 0f);
        wallZs.transform.position = new Vector3(20f, 9.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
