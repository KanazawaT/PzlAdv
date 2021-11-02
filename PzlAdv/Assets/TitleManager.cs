using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static int stage = -1;

    public GameObject title;
    public GameObject select;

    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(737, 415, false, 60);
        if (stage == -1)
        {
            title.SetActive(true);
        }
        else
        {
            select.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ステージを選択
    public void SetStage(int num)
    {
        stage = num;
    }

    //アドベンチャーシーンへ遷移
    public void StartAdv()
    {
        fade.SetActive(true);
        StartCoroutine(GoAdv());
    }

    IEnumerator GoAdv()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Scenes/AdvScene");
    }

    //ステージ選択を表示
    public void ViewSelectPanel()
    {
        title.SetActive(false);
        select.SetActive(true);
    }
}
