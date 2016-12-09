/**
 * Design & Write All By CG 
 * Copy that please write down author's name!
 * Date: July 4, 2016
 */



using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public partial class SnakeGame : MonoBehaviour
{
    public GameObject snakeBody;
    public GameObject snakeHead;
    public GameObject mainCamera;

    GameObject currentSnakeHead;

    Vector3[] snakeList;
    GameStatus gameStatus = GameStatus.over;
    Direction moveDirection = Direction.forward;
    Direction bodyDirection = Direction.up;//蛇头的头顶朝向
    int snakeHeadPositon;
    int snakeTailPositon;
    int snakeLength;

    int Score = 0;

    BasicDirection turnDirection;
    // Use this for initialization
    void Start()
    {
        InitGame();
        StartCoroutine(RefreshGame());
        DrawGame();
        PutCamera();
        ShowDialog("按Home键开始或点击开始按钮开始！", "开    始", () =>
          {
              gameStatus = GameStatus.gaming;
              DestroyDialog();
              ShowScore(Score);
          });
    }


    IEnumerator RefreshGame()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (gameStatus == GameStatus.gaming)
            {
                Turn();
                MoveSnake();
                PutCamera();


            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            if (gameStatus == GameStatus.over)
            {
                gameStatus = GameStatus.gaming;
                DestroyDialog();
                ShowScore(Score);
            }
            else if (gameStatus == GameStatus.gaming)
            {
                gameStatus = GameStatus.pause;
                DestroyDialog();
                ShowDialog("游戏暂停，按Home键继续或点击开始按钮继续，或按End按钮重新开始！", "继    续", () =>
                {
                    gameStatus = GameStatus.gaming;
                    DestroyDialog();
                    ShowScore(Score);
                }, "结    束",
                () =>
                {
                    gameStatus = GameStatus.over;
                    InitGame();
                    DrawGame();
                    PutCamera();
                    DestroyDialog();
                    ShowDialog("游戏终止，按Home键开始或点击开始按钮开始！", "开    始", () =>
                    {
                        gameStatus = GameStatus.gaming;
                        DestroyDialog();
                        ShowScore(Score);
                    });
                });
            }
            else if (gameStatus == GameStatus.pause)
            {
                gameStatus = GameStatus.gaming;
                DestroyDialog();
                ShowScore(Score);
            }
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {
            if (gameStatus != GameStatus.over)
            {
                gameStatus = GameStatus.over;
                InitGame();
                DrawGame();
                PutCamera();
                DestroyDialog();
                ShowDialog("游戏终止，按Home键开始或点击开始按钮开始！", "开    始", () =>
                {
                    gameStatus = GameStatus.gaming;
                    DestroyDialog();
                });
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))//控制
        {

            Turn(BasicDirection.up);

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Turn(BasicDirection.down);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))//左转
        {
            Turn(BasicDirection.left);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))//右转
        {

            Turn(BasicDirection.right);

        }
    }

    void InitGame()
    {
        if (GameObject.Find("body") != null)
        {
            if (snakeHeadPositon > snakeTailPositon)
            {
                for (int i = snakeTailPositon; i < snakeHeadPositon; i++)
                {
                    DestroyImmediate(GameObject.Find("body"));
                }
            }
            else
            {
                for (int i = snakeTailPositon; i < snakeList.Length; i++)
                {
                    DestroyImmediate(GameObject.Find("body"));
                }
                for (int i = 0; i < snakeHeadPositon; i++)
                {
                    DestroyImmediate(GameObject.Find("body"));
                }
            }
        }

        snakeList = new Vector3[20];
        //print(snakeList.Length);//log out
        snakeHeadPositon = 3;
        snakeTailPositon = 0;
        snakeLength = 4;
        moveDirection = Direction.forward;
        bodyDirection = Direction.up;//蛇头的头顶朝向
        turnDirection = BasicDirection.none;

        //生成食物
        RefreshFood();

        //Reset SnakeHead Cube
        DestroyImmediate(GameObject.Find("head"));
        currentSnakeHead = GameObject.Instantiate(snakeHead);
        currentSnakeHead.transform.position = snakeList[snakeHeadPositon];
        currentSnakeHead.name = "head";

        //Reset Main Camera
        DestroyImmediate(GameObject.Find("MainCam"));
        mainCamera = new GameObject();
        mainCamera.name = "MainCam";
        mainCamera.AddComponent<Camera>();
        mainCamera.AddComponent<GUILayer>();
        mainCamera.AddComponent<FlareLayer>();
        mainCamera.AddComponent<AudioListener>();
        mainCamera.GetComponent<Camera>().orthographic = false;
        mainCamera.GetComponent<Camera>().nearClipPlane = 0.3f;
        mainCamera.GetComponent<Camera>().farClipPlane = 1000;


        snakeList[0] = new Vector3(0, 0, 0);
        snakeList[1] = new Vector3(0, 0, 1);
        snakeList[2] = new Vector3(0, 0, 2);
        snakeList[3] = new Vector3(0, 0, 3);
    }

    void DrawGame()
    {
        for (int i = 0; i < snakeLength - 1; i++)
        {
            GameObject g = GameObject.Find("body");
            DestroyImmediate(g);
        }
        //Destroy(GameObject.Find("head")); don't destroy head, because if destroy it ,we'll lost rotate

        if (snakeHeadPositon > snakeTailPositon)
        {
            for (int i = snakeTailPositon; i < snakeHeadPositon; i++)
            {
                GameObject g0 = GameObject.Instantiate(snakeBody);
                g0.transform.position = snakeList[i];
                g0.name = "body";
            }
            currentSnakeHead.transform.position = snakeList[snakeHeadPositon];
        }
        else
        {
            for (int i = snakeTailPositon; i < snakeList.Length; i++)
            {
                GameObject g0 = GameObject.Instantiate(snakeBody);
                g0.transform.position = snakeList[i];
                g0.name = "body";
            }
            for (int i = 0; i < snakeHeadPositon; i++)
            {
                GameObject g0 = GameObject.Instantiate(snakeBody);
                g0.transform.position = snakeList[i];
                g0.name = "body";
            }
            currentSnakeHead.transform.position = snakeList[snakeHeadPositon];
        }


        //if (item.Equals(snakeList[snakeList.Length - 1]))
        //{
        //    GameObject g0 = GameObject.Instantiate(snakeHead);
        //    g0.transform.position = item;
        //    g0.name = "head";
        //    continue;
        //}
        //GameObject g = GameObject.Instantiate(snakeBody);
        //g.transform.position = item;
        //g.name = "body";

    }

    void MoveSnake()//new MoveSnake Method 
    {
        if (snakeLength <= snakeList.Length)
        {
            if (IsHitWall())
            {
                gameStatus = GameStatus.over;
                InitGame();
                PutCamera();
                DrawGame();
                DestroyImmediate(GameObject.Find("Canvas"));
                ShowDialog("你撞墙了，点击开始重新开始", "开    始", () =>
                {
                    DestroyDialog();
                    gameStatus = GameStatus.gaming;
                    ShowScore(Score);
                });
                return;
            }
            switch (moveDirection)
            {
                case Direction.up:
                    //if (snakeHeadPositon<snakeList.Length-1)
                    //{
                    //    snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y + 1, snakeList[snakeHeadPositon].z);
                    //    snakeHeadPositon++;
                    //    snakeTailPositon++;
                    //}
                    //else
                    //{

                    //}
                    if (snakeHeadPositon > snakeTailPositon)
                    {
                        if (snakeHeadPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y + 1, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[0] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y + 1, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon = 0;
                            snakeTailPositon++;
                        }
                    }
                    else
                    {
                        if (snakeTailPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y + 1, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y + 1, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon = 0;
                        }
                    }
                    break;
                case Direction.down:
                    if (snakeHeadPositon > snakeTailPositon)
                    {
                        if (snakeHeadPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y - 1, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[0] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y - 1, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon = 0;
                            snakeTailPositon++;
                        }
                    }
                    else
                    {
                        if (snakeTailPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y - 1, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y - 1, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon = 0;
                        }
                    }
                    break;
                case Direction.left:
                    if (snakeHeadPositon > snakeTailPositon)
                    {
                        if (snakeHeadPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x - 1, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[0] = new Vector3(snakeList[snakeHeadPositon].x - 1, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon = 0;
                            snakeTailPositon++;
                        }
                    }
                    else
                    {
                        if (snakeTailPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x - 1, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x - 1, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon = 0;
                        }
                    }
                    break;
                case Direction.right:
                    if (snakeHeadPositon > snakeTailPositon)
                    {
                        if (snakeHeadPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x + 1, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[0] = new Vector3(snakeList[snakeHeadPositon].x + 1, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon = 0;
                            snakeTailPositon++;
                        }
                    }
                    else
                    {
                        if (snakeTailPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x + 1, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x + 1, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                            snakeHeadPositon++;
                            snakeTailPositon = 0;
                        }
                    }
                    break;
                case Direction.back:
                    if (snakeHeadPositon > snakeTailPositon)
                    {
                        if (snakeHeadPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z - 1);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[0] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z - 1);
                            snakeHeadPositon = 0;
                            snakeTailPositon++;
                        }
                    }
                    else
                    {
                        if (snakeTailPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z - 1);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z - 1);
                            snakeHeadPositon++;
                            snakeTailPositon = 0;
                        }
                    }
                    break;
                case Direction.forward:
                    if (snakeHeadPositon > snakeTailPositon)
                    {
                        if (snakeHeadPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z + 1);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[0] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z + 1);
                            snakeHeadPositon = 0;
                            snakeTailPositon++;
                        }
                    }
                    else
                    {
                        if (snakeTailPositon < snakeList.Length - 1)
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z + 1);
                            snakeHeadPositon++;
                            snakeTailPositon++;
                        }
                        else
                        {
                            snakeList[snakeHeadPositon + 1] = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z + 1);
                            snakeHeadPositon++;
                            snakeTailPositon = 0;
                        }
                    }
                    break;
                default:
                    break;
            }
            DrawGame();
            if (IsEatingFood())
            {
                Score += 10;
                AddSnakeLength();
                RefreshFood();
            }

            if (IsHitItsef())
            {
                gameStatus = GameStatus.over;
                InitGame();
                PutCamera();
                DrawGame();
                DestroyImmediate(GameObject.Find("Canvas"));
                ShowDialog("你撞到自己了，点击开始重新开始", "开    始", () =>
                {

                    DestroyDialog();
                    gameStatus = GameStatus.gaming;
                    ShowScore(Score);
                });
            }
        }
    }


    void Turn()
    {

        switch (turnDirection)
        {
            case BasicDirection.up:
                currentSnakeHead.transform.Rotate(-90, 0, 0, Space.Self);
                mainCamera.transform.Rotate(-90, 0, 0, Space.Self);

                if (moveDirection == Direction.forward)
                {
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.up;
                            break;
                        case Direction.down:
                            moveDirection = Direction.down;
                            break;
                        case Direction.left:
                            moveDirection = Direction.left;
                            break;
                        case Direction.right:
                            moveDirection = Direction.right;
                            break;
                        case Direction.back:
                            break;
                        case Direction.forward:
                            break;
                        default:
                            break;
                    }
                    bodyDirection = Direction.back;
                }
                else if (moveDirection == Direction.back)
                {
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.up;
                            break;
                        case Direction.down:
                            moveDirection = Direction.down;
                            break;
                        case Direction.left:
                            moveDirection = Direction.left;
                            break;
                        case Direction.right:
                            moveDirection = Direction.right;
                            break;
                        case Direction.back:
                            break;
                        case Direction.forward:
                            break;
                        default:
                            break;
                    }
                    bodyDirection = Direction.forward;
                }
                else if (moveDirection == Direction.left)
                {
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.up;
                            break;
                        case Direction.down:
                            moveDirection = Direction.down;
                            break;
                        case Direction.left:
                            /*moveDirection = Direction.left;*/
                            break;
                        case Direction.right:
                            /*moveDirection = Direction.right;*/
                            break;
                        case Direction.back:
                            moveDirection = Direction.back;
                            break;
                        case Direction.forward:
                            moveDirection = Direction.forward;
                            break;
                        default:
                            break;
                    }
                    bodyDirection = Direction.right;
                }
                else if (moveDirection == Direction.right)
                {
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.up;
                            break;
                        case Direction.down:
                            moveDirection = Direction.down;
                            break;
                        case Direction.left:
                            /* moveDirection = Direction.left;*/
                            break;
                        case Direction.right:
                            /*moveDirection = Direction.right;*/
                            break;
                        case Direction.back:
                            moveDirection = Direction.back;
                            break;
                        case Direction.forward:
                            moveDirection = Direction.forward;
                            break;
                        default:
                            break;
                    }
                    bodyDirection = Direction.left;
                }
                else if (moveDirection == Direction.up)
                {
                    if (bodyDirection == Direction.back)
                    {
                        moveDirection = Direction.back;
                        bodyDirection = Direction.down;
                    }
                    else if (bodyDirection == Direction.forward)
                    {
                        moveDirection = Direction.forward;
                        bodyDirection = Direction.down;
                    }
                    else if (bodyDirection == Direction.left)
                    {
                        moveDirection = Direction.left;
                        bodyDirection = Direction.down;
                    }
                    else if (bodyDirection == Direction.right)
                    {
                        moveDirection = Direction.right;
                        bodyDirection = Direction.down;
                    }
                }
                else if (moveDirection == Direction.down)
                {
                    if (bodyDirection == Direction.back)
                    {
                        moveDirection = Direction.back;
                        bodyDirection = Direction.up;
                    }
                    else if (bodyDirection == Direction.forward)
                    {
                        moveDirection = Direction.forward;
                        bodyDirection = Direction.up;
                    }
                    else if (bodyDirection == Direction.left)
                    {
                        moveDirection = Direction.left;
                        bodyDirection = Direction.up;
                    }
                    else if (bodyDirection == Direction.right)
                    {
                        moveDirection = Direction.right;
                        bodyDirection = Direction.up;
                    }
                }
                break;
            case BasicDirection.down:
                currentSnakeHead.transform.Rotate(90, 0, 0, Space.Self);
                mainCamera.transform.Rotate(90, 0, 0, Space.Self);

                if (moveDirection == Direction.forward)
                {
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.down;
                            break;
                        case Direction.down:
                            moveDirection = Direction.up;
                            break;
                        case Direction.left:
                            moveDirection = Direction.right;
                            break;
                        case Direction.right:
                            moveDirection = Direction.left;
                            break;
                        case Direction.back:
                            /* moveDirection = Direction.back;*/
                            break;
                        case Direction.forward:
                            /*moveDirection = Direction.forward;*/
                            break;
                        default:
                            break;
                    }
                    bodyDirection = Direction.forward;
                }
                else if (moveDirection == Direction.back)
                {
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.down;
                            break;
                        case Direction.down:
                            moveDirection = Direction.up;
                            break;
                        case Direction.left:
                            moveDirection = Direction.right;
                            break;
                        case Direction.right:
                            moveDirection = Direction.left;
                            break;
                        case Direction.back:
                            /*moveDirection = Direction.back;*/
                            break;
                        case Direction.forward:
                            /*moveDirection = Direction.forward;*/
                            break;
                        default:
                            break;
                    }
                    bodyDirection = Direction.back;
                }
                else if (moveDirection == Direction.left)
                {
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.down;
                            break;
                        case Direction.down:
                            moveDirection = Direction.up;
                            break;
                        case Direction.left:
                            /*moveDirection = Direction.left;*/
                            break;
                        case Direction.right:
                            /*moveDirection = Direction.right;*/
                            break;
                        case Direction.back:
                            moveDirection = Direction.forward;
                            break;
                        case Direction.forward:
                            moveDirection = Direction.back;
                            break;
                        default:
                            break;
                    }
                    bodyDirection = Direction.left;
                }
                else if (moveDirection == Direction.right)
                {
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.down;
                            break;
                        case Direction.down:
                            moveDirection = Direction.up;
                            break;
                        case Direction.left:
                            /*moveDirection = Direction.left;*/
                            break;
                        case Direction.right:
                            /*moveDirection = Direction.right;*/
                            break;
                        case Direction.back:
                            moveDirection = Direction.forward;
                            break;
                        case Direction.forward:
                            moveDirection = Direction.back;
                            break;
                        default:
                            break;
                    }
                    bodyDirection = Direction.right;
                }
                else if (moveDirection == Direction.up)
                {
                    if (bodyDirection == Direction.back)
                    {
                        moveDirection = Direction.forward;
                        bodyDirection = Direction.up;
                    }
                    else if (bodyDirection == Direction.forward)
                    {
                        moveDirection = Direction.back;
                        bodyDirection = Direction.up;
                    }
                    else if (bodyDirection == Direction.left)
                    {
                        moveDirection = Direction.right;
                        bodyDirection = Direction.up;
                    }
                    else if (bodyDirection == Direction.right)
                    {
                        moveDirection = Direction.left;
                        bodyDirection = Direction.up;
                    }
                }
                else if (moveDirection == Direction.down)
                {
                    if (bodyDirection == Direction.back)
                    {
                        moveDirection = Direction.forward;
                        bodyDirection = Direction.down;
                    }
                    else if (bodyDirection == Direction.forward)
                    {
                        moveDirection = Direction.back;
                        bodyDirection = Direction.down;
                    }
                    else if (bodyDirection == Direction.left)
                    {
                        moveDirection = Direction.right;
                        bodyDirection = Direction.down;
                    }
                    else if (bodyDirection == Direction.right)
                    {
                        moveDirection = Direction.left;
                        bodyDirection = Direction.down;
                    }
                }
                break;
            case BasicDirection.left:
                currentSnakeHead.transform.Rotate(0, -90, 0, Space.Self);
                mainCamera.transform.Rotate(0, -90, 0, Space.Self);

                if (moveDirection == Direction.forward)
                {
                    //moveDirection = Direction.down;
                    //bodyDirection = Direction.forward;
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.left;
                            break;
                        case Direction.down:
                            moveDirection = Direction.right;
                            break;
                        case Direction.left:
                            moveDirection = Direction.down;
                            break;
                        case Direction.right:
                            moveDirection = Direction.up;
                            break;
                        case Direction.back:
                            /*moveDirection = Direction.back;*/
                            break;
                        case Direction.forward:
                            /*moveDirection = Direction.forward;*/
                            break;
                        default:
                            break;
                    }
                }
                else if (moveDirection == Direction.back)
                {
                    //moveDirection = Direction.down;
                    //bodyDirection = Direction.back;
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.right;
                            break;
                        case Direction.down:
                            moveDirection = Direction.left;
                            break;
                        case Direction.left:
                            moveDirection = Direction.up;
                            break;
                        case Direction.right:
                            moveDirection = Direction.down;
                            break;
                        case Direction.back:
                            /*moveDirection = Direction.back;*/
                            break;
                        case Direction.forward:
                            /*moveDirection = Direction.forward;*/
                            break;
                        default:
                            break;
                    }
                }
                else if (moveDirection == Direction.left)
                {
                    //moveDirection = Direction.down;
                    //bodyDirection = Direction.left;
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.back;
                            break;
                        case Direction.down:
                            moveDirection = Direction.forward;
                            break;
                        case Direction.left:
                            /*moveDirection = Direction.down;*/
                            break;
                        case Direction.right:
                            /*moveDirection = Direction.up;*/
                            break;
                        case Direction.back:
                            moveDirection = Direction.down;
                            break;
                        case Direction.forward:
                            moveDirection = Direction.up;
                            break;
                        default:
                            break;
                    }
                }
                else if (moveDirection == Direction.right)
                {
                    //moveDirection = Direction.down;
                    //bodyDirection = Direction.right;
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.forward;
                            break;
                        case Direction.down:
                            moveDirection = Direction.back;
                            break;
                        case Direction.left:
                            /*moveDirection = Direction.down;*/
                            break;
                        case Direction.right:
                            /*moveDirection = Direction.up;*/
                            break;
                        case Direction.back:
                            moveDirection = Direction.up;
                            break;
                        case Direction.forward:
                            moveDirection = Direction.down;
                            break;
                        default:
                            break;
                    }
                }
                else if (moveDirection == Direction.up)
                {
                    if (bodyDirection == Direction.back)
                    {
                        moveDirection = Direction.left;
                    }
                    else if (bodyDirection == Direction.forward)
                    {
                        moveDirection = Direction.right;
                    }
                    else if (bodyDirection == Direction.left)
                    {
                        moveDirection = Direction.forward;
                    }
                    else if (bodyDirection == Direction.right)
                    {
                        moveDirection = Direction.back;
                    }
                }
                else if (moveDirection == Direction.down)
                {
                    if (bodyDirection == Direction.back)
                    {
                        moveDirection = Direction.right;
                    }
                    else if (bodyDirection == Direction.forward)
                    {
                        moveDirection = Direction.left;
                    }
                    else if (bodyDirection == Direction.left)
                    {
                        moveDirection = Direction.back;
                    }
                    else if (bodyDirection == Direction.right)
                    {
                        moveDirection = Direction.forward;
                    }
                }
                break;
            case BasicDirection.right:
                currentSnakeHead.transform.Rotate(0, 90, 0, Space.Self);
                mainCamera.transform.Rotate(0, 90, 0, Space.Self);

                if (moveDirection == Direction.forward)
                {
                    //moveDirection = Direction.down;
                    //bodyDirection = Direction.forward;
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.right;
                            break;
                        case Direction.down:
                            moveDirection = Direction.left;
                            break;
                        case Direction.left:
                            moveDirection = Direction.up;
                            break;
                        case Direction.right:
                            moveDirection = Direction.down;
                            break;
                        case Direction.back:
                            /*moveDirection = Direction.down;*/
                            break;
                        case Direction.forward:
                            /* moveDirection = Direction.up;*/
                            break;
                        default:
                            break;
                    }
                }
                else if (moveDirection == Direction.back)
                {
                    //moveDirection = Direction.down;
                    //bodyDirection = Direction.back;
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.left;
                            break;
                        case Direction.down:
                            moveDirection = Direction.right;
                            break;
                        case Direction.left:
                            moveDirection = Direction.down;
                            break;
                        case Direction.right:
                            moveDirection = Direction.up;
                            break;
                        case Direction.back:
                            /*moveDirection = Direction.down;*/
                            break;
                        case Direction.forward:
                            /* moveDirection = Direction.up;*/
                            break;
                        default:
                            break;
                    }
                }
                else if (moveDirection == Direction.left)
                {
                    //moveDirection = Direction.down;
                    //bodyDirection = Direction.left;
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.forward;
                            break;
                        case Direction.down:
                            moveDirection = Direction.back;
                            break;
                        case Direction.left:
                            /* moveDirection = Direction.down;*/
                            break;
                        case Direction.right:
                            /* moveDirection = Direction.up;*/
                            break;
                        case Direction.back:
                            moveDirection = Direction.up;
                            break;
                        case Direction.forward:
                            moveDirection = Direction.down;
                            break;
                        default:
                            break;
                    }
                }
                else if (moveDirection == Direction.right)
                {
                    //moveDirection = Direction.down;
                    //bodyDirection = Direction.right;
                    switch (bodyDirection)
                    {
                        case Direction.up:
                            moveDirection = Direction.back;
                            break;
                        case Direction.down:
                            moveDirection = Direction.forward;
                            break;
                        case Direction.left:
                            /* moveDirection = Direction.down;*/
                            break;
                        case Direction.right:
                            /* moveDirection = Direction.up;*/
                            break;
                        case Direction.back:
                            moveDirection = Direction.down;
                            break;
                        case Direction.forward:
                            moveDirection = Direction.up;
                            break;
                        default:
                            break;
                    }
                }
                else if (moveDirection == Direction.up)
                {
                    if (bodyDirection == Direction.back)
                    {
                        moveDirection = Direction.right;
                    }
                    else if (bodyDirection == Direction.forward)
                    {
                        moveDirection = Direction.left;
                    }
                    else if (bodyDirection == Direction.left)
                    {
                        moveDirection = Direction.back;
                    }
                    else if (bodyDirection == Direction.right)
                    {
                        moveDirection = Direction.forward;
                    }
                }
                else if (moveDirection == Direction.down)
                {
                    if (bodyDirection == Direction.back)
                    {
                        moveDirection = Direction.left;
                    }
                    else if (bodyDirection == Direction.forward)
                    {
                        moveDirection = Direction.right;
                    }
                    else if (bodyDirection == Direction.left)
                    {
                        moveDirection = Direction.forward;
                    }
                    else if (bodyDirection == Direction.right)
                    {
                        moveDirection = Direction.back;
                    }
                }
                break;
            default:
                break;
        }
        Turn(BasicDirection.none);
    }

    void Turn(BasicDirection d)
    {
        switch (d)
        {
            case BasicDirection.up:
                turnDirection = BasicDirection.up;
                break;
            case BasicDirection.down:
                turnDirection = BasicDirection.down;
                break;
            case BasicDirection.left:
                turnDirection = BasicDirection.left;
                break;
            case BasicDirection.right:
                turnDirection = BasicDirection.right;
                break;
            case BasicDirection.none:
                turnDirection = BasicDirection.none;
                break;
            default:
                break;
        }
    }

    void PutCamera()
    {
        //摄像头跟随 （Camera Follow Core By CG）
        switch (moveDirection)
        {
            case Direction.up:
                mainCamera.transform.position = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                break;
            case Direction.down:
                mainCamera.transform.position = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                break;
            case Direction.left:
                mainCamera.transform.position = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                break;
            case Direction.right:
                mainCamera.transform.position = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                break;
            case Direction.back:
                mainCamera.transform.position = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                break;
            case Direction.forward:
                mainCamera.transform.position = new Vector3(snakeList[snakeHeadPositon].x, snakeList[snakeHeadPositon].y, snakeList[snakeHeadPositon].z);
                break;
            default:
                break;
        }

    }
    void RefreshFood()
    {
        DestroyImmediate(GameObject.Find("food"));
        int x = (int)(Random.Range(0f, 2f) * 10);
        int y = (int)(Random.Range(0f, 2f) * 10);
        int z = (int)(Random.Range(0f, 2f) * 10);
        print("坐标： " + x + ", " + y + ", " + z);
        foreach (var item in snakeList)
        {
            if (item.x == x && item.y == y && item.z == z)
            {
                x = (int)(Random.Range(0f, 2f) * 10);
                y = (int)(Random.Range(0f, 2f) * 10);
                z = (int)(Random.Range(0f, 2f) * 10);
            }
        }
        GameObject food = GameObject.Instantiate(GameObject.Find("Food"));
        food.name = "food";
        food.transform.position = new Vector3(x, y, z);
    }

    bool IsEatingFood()
    {
        Vector3 food = GameObject.Find("food").transform.position;
        switch (moveDirection)
        {
            case Direction.up:
                if (snakeList[snakeHeadPositon].x == food.x && snakeList[snakeHeadPositon].y + 1 == food.y && snakeList[snakeHeadPositon].z == food.z)
                {
                    return true;
                }
                break;
            case Direction.down:
                if (snakeList[snakeHeadPositon].x == food.x && snakeList[snakeHeadPositon].y - 1 == food.y && snakeList[snakeHeadPositon].z == food.z)
                {
                    return true;
                }
                break;
            case Direction.left:
                if (snakeList[snakeHeadPositon].x - 1 == food.x && snakeList[snakeHeadPositon].y == food.y && snakeList[snakeHeadPositon].z == food.z)
                {
                    return true;
                }
                break;
            case Direction.right:
                if (snakeList[snakeHeadPositon].x + 1 == food.x && snakeList[snakeHeadPositon].y == food.y && snakeList[snakeHeadPositon].z == food.z)
                {
                    return true;
                }
                break;
            case Direction.back:
                if (snakeList[snakeHeadPositon].x == food.x && snakeList[snakeHeadPositon].y == food.y && snakeList[snakeHeadPositon].z - 1 == food.z)
                {
                    return true;
                }
                break;
            case Direction.forward:
                if (snakeList[snakeHeadPositon].x == food.x && snakeList[snakeHeadPositon].y == food.y && snakeList[snakeHeadPositon].z + 1 == food.z)
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }
    bool IsHitWall()
    {
        switch (moveDirection)
        {
            case Direction.up:
                if (snakeList[snakeHeadPositon].y >= 19)
                {
                    return true;
                }
                break;
            case Direction.down:
                if (snakeList[snakeHeadPositon].y <= 0)
                {
                    return true;
                }
                break;
            case Direction.left:
                if (snakeList[snakeHeadPositon].x <= 0)
                {
                    return true;
                }
                break;
            case Direction.right:
                if (snakeList[snakeHeadPositon].x >= 19)
                {
                    return true;
                }
                break;
            case Direction.back:
                if (snakeList[snakeHeadPositon].z <= 0)
                {
                    return true;
                }
                break;
            case Direction.forward:
                if (snakeList[snakeHeadPositon].z >= 19)
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    bool IsHitItsef()
    {
        if (snakeHeadPositon > snakeTailPositon)
        {
            for (int i = snakeTailPositon; i < snakeLength; i++)
            {
                if (snakeList[i].x == snakeList[snakeHeadPositon].x && snakeList[i].y == snakeList[snakeHeadPositon].y && snakeList[i].x == snakeList[snakeHeadPositon].z)
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = snakeTailPositon; i < snakeList.Length; i++)
            {
                if (snakeList[i].x == snakeList[snakeHeadPositon].x && snakeList[i].y == snakeList[snakeHeadPositon].y && snakeList[i].x == snakeList[snakeHeadPositon].z)
                {
                    return true;
                }
            }
            for (int i = 0; i < snakeHeadPositon; i++)
            {
                if (snakeList[i].x == snakeList[snakeHeadPositon].x && snakeList[i].y == snakeList[snakeHeadPositon].y && snakeList[i].x == snakeList[snakeHeadPositon].z)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void AddSnakeLength()
    {
        if (snakeLength < snakeList.Length)
        {
            if (snakeTailPositon > 0)
            {
                snakeTailPositon--;

                snakeLength++;
            }
            else if (snakeTailPositon == 0)
            {
                snakeTailPositon = snakeList.Length - 1;

                snakeLength++;
            }
        }
        DestroyImmediate(GameObject.Find("Canvas"));
        ShowScore(Score);
    }
}
