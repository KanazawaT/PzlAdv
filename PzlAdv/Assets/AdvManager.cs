using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class AdvManager : MonoBehaviour
{
    //�e�L�X�g�n
    public Text mainText;
    public Text nameText;
    public Text titleText;
    public GameObject namePanel;
    public GameObject material;
    //�N���b�N�҂�
    public Image waitMark;

    //��������̑���
    const float WAIT_BC = 0.03f;

    //�I�u�W�F�N�g�̃��X�g
    public GameObject[] objList = new GameObject[10];

    public Sprite test;

    //�y�[�W�ƍs�̃L���[
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
        //��ʃN���b�N
        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    //�e�L�X�g�t�@�C������ǂݍ���
    string Load(string name)
    {
        TextAsset loadedText = Resources.Load<TextAsset>("Story/" + "test");
        return loadedText.text.Replace("\r\n", "~").Replace("\r", "~");
    }

    //�e�L�X�g�𖽗߂��ƂɃL���[�Ɋi�[
    void SplitText(string loadedText)
    {
        string[] splitText = loadedText.Split('~');
        this.pageQ.Clear();
        foreach (string s in splitText)
        {
            //�󕶎��͏���
            if (Equals(s, "") == false)
            {
                this.pageQ.Enqueue(s);
            }
        }
    }

    //��s�̖��߂�ǂݍ��ݑ�����
    void ExecuteSymbol()
    {
        if (this.pageQ.Count <= 0)
        {

            //�I������
            Debug.Log("END");
            return;
        }

        isWaiting = false;
        waitMark.enabled = false;

        //���߂̎�ނ𔻕ʂ��Ď��s
        string sText = this.pageQ.Dequeue();
        string sType = sText.Substring(0, 1);
        sText = sText.Remove(0, 1);

        if (Equals(sType, "&"))
        {
            //�y�[�W����
            NextPage(sText, true);
        }
        else if (Equals(sType, "|"))
        {
            //�e�L�X�g�\��
            NextLine(sText, true);
        }
        else if (Equals(sType, "#"))
        {
            //�^�C�g���\��
            Title(sText, true);
        }
        else if (Equals(sType, "+"))
        {
            //�N���b�N�҂�
            isWaiting = true;
            waitMark.enabled = true;
        }
        else if (Equals(sType, "%"))
        {
            Operation(sText, true);
        }
        else if (Equals(sType, "!"))
        {
            //�I�u�W�F����n
            OpeObj(sText, true);
        }
        

    }

    //���̃y�[�W
    void NextPage(string str, bool con)
    {
        //���O��\��(���O���Ȃ��ꍇ�̓p�l�����B��)
        if (Equals(str, ""))
        {
            namePanel.SetActive(false);
        }
        else
        {
            namePanel.SetActive(true);
            this.nameText.text = str;
        }
        //�e�L�X�g�����Z�b�g
        this.mainText.text = "";
        mainText.fontSize = 30;
        this.titleText.text = "";

        Continue(con);
    }

    //���̃e�L�X�g
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

    //1�������̕\��
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
    //1������\��
    bool ShowChar()
    {
        if (this.lineQ.Count <= 0)
        {
            return false;
        }
        this.mainText.text += lineQ.Dequeue(); ;
        return true;
    }

    //�Ȃ񂩕\�����������
    void Title(string str, bool con)
    {
        NextPage("", false);
        titleText.color = new Color(1, 1, 1, 0);
        this.titleText.text = str;

        StartCoroutine(FadeInText(titleText, con));
    }

    //�ʏ푀��n�̖��ߎ���
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

    //size����
    //%size <�t�H���g�T�C�Y>
    void OpeFont(string[] ope, bool con)
    {
        int size = int.Parse(ope[1]);
        mainText.fontSize = size;
        Continue(con);
    }

    //wait����
    //%wait <����>
    IEnumerator OpeWait(string[] ope, bool con)
    {
        float time = float.Parse(ope[1]);
        yield return new WaitForSeconds(time);
        Continue(con);
    }

    //�I�u�W�F�N�g����n�̖��ߎ���
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

    //pic����
    //!pic ���X�g�ԍ� �Ώ�
    void ObjPic(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        //objList[num] = material.transform.Find(ope[2]).gameObject;

        SpriteRenderer sr = objList[num].GetComponent<SpriteRenderer>();
        Debug.Log("AdvPic/" + ope[2]);
        sr.sprite = Resources.Load<Sprite>("AdvPic/" + ope[2]);
        Continue(con);
    }

    //pos����
    //!pos ���X�g�ԍ� <x> <y>
    void ObjPos(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        float x = float.Parse(ope[2]);
        float y = float.Parse(ope[3]);
        objList[num].transform.position = new Vector2(x, y);

        Continue(con);
    }

    //active����
    //!active ���X�g�ԍ� true/false
    void ObjActive(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        bool type = System.Convert.ToBoolean(ope[2]);
        objList[num].SetActive(type);

        Continue(con);
    }

    //scale����
    //!scale ���X�g�ԍ� <size>
    void ObjScale(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        float scale = float.Parse(ope[2]);
        objList[num].transform.localScale = new Vector2(scale, scale);

        Continue(con);
    }

    //fade����
    //!fade ���X�g�ԍ� <�ړI�l> <����>
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

    //order����
    //!order ���X�g�ԍ� <�ԍ�>
    void ObjOrder(string[] ope, bool con)
    {
        int num = int.Parse(ope[1]);
        int order = int.Parse(ope[2]);
        objList[num].GetComponent<SpriteRenderer>().sortingOrder = order;

        Continue(con);
    }

    //move����
    //!move
    void ObjMove (string[] ope, bool con)
    {

    }

    //�N���b�N���ɓ���
    void Click()
    {

        if (isWaiting)
        {
            ExecuteSymbol();
        }
    }

    //�e�L�X�g�p�̃t�F�[�h�C��
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

    //�����𑱂��邩�ǂ���
    void Continue(bool con)
    {
        if (con)
        {
            ExecuteSymbol();
        }
    }
}
