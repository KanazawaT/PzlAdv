using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    //�V���O���g���ɂ���
    public static AudioManager instance;

    //�I�[�f�B�I�\�[�X
    public AudioSource seSource;
    public AudioSource bgmSource;

    //�����̔z��
    public AudioClip[] seList;
    public AudioClip[] bgmList;

    //BGM�̃t�F�[�h�C���Ŏg�p
    bool nowFade = false;
    float nowVolume;

    //�V���O���g���Ɋւ���ݒ�
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

    //�w���SE�𗬂�
    public void soundSE(int number)
    {
        seSource.PlayOneShot(seList[number]);
    }

    //�w���BGM�𗬂�
    public void soundBGM(int number)
    {
        bgmSource.clip = bgmList[number];
        bgmSource.Play();

        //BGM�̃t�F�[�h�C���J�n
        nowFade = true;
        nowVolume = 0;
    }


    void Update()
    {
        //���ʂ𑀍�
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
