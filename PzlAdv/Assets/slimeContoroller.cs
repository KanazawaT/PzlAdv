using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeContoroller : MonoBehaviour
{
    //なんか色々と初期設定
    public Rigidbody2D rigid;
    float speed = 5.0f;
    int ax, ay; //移動方向
    float vx = 0, vy = 0;   //ベクトルの向き
    public int gx = 0, gy = 0;     //目指す座標
    public bool isMoving = false;  //移動中ならtrueそうでなければfalse

    // Start is called before the first frame update
    void Start()
    {
        //this.rigid = this.GetComponent<Rigidbody2D>();

        //適当にスタート地点決める
        this.gx = 0;
        this.gy = 0;
        this.transform.position = new Vector2(this.gx, this.gy);
    }


    // Update is called once per frame
    void Update()
    {

        //移動中ではないのでキー入力待ち
        if (this.isMoving == false)
        {
            //axとayを0にする
            this.ax = 0;
            this.ay = 0;

            //上下左右のキー入力を受けた
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

            //移動することになった
            if (this.ax != 0 || this.ay != 0)
            {
                this.isMoving = true;
                this.gx += this.ax;
                this.gy += this.ay;
            }

        }

        //isMovingがtrueなので移動させる
        if (this.isMoving == true)
        {
            //vxとvyを0にする
            this.vx = 0;
            this.vy = 0;

            //x方向の移動
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
            //y方向の移動
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
            //ベクトルに反映
            this.rigid.velocity = new Vector2(this.vx, this.vy);
        }
    }
}
