using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeContoroller : MonoBehaviour
{
    //�Ȃ񂩐F�X�Ə����ݒ�
    public Rigidbody2D rigid;
    float speed = 5.0f; //�ړ��X�s�[�h
    int direction = 0; //����(0:��, 1:��, 2:��, 3:��)
    Vector2 vector = new Vector2(0, 0);   //�ړ��̃x�N�g��
    Vector2 goalPos = new Vector2(0, 0);    //�ړ��Ŗڎw�����W
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
            this.isMoving = true;
            if (Input.GetKeyDown(KeyCode.UpArrow)) //��
            {
                this.direction = 0;
                this.goalPos.y += 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))  //��
            {
                this.direction = 1;
                this.goalPos.x += 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))   //��
            {
                this.direction = 2;
                this.goalPos.y -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))   //��
            {
                this.direction = 3;
                this.goalPos.x -= 1;
            }
            else
            {
                this.isMoving = false;
            }

            //���̃t���[������ړ��J�n
            if (this.isMoving = true)
            {
                //���݂͓��ɏ����Ȃ�
            }

        }

        //�ړ���
        if (this.isMoving == true)
        {
            //vx��vy��0�ɂ���
            this.vector = new Vector2(0, 0);

            //x�����̈ړ�
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
            //���ۂ̃x�N�g���ɔ��f
            this.rigid.velocity = vector;
        }
    }
}
