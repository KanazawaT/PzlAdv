using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //�V�[���̐؂�ւ��ɕK�v

public class ChangeScene : MonoBehaviour //�V�[���؂�ւ��̂��߂̔ėp�X�N���v�g
{
    public string sceneName; //�ǂݍ��ރV�[����
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
