using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdvManager : MonoBehaviour
{
    //テキスト系
    public Text mainText;
    public Text nameText;
    public Text titleText;
    Animator titleTextA;
    public GameObject namePanel;
    //クリック待ち
    public Image waitMark;

    const float WAIT_BC = 0.03f;        

    //ページと行のキュー
    Queue<string> pageQ = new Queue<string>();
    Queue<char> lineQ = new Queue<char>();

    bool isWaiting = true;

    void Start()
    {
        titleTextA = titleText.GetComponent<Animator>();

        SplitText(Load("a"));

        ExecuteSymbol();
    }

    void Update()
    {
        //画面クリック
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    //テキストファイルから読み込み
    string Load(string name)
    {
        TextAsset loadedText = Resources.Load<TextAsset>("Story/" + "test");
        return loadedText.text.Replace("\r\n", "/").Replace("\r", "/");
    }

    //テキストを命令ごとにキューに格納
    void SplitText(string loadedtext)
    {
        string[] splitText = loadedtext.Split('/');
        this.pageQ.Clear();
        foreach (string s in splitText)
        {
            this.pageQ.Enqueue(s);
        }
    }

    //一行の命令を読み込み続ける
    void ExecuteSymbol()
    {
        if (this.pageQ.Count <= 1)
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
            NextPage(sText, true);
        }
        else if (Equals(sType, "|"))
        {
            NextLine(sText, true);
        }
        else if (Equals(sType, "#"))
        {
            Title(sText, true);
        }
        else if (Equals(sType, "+"))
        {
            isWaiting = true;
            waitMark.enabled = true;
        }
        

    }

    //改ページ
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
        this.titleText.text = "";

        if (con)
        {
            ExecuteSymbol();
        }
    }

    //改行
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
        //titleTextA.SetTrigger("fadeIn");
        this.titleText.text = str;

        StartCoroutine(FadeInText(titleText, con));
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
        if (con)
        {
            ExecuteSymbol();
        }
    }

    /*IEnumerator FadeIn(GameObject obj, float time, bool con)
    {
        float spTime = time / 10;
        float a = 0;

        while (a <= 1)
        {
            a += 0.05f;
            Debug.Log(a);
            obj.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.05f);
        }
        
        if (con)
        {
            ExecuteSymbol();
        }
    }
    */
}
