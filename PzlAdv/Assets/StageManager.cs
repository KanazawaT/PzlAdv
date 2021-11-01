using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverObject//動かせるギミック用
{
    public OverObject()
	{
        this.x = 0;
        this.y = 0;
        this.id = 0;
        this.state = 0;
	}
    public GameObject gameObject;
    public int x, y,id,state;//GameObject型の座標系だとfloat型なのでint型を別で用意.あとそのオブジェクトのID
}

public class StageManager : MonoBehaviour
{

    public GameObject floorPrefab;//床
    public GameObject wallPrefab1;//WallPrefab;
    public GameObject wallPrefab2;
    public GameObject stonePrefab;//岩のプレハブ
    public GameObject icePrefab;//氷のプレハブ
    public GameObject holePrefab1;//穴のプレハブ
    public GameObject buriedholePrefab1;//岩が埋まった穴のプレハブ
    public Sprite buriedhole1;


    int movingCount = 0;//移動中のものを数える

    public const int maxObjNum = 2;//動かせるオブジェクトの最大数
    const int stageHeight = 10;
    const int stageWidth = 10;

    int[,] Board;//ステージの様子を記録
    OverObject[] terrain;//岩などの動くギミックは別で記録
    //Vector2Int rockGoalStep = new Vector2Int(); //岩の目標地点のマス目(現在はRockMove関数でしか使っていないが、滑らかに動かすときにUpdateなどで呼び出すことを想定してここで宣言)
    //Vector3 rockGoalPrint = new Vector3(); //岩の目標地点のtransform座標()
    /* マップの番号
    0.床
    1,2.壁
    3.氷
    4.未使用
    5.岩
    6.穴
    7岩の落ちた穴
    オブジェクトのidは、(terrain配列の添え字+5)になるようにしました
    */

    // Start is called before the first frame update
    void Start()
    {
        this.Board = new int[stageWidth, stageHeight]//マップをここで作る
        {
            { 2,2,2,2,2,2,2,2,2,2},
            { 1,1,1,1,1,1,1,1,1,1},
            { 1,0,0,0,0,1,0,0,0,1},
            { 1,0,0,0,0,1,0,2,2,1},
            { 1,0,0,3,3,3,1,1,1,1},
            { 1,0,0,3,3,3,1,0,0,1},
            { 1,0,0,0,2,0,0,0,0,1},
            { 1,0,0,0,1,0,0,0,0,1},
            { 1,2,2,2,1,2,2,2,2,1},
            { 1,1,1,1,1,1,1,1,1,1}
        };
        this.terrain = new OverObject[maxObjNum];//岩などのオブジェクトを増やすたびにmaxObjectNumを書き換えること
        for (int i = 0; i < maxObjNum; i++)
        {
            this.terrain[i] = new OverObject();
        }



        //岩の座標は一つずつ代入
        this.terrain[0].id = 5; //id(現状0～2)がboardの壁床で使われているので、idは5から始める
        this.terrain[0].x = 3;
        this.terrain[0].y = 4;

        //穴の座標も一つずつ代入
        this.terrain[1].id = 6;
        this.terrain[1].x = 2;
        this.terrain[1].y = 3;

        //ステージ生成
        for (int x = 0; x < stageWidth; x++)
        {
            for (int y = 0; y < stageHeight; y++)
            {

                GameObject go;

                switch (this.Board[y, x])
                {
                    case 0:
                        go = Instantiate(this.floorPrefab) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        break;
                    case 1:
                        go = Instantiate(this.wallPrefab1) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        break;
                    case 2:
                        go = Instantiate(this.wallPrefab2) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        /*this.terrain[objectCount].gameObject = go;
                        this.terrain[objectCount].id = 2;
                        this.terrain[objectCount].x = x;
                        this.terrain[objectCount].y = y;
                        this.Board[x, y] = 0;
                            
                        objectCount++;*/
                        break;
                    case 3:
                        go = Instantiate(this.icePrefab) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        break;

                }

            }
        }

        //岩などのオブジェクト生成
        for (int i = 0; i < maxObjNum; i++)
        {
            if (terrain[i].id != 0)
            {
                GameObject go;
                switch (terrain[i].id)
                {
                    //新たにid7の岩を出したい場合、ここにcase7: と宣言
                    case 5:
                        go = Instantiate(this.stonePrefab) as GameObject;
                        go.transform.position = new Vector3(terrain[i].x, terrain[i].y / 2.0f, terrain[i].y);
                        terrain[i].gameObject = go;
                        break;
                    //新たに出したい穴のidをここにcaseで宣言
                    case 6:
                        go = Instantiate(this.holePrefab1) as GameObject;
                        go.transform.position = new Vector3(terrain[i].x, terrain[i].y / 2.0f - 0.25f, terrain[i].y + 10f);
                        terrain[i].gameObject = go;
                        break;
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    
    public int GetTargetId(int x, int y)//指定地点にあるオブジェクトを返却する上に岩等が乗ってた場合そっちを返す
	{
        for(int i = 0;i < maxObjNum;i++)
		{
            if(this.terrain[i].x == x && this.terrain[i].y == y)
			{
                return this.terrain[i].id;
			}

        }

        return this.Board[y, x];

	}
    
    public bool CheckPassing(int x,int y,Vector2 direction)//指定した座標の通行可否を返す
	{
        for(int i = 0;i < maxObjNum; i++)//上に動かせるものが置いてあったらそちらを優先
		{
            if(this.terrain[i].x == x && this.terrain[i].y == y)
			{
                if(this.terrain[i].id == 5)
				{
                    //岩ならfalseを返す
                    return false;
				}
                if (this.terrain[i].id == 6)
                {
                    //穴ならここで判定
                    if (this.terrain[i].state == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
			}
		}
        //オブジェクトが上に乗ってなければ地形によって通行可否を判断
        if(x >= 0 && x < stageHeight && y >= 0 && y < stageHeight)
		{
            if(this.Board[y,x] == 0 || this.Board[y,x] == 3)//通れる地形(何もない床)なら
		    {
                return true;
			}
			else//そうでない(壁)なら
			{
                return false;
			}
		}
        
        else//配列の範囲外なら
		{
            return false;
		}
	}

    //指定座標の岩を動かす
    public bool RockMove(Vector2Int pos, int direction)
    {
        bool result = false;

        for (int i = 0; i < maxObjNum; i++)
        {
            if (this.terrain[i].x == pos.x && this.terrain[i].y == pos.y)
            {
                result = this.terrain[i].gameObject.GetComponent<StoneController>().StartMoving(pos, direction, i);
                break;
            }
        }
        return result;
    }

    //岩の移動完了
    public void FinishRockMove(Vector2Int pos, int index)
    {
        terrain[index].x = pos.x;
        terrain[index].y = pos.y;
    }

    //岩が落ちました
    public void FallRock(Vector2Int pos)
    {
        Debug.Log("a");
        for (int i = 0; i < maxObjNum; i++)
        {
            if (this.terrain[i].x == pos.x && this.terrain[i].y == pos.y)
            {
                this.terrain[i].gameObject.GetComponent<SpriteRenderer>().sprite = this.buriedhole1;
                this.terrain[i].id = 7;
                break;
            }
        }

    }

    //落ちた岩を削除する
    public void DestroyRock(int index)
    {
        this.terrain[index].id = -1;
        this.terrain[index].x = -1;
        this.terrain[index].y = -1;
        Destroy(this.terrain[index].gameObject);
    }

    //movingCountを操作
    public int MovingCount(int num)
    {
        this.movingCount += num;
        return this.movingCount;
    }
    
}
