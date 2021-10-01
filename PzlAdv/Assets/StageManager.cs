using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverObject//動かせるギミック用
{
    public GameObject gameObject;
    public int x, y;//GameObject型の座標系だとfloat型なのでint型を別で用意
}

public class StageManager : MonoBehaviour
{
    

    public GameObject wallPrefab;//WallPrefab;
    public GameObject stonePrefab;//岩のプレハブ
    
    //public const int objectNum = 10;//動かせるオブジェクトの最大数

    //const int maxObjcts = 10;　//
    int[,] Board;//ステージの様子を記録
    OverObject[] terrain;//岩などの動くギミックは別で記録
    //const int stageHeight = 10;
    //const int stageWidth = 10;

    // Start is called before the first frame update
    void Start()
    {
        this.Board = new int[10, 10]//マップをここで作る
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
        this.terrain = new OverObject[10];
        
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (this.Board[x, y] != 0)
                {
                    GameObject go;

                    switch (this.Board[x, y])
					{
                        case 1:
                            go = Instantiate(this.wallPrefab) as GameObject;
                            go.transform.position = new Vector3(x, y, 0);
                            break;
                        case 2:
                            go = Instantiate(this.stonePrefab) as GameObject;
                            go.transform.position = new Vector3(x, y, 0);
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

    
    public int GetTargetId(int x, int y)//指定地点にあるオブジェクトを返却する
	{
        
        return this.Board[x, y];

	}
    
    public bool CheckPassing(int x,int y,Vector2 direction)//指定した座標の通行可否を返す
	{
        return true;
	}
}
