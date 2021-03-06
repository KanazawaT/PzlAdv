using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    public Rigidbody2D rigid;
    float speed = 1.0f; //移動スピード
    int direction = 0; //向き(0:↑, 1:→, 2:↓, 3:←)
    Vector2 motion;   //移動のベクトル
    Vector2 goalPosF;    //移動で目指す実際の座標
    Vector2Int goalPosI;     //マス目上の移動先座標
    public bool isMoving = false;  //移動中ならtrueそうでなければfalse
    float heightProp = 0.5f;    //マスの縦の比率を設定する(1.0fで横と同じ、0.5fで半分)
    int goalId;    //目指す座標のID
    StageManager stageManagerS; //StageManagerのスクリプト
    int index; //この岩のterrain上での添字

    // Start is called before the first frame update
    void Start()
    {
        stageManagerS = GameObject.Find("StageManager").GetComponent<StageManager>();
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

    //移動開始を受け付ける処理
    public bool GetStartMoving(Vector2Int pos, int direction, int index)
    {
        this.direction = direction;
        this.goalPosI = pos;
        this.index = index;

        return StartMoving();
    }

    //移動を開始する処理
    bool StartMoving()
    {

        //目標の移動先をnewPosに設定       
        Vector2Int newPos = goalPosI + DirectionToVector2(direction);

        //移動先がどんなか確かめる
        int id = stageManagerS.GetTargetId(newPos.x, newPos.y);

        switch (id)
        {
            case 6: //穴
            case 0: //床
            case 3: //氷の床
                //移動開始に成功
                
                //床が氷だった場合
                if (UnderIce(goalPosI))
                {
                    this.speed = 2.0f;
                }
                else
                {
                    this.speed = 1.0f;
                }
                
                //移動継続時には実行しない
                if (isMoving == false)
                {
                    this.isMoving = true;
                    //movingCountを増やす
                    stageManagerS.MovingCount(1);
                }

                //goalPosを更新
                this.goalPosI = newPos;
                this.goalPosF = ChangePosType(goalPosI);
                //motionに値を反映、rigidも更新
                this.motion = DirectionToVector2(this.direction);
                this.motion *= this.speed;
                //this.motion.y *= heightProp;  //縦移動の際に移動速度を変える処理
                this.rigid.velocity = this.motion;
                return true;
            default: //その他
                //移動開始失敗
                this.isMoving = false;
                return false;
        }
    }
    //移動を終了する処理
    void FinishMoving()
    {        
        //現在地が穴だったら
        if (stageManagerS.GetTargetId(goalPosI.x, goalPosI.y) == 6)
        {
            stageManagerS.FallRock(goalPosI);
            stageManagerS.DestroyRock(index);
        }

        //terrainの更新
        stageManagerS.ChageTerrainPos(this.goalPosI, this.index);
        //movingCountをへらす
        stageManagerS.MovingCount(-1);

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

        //到着したけど床が氷だった場合
        if (result)
        {
            if (UnderIce(goalPosI))
            {
                return !StartMoving();
            }
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

    //床が氷の場合はgoalPosを調整
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

    //氷の上に乗ってるかどうか
    bool UnderIce(Vector2Int pos)
    {
        return this.stageManagerS.GetTargetId(pos.x, pos.y) == 3;
    }
}

