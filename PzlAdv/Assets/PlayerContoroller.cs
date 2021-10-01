using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour
{
    //�Ȃ񂩐F�X�Ə����ݒ�
    public Rigidbody2D rigid;
    float speed = 5.0f; //�ړ��X�s�[�h
    int direction = 0; //����(0:��, 1:��, 2:��, 3:��)
    Vector2 vector = new Vector2(0, 0);   //�ړ��̃x�N�g��
    Vector2 goalPos = new Vector2(0, 0);    //�ړ��Ŗڎw�����W
    bool getKey;    //�L�[���͂����������̔���p
    bool isMoving = false;  //�ړ����Ȃ�true�����łȂ����false

    // Start is called before the first frame update
    void Start()
    {
        //�����ʒu�ֈړ�
        //�������ύX���������goalPos�̒l��M��
        this.transform.position = goalPos;
    }


    // Update is called once per frame
    void Update()
    {

        //�ړ����ł͂Ȃ�
        if (this.isMoving == false)
        {
            //�㉺���E�̃L�[���͂��󂯂���
            //isMoving��true�ɂ���direction���X�V
            this.getKey = true;
            if (Input.GetKeyDown(KeyCode.UpArrow)) //��
            {
                this.direction = 0;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))  //��
            {
                this.direction = 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))   //��
            {
                this.direction = 2;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))   //��
            {
                this.direction = 3;
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

            /*//x�����̈ړ�
            if (this.direction % 2 == 1)
            {
                if (this.transform.position.x < this.goalPos.x - 0.1)
                {
                    this.vector.x = speed;
                }
                else if (this.transform.position.x > this.goalPos.x + 0.1)
                {
                    this.vector.x = -speed;
                }
                else
                {
                    this.transform.position = new Vector2(this.goalPos.x, this.transform.position.y);
                    this.isMoving = false;
                }
            }
            //y�����̈ړ�
            else
            {
                if (this.transform.position.y < this.goalPos.y - 0.1)
                {
                    this.vector.y = speed;
                }
                else if (this.transform.position.y > this.goalPos.y + 0.1)
                {
                    this.vector.y = -speed;
                }
                else
                {
                    this.transform.position = new Vector2(this.transform.position.x, this.goalPos.y);
                    this.isMoving = false;
                }
            }
            */
        }
    }

    //int�^��direction��Vector2�ɕϊ�
    Vector2 DirectionToVector2(int direction)
    {
        Vector2 result = new Vector2(0, 0);

        switch(direction)
        {
            case 0:
                result.y = 1;
                break;
            case 1:
                result.x = 1;
                break;
            case 2:
                result.y = -1;
                break;
            case 3:
                result.x = -1;
                break;
        }
        return result;
    }

    //�ړ����J�n���鏈��
    void StartMoving()
    {
        //�܂������Ă����1�}�X���goalPos�ɐݒ�
        int type = 0; //GetTargetID(this.goalPos + DirectionToVector2(direction));
        if (type == 2)   //�₾������
        {
            //��������֐����Ăяo��
        }
        else
        {
            //goalPos���X�V
            this.goalPos = this.goalPos + DirectionToVector2(direction);
        }

        this.vector = DirectionToVector2(this.direction);
        this.vector *= this.speed;

        this.rigid.velocity = vector;

        isMoving = true;
    }

    //�ړ����I�����鏈��
    void FinishMoving()
    {
        this.transform.position = this.goalPos;
        this.vector = new Vector2(0, 0);
        this.rigid.velocity = vector;

        isMoving = false;
    }

    //�ړ����I�����ׂ����`�F�b�N
    bool CheckMoving()
    {
        bool result = false;

        if (this.direction == 0 && this.transform.position.y >= this.goalPos.y)
        {
            result = true;
        }
        else if (this.direction == 1 && this.transform.position.x >= this.goalPos.x)
        {
            result = true;
        }
        else if (this.direction == 2 && this.transform.position.y <= this.goalPos.y)
        {
            result = true;
        }
        else if (this.direction == 3 && this.transform.position.x <= this.goalPos.x)
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
}