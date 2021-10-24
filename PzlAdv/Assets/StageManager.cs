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
	}
    public GameObject gameObject;
    public int x, y,id;//GameObject型の座標系だとfloat型なのでint型を別で用意.あとそのオブジェクトのID
}

public class StageManager : MonoBehaviour
{

    public GameObject floorPrefab;//床
    public GameObject wallPrefab1;//WallPrefab;
    public GameObject wallPrefab2;
    public GameObject stonePrefab;//岩のプレハブ
    
    public const int maxObjNum = 1;//動かせるオブジェクトの最大数
    const int stageHeight = 10;
    const int stageWidth = 10;

    int[,] Board;//ステージの様子を記録
    OverObject[] terrain;//岩などの動くギミックは別で記録
    
    // Start is called before the first frame update
    void Start()
    {
        this.Board = new int[stageWidth, stageHeight]//マップをここで作る
        {
            { 2,2,2,2,2,2,2,2,2,2},
            { 1,1,1,1,1,1,1,1,1,1},
            { 1,0,0,0,0,1,0,0,0,1},
            { 1,0,0,0,0,1,0,2,2,1},
            { 1,0,0,0,0,0,0,1,1,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,0,0,0,2,0,0,0,0,1},
            { 1,0,0,0,1,0,0,0,0,1},
            { 1,2,2,2,1,2,2,2,2,1},
            { 1,1,1,1,1,1,1,1,1,1}

        };
        this.terrain = new OverObject[maxObjNum];//岩などのオブジェクトを増やすたびにmaxObjectNumを書き換えること
        for(int i = 0;i < maxObjNum; i++)
		{
            this.terrain[i] = new OverObject();
		}
        //岩の座標は一つずつ代入
        this.terrain[0].id = 11; //id(現状0～2)がboardの壁床で使われているので、idは11から始める
        this.terrain[0].x = 3;
        this.terrain[0].y = 4;

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
                        go.transform.position = new Vector3(x, y/2.0f-0.25f, 10 + y);
                        break;
                    case 1:
                        go = Instantiate(this.wallPrefab1) as GameObject;
                        go.transform.position = new Vector3(x, y/2.0f - 0.25f, 10 + y);
                        break;
                    case 2:
                        go = Instantiate(this.wallPrefab2) as GameObject;
                        go.transform.position = new Vector3(x, y/2.0f - 0.25f, 10 + y);
                        /*this.terrain[objectCount].gameObject = go;
                        this.terrain[objectCount].id = 2;
                        this.terrain[objectCount].x = x;
                        this.terrain[objectCount].y = y;
                        this.Board[x, y] = 0;
                            
                        objectCount++;*/
                        break;
                }
               
            }
        }
        //岩などのオブジェクト生成
        for(int i = 0;i < maxObjNum; i++)
		{
            if (terrain[i].id != 0)
			{
                GameObject go;
				switch(terrain[i].id){
                    case 11:
                        go = Instantiate(this.stonePrefab) as GameObject;
                        go.transform.position = new Vector3(terrain[i].x, terrain[i].y / 2.0f, terrain[i].y);
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
                if(this.terrain[i].id == 10m)
				{
                    return false;
				}
			}
		}
        //オブジェクトが上に乗ってなければ地形によって通行可否を判断
        if(x >= 0 && x < stageHeight && y >= 0 && y < stageHeight)
		{
            if(this.Board[y,x] == 0)//通れる地形(何もない床)なら
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

    public bool RockMove(Vector2Int direction, int index)
    {
        /* ここうまく動かなかったんでとりあえずコメントアウトしました
        //岩を動かすメソッド
        //第一引数はPlayerControllerのDirectionToVector2(direction)にすることを想定
        //第二引数はGetTargetIdで得たid11以上のオブジェクトのid
        Vector2Int rockGoal;
        index -= 11;
        if (index < 0)//idが11未満ならエラー
        {
            Debug.Log("エラー:StageManagerクラスのRockMoveの引数にしたidが" + id);
            yield break;
        }
        rockGoal.x = this.terrain[index].x + direction.x;
        rockGoal.y = this.terrain[index].y + direction.y;
        if (CheckPassing(rockGoal.x, rockGoal.y, direction) == true)
        {
            this.terrian[index].x = rockGoal.x;
            this.terrian[index].y = rockGoal.y;
            return true;
        }
        */
        return false;
    }
}
