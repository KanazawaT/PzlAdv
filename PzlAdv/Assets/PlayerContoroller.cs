using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour
{
    //なんか色々と初期設定
    public Rigidbody2D rigid;
    float speed = 5.0f; //移動スピード
    int direction = 0; //向き(0:↑, 1:→, 2:↓, 3:←)
    Vector2 vector = new Vector2(0, 0);   //移動のベクトル
    Vector2 goalPos = new Vector2(0, 0);    //移動で目指す座標
    bool getKey;    //キー入力があったかの判定用
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
            this.getKey = true;
            if (Input.GetKeyDown(KeyCode.UpArrow)) //↑
            {
                this.direction = 0;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))  //→
            {
                this.direction = 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))   //↓
            {
                this.direction = 2;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))   //←
            {
                this.direction = 3;
            }
            else
            {
                this.getKey = false;
            }

            //移動開始させる
            if (this.getKey == true)
            {             
                StartMoving();

            }

        }

        //移動中
        if (this.isMoving == true)
        {
            //goalPosに到着していたら停止
            if (CheckMoving())
            {
                FinishMoving();
            }

            /*//x方向の移動
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
            */
        }
    }

    //int型のdirectionをVector2に変換
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

    //移動を開始する処理
    void StartMoving()
    {
        //まず向いてる方向1マス先をgoalPosに設定
        int type = 0; //GetTargetID(this.goalPos + DirectionToVector2(direction));
        if (type == 2)   //岩だったら
        {
            //岩を押す関数を呼び出す
        }
        else
        {
            //goalPosを更新
            this.goalPos = this.goalPos + DirectionToVector2(direction);
        }

        this.vector = DirectionToVector2(this.direction);
        this.vector *= this.speed;

        this.rigid.velocity = vector;

        isMoving = true;
    }

    //移動を終了する処理
    void FinishMoving()
    {
        this.transform.position = this.goalPos;
        this.vector = new Vector2(0, 0);
        this.rigid.velocity = vector;

        isMoving = false;
    }

    //移動を終了すべきかチェック
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

        //goalPosへの移動は完了しているが…
        if (result)
        {
            //もし地面が氷なら移動継続
            //result = false;
        }

        return result;
    }
}