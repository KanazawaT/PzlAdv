using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour
{
    //なんか色々と初期設定
    public Rigidbody2D rigid;
    float speed = 3.0f; //移動スピード
    int direction = 0; //向き(0:↑, 1:→, 2:↓, 3:←)
    public Vector2 motion;   //移動のベクトル
    Vector2 goalPosF;    //移動で目指す実際の座標
    Vector2Int goalPosI;     //マス目上の移動先座標
    bool getKey;    //キー入力があったかの判定用
    bool isMoving = false;  //移動中ならtrueそうでなければfalse
    float heightProp = 0.5f;    //マスの縦の比率を設定する(1.0fで横と同じ、0.5fで半分)
    Animator anim;       //アニメ制御用

    public GameObject stageManager; //StageManagerを呼び出す

    // Start is called before the first frame update
    void Start()
    {
        //初期位置を設定して配置
        goalPosI = new Vector2Int(3, 3);    //ここで初期位置を設定
        goalPosF = ChangePosType(goalPosI);
        this.transform.position = goalPosF;

        this.anim = GetComponent<Animator>();   //アニメ制御用
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
            if (Input.GetKey(KeyCode.UpArrow)) //↑
            {
                this.direction = 0;
                anim.SetInteger("direction", 0);    //direction変更時にアニメ用のdirectionも変更
            }
            else if (Input.GetKey(KeyCode.RightArrow))  //→
            {
                this.direction = 1;
                anim.SetInteger("direction", 1);
            }
            else if (Input.GetKey(KeyCode.DownArrow))   //↓
            {
                this.direction = 2;
                anim.SetInteger("direction", 2);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))   //←
            {
                this.direction = 3;
                anim.SetInteger("direction", 3);
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
        }
    }

    //int型のdirectionをVector2に変換
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

    //マス目上の座標を実際の座標に変換
    Vector2 ChangePosType(Vector2Int vectorI)
    {
        return new Vector2(vectorI.x, vectorI.y * heightProp);
    }

    //移動を開始する処理
    void StartMoving()
    {
        //目標の移動先をnewPosに設定
        Vector2Int newPos = goalPosI + DirectionToVector2(direction);

        //移動先がどんなか確かめる
        //int type = 0; //GetTargetID(); 今は仮で0をいれてます
        
        
        
        Debug.Log(newPos.x);

        int type = stageManager.GetComponent<StageManager>().GetTargetId(newPos.x,newPos.y);

        Debug.Log("type=" + type);

        switch (type)
        {
            case 5: //岩
                    //岩を押す処理
                if (stageManager.GetComponent<StageManager>().RockMove(DirectionToVector2(direction), type) == true)
                {
                    Debug.Log("iwa");
                    //移動開始に成功
                    this.isMoving = true;
                    anim.SetFloat("speed", 1.0f);       //移動始めたらアニメーションも動く
                    //gxとgyを更新
                    this.goalPosI = newPos;
                    //goalPosを更新
                    this.goalPosF = ChangePosType(goalPosI);
                    //motionに値を反映、rigidも更新
                    this.motion = DirectionToVector2(this.direction);
                    this.motion *= this.speed;
                    //this.motion.y *= heightProp;  //縦移動の際に移動速度を変える処理
                    this.rigid.velocity = this.motion;
                }
                break;
            case 6:
            case 2:
            case 1: //壁
                //移動開始に失敗
                this.isMoving = false;
                anim.SetFloat("speed", 0.0f);    //移動止めたらアニメーション停止
                //失敗時の共通処理とかあればここに
                break;
            default: //その他
                //移動開始に成功
                this.isMoving = true;
                anim.SetFloat("speed", 1.0f);       //移動始めたらアニメーションも動く
                //gxとgyを更新
                this.goalPosI = newPos;
                //goalPosを更新
                this.goalPosF = ChangePosType(goalPosI);
                //motionに値を反映、rigidも更新
                this.motion = DirectionToVector2(this.direction);
                this.motion *= this.speed;
                //this.motion.y *= heightProp;  //縦移動の際に移動速度を変える処理
                this.rigid.velocity = this.motion;
                break;
        }
    }

    //移動を終了する処理
    void FinishMoving()
    {
        this.transform.position = this.goalPosF;
        this.transform.Translate(0, 0, this.transform.position.y*2 - this.transform.position.z);//z座標をy座標に
        this.motion = Vector2.zero;
        this.rigid.velocity = motion;

        isMoving = false;
        anim.SetFloat("speed", 0.0f);    //移動止めたらアニメーション停止
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
}