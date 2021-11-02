using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSetter : MonoBehaviour
{

    public GameObject panel;
    public GameObject restartButton; //RESTARTボタン
    public GameObject giveupButton;
    public GameObject returnButton;//ネクストボタン
    // Start is called before the first frame update
    void Start()
    {
        //ボタンを非表示にする
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panel.SetActive(true); //パネルを表示する
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
