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

    public GameObject cam; //�J����

    public GameObject floorPrefab;//��
    public GameObject wallPrefab1;//WallPrefab;
    public GameObject wallPrefab2;
    public GameObject stonePrefab;//��̃v���n�u
    public GameObject icePrefab;//�X�̃v���n�u
    public GameObject holePrefab1;//���̃v���n�u
    public GameObject buriedholePrefab1;//�₪���܂������̃v���n�u
    public GameObject goalPrefab;//�S�[���̃v���n�u
    public Sprite buriedhole1;//��̖��܂������̉摜

    PlayerContoroller playerCS; //PlayerController�̃X�N���v�g

    public int movingCount = 0;//�ړ����̂��̂𐔂���

    int maxObjNum;//��������I�u�W�F�N�g�̍ő吔
    int objNum = 0; //�I�u�W�F�N�g�̐ݒ�Ɏg�p
    int stageHeight;
    int stageWidth;
    Vector2Int startPos; //�v���C���[�̃X�^�[�g�n�_
    int[,] Board;//�X�e�[�W�̗l�q���L�^
    OverObject[] terrain;//��Ȃǂ̓����M�~�b�N�͕ʂŋL�^

    int stage;//�X�e�[�W����ۑ�

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

        this.stage = TitleManager.stage;

        SetBoardInf(this.stage);
        SetTerrainInf(this.stage);
        GenerateStage();
        playerCS.SetPos(this.startPos);

        cam.transform.position = new Vector3(this.stageWidth / 2, this.stageHeight / 4, -10);

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetStage();
        }
        */

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
        this.terrain[index].gameObject.SetActive(false);
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
                this.maxObjNum = 4;
                this.stageHeight = 10;
                this.stageWidth = 10;
                this.startPos = new Vector2Int(1, 5);
                this.Board = new int[10, 10]//�}�b�v�������ō��
                {
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 1,2,2,1,2,1,2,2,2,1},
                    { 1,0,0,1,0,2,0,0,0,1},
                    { 1,0,0,1,0,0,0,0,0,1},
                    { 1,0,0,1,0,0,0,0,0,1},
                    { 1,0,0,2,2,0,2,0,0,1},
                    { 1,0,0,0,0,0,0,0,0,1},
                    { 1,0,0,0,0,0,0,0,0,1},
                    { 1,1,1,1,1,4,1,1,1,1},
                    { 2,2,2,2,2,2,2,2,2,2}
                };               
                break;
            case 2:
                this.maxObjNum = 6;
                this.stageHeight = 10;
                this.stageWidth = 10;
                this.startPos = new Vector2Int(8, 7);
                this.Board = new int[10, 10]//�}�b�v�������ō��
                {
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 1,2,2,2,2,2,2,2,2,1},
                    { 1,0,0,0,0,0,0,0,0,1},
                    { 1,0,0,0,2,2,2,1,0,1},
                    { 1,3,3,3,3,3,3,2,3,1},
                    { 1,3,3,3,3,3,3,3,3,1},
                    { 1,1,3,3,3,3,3,3,3,1},
                    { 1,1,3,3,3,3,1,4,1,1},
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 2,2,2,2,2,2,2,2,2,2}
                };
                break;
            case 3:
                this.maxObjNum = 4;
                this.stageHeight = 12;
                this.stageWidth = 10;
                this.startPos = new Vector2Int(3, 3);
                this.Board = new int[12, 10]//�}�b�v�������ō��
                {
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 1,2,2,2,2,1,2,2,2,1},
                    { 1,0,0,0,0,2,3,3,3,1},
                    { 1,0,0,3,3,3,3,3,3,1},
                    { 1,3,3,3,3,3,3,3,3,4},
                    { 1,0,0,3,3,3,3,3,3,1},
                    { 1,0,0,0,3,3,3,3,3,1},
                    { 1,0,0,0,3,3,3,3,3,1},
                    { 1,0,0,0,3,3,3,3,3,1},
                    { 1,3,0,0,1,0,3,3,3,1},
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 2,2,2,2,2,2,2,2,2,2}
                };
                break;
            case 4:
                this.maxObjNum = 12;
                this.stageHeight = 15;
                this.stageWidth = 15;
                this.startPos = new Vector2Int(3, 2);
                this.Board = new int[15, 15]//�}�b�v�������ō��
                {
                    { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    { 1,2,2,2,2,1,1,1,1,1,1,2,2,2,1},
                    { 1,3,3,3,3,1,2,2,1,2,1,0,0,0,1},
                    { 1,3,0,3,3,1,0,0,2,0,2,0,2,0,1},
                    { 1,2,3,3,2,1,0,0,3,3,3,3,3,0,1},
                    { 1,3,3,3,3,2,0,0,2,3,3,3,3,2,1},
                    { 1,3,3,3,3,0,0,0,3,3,3,3,3,3,1},
                    { 1,3,2,3,3,2,2,2,3,3,3,3,3,3,1},
                    { 1,3,3,3,1,0,0,0,2,3,3,3,1,1,1},
                    { 1,3,1,3,2,0,0,0,0,2,0,2,2,2,1},
                    { 1,1,1,0,0,0,0,3,3,0,0,0,0,0,1},
                    { 1,2,2,0,2,0,2,0,0,0,0,0,0,0,1},
                    { 1,4,0,0,0,0,0,0,0,0,0,0,0,0,1},
                    { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}


                };
                break;
                

        }
    }

    //�X�e�[�W��̃I�u�W�F�N�g�̏���ݒ�
    void SetTerrainInf(int stage)
    {
        this.objNum = 0;
        this.terrain = new OverObject[this.maxObjNum];//��Ȃǂ̃I�u�W�F�N�g�𑝂₷���т�maxObjectNum�����������邱��
        for (int i = 0; i < maxObjNum; i++)
        {
            this.terrain[i] = new OverObject();
        }

        switch (stage)
        {
            case 1:
                AddObj(5, 6, 6);
                AddObj(5, 2, 5);
                AddObj(6, 5, 5);
                AddObj(6, 5, 4);
                break;
            case 2:
                AddObj(5, 8, 6);
                AddObj(5, 3, 3);
                AddObj(5, 3, 6);
                AddObj(5, 2, 6);
                AddObj(6, 4, 4);
                AddObj(6, 6, 3);
                break;
            case 3:
                AddObj(5, 3, 5);
                AddObj(5, 7, 3);
                AddObj(6, 8, 4);
                AddObj(6, 1, 4);
                break;
            case 4:
                AddObj(5, 4, 4);
                AddObj(5, 8, 3);
                AddObj(5, 10, 5);
                AddObj(5, 12, 3);
                AddObj(5, 12, 10);
                AddObj(6, 2, 2);
                AddObj(6, 4, 2);
                AddObj(6, 5, 4);
                AddObj(6, 6, 4);
                AddObj(6, 10, 2);
                AddObj(6, 10, 4);
                AddObj(6, 10, 8);
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

        GenerateTerrain();
        
    }

    //�I�u�W�F�𐶐�
    void GenerateTerrain()
    {
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

    //�X�e�[�W�����Z�b�g����
    void ResetStage()
    {
        //�I�u�W�F�����ׂč폜
        for (int i = 0; i < this.maxObjNum; i++)
        {
            Destroy(this.terrain[i].gameObject);
        }
        //terrain���ŏ��̏�Ԃɕ���
        SetTerrainInf(this.stage);
        //�Đ���
        GenerateTerrain();
        //�v���C���[�����Z�b�g
        playerCS.SetPos(this.startPos);
        playerCS.Reset();
        playerCS.ResetDirection();
        //movingCount�̃��Z�b�g
        this.movingCount = 0;
    }

}
