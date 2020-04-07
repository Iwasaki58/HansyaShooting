using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Wall : MonoBehaviour
{
    //左右の壁についている図形を動かす

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()//上から下に動かす
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(this.transform.position.x, -18f), Time.deltaTime * 1.5f);//少しずつ下に移動
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Delete")//画面外に出たらこれを消去
        {
            Destroy(this.gameObject);
        }
    }
}
