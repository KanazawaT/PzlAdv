using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    float span = 1.0f;
    float delta;
    int[,] board;

    // Start is called before the first frame update
    void Start()
    {
        board = new int[10, 10]
        {
            { 1,1,1,1,1,1,1,1,1,1},
            { 1,0,1,0,0,1,0,0,0,1},
            { 1,0,1,0,0,1,0,0,0,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,0,0,0,0,0,0,1,1,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,1,1,0,1,0,0,0,0,1},
            { 1,0,0,0,0,1,0,0,0,1},
            { 1,0,0,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,1,1,1,1}

        };
        for(int x = 0;x < 10; x++)
		{
            for(int y = 0;y < 10; y++)
			{
                if(board[x,y] == 1)
				{
                    GameObject go = Instantiate(wallPrefab) as GameObject;
                    go.transform.position = new Vector3(x-5, y-5, 0);
                }
			}
		}

    }

    // Update is called once per frame
    void Update()
    {
        /*this.delta += Time.deltaTime;
        if(this.delta > this.span)
		{
            this.delta = 0;
            GameObject go = Instantiate(wallPrefab) as GameObject;
            int px = Random.Range(-6, 7);
            go.transform.position = new Vector3(px, 5, 0);
		}*/
    }
}
