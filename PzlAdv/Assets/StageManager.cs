using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverObject//��������M�~�b�N�p
{
    public OverObject()
	{
        this.x = 0;
        this.y = 0;
        this.id = 0;
	}
    public GameObject gameObject;
    public int x, y,id;//GameObject�^�̍��W�n����float�^�Ȃ̂�int�^��ʂŗp��.���Ƃ��̃I�u�W�F�N�g��ID
}

public class StageManager : MonoBehaviour
{

    public GameObject floorPrefab;//��
    public GameObject wallPrefab1;//WallPrefab;
    public GameObject wallPrefab2;
    public GameObject stonePrefab;//��̃v���n�u
    
    public const int maxObjNum = 1;//��������I�u�W�F�N�g�̍ő吔
    const int stageHeight = 10;
    const int stageWidth = 10;

    int[,] Board;//�X�e�[�W�̗l�q���L�^
    OverObject[] terrain;//��Ȃǂ̓����M�~�b�N�͕ʂŋL�^
    Vector2Int rockGoalStep = new Vector2Int(); //��̖ڕW�n�_�̃}�X��
    Vector3 rockGoalPrint = new Vector3(); //��̖ڕW�n�_��transform���W

    // Start is called before the first frame update
    void Start()
    {
        this.Board = new int[stageWidth, stageHeight]//�}�b�v�������ō��
        {
            { 2,2,2,2,2,2,2,2,2,2},
            { 1,1,1,1,1,1,1,1,1,1},
            { 1,0,0,0,0,1,0,0,0,1},
            { 1,0,0,0,0,1,0,2,2,1},
            { 1,0,0,0,0,0,0,1,1,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,0,0,0,2,0,0,0,0,1},
            { 1,0,0,0,1,0,0,0,0,1},
            { 1,2,2,2,1,2,2,2,2,1},
            { 1,1,1,1,1,1,1,1,1,1}

        };
        this.terrain = new OverObject[maxObjNum];//��Ȃǂ̃I�u�W�F�N�g�𑝂₷���т�maxObjectNum�����������邱��
        for(int i = 0;i < maxObjNum; i++)
		{
            this.terrain[i] = new OverObject();
		}
        //��̍��W�͈�����
        this.terrain[0].id = 5; //id(����0�`2)��board�̕Ǐ��Ŏg���Ă���̂ŁAid��5����n�߂�
        this.terrain[0].x = 3;
        this.terrain[0].y = 4;

        //�X�e�[�W����
        for (int x = 0; x < stageWidth; x++)
        {
            for (int y = 0; y < stageHeight; y++)
            {
                
                GameObject go;

                switch (this.Board[y, x])
				{
                    case 0:
                        go = Instantiate(this.floorPrefab) as GameObject;
                        go.transform.position = new Vector3(x, y/2.0f-0.25f, 10 + y);
                        break;
                    case 1:
                        go = Instantiate(this.wallPrefab1) as GameObject;
                        go.transform.position = new Vector3(x, y/2.0f - 0.25f, 10 + y);
                        break;
                    case 2:
                        go = Instantiate(this.wallPrefab2) as GameObject;
                        go.transform.position = new Vector3(x, y/2.0f - 0.25f, 10 + y);
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
        //��Ȃǂ̃I�u�W�F�N�g����
        for(int i = 0;i < maxObjNum; i++)
		{
            if (terrain[i].id != 0)
			{
                GameObject go;
				switch(terrain[i].id){
                    case 5:
                        go = Instantiate(this.stonePrefab) as GameObject;
                        go.transform.position = new Vector3(terrain[i].x, terrain[i].y / 2.0f, terrain[i].y);
                        terrain[i].gameObject = go;
                    break;
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

        return this.Board[y, x];

	}
    
    public bool CheckPassing(int x,int y,Vector2 direction)//�w�肵�����W�̒ʍs�ۂ�Ԃ�
	{
        for(int i = 0;i < maxObjNum; i++)//��ɓ���������̂��u���Ă������炻�����D��
		{
            if(this.terrain[i].x == x && this.terrain[i].y == y)
			{
                if(this.terrain[i].id == 10m)
				{
                    return false;
				}
			}
		}
        //�I�u�W�F�N�g����ɏ���ĂȂ���Βn�`�ɂ���Ēʍs�ۂ𔻒f
        if(x >= 0 && x < stageHeight && y >= 0 && y < stageHeight)
		{
            if(this.Board[y,x] == 0)//�ʂ��n�`(�����Ȃ���)�Ȃ�
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

    public bool RockMove(Vector2Int direction, int index)
    {

        //��𓮂������\�b�h
        //��������PlayerController��DirectionToVector2(direction)�ɂ��邱�Ƃ�z��
        //��������GetTargetId�œ���id11�ȏ�̃I�u�W�F�N�g��id


        index -= 5;
        if (index < 0)//id��11�����Ȃ�G���[
        {
            Debug.Log("�G���[:StageManager�N���X��RockMove�̈����ɂ���id��" + index + 11);
            return false;
        }

        rockGoalStep.x = this.terrain[index].x + direction.x;
        rockGoalStep.y = this.terrain[index].y + direction.y;
        Debug.Log("Step(" + rockGoalStep.x + "," + rockGoalStep.y + ")");

        rockGoalPrint.x = (float)rockGoalStep.x;
        rockGoalPrint.y = (float)rockGoalStep.y / 2;

        if (CheckPassing(rockGoalStep.x, rockGoalStep.y, direction) == true)
        {
            terrain[index].gameObject.transform.position = rockGoalPrint;
            this.terrain[index].x = rockGoalStep.x;
            this.terrain[index].y = rockGoalStep.y;
            return true;
        }

        return false;
    }
}
