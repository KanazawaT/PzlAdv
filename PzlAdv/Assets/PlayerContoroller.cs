using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour
{
    //�Ȃ񂩐F�X�Ə����ݒ�
    public Rigidbody2D rigid;
    float speed = 3.0f; //�ړ��X�s�[�h
    int direction = 0; //����(0:��, 1:��, 2:��, 3:��)
    public Vector2 motion;   //�ړ��̃x�N�g��
    Vector2 goalPosF;    //�ړ��Ŗڎw�����ۂ̍��W
    Vector2Int goalPosI;     //�}�X�ڏ�̈ړ�����W
    bool getKey;    //�L�[���͂����������̔���p
    bool isMoving = false;  //�ړ����Ȃ�true�����łȂ����false
    float heightProp = 0.5f;    //�}�X�̏c�̔䗦��ݒ肷��(1.0f�ŉ��Ɠ����A0.5f�Ŕ���)
    bool withRock = false; //��ƈꏏ���ǂ���
    Animator anim;       //�A�j������p

    public GameObject stageManager; //StageManager���Ăяo��
    StageManager stageManagerS; //StageManager�̃X�N���v�g

    // Start is called before the first frame update
    void Start()
    {

        this.stageManagerS = this.stageManager.GetComponent<StageManager>();

        //�����ʒu��ݒ肵�Ĕz�u
        goalPosI = new Vector2Int(3, 3);    //�����ŏ����ʒu��ݒ�
        goalPosF = ChangePosType(goalPosI);
        this.transform.position = goalPosF;

        this.anim = GetComponent<Animator>();   //�A�j������p
    }


    // Update is called once per frame
    void Update()
    {

        //���ɉ��������Ă��Ȃ�
        if (stageManagerS.MovingCount(0) == 0)
        {
            //�㉺���E�̃L�[���͂��󂯂���
            //isMoving��true�ɂ���direction���X�V
            this.getKey = true;
            if (Input.GetKey(KeyCode.UpArrow)) //��
            {
                this.direction = 0;
                anim.SetInteger("direction", 0);    //direction�ύX���ɃA�j���p��direction���ύX
            }
            else if (Input.GetKey(KeyCode.RightArrow))  //��
            {
                this.direction = 1;
                anim.SetInteger("direction", 1);
            }
            else if (Input.GetKey(KeyCode.DownArrow))   //��
            {
                this.direction = 2;
                anim.SetInteger("direction", 2);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))   //��
            {
                this.direction = 3;
                anim.SetInteger("direction", 3);
            }
            else
            {
                this.getKey = false;
            }

            //�ړ��J�n������
            if (this.getKey == true)
            {             
                StartMoving();

            }

        }

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

    //int�^��direction��Vector2�ɕϊ�
    Vector2Int DirectionToVector2(int direction)
    {
        Vector2Int result;

        switch(direction)
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

    //�ړ����J�n���鏈��
    bool StartMoving()
    {
        //�ڕW�̈ړ����newPos�ɐݒ�
        //Debug.Log(newPos);
        Vector2Int newPos = this.goalPosI + DirectionToVector2(this.direction);
        //Debug.Log(newPos);

        //�ړ��悪�ǂ�Ȃ��m���߂�
        int id = stageManagerS.GetTargetId(newPos.x,newPos.y);

        switch (id)
        {
            case 6: //��
            case 2: //��
            case 1: //��
                //���s���̋��ʏ����Ƃ�����΂�����
                return false;
            default: //���̑�
                     ////�ړ��J�n�ɐ���
                     //�X�s�[�h�̒���
                     //�₾�����ꍇ
                if (id == 5)
                {
                    if (isMoving == false)
                    {
                        if (this.stageManagerS.RockMove(newPos, this.direction))
                        {
                            //�₪������
                            this.withRock = true;
                            this.speed = 1.0f;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }                
                else
                {
                    withRock = false;

                    if (UnderIce(goalPosI))
                    {
                        if (withRock)
                        {
                            this.speed = 3.0f;
                        }
                        else
                        {
                            this.speed = 4.0f;
                        }
                    }
                    else
                    {
                        this.speed = 3.0f;
                    }
                }

                //�ړ��p�����ɂ͎��s���Ȃ�
                if (isMoving == false)
                {
                    this.isMoving = true;
                    //movingCount�𑝂₷
                    stageManagerS.MovingCount(1);
                    anim.SetFloat("speed", 1.0f);       //�ړ��n�߂���A�j���[�V����������
                }
                
                this.isMoving = true;                  
                //goalPos���X�V
                this.goalPosI = newPos;
                this.goalPosF = ChangePosType(goalPosI);
                //this.motion.y *= heightProp;  //�c�ړ��̍ۂɈړ����x��ς��鏈��
                this.motion = DirectionToVector2(this.direction);
                this.motion *= speed;
                //motion�ɒl�𔽉f�Arigid���X�V
                this.rigid.velocity = this.motion;               
                return true;
        }
    }

    //�ړ����I�����鏈��
    void FinishMoving()
    {
        this.transform.position = this.goalPosF;
        this.transform.Translate(0, 0, this.transform.position.y*2 - this.transform.position.z);//z���W��y���W��
        this.motion = Vector2.zero;
        this.rigid.velocity = motion;

        isMoving = false;
        //movingCount���ւ炷
        stageManagerS.MovingCount(-1);

        anim.SetFloat("speed", 0.0f);    //�ړ��~�߂���A�j���[�V������~

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

        //�����������Ǐ����X�������ꍇ
        if (result)
        {
            if (UnderIce(goalPosI) && !withRock)
            {
                return !StartMoving();
            }
        }

        return result;
    }

    //�X�̏�ɏ���Ă邩�ǂ���
    bool UnderIce(Vector2Int pos)
    {
        return this.stageManagerS.GetTargetId(pos.x, pos.y) == 3;
    }
}