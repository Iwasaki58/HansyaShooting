using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //アイテム

    float Move = -110f;//スピード
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()//とにかく下に移動
    {
        rigid.velocity = new Vector2(0, Move * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Delete")//画面外に出たらこれを消去
        {
            Destroy(this.gameObject);
        }
    }
}
