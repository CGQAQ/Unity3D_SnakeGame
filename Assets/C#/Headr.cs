using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public partial class SnakeGame : MonoBehaviour
{
    enum Direction
    {
        up,
        down,
        left,
        right,
        back,
        forward
    }
    enum BasicDirection
    {
        up,
        down,
        left,
        right,
        none
    }

    enum GameStatus
    {
        gaming,
        over,
        pause
    }

    void ShowDialog(string text, string button1, UnityEngine.Events.UnityAction ok, string button2 = null, UnityEngine.Events.UnityAction cancel = null)
    {
        GameObject C = new GameObject("Canvas");
        C.AddComponent<Canvas>();
        C.AddComponent<CanvasScaler>();
        C.AddComponent<GraphicRaycaster>();
        C.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        C.AddComponent<Image>();
        C.GetComponent<Image>().color = new Color32(255, 255, 255, 120);


        GameObject T = new GameObject("Text");
        T.AddComponent<Text>();
        T.GetComponent<Text>().text = text;
        T.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("宋体", 15);
        T.GetComponent<Text>().fontSize = 30;
        T.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        T.GetComponent<Text>().color = new Color32(0, 0, 0, 255);
        T.transform.SetParent(C.transform);
        T.GetComponent<Text>().rectTransform.localPosition = new Vector3(0, 0, 0);
        T.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(1000, 500);

        if (button2 == null)
        {
            GameObject B1 = new GameObject("Button1");
            B1.AddComponent<Button>();
            B1.AddComponent<Image>();
            B1.GetComponent<Image>().color = new Color32(86, 176, 97, 255);
            //B1.GetComponent<Image>().overrideSprite = Resources.GetBuiltinResource<Sprite>(@"E:\Work\Unity\SnakeGame\hello._Data\Resources\unity_builtin_extra");
            //B1.AddComponent<RectTransform>();
            //B1.GetComponent<Button>().text = "一二三三二一";
            //B1.GetComponent<Button>().font = Font.CreateDynamicFontFromOSFont("宋体", 15);
            B1.transform.SetParent(C.transform);
            B1.GetComponent<RectTransform>().localPosition = new Vector3(0, -200, 0);
            B1.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 40);
            B1.GetComponent<Button>().targetGraphic = B1.GetComponent<Image>();
            B1.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
            ColorBlock cb = new ColorBlock();
            cb.normalColor = new Color32(255, 255, 255, 255);
            cb.highlightedColor = new Color32(222, 222, 222, 255);
            cb.pressedColor = new Color32(180, 180, 180, 255);
            cb.disabledColor = new Color32(150, 150, 150, 255);
            cb.fadeDuration = 0.1f;
            cb.colorMultiplier = 1;
            B1.GetComponent<Button>().colors = cb;


            GameObject BT1 = new GameObject("BT1");
            BT1.AddComponent<Text>();
            //BT1.AddComponent<RectTransform>();
            BT1.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("宋体", 15);
            BT1.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            BT1.GetComponent<Text>().text = button1;
            BT1.transform.SetParent(B1.transform);
            BT1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            BT1.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            BT1.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            BT1.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

            //BT1.GetComponent<RectTransform>().localPosition = B1.GetComponent<RectTransform>().localPosition;


            //B1.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            /*B1. = DialogCancelButtonCallBack();*/
            B1.GetComponent<Button>().onClick.AddListener(ok);
        }
        else
        {
            GameObject B1 = new GameObject("Button1");
            B1.AddComponent<Button>();
            B1.AddComponent<Image>();
            B1.GetComponent<Image>().color = new Color32(86, 176, 97, 255);
            //B1.GetComponent<Image>().overrideSprite = Resources.GetBuiltinResource<Sprite>(@"E:\Work\Unity\SnakeGame\hello._Data\Resources\unity_builtin_extra");
            //B1.AddComponent<RectTransform>();
            //B1.GetComponent<Button>().text = "一二三三二一";
            //B1.GetComponent<Button>().font = Font.CreateDynamicFontFromOSFont("宋体", 15);
            B1.transform.SetParent(C.transform);
            B1.GetComponent<RectTransform>().localPosition = new Vector3(-150, -200, 0);
            B1.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 40);
            B1.GetComponent<Button>().targetGraphic = B1.GetComponent<Image>();
            B1.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
            ColorBlock cb1 = new ColorBlock();
            cb1.normalColor = new Color32(255, 255, 255, 255);
            cb1.highlightedColor = new Color32(222, 222, 222, 255);
            cb1.pressedColor = new Color32(180, 180, 180, 255);
            cb1.disabledColor = new Color32(150, 150, 150, 255);
            cb1.fadeDuration = 0.1f;
            cb1.colorMultiplier = 1;
            B1.GetComponent<Button>().colors = cb1;
            B1.GetComponent<Button>().onClick.AddListener(ok);


            GameObject BT1 = new GameObject("BT1");
            BT1.AddComponent<Text>();
            //BT1.AddComponent<RectTransform>();
            BT1.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("宋体", 15);
            BT1.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            BT1.GetComponent<Text>().text = button1;
            BT1.transform.SetParent(B1.transform);
            BT1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            BT1.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            BT1.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            BT1.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);




            GameObject B2 = new GameObject("Button2");
            B2.AddComponent<Button>();
            B2.AddComponent<Image>();
            B2.GetComponent<Image>().color = new Color32(86, 176, 97, 255);
            //B1.GetComponent<Image>().overrideSprite = Resources.GetBuiltinResource<Sprite>(@"E:\Work\Unity\SnakeGame\hello._Data\Resources\unity_builtin_extra");
            //B2.AddComponent<RectTransform>();
            //B1.GetComponent<Button>().text = "一二三三二一";
            //B1.GetComponent<Button>().font = Font.CreateDynamicFontFromOSFont("宋体", 15);
            B2.transform.SetParent(C.transform);
            B2.GetComponent<RectTransform>().localPosition = new Vector3(150, -200, 0);
            B2.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 40);
            B2.GetComponent<Button>().targetGraphic = B2.GetComponent<Image>();
            B2.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
            ColorBlock cb2 = new ColorBlock();
            cb2.normalColor = new Color32(255, 255, 255, 255);
            cb2.highlightedColor = new Color32(222, 222, 222, 255);
            cb2.pressedColor = new Color32(180, 180, 180, 255);
            cb2.disabledColor = new Color32(150, 150, 150, 255);
            cb2.fadeDuration = 0.1f;
            cb2.colorMultiplier = 1;
            B2.GetComponent<Button>().colors = cb2;


            GameObject BT2 = new GameObject("BT2");
            BT2.AddComponent<Text>();
            //BT1.AddComponent<RectTransform>();
            BT2.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("宋体", 15);
            BT2.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            BT2.GetComponent<Text>().text = button2;
            BT2.transform.SetParent(B2.transform);
            BT2.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            BT2.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            BT2.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            BT2.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            B2.GetComponent<Button>().onClick.AddListener(cancel);

        }

        GameObject es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();

    }

    void DestroyDialog()
    {
        DestroyImmediate(GameObject.Find("Canvas"));
        DestroyImmediate(GameObject.Find("EventSystem"));
    }
    void ShowScore(int score)
    {
        GameObject C = new GameObject("Canvas");
        C.AddComponent<Canvas>();
        C.AddComponent<CanvasScaler>();
        C.AddComponent<GraphicRaycaster>();
        C.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        //C.AddComponent<Image>();
        //C.GetComponent<Image>().color = new Color32(255, 255, 255, 120);


        GameObject T = new GameObject("Text");
        T.AddComponent<Text>();
        T.GetComponent<Text>().text = "分数： " + score;
        T.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("宋体", 15);
        T.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        T.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        T.GetComponent<Text>().fontSize = 30;
        T.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        T.GetComponent<Text>().color = new Color32(0, 0, 0, 255);
        T.transform.SetParent(C.transform);
        T.GetComponent<Text>().rectTransform.position = new Vector3(100, 50, 0);
        T.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(200, 100);
    }
}