using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverObject//“®‚©‚¹‚éƒMƒ~ƒbƒN—p
{
    public OverObject()
	{
        this.x = 0;
        this.y = 0;
        this.id = 0;
	}
    public GameObject gameObject;
    public int x, y,id;//GameObjectŒ^‚ÌÀ•WŒn‚¾‚ÆfloatŒ^‚È‚Ì‚ÅintŒ^‚ğ•Ê‚Å—pˆÓ.‚ ‚Æ‚»‚ÌƒIƒuƒWƒFƒNƒg‚ÌID
}

public class StageManager : MonoBehaviour
{

    public GameObject floorPrefab;//°
    public GameObject wallPrefab1;//WallPrefab;
    public GameObject wallPrefab2;
    public GameObject stonePrefab;//Šâ‚ÌƒvƒŒƒnƒu

    public const int maxObjNum = 1;//“®‚©‚¹‚éƒIƒuƒWƒFƒNƒg‚ÌÅ‘å”
    const int stageHeight = 10;
    const int stageWidth = 10;

    int[,] Board;//ƒXƒe[ƒW‚Ì—lq‚ğ‹L˜^
    OverObject[] terrain;//Šâ‚È‚Ç‚Ì“®‚­ƒMƒ~ƒbƒN‚Í•Ê‚Å‹L˜^
    Vector2Int rockGoalStep = new Vector2Int(); //Šâ‚Ì–Ú•W’n“_‚Ìƒ}ƒX–Ú
    Vector3 rockGoalPrint = new Vector3(); //Šâ‚Ì–Ú•W’n“_‚ÌtransformÀ•W

    // Start is called before the first frame update
    void Start()
    {
        this.Board = new int[stageWidth, stageHeight]//ƒ}ƒbƒv‚ğ‚±‚±‚Åì‚é
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
        this.terrain = new OverObject[maxObjNum];//Šâ‚È‚Ç‚ÌƒIƒuƒWƒFƒNƒg‚ğ‘‚â‚·‚½‚Ñ‚ÉmaxObjectNum‚ğ‘‚«Š·‚¦‚é‚±‚Æ
        for (int i = 0; i < maxObjNum; i++)
        {
            this.terrain[i] = new OverObject();
        }
        //Šâ‚ÌÀ•W‚Íˆê‚Â‚¸‚Â‘ã“ü
        this.terrain[0].id = 5; //id(Œ»ó0`2)‚ªboard‚Ì•Ç°‚Åg‚í‚ê‚Ä‚¢‚é‚Ì‚ÅAid‚Í5‚©‚çn‚ß‚é
        this.terrain[0].x = 3;
        this.terrain[0].y = 4;

        //ƒXƒe[ƒW¶¬
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
                }

            }
        }
        //Šâ‚È‚Ç‚ÌƒIƒuƒWƒFƒNƒg¶¬
        for (int i = 0; i < maxObjNum; i++)
        {
            if (terrain[i].id != 0)
            {
                
                switch (terrain[i].id) {
                    case 5:
                        GameObject go;
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


    public int GetTargetId(int x, int y)//w’è’n“_‚É‚ ‚éƒIƒuƒWƒFƒNƒg‚ğ•Ô‹p‚·‚éã‚ÉŠâ“™‚ªæ‚Á‚Ä‚½ê‡‚»‚Á‚¿‚ğ•Ô‚·
    {
        for (int i = 0; i < maxObjNum; i++)
        {
            if (this.terrain[i].x == x && this.terrain[i].y == y)
            {
                return this.terrain[i].id;
            }

        }

        return this.Board[y, x];

    }

    public bool CheckPassing(int x, int y, Vector2 direction)//w’è‚µ‚½À•W‚Ì’Ês‰Â”Û‚ğ•Ô‚·
    {
        for (int i = 0; i < maxObjNum; i++)//ã‚É“®‚©‚¹‚é‚à‚Ì‚ª’u‚¢‚Ä‚ ‚Á‚½‚ç‚»‚¿‚ç‚ğ—Dæ
        {
            if (this.terrain[i].x == x && this.terrain[i].y == y)
            {
                if (this.terrain[i].id == 10m)
                {
                    return false;
                }
            }
        }
        //ƒIƒuƒWƒFƒNƒg‚ªã‚Éæ‚Á‚Ä‚È‚¯‚ê‚Î’nŒ`‚É‚æ‚Á‚Ä’Ês‰Â”Û‚ğ”»’f
        if (x >= 0 && x < stageHeight && y >= 0 && y < stageHeight)
        {
            if (this.Board[y, x] == 0)//’Ê‚ê‚é’nŒ`(‰½‚à‚È‚¢°)‚È‚ç
            {
                return true;
            }
            else//‚»‚¤‚Å‚È‚¢(•Ç)‚È‚ç
            {
                return false;
            }
        }

        else//”z—ñ‚Ì”ÍˆÍŠO‚È‚ç
        {
            return false;
        }
    }

    public bool RockMove(Vector2Int direction, int index)
    {

        //Šâ‚ğ“®‚©‚·ƒƒ\ƒbƒh
        //‘æˆêˆø”‚ÍPlayerController‚ÌDirectionToVector2(direction)‚É‚·‚é‚±‚Æ‚ğ‘z’è
        //‘æ“ñˆø”‚ÍGetTargetId‚Å“¾‚½id11ˆÈã‚ÌƒIƒuƒWƒFƒNƒg‚Ìid
        

        index -= 5;
        if (index < 0)//id‚ª11–¢–‚È‚çƒGƒ‰[
        {
            Debug.Log("ƒGƒ‰[:StageManagerƒNƒ‰ƒX‚ÌRockMove‚Ìˆø”‚É‚µ‚½id‚ª" + index + 11);
            return false;
        }

        rockGoalStep.x = this.terrain[index].x + direction.x;
        rockGoalStep.y = this.terrain[index].y + direction.y;
        Debug.Log("Step(" + rockGoalStep.x + "," + rockGoalStep.y + ")");

<<<<<<< HEAD
        if (CheckPassing(rockGoal.x, rockGoal.y, direction) == true)
        {
            
=======
        rockGoalPrint.x = (float)rockGoalStep.x;
        rockGoalPrint.y = (float)rockGoalStep.y/2;

        if (CheckPassing(rockGoalStep.x, rockGoalStep.y, direction) == true)
        {
            terrain[index].gameObject.transform.position = rockGoalPrint;
            this.terrain[index].x = rockGoalStep.x;
            this.terrain[index].y = rockGoalStep.y;
>>>>>>> ã¢ãˆã›ï½Œ
            return true;
        }



        return false;
    }
}
