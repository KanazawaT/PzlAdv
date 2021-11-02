using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSetter : MonoBehaviour
{

    public GameObject panel;
    public GameObject restartButton; //RESTART�{�^��
    public GameObject giveupButton;
    public GameObject returnButton;//�l�N�X�g�{�^��
    // Start is called before the first frame update
    void Start()
    {
        //�{�^�����\���ɂ���
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panel.SetActive(true); //�p�l����\������
        }
        else
        {

        }
    }
    public void PanelDelete()
    {
        panel.SetActive(false);
    }
}
