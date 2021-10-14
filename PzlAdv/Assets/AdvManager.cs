using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdvManager : MonoBehaviour
{
    //�e�L�X�g�n
    public Text mainText;
    public Text nameText;
    public Text titleText;
    Animator titleTextA;
    public GameObject namePanel;
    //�N���b�N�҂�
    public Image waitMark;

    const float WAIT_BC = 0.03f;        

    //�y�[�W�ƍs�̃L���[
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
        //��ʃN���b�N
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    //�e�L�X�g�t�@�C������ǂݍ���
    string Load(string name)
    {
        TextAsset loadedText = Resources.Load<TextAsset>("Story/" + "test");
        return loadedText.text.Replace("\r\n", "/").Replace("\r", "/");
    }

    //�e�L�X�g�𖽗߂��ƂɃL���[�Ɋi�[
    void SplitText(string loadedtext)
    {
        string[] splitText = loadedtext.Split('/');
        this.pageQ.Clear();
        foreach (string s in splitText)
        {
            this.pageQ.Enqueue(s);
        }
    }

    //��s�̖��߂�ǂݍ��ݑ�����
    void ExecuteSymbol()
    {
        if (this.pageQ.Count <= 1)
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

    //���y�[�W
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
        this.titleText.text = "";

        if (con)
        {
            ExecuteSymbol();
        }
    }

    //���s
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
        //titleTextA.SetTrigger("fadeIn");
        this.titleText.text = str;

        StartCoroutine(FadeInText(titleText, con));
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
