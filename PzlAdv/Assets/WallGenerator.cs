using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    float span = 1.0;
    float delta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
        if(this.delta > this.span)
		{
            this.delta = 0;
            GameObject go = Instantiate(wallPrefab) as GameObject;
            int px = Random.Range(-6, 7);
            go.transform.position = new Vector3(px, 7, 0);
		}
    }
}
