using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverObject//��������M�~�b�N�p
{
    public GameObject gameObject;
    public int x, y,id;//GameObject�^�̍��W�n����float�^�Ȃ̂�int�^��ʂŗp��.���Ƃ��̃I�u�W�F�N�g��ID
}

public class StageManager : MonoBehaviour
{
    

    public GameObject wallPrefab;//WallPrefab;
    public GameObject stonePrefab;//��̃v���n�u
    
    public const int maxObjNum = 10;//��������I�u�W�F�N�g�̍ő吔
    const int stageHeight = 10;
    const int stageWidth = 10;

    int[,] Board;//�X�e�[�W�̗l�q���L�^
    OverObject[] terrain;//��Ȃǂ̓����M�~�b�N�͕ʂŋL�^
    
    // Start is called before the first frame update
    void Start()
    {
        this.Board = new int[stageWidth, stageHeight]//�}�b�v�������ō��
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
        this.terrain = new OverObject[maxObjNum];
        int objectCount = 0;//�ǂݍ��񂾓�������I�u�W�F�N�g���𐔂���Ǐ��ϐ�
        for (int x = 0; x < stageWidth; x++)
        {
            for (int y = 0; y < stageHeight; y++)
            {
                if (this.Board[x, y] != 0)
                {
                    GameObject go;

                    switch (this.Board[x, y])
					{
                        case 1:
                            go = Instantiate(this.wallPrefab) as GameObject;
                            go.transform.position = new Vector3(x, y, 10);
                            break;
                        case 2:
                            go = Instantiate(this.stonePrefab) as GameObject;
                            go.transform.position = new Vector3(x, y, 10);
                            /*this.terrain[objectCount].gameObject = go;
                            this.terrain[objectCount].id = 2;
                            this.terrain[objectCount].x = x;
                            this.terrain[objectCount].y = y;
                            this.Board[x, y] = 0;
                            
                            objectCount++;*/
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

    
    public int GetTargetId(int x, int y)//�w��n�_�ɂ���I�u�W�F�N�g��ԋp�����Ɋⓙ������Ă��ꍇ��������Ԃ�
	{
        for(int i = 0;i < maxObjNum;i++)
		{
            if(this.terrain[i].x == x && this.terrain[i].y == y)
			{
                return this.terrain[i].id;
			}

        }

        return this.Board[x, y];

	}
    
    public bool CheckPassing(int x,int y,Vector2 direction)//�w�肵�����W�̒ʍs�ۂ�Ԃ�
	{
        for(int i = 0;i < maxObjNum; i++)//��ɓ���������̂��u���Ă������炻�����D��
		{
            if(this.terrain[i].x == x && this.terrain[i].y == y)
			{
                if(this.terrain[i].id == 2)
				{
                    return false;
				}
			}
		}
        //�I�u�W�F�N�g����ɏ���ĂȂ���Βn�`�ɂ���Ēʍs�ۂ𔻒f
        if(x >= 0 && x < stageHeight && y >= 0 && y < stageHeight)
		{
            if(this.Board[x,y] == 0)//�ʂ��n�`(�����Ȃ���)�Ȃ�
		    {
                return true;
			}
			else//�����łȂ�(��)�Ȃ�
			{
                return false;
			}
		}
        
        else//�z��͈̔͊O�Ȃ�
		{
            return false;
		}
	}
}
