using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    public Rigidbody2D rigid;
    float speed = 1.0f; //�ړ��X�s�[�h
    int direction = 0; //����(0:��, 1:��, 2:��, 3:��)
    Vector2 motion;   //�ړ��̃x�N�g��
    Vector2 goalPosF;    //�ړ��Ŗڎw�����ۂ̍��W
    Vector2Int goalPosI;     //�}�X�ڏ�̈ړ�����W
    bool isMoving = false;  //�ړ����Ȃ�true�����łȂ����false
    float heightProp = 0.5f;    //�}�X�̏c�̔䗦��ݒ肷��(1.0f�ŉ��Ɠ����A0.5f�Ŕ���)
    int goalId;    //�ڎw�����W��ID
    StageManager stageManagerS; //StageManager�̃X�N���v�g
    int index; //���̊��terrain��ł̓Y��

    // Start is called before the first frame update
    void Start()
    {
        stageManagerS = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //�ړ���
        if (this.isMoving == true)
        {
            //goalPos�ɓ������Ă������~
            if (CheckMoving())
            {
                FinishMoving();
            }
        }
    }

    //�ړ����J�n���鏈��
    public bool StartMoving(Vector2Int pos, int direction, int index)
    {

        //�ڕW�̈ړ����newPos�ɐݒ�
        this.direction = direction;
        this.goalPosI = pos;
        Vector2Int newPos = goalPosI + DirectionToVector2(direction);

        //�ړ��悪�ǂ�Ȃ��m���߂�
        this.goalId = stageManagerS.GetTargetId(newPos.x, newPos.y);

        switch (this.goalId)
        {
            case 6: //��
            case 0: //��
            case 3: //�X�̏�
                //�ړ��J�n�ɐ���
                this.isMoving = true;
                //movingCount�𑝂₷
                stageManagerS.MovingCount(1);
                //�X�������ꍇ
                if (this.goalId == 3)
                {
                    this.speed = 3.0f;
                }
                else
                {
                    this.speed = 1.5f;
                }
                //goalPos���X�V
                if (goalId == 3)
                {
                    this.goalPosI = IceCheck(newPos, DirectionToVector2(this.direction));                    
                }
                else
                {
                    this.goalPosI = newPos;
                }
                //goalPos���X�V
                this.goalPosF = ChangePosType(goalPosI);
                //motion�ɒl�𔽉f�Arigid���X�V
                this.motion = DirectionToVector2(this.direction);
                this.motion *= this.speed;
                //this.motion.y *= heightProp;  //�c�ړ��̍ۂɈړ����x��ς��鏈��
                this.rigid.velocity = this.motion;
                return true;
            default: //���̑�
                //�ړ��J�n���s
                this.isMoving = false;
                return false;
        }
    }
    //�ړ����I�����鏈��
    void FinishMoving()
    {
        this.transform.position = this.goalPosF;
        this.transform.Translate(0, 0, this.transform.position.y * 2 - this.transform.position.z);//z���W��y���W��
        this.motion = Vector2.zero;
        this.rigid.velocity = motion;

        //���ݒn������������
        if (stageManagerS.GetTargetId(goalPosI.x, goalPosI.y) == 6)
        {
            stageManagerS.FallRock(goalPosI);
            stageManagerS.DestroyRock(index);
        }

        stageManagerS.FinishRockMove(this.goalPosI, this.index);

        isMoving = false;
        //movingCount���ւ炷
        stageManagerS.MovingCount(-1);
    }

    //�ړ����I�����ׂ����`�F�b�N
    bool CheckMoving()
    {
        bool result = false;

        if (this.direction == 0 && this.transform.position.y >= this.goalPosF.y)
        {
            result = true;
        }
        else if (this.direction == 1 && this.transform.position.x >= this.goalPosF.x)
        {
            result = true;
        }
        else if (this.direction == 2 && this.transform.position.y <= this.goalPosF.y)
        {
            result = true;
        }
        else if (this.direction == 3 && this.transform.position.x <= this.goalPosF.x)
        {
            result = true;
        }
        return result;
    }

    //int�^��direction��Vector2�ɕϊ�
    Vector2Int DirectionToVector2(int direction)
    {
        Vector2Int result;

        switch (direction)
        {
            case 0:
                result = Vector2Int.up;
                break;
            case 1:
                result = Vector2Int.right;
                break;
            case 2:
                result = Vector2Int.down;
                break;
            default:
                result = Vector2Int.left;
                break;
        }
        return result;
    }
    //�}�X�ڏ�̍��W�����ۂ̍��W�ɕϊ�
    Vector2 ChangePosType(Vector2Int vectorI)
    {
        return new Vector2(vectorI.x, vectorI.y * heightProp);
    }

    //�����X�̏ꍇ��goalPos�𒲐�
    Vector2Int IceCheck(Vector2Int pos, Vector2Int direction)
    {
        int type = this.stageManagerS.GetTargetId(pos.x + direction.x, pos.y + direction.y);

        if (type == 3)
        {
            return IceCheck(pos + direction, direction);
        }
        else if (type == 5 || type == 1 || type == 2 || type == 6)
        {
            return pos;
        }
        return pos + direction;
    }
}

