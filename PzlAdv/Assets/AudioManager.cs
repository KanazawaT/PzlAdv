using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    //シングルトンにする
    public static AudioManager instance;

    //オーディオソース
    public AudioSource seSource;
    public AudioSource bgmSource;

    //音源の配列
    public AudioClip[] seList;
    public AudioClip[] bgmList;

    //BGMのフェードインで使用
    bool nowFade = false;
    float nowVolume;

    //シングルトンに関する設定
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    //指定のSEを流す
    public void soundSE(int number)
    {
        seSource.PlayOneShot(seList[number]);
    }

    //指定のBGMを流す
    public void soundBGM(int number)
    {
        bgmSource.clip = bgmList[number];
        bgmSource.Play();

        //BGMのフェードイン開始
        nowFade = true;
        nowVolume = 0;
    }


    void Update()
    {
        //音量を操作
        if (nowFade)
        {
            nowVolume += Time.deltaTime / 2;
            bgmSource.volume = nowVolume;
            if (nowVolume >= 1)
            {
                bgmSource.volume = 1;
                nowFade = false;
            }
        }
    }


}
