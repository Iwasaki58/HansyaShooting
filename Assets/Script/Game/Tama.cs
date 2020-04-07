using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tama : MonoBehaviour
{
    //弾
    Rigidbody2D rigid;

    Vector2 Houkou = new Vector2(0,0);//今向いている方向

    float speed = 200f;//スピード

    Audio audio_script;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.parent = GameObject.Find("Tama_Matome").transform;//弾をまとめるオブジェクトの子要素にする

        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();

        rigid = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()//今の方向に移動
    {
        rigid.velocity = Houkou.normalized * speed * Time.deltaTime;//設定されている方向に真っすぐ移動
    }

    public void Set_Kakudo(Vector2 vector2,bool a,bool Elite)//初期の方向を設定(方向,プレイヤーの弾かどうか,強化版の敵の弾か)
    {
        Houkou = vector2;//引数の値を方向に設定

        SpriteRenderer render = this.gameObject.GetComponent<SpriteRenderer>();//色を変えるためにspriterenderを取得
        if (a == true)//プレイヤーの弾だったら
        {
            render.color = Color.red;//プレイヤーなら赤色にする
        }
        else//敵の弾なら
        {
            render.color = Color.black;//敵なら黒色にする

            if (Elite == true)//もし放っている敵が強化版なら
            {
                speed *= 1.5f;//弾のスピードを1.5倍
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)//何かにあたったら
    {
        Vector2 refrectVec = Vector2.Reflect(Houkou, coll.contacts[0].normal);//反射したベクトルを取得
        Houkou = refrectVec;//方向を反射したベクトルに設定

        switch (coll.gameObject.tag)//当たったもののタグを参照
        {
            case "Player"://プレイヤーに当たったら
                coll.gameObject.GetComponent<Player>().Life_Damage();//体力を減らす
                break;
            case "Enemy"://敵に当たったら
                audio_script.Sound("Hansya");//反射した時の音を鳴らす
                coll.gameObject.GetComponent<Enemy>().Life_Damage();//体力を減らす
                break;
            case "Wall"://バリアに当たったら
                audio_script.Sound("Hansya");//反射した時の音を鳴らす
                coll.gameObject.GetComponent<Wall_Life>().Life_Damage();
                break;
            default://ただの壁
                audio_script.Sound("Hansya");//反射した時の音を鳴らす
                break;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
