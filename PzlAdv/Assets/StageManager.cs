using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverObject//��������M�~�b�N�p
{
    public GameObject gameObject;
    public int x, y;//GameObject�^�̍��W�n����float�^�Ȃ̂�int�^��ʂŗp��
}

public class StageManager : MonoBehaviour
{
    

    public GameObject wallPrefab;//WallPrefab;
    public GameObject stonePrefab;//��̃v���n�u
    
    //public const int objectNum = 10;//��������I�u�W�F�N�g�̍ő吔

    //const int maxObjcts = 10;�@//
    int[,] Board;//�X�e�[�W�̗l�q���L�^
    OverObject[] terrain;//��Ȃǂ̓����M�~�b�N�͕ʂŋL�^
    //const int stageHeight = 10;
    //const int stageWidth = 10;

    // Start is called before the first frame update
    void Start()
    {
        this.Board = new int[10, 10]//�}�b�v�������ō��
        {
            { 1,1,1,1,1,1,1,1,1,1},
            { 1,0,0,1,0,1,0,0,0,1},
            { 1,0,2,0,0,1,0,0,0,1},
            { 1,0,0,1,0,0,0,0,0,1},
            { 1,1,0,0,0,0,0,1,1,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,1,1,0,1,0,0,0,0,1},
            { 1,0,0,2,0,1,0,0,0,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,1,1,1,1}

        };
        this.terrain = new OverObject[10];
        
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (this.Board[x, y] != 0)
                {
                    GameObject go;

                    switch (this.Board[x, y])
					{
                        case 1:
                            go = Instantiate(this.wallPrefab) as GameObject;
                            go.transform.position = new Vector3(x, y, 0);
                            break;
                        case 2:
                            go = Instantiate(this.stonePrefab) as GameObject;
                            go.transform.position = new Vector3(x, y, 0);
                            break;
                    }
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {


    }

    
    public int GetTargetId(int x, int y)//�w��n�_�ɂ���I�u�W�F�N�g��ԋp����
	{
        
        return this.Board[x, y];

	}
    
    public bool CheckPassing(int x,int y,Vector2 direction)//�w�肵�����W�̒ʍs�ۂ�Ԃ�
	{
        return true;
	}
}
