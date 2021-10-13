using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverObject//動かせるギミック用
{
    public GameObject gameObject;
    public int x, y,id;//GameObject型の座標系だとfloat型なのでint型を別で用意.あとそのオブジェクトのID
}

public class StageManager : MonoBehaviour
{
    

    public GameObject wallPrefab;//WallPrefab;
    public GameObject stonePrefab;//岩のプレハブ
    
    public const int maxObjNum = 10;//動かせるオブジェクトの最大数
    const int stageHeight = 10;
    const int stageWidth = 10;

    int[,] Board;//ステージの様子を記録
    OverObject[] terrain;//岩などの動くギミックは別で記録
    
    // Start is called before the first frame update
    void Start()
    {
        this.Board = new int[stageWidth, stageHeight]//マップをここで作る
        {
            { 1,1,1,1,1,1,1,1,1,1},
            { 1,0,0,1,0,1,0,0,0,1},
            { 1,0,2,0,0,1,0,0,0,1},
            { 1,0,0,1,0,0,0,0,0,1},
            { 1,1,0,0,0,0,0,1,1,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,1,1,0,1,0,0,0,0,1},
            { 1,0,0,2,0,1,0,0,0,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,1,1,1,1}

        };
        this.terrain = new OverObject[maxObjNum];
        int objectCount = 0;//読み込んだ動かせるオブジェクト数を数える局所変数
        for (int x = 0; x < stageWidth; x++)
        {
            for (int y = 0; y < stageHeight; y++)
            {
                if (this.Board[x, y] != 0)
                {
                    GameObject go;

                    switch (this.Board[x, y])
					{
                        case 1:
                            go = Instantiate(this.wallPrefab) as GameObject;
                            go.transform.position = new Vector3(x, y, 10);
                            break;
                        case 2:
                            go = Instantiate(this.stonePrefab) as GameObject;
                            go.transform.position = new Vector3(x, y, 10);
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

        return this.Board[x, y];

	}
    
    public bool CheckPassing(int x,int y,Vector2 direction)//指定した座標の通行可否を返す
	{
        for(int i = 0;i < maxObjNum; i++)//上に動かせるものが置いてあったらそちらを優先
		{
            if(this.terrain[i].x == x && this.terrain[i].y == y)
			{
                if(this.terrain[i].id == 2)
				{
                    return false;
				}
			}
		}
        //オブジェクトが上に乗ってなければ地形によって通行可否を判断
        if(x >= 0 && x < stageHeight && y >= 0 && y < stageHeight)
		{
            if(this.Board[x,y] == 0)//通れる地形(何もない床)なら
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
}
