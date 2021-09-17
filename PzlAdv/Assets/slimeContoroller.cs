using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeContoroller : MonoBehaviour
{
    Rigidbody2D rigid;
    float speed = 5.0f;
    float vx = 0, vy = 0;//フレーム枚の速度
    int gx = 0, gy = 0;//目指す座標

    // Start is called before the first frame update
    void Start()
    {
        this.rigid = this.GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        this.vx = 0;
        this.vy = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.gx--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.gx++;
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.gy--;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.gy++;
        }
        if(this.transform.position.x < this.gx - 0.1)
		{
            this.vx = 3;
		}
        else if(this.transform.position.x > this.gx + 0.1)
        {
            this.vx = -3;
		}
        else
		{
            this.transform.position = new Vector2(this.gx,this.transform.position.y);
		}
        if (this.transform.position.y < this.gy - 0.1)
        {
            this.vy = 3;
        }
        else if (this.transform.position.y > this.gy + 0.1)
        {
            this.vy = -3;
        }
        else
        {
            this.transform.position = new Vector2(this.transform.position.x, this.gy);
        }

        this.rigid.velocity = new Vector2(this.vx, this.vy);
    }
}
