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

    public GameObject cam; //カメラ

    public GameObject floorPrefab;//床
    public GameObject wallPrefab1;//WallPrefab;
    public GameObject wallPrefab2;
    public GameObject stonePrefab;//岩のプレハブ
    public GameObject icePrefab;//氷のプレハブ
    public GameObject holePrefab1;//穴のプレハブ
    public GameObject buriedholePrefab1;//岩が埋まった穴のプレハブ
    public GameObject goalPrefab;//ゴールのプレハブ
    public Sprite buriedhole1;//岩の埋まった穴の画像

    PlayerContoroller playerCS; //PlayerControllerのスクリプト

    public int movingCount = 0;//移動中のものを数える

    int maxObjNum;//動かせるオブジェクトの最大数
    int objNum = 0; //オブジェクトの設定に使用
    int stageHeight;
    int stageWidth;
    Vector2Int startPos; //プレイヤーのスタート地点
    int[,] Board;//ステージの様子を記録
    OverObject[] terrain;//岩などの動くギミックは別で記録

    int stage;//ステージ数を保存

    //Vector2Int rockGoalStep = new Vector2Int(); //岩の目標地点のマス目(現在はRockMove関数でしか使っていないが、滑らかに動かすときにUpdateなどで呼び出すことを想定してここで宣言)
    //Vector3 rockGoalPrint = new Vector3(); //岩の目標地点のtransform座標()
    /* マップの番号
    0.床
    1,2.壁
    3.氷
    4.ゴール
    5.岩
    6.穴
    7岩の落ちた穴
    */

    // Start is called before the first frame update
    void Start()
    {
        playerCS = GameObject.Find("Char").GetComponent<PlayerContoroller>();

        this.stage = TitleManager.stage;

        SetBoardInf(this.stage);
        SetTerrainInf(this.stage);
        GenerateStage();
        playerCS.SetPos(this.startPos);

        cam.transform.position = new Vector3(this.stageWidth / 2, this.stageHeight / 4, -10);

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetStage();
        }
        */

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

        return this.Board[stageHeight - y - 1, x];

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
                result = this.terrain[i].gameObject.GetComponent<StoneController>().GetStartMoving(pos, direction, i);
                break;
            }
        }
        return result;
    }

    //岩の移動完了
    public void ChageTerrainPos(Vector2Int pos, int index)
    {
        terrain[index].x = pos.x;
        terrain[index].y = pos.y;
    }

    //岩が落ちました
    public void FallRock(Vector2Int pos)
    {
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
        this.terrain[index].gameObject.SetActive(false);
    }

    //movingCountを操作
    public int MovingCount(int num)
    {
        this.movingCount += num;
        return this.movingCount;
    }

    //ステージ情報を設定
    void SetBoardInf(int stage)
    {
        switch(stage)
        {
            case 1:
                this.maxObjNum = 4;
                this.stageHeight = 10;
                this.stageWidth = 10;
                this.startPos = new Vector2Int(1, 5);
                this.Board = new int[10, 10]//マップをここで作る
                {
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 1,2,2,1,2,1,2,2,2,1},
                    { 1,0,0,1,0,2,0,0,0,1},
                    { 1,0,0,1,0,0,0,0,0,1},
                    { 1,0,0,1,0,0,0,0,0,1},
                    { 1,0,0,2,2,0,2,0,0,1},
                    { 1,0,0,0,0,0,0,0,0,1},
                    { 1,0,0,0,0,0,0,0,0,1},
                    { 1,1,1,1,1,4,1,1,1,1},
                    { 2,2,2,2,2,2,2,2,2,2}
                };               
                break;
            case 2:
                this.maxObjNum = 6;
                this.stageHeight = 10;
                this.stageWidth = 10;
                this.startPos = new Vector2Int(8, 7);
                this.Board = new int[10, 10]//マップをここで作る
                {
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 1,2,2,2,2,2,2,2,2,1},
                    { 1,0,0,0,0,0,0,0,0,1},
                    { 1,0,0,0,2,2,2,1,0,1},
                    { 1,3,3,3,3,3,3,2,3,1},
                    { 1,3,3,3,3,3,3,3,3,1},
                    { 1,1,3,3,3,3,3,3,3,1},
                    { 1,1,3,3,3,3,1,4,1,1},
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 2,2,2,2,2,2,2,2,2,2}
                };
                break;
            case 3:
                this.maxObjNum = 4;
                this.stageHeight = 12;
                this.stageWidth = 10;
                this.startPos = new Vector2Int(3, 3);
                this.Board = new int[12, 10]//マップをここで作る
                {
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 1,2,2,2,2,1,2,2,2,1},
                    { 1,0,0,0,0,2,3,3,3,1},
                    { 1,0,0,3,3,3,3,3,3,1},
                    { 1,3,3,3,3,3,3,3,3,4},
                    { 1,0,0,3,3,3,3,3,3,1},
                    { 1,0,0,0,3,3,3,3,3,1},
                    { 1,0,0,0,3,3,3,3,3,1},
                    { 1,0,0,0,3,3,3,3,3,1},
                    { 1,3,0,0,1,0,3,3,3,1},
                    { 1,1,1,1,1,1,1,1,1,1},
                    { 2,2,2,2,2,2,2,2,2,2}
                };
                break;
            case 4:
                this.maxObjNum = 12;
                this.stageHeight = 15;
                this.stageWidth = 15;
                this.startPos = new Vector2Int(3, 2);
                this.Board = new int[15, 15]//マップをここで作る
                {
                    { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    { 1,2,2,2,2,1,1,1,1,1,1,2,2,2,1},
                    { 1,3,3,3,3,1,2,2,1,2,1,0,0,0,1},
                    { 1,3,0,3,3,1,0,0,2,0,2,0,2,0,1},
                    { 1,2,3,3,2,1,0,0,3,3,3,3,3,0,1},
                    { 1,3,3,3,3,2,0,0,2,3,3,3,3,2,1},
                    { 1,3,3,3,3,0,0,0,3,3,3,3,3,3,1},
                    { 1,3,2,3,3,2,2,2,3,3,3,3,3,3,1},
                    { 1,3,3,3,1,0,0,0,2,3,3,3,1,1,1},
                    { 1,3,1,3,2,0,0,0,0,2,0,2,2,2,1},
                    { 1,1,1,0,0,0,0,3,3,0,0,0,0,0,1},
                    { 1,2,2,0,2,0,2,0,0,0,0,0,0,0,1},
                    { 1,4,0,0,0,0,0,0,0,0,0,0,0,0,1},
                    { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}


                };
                break;
                

        }
    }

    //ステージ上のオブジェクトの情報を設定
    void SetTerrainInf(int stage)
    {
        this.objNum = 0;
        this.terrain = new OverObject[this.maxObjNum];//岩などのオブジェクトを増やすたびにmaxObjectNumを書き換えること
        for (int i = 0; i < maxObjNum; i++)
        {
            this.terrain[i] = new OverObject();
        }

        switch (stage)
        {
            case 1:
                AddObj(5, 6, 6);
                AddObj(5, 2, 5);
                AddObj(6, 5, 5);
                AddObj(6, 5, 4);
                break;
            case 2:
                AddObj(5, 8, 6);
                AddObj(5, 3, 3);
                AddObj(5, 3, 6);
                AddObj(5, 2, 6);
                AddObj(6, 4, 4);
                AddObj(6, 6, 3);
                break;
            case 3:
                AddObj(5, 3, 5);
                AddObj(5, 7, 3);
                AddObj(6, 8, 4);
                AddObj(6, 1, 4);
                break;
            case 4:
                AddObj(5, 4, 4);
                AddObj(5, 8, 3);
                AddObj(5, 10, 5);
                AddObj(5, 12, 3);
                AddObj(5, 12, 10);
                AddObj(6, 2, 2);
                AddObj(6, 4, 2);
                AddObj(6, 5, 4);
                AddObj(6, 6, 4);
                AddObj(6, 10, 2);
                AddObj(6, 10, 4);
                AddObj(6, 10, 8);
                break;

        }
    }
    
    //ステージ上のオブジェクトを追加
    void AddObj(int id, int x, int y)
    {
        this.terrain[this.objNum].x = x;
        this.terrain[this.objNum].y = y;
        this.terrain[this.objNum].id = id;
        this.objNum++;
    }

    //ステージを生成
    void GenerateStage()
    {

        //ステージ生成
        for (int x = 0; x < this.stageWidth; x++)
        {
            for (int y = 0; y < this.stageHeight; y++)
            {

                GameObject go;

                switch (this.Board[stageHeight - y - 1, x])
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
                    case 4:
                        go = Instantiate(this.goalPrefab) as GameObject;
                        go.transform.position = new Vector3(x, y / 2.0f - 0.25f, 10 + y);
                        break;


                }

            }
        }

        GenerateTerrain();
        
    }

    //オブジェを生成
    void GenerateTerrain()
    {
        for (int i = 0; i < this.maxObjNum; i++)
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

    //ステージをリセットする
    void ResetStage()
    {
        //オブジェをすべて削除
        for (int i = 0; i < this.maxObjNum; i++)
        {
            Destroy(this.terrain[i].gameObject);
        }
        //terrainを最初の状態に復元
        SetTerrainInf(this.stage);
        //再生成
        GenerateTerrain();
        //プレイヤーもリセット
        playerCS.SetPos(this.startPos);
        playerCS.Reset();
        playerCS.ResetDirection();
        //movingCountのリセット
        this.movingCount = 0;
    }

}
