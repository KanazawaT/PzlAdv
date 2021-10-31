using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    public Rigidbody2D rigid;
    float speed = 3.0f; //�ړ��X�s�[�h
    int direction = 0; //����(0:��, 1:��, 2:��, 3:��)
    public Vector2 motion;   //�ړ��̃x�N�g��
    Vector2 goalPosF;    //�ړ��Ŗڎw�����ۂ̍��W
    Vector2Int goalPosI;     //�}�X�ڏ�̈ړ�����W
    bool isMoving = false;  //�ړ����Ȃ�true�����łȂ����false
    float heightProp = 0.5f;    //�}�X�̏c�̔䗦��ݒ肷��(1.0f�ŉ��Ɠ����A0.5f�Ŕ���)
    public GameObject stageManager; //StageManager���Ăяo��

    // Start is called before the first frame update
    void Start()
    {
        
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
    public bool StartMoving(Vector2Int d)
    {
        //�ڕW�̈ړ����newPos�ɐݒ�
        Vector2Int newPos = goalPosI + d;

        //�ړ��悪�ǂ�Ȃ��m���߂�
        //int type = 0; //GetTargetID(); ���͉���0������Ă܂�



        Debug.Log(newPos.x);

        int type = stageManager.GetComponent<StageManager>().GetTargetId(newPos.x, newPos.y);

        Debug.Log("type=" + type);

        switch (type)
        {




            case 6: //��
                if (true)
                {
                    //�ړ��J�n�Ɏ��s
                }
                else
                {
                    //�ړ��J�n�ɐ���
                }
                break;
            case 2:
            case 1: //��
                //�ړ��J�n�Ɏ��s
                this.isMoving = false;
                return false;
                break;
            default: //���̑�
                //�ړ��J�n�ɐ���
                this.isMoving = true;
                //anim.SetFloat("speed", 1.0f);       //�ړ��n�߂���A�j���[�V����������
                //gx��gy���X�V
                this.goalPosI = newPos;
                //goalPos���X�V
                this.goalPosF = ChangePosType(goalPosI);
                //motion�ɒl�𔽉f�Arigid���X�V
                this.motion = DirectionToVector2(this.direction);
                this.motion *= this.speed;
                //this.motion.y *= heightProp;  //�c�ړ��̍ۂɈړ����x��ς��鏈��
                this.rigid.velocity = this.motion;
                return true;
                break;
        }
        return false;
    }
    //�ړ����I�����鏈��
    void FinishMoving()
    {
        this.transform.position = this.goalPosF;
        this.transform.Translate(0, 0, this.transform.position.y * 2 - this.transform.position.z);//z���W��y���W��
        this.motion = Vector2.zero;
        this.rigid.velocity = motion;

        isMoving = false;
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

        //goalPos�ւ̈ړ��͊������Ă��邪�c
        if (result)
        {
            //�����n�ʂ��X�Ȃ�ړ��p��
            //result = false;
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
}

