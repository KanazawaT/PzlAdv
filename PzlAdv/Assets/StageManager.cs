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
        this.state = 0;
	}
    public GameObject gameObject;
    public int x, y,id,state;//GameObject�^�̍��W�n����float�^�Ȃ̂�int�^��ʂŗp��.���Ƃ��̃I�u�W�F�N�g��ID
}

public class StageManager : MonoBehaviour
{

    public GameObject floorPrefab;//��
    public GameObject wallPrefab1;//WallPrefab;
    public GameObject wallPrefab2;
    public GameObject stonePrefab;//��̃v���n�u
    public GameObject icePrefab;//�X�̃v���n�u
    public GameObject holePrefab1;//���̃v���n�u
    public GameObject buriedholePrefab1;//�₪���܂������̃v���n�u
    public GameObject goalPrefab;
    public Sprite buriedhole1;

    PlayerContoroller playerCS; //PlayerController�̃X�N���v�g

    public int movingCount = 0;//�ړ����̂��̂𐔂���

    int maxObjNum;//��������I�u�W�F�N�g�̍ő吔
    int objNum = 0; //�I�u�W�F�N�g�̐ݒ�Ɏg�p
    int stageHeight;
    int stageWidth;

    int[,] Board;//�X�e�[�W�̗l�q���L�^
    OverObject[] terrain;//��Ȃǂ̓����M�~�b�N�͕ʂŋL�^
    //Vector2Int rockGoalStep = new Vector2Int(); //��̖ڕW�n�_�̃}�X��(���݂�RockMove�֐��ł����g���Ă��Ȃ����A���炩�ɓ������Ƃ���Update�ȂǂŌĂяo�����Ƃ�z�肵�Ă����Ő錾)
    //Vector3 rockGoalPrint = new Vector3(); //��̖ڕW�n�_��transform���W()
    /* �}�b�v�̔ԍ�
    0.��
    1,2.��
    3.�X
    4.�S�[��
    5.��
    6.��
    7��̗�������
    */

    // Start is called before the first frame update
    void Start()
    {
        playerCS = GameObject.Find("Char").GetComponent<PlayerContoroller>();

        SetBoardInf(4);
        SetTerrainInf(4);
        GenerateStage();
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

        return this.Board[stageHeight - y - 1, x];

	}

