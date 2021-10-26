using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class AdvManager : MonoBehaviour
{
    //テキスト系
    public Text mainText;
    public Text nameText;
    public Text titleText;
    public GameObject namePanel;
    public GameObject material;
    //クリック待ち
    public Image waitMark;

    //文字送りの速さ
    const float WAIT_BC = 0.03f;

    //オブジェクトのリスト
    public GameObject[] objList = new GameObject[10];

    public Sprite test;

    //ページと行のキュー
    Queue<string> pageQ = new Queue<string>();
    Queue<char> lineQ = new Queue<char>();

    public bool isWaiting = true;

    void Start()
    {
        SplitText(Load("a"));

        ExecuteSymbol();
    }

    void Update()
    {
        //画面クリック
        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    //テキストファイルから読み込み
    string Load(string name)
    {
        TextAsset loadedText = Resources.Load<TextAsset>("Story/" + "test");
        return loadedText.text.Replace("\r\n", "~").Replace("\r", "~");
    }

    //テキストを命令ごとにキューに格納
    void SplitText(string loadedText)
    {
        string[] splitText = loadedText.Split('~');
        this.pageQ.Clear();
        foreach (string s in splitText)
        {
            //空文字は除く
            if (Equals(s, "") == false)
            {
                this.pageQ.Enqueue(s);
            }
        }
    }

    //一行の命令を読み込み続ける
    void ExecuteSymbol()
    {
        if (this.pageQ.Count <= 0)
        {

            //終了処理
            Debug.Log("END");
            return;
        }

        isWaiting = false;
        waitMark.enabled = false;

        //命令の種類を判別して実行
        string sText = this.pageQ.Dequeue();
        string sType = sText.Substring(0, 1);
        sText = sText.Remove(0, 1);

        if (Equals(sType, "&"))
        {
            //ページ送り
            NextPage(sText, true);
        }
        else if (Equals(sType, "|"))
        {
            //テキスト表示
            NextLine(sText, true);
        }
        else if (Equals(sType, "#"))
        {
            //タイトル表示
            Title(sText, true);
        }
        else if (Equals(sType, "+"))
        {
            //クリック待ち
            isWaiting = true;
            waitMark.enabled = true;
        }
        else if (Equals(sType, "%"))
        {
            Operation(sText, true);
        }
        else if (Equals(sType, "!"))
        {
            //オブジェ操作系
            OpeObj(sText, true);
        }
        

    }

    //次のページ
    void NextPage(string str, bool con)
    {
        //名前を表示(名前がない場合はパネルを隠す)
        if (Equals(str, ""))
        {
            namePanel.SetActive(false);
        }
        else
        {
            namePanel.SetActive(true);
            this.nameText.text = str;
        }
        //テキストをリセット
        this.mainText.text = "";
        mainText.fontSize = 30;
        this.titleText.text = "";

        Continue(con);
    }

    //次のテキスト
    void NextLine(string str, bool con)
    {
        char[] charA = str.ToCharArray();
        this.lineQ.Clear();
        foreach(char c in charA)
        {
            this.lineQ.Enqueue(c);
        }
        StartCoroutine(ShowLine(con));
    }

    //1文字ずつの表示
    IEnumerator ShowLine(bool con)
    {
        ShowChar();
        while (ShowChar())
        {
            yield return new WaitForSeconds(WAIT_BC);           
        }
        this.mainText.text += "\n";
        if (con)
        {
            ExecuteSymbol();
        }
    }
    //1文字を表示
    bool ShowChar()
    {
        if (this.lineQ.Count <= 0)
        {
            return false;
        }
        this.mainText.text += lineQ.Dequeue(); ;
        return true;
    }

    //なんか表示したいやつ
    void Title(string str, bool con)
    {
        NextPage("", false);
        titleText.color = new Color(1, 1, 1, 0);
        this.titleText.text = str;

        StartCoroutine(FadeInText(titleText, con));
    }

    //通常操作系の命令識別
    void Operation(string str, bool con)
    {
        string[] ope = str.Split(' ');
        if (Equals(ope[0], "size"))
        {
            OpeFont(ope, con);
        }
        else if (Equals(ope[0], "wait"))
        {
            StartCoroutine(OpeWait(ope, con));
        }
    }

    //size命令
    //%size <フォントサイズ>
    void OpeFont(string[] ope, bool con)
    {
        int size = int.Parse(ope[1]);
        mainText.fontSize = size;
        Continue(con);
    }

    //wait命令
    //%wait <時間>
    IEnumerator OpeWait(string[] ope, bool con)
    {
        float time = float.Parse(ope[1]);
        yield return new WaitForSeconds(time);
        Continue(con);
    }

    //オブジェクト操作系の命令識別
    void OpeObj(string str, bool con)
    {
        string[] ope = str.Split(' ');
        if (Equals(ope[0], "pic"))
        {
            ObjPic(ope, con);
        }
        else if (Equals(ope[0], "pos"))
        {
            ObjPos(ope, con);
        }
        else if (Equals(ope[0], "active"))
        {
            ObjActive(ope, con);
        }
        else if (Equals(ope[0], "scale"))
        {
            ObjScale(ope, con);
        }
        else if (Equals(ope[0], "fade"))
        {
            StartCoroutine(ObjFade(ope, con));
        }
        else if (Equals(ope[0], "order"))
        {
            ObjOrder(ope, con);
        }
        
       
    }

    //pic命令
    //!pic リスト番号 対象
    void ObjPic(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        //objList[num] = material.transform.Find(ope[2]).gameObject;

        SpriteRenderer sr = objList[num].GetComponent<SpriteRenderer>();
        Debug.Log("AdvPic/" + ope[2]);
        sr.sprite = Resources.Load<Sprite>("AdvPic/" + ope[2]);
        Continue(con);
    }

    //pos命令
    //!pos リスト番号 <x> <y>
    void ObjPos(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        float x = float.Parse(ope[2]);
        float y = float.Parse(ope[3]);
        objList[num].transform.position = new Vector2(x, y);

        Continue(con);
    }

    //active命令
    //!active リスト番号 true/false
    void ObjActive(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        bool type = System.Convert.ToBoolean(ope[2]);
        objList[num].SetActive(type);

        Continue(con);
    }

    //scale命令
    //!scale リスト番号 <size>
    void ObjScale(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        float scale = float.Parse(ope[2]);
        objList[num].transform.localScale = new Vector2(scale, scale);

        Continue(con);
    }

    //fade命令
    //!fade リスト番号 <目的値> <時間>
    IEnumerator ObjFade(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        float lastA = float.Parse(ope[2]);
        SpriteRenderer sr = objList[num].GetComponent<SpriteRenderer>();
        float nowA = sr.color.a;
        float time = float.Parse(ope[3]);

        if (time != 0)
        {
            float change = (lastA - nowA) / (20 * time);

            while (time > 0)
            {
                nowA += change;
                sr.color = new Color(1, 1, 1, nowA);
                time -= 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        sr.color = new Color(1, 1, 1, lastA);

        Continue(con);
    }

    //order命令
    //!order リスト番号 <番号>
    void ObjOrder(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        int order = int.Parse(ope[2]);
        objList[num].GetComponent<SpriteRenderer>().sortingOrder = order;

        Continue(con);
    }

    //move命令
    //!move
    void ObjMove (string[] ope, bool con)
    {

    }

    //クリック時に動作
    void Click()
    {

        if (isWaiting)
        {
            ExecuteSymbol();
        }
    }

    //テキスト用のフェードイン
    IEnumerator FadeInText(Text text, bool con)
    {
        float a = 0;
        while (a <= 1)
        {
            a += 0.03f;
            text.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.05f);
        }
        Continue(con);
    }   

    //処理を続けるかどうか
    void Continue(bool con)
    {
        if (con)
        {
            ExecuteSymbol();
        }
    }
}
