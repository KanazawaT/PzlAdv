using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeContoroller : MonoBehaviour
{
    //なんか色々と初期設定
    public Rigidbody2D rigid;
    float speed = 5.0f; //移動スピード
    int direction = 0; //向き(0:↑, 1:→, 2:↓, 3:←)
    Vector2 vector = new Vector2(0, 0);   //移動のベクトル
    Vector2 goalPos = new Vector2(0, 0);    //移動で目指す座標
    bool isMoving = false;  //移動中ならtrueそうでなければfalse

    // Start is called before the first frame update
    void Start()
    {
        //初期位置へ移動
        //↑これを変更したければgoalPosの値を弄る
        this.transform.position = goalPos;
    }


    // Update is called once per frame
    void Update()
    {

        //移動中ではない
        if (this.isMoving == false)
        {
            //上下左右のキー入力を受けたら
            //isMovingをtrueにしてdirectionを更新
            this.isMoving = true;
            if (Input.GetKeyDown(KeyCode.UpArrow)) //↑
            {
                this.direction = 0;
                this.goalPos.y += 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))  //→
            {
                this.direction = 1;
                this.goalPos.x += 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))   //↓
            {
                this.direction = 2;
                this.goalPos.y -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))   //←
            {
                this.direction = 3;
                this.goalPos.x -= 1;
            }
            else
            {
                this.isMoving = false;
            }

            //次のフレームから移動開始
            if (this.isMoving = true)
            {
                //現在は特に処理なし
            }

        }

        //移動中
        if (this.isMoving == true)
        {
            //vxとvyを0にする
            this.vector = new Vector2(0, 0);

            //x方向の移動
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
            //y方向の移動
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
            //実際のベクトルに反映
            this.rigid.velocity = vector;
        }
    }
}