    public bool CheckPassing(int x,int y,Vector2 direction)//�w�肵�����W�̒ʍs�ۂ�Ԃ�
	{
        for(int i = 0;i < maxObjNum; i++)//��ɓ���������̂��u���Ă������炻�����D��
		{
            if(this.terrain[i].x == x && this.terrain[i].y == y)
			{
                if(this.terrain[i].id == 5)
				{
                    //��Ȃ�false��Ԃ�
                    return false;
				}
                if (this.terrain[i].id == 6)
                {
                    //���Ȃ炱���Ŕ���
                    if (this.terrain[i].state == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
			}
		}
        //�I�u�W�F�N�g����ɏ���ĂȂ���Βn�`�ɂ���Ēʍs�ۂ𔻒f
        if(x >= 0 && x < stageHeight && y >= 0 && y < stageHeight)
		{
            if(this.Board[y,x] == 0 || this.Board[y,x] == 3)//�ʂ��n�`(�����Ȃ���)�Ȃ�
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

    //�w����W�̊�𓮂���
    public bool RockMove(Vector2Int pos, int direction)
    {
        bool result = false;

        for (int i = 0; i < maxObjNum; i++)
        {
            if (this.terrain[i].x == pos.x && this.terrain[i].y == pos.y)
            {
                result = this.terrain[i].gameObject.GetComponent<StoneController>().GetStartMoving(pos, direction, i);
                break;
            }
        }
        return result;
    }

    //��̈ړ�����
    public void ChageTerrainPos(Vector2Int pos, int index)
    {
        terrain[index].x = pos.x;
        terrain[index].y = pos.y;
    }

    //�₪�����܂���
    public void FallRock(Vector2Int pos)
    {
        for (int i = 0; i < maxObjNum; i++)
        {
            if (this.terrain[i].x == pos.x && this.terrain[i].y == pos.y)
            {
                this.terrain[i].gameObject.GetComponent<SpriteRenderer>().sprite = this.buriedhole1;
                this.terrain[i].id = 7;
                break;
            }
        }

    }

    //����������폜����
    public void DestroyRock(int index)
    {
        this.terrain[index].id = -1;
        this.terrain[index].x = -1;
        this.terrain[index].y = -1;
        Destroy(this.terrain[index].gameObject);
    }

    //movingCount�𑀍�
    public int MovingCount(int num)
    {
        this.movingCount += num;
        return this.movingCount;
    }

    //�X�e�[�W����ݒ�
    void SetBoardInf(int stage)
    {
        switch(stage)
        {
            case 1:
                this.maxObjNum = 2;
                this.stageHeight = 9;
                this.stageWidth = 10;
                this.Board = new int[9, 10]//�}�b�v�������ō��
                {
                    { 2,2,2,2,2,2,2,2,2,2},
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 1,0,0,0,0,1,0,0,0,1},
                    { 1,0,0,0,0,0,1,1,1,1},
                    { 1,0,0,3,0,0,1,0,0,1},
                    { 1,0,0,3,3,3,3,0,0,1},
                    { 1,3,0,0,1,0,0,0,0,1},
                    { 1,2,2,2,1,2,2,2,2,1},
                    { 1,1,1,1,1,1,1,1,1,1}
                };               
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                this.maxObjNum = 2;
                this.stageHeight = 11;
                this.stageWidth = 11;
                this.Board = new int[11, 11]//�}�b�v�������ō��
                {
                    { 1,1,1,1,1,1,1,1,1,1,1},
                    { 1,1,1,1,1,1,3,1,1,1,1},
                    { 1,0,0,0,1,3,3,3,1,1,1},
                    { 1,0,1,3,3,3,3,3,1,1,1},
                    { 1,0,0,3,3,3,3,0,1,1,1},
                    { 1,1,1,3,3,3,3,3,0,4,1},
                    { 1,1,0,3,3,3,3,3,1,1,1},
                    { 1,1,1,3,1,3,3,3,3,1,1},
                    { 1,1,0,0,0,0,1,1,1,1,1},
                    { 1,1,0,0,0,0,1,1,1,1,1},
                    { 1,1,1,1,1,1,1,1,1,1,1},
                };
                break;
                

        }
    }

    //�X�e�[�W��̃I�u�W�F�N�g�̏���ݒ�
    void SetTerrainInf(int stage)
    {
        this.terrain = new OverObject[this.maxObjNum];//��Ȃǂ̃I�u�W�F�N�g�𑝂₷���т�maxObjectNum�����������邱��
        for (int i = 0; i < maxObjNum; i++)
        {
            this.terrain[i] = new OverObject();
        }

        switch (stage)
        {
            case 1:
                AddObj(5, 3, 4);
                AddObj(6, 2, 3);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                AddObj(5, 3, 7);
                AddObj(6, 5, 5);
                playerCS.SetPos(3, 1);
                break;

        }
    }
    
    //�X�e�[�W��̃I�u�W�F�N�g��ǉ�
    void AddObj(int id, int x, int y)
    {
        this.terrain[this.objNum].x = x;
        this.terrain[this.objNum].y = y;
        this.terrain[this.objNum].id = id;
        this.objNum++;
    }

    //�X�e�[�W�𐶐�
    void GenerateStage()
    {

        //�X�e�[�W����
        for (int x = 0; x < this.stageWidth; x++)
        {
            for (int y = 0; y < this.stageHeight; y++)
            {

                GameObject go;

                switch (this.Board[stageHeight - y - 1, x])
                {
                    case 0:
                        go = Instantiate(this.floorPrefab) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        break;
                    case 1:
                        go = Instantiate(this.wallPrefab1) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        break;
                    case 2:
                        go = Instantiate(this.wallPrefab2) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        /*this.terrain[objectCount].gameObject = go;
                        this.terrain[objectCount].id = 2;
                        this.terrain[objectCount].x = x;
                        this.terrain[objectCount].y = y;
                        this.Board[x, y] = 0;
                            
                        objectCount++;*/
                        break;
                    case 3:
                        go = Instantiate(this.icePrefab) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        break;
                    case 4:
                        go = Instantiate(this.goalPrefab) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        break;


                }

            }
        }

        //��Ȃǂ̃I�u�W�F�N�g����
        for (int i = 0; i < this.maxObjNum; i++)
        {
            if (terrain[i].id != 0)
            {
                GameObject go;
                switch (terrain[i].id)
                {
                    //�V����id7�̊���o�������ꍇ�A������case7: �Ɛ錾
                    case 5:
                        go = Instantiate(this.stonePrefab) as GameObject;
                        go.transform.position = new Vector3(terrain[i].x, terrain[i].y / 2.0f, terrain[i].y);
                        terrain[i].gameObject = go;
                        break;
                    //�V���ɏo����������id��������case�Ő錾
                    case 6:
                        go = Instantiate(this.holePrefab1) as GameObject;
                        go.transform.position = new Vector3(terrain[i].x, terrain[i].y / 2.0f - 0.25f, terrain[i].y + 10f);
                        terrain[i].gameObject = go;
                        break;
                }
            }

        }
    }

}
