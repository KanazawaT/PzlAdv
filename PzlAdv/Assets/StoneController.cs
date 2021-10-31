using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    public Rigidbody2D rigid;
    float speed = 3.0f; //移動スピード
    int direction = 0; //向き(0:↑, 1:→, 2:↓, 3:←)
    public Vector2 motion;   //移動のベクトル
    Vector2 goalPosF;    //移動で目指す実際の座標
    Vector2Int goalPosI;     //マス目上の移動先座標
    bool isMoving = false;  //移動中ならtrueそうでなければfalse
    float heightProp = 0.5f;    //マスの縦の比率を設定する(1.0fで横と同じ、0.5fで半分)
    public GameObject stageManager; //StageManagerを呼び出す

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //移動中
        if (this.isMoving == true)
        {
            //goalPosに到着していたら停止
            if (CheckMoving())
            {
                FinishMoving();
            }
        }
    }

    //移動を開始する処理
    public bool StartMoving(Vector2Int d)
    {
        //目標の移動先をnewPosに設定
        Vector2Int newPos = goalPosI + d;

        //移動先がどんなか確かめる
        //int type = 0; //GetTargetID(); 今は仮で0をいれてます



        Debug.Log(newPos.x);

        int type = stageManager.GetComponent<StageManager>().GetTargetId(newPos.x, newPos.y);

        Debug.Log("type=" + type);

        switch (type)
        {




            case 6: //穴
                if (true)
                {
                    //移動開始に失敗
                }
                else
                {
                    //移動開始に成功
                }
                break;
            case 2:
            case 1: //壁
                //移動開始に失敗
                this.isMoving = false;
                return false;
                break;
            default: //その他
                //移動開始に成功
                this.isMoving = true;
                //anim.SetFloat("speed", 1.0f);       //移動始めたらアニメーションも動く
                //gxとgyを更新
                this.goalPosI = newPos;
                //goalPosを更新
                this.goalPosF = ChangePosType(goalPosI);
                //motionに値を反映、rigidも更新
                this.motion = DirectionToVector2(this.direction);
                this.motion *= this.speed;
                //this.motion.y *= heightProp;  //縦移動の際に移動速度を変える処理
                this.rigid.velocity = this.motion;
                return true;
                break;
        }
        return false;
    }
    //移動を終了する処理
    void FinishMoving()
    {
        this.transform.position = this.goalPosF;
        this.transform.Translate(0, 0, this.transform.position.y * 2 - this.transform.position.z);//z座標をy座標に
        this.motion = Vector2.zero;
        this.rigid.velocity = motion;

        isMoving = false;
    }

    //移動を終了すべきかチェック
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

        //goalPosへの移動は完了しているが…
        if (result)
        {
            //もし地面が氷なら移動継続
            //result = false;
        }

        return result;
    }

    //int型のdirectionをVector2に変換
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
    //マス目上の座標を実際の座標に変換
    Vector2 ChangePosType(Vector2Int vectorI)
    {
        return new Vector2(vectorI.x, vectorI.y * heightProp);
    }
}

