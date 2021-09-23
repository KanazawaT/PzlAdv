using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeContoroller : MonoBehaviour
{
    //�Ȃ񂩐F�X�Ə����ݒ�
    public Rigidbody2D rigid;
    float speed = 5.0f;
    int ax, ay; //�ړ�����
    float vx = 0, vy = 0;   //�x�N�g���̌���
    public int gx = 0, gy = 0;     //�ڎw�����W
    public bool isMoving = false;  //�ړ����Ȃ�true�����łȂ����false

    // Start is called before the first frame update
    void Start()
    {
        //this.rigid = this.GetComponent<Rigidbody2D>();

        //�K���ɃX�^�[�g�n�_���߂�
        this.gx = 0;
        this.gy = 0;
        this.transform.position = new Vector2(this.gx, this.gy);
    }


    // Update is called once per frame
    void Update()
    {

        //�ړ����ł͂Ȃ��̂ŃL�[���͑҂�
        if (this.isMoving == false)
        {
            //ax��ay��0�ɂ���
            this.ax = 0;
            this.ay = 0;

            //�㉺���E�̃L�[���͂��󂯂�
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.ax = -1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.ax = 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                this.ay = -1;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                this.ay = 1;
            }

            //�ړ����邱�ƂɂȂ���
            if (this.ax != 0 || this.ay != 0)
            {
                this.isMoving = true;
                this.gx += this.ax;
                this.gy += this.ay;
            }

        }

        //isMoving��true�Ȃ̂ňړ�������
        if (this.isMoving == true)
        {
            //vx��vy��0�ɂ���
            this.vx = 0;
            this.vy = 0;

            //x�����̈ړ�
            if (this.ax != 0)
            {
                if (this.transform.position.x < this.gx - 0.1)
                {
                    this.vx = speed;
                }
                else if (this.transform.position.x > this.gx + 0.1)
                {
                    this.vx = -speed;
                }
                else
                {
                    this.transform.position = new Vector2(this.gx, this.transform.position.y);
                    this.isMoving = false;
                }
            }
            //y�����̈ړ�
            else
            {
                if (this.transform.position.y < this.gy - 0.1)
                {
                    this.vy = speed;
                }
                else if (this.transform.position.y > this.gy + 0.1)
                {
                    this.vy = -speed;
                }
                else
                {
                    this.transform.position = new Vector2(this.transform.position.x, this.gy);
                    this.isMoving = false;
                }
            }
            //�x�N�g���ɔ��f
            this.rigid.velocity = new Vector2(this.vx, this.vy);
        }
    }
}
