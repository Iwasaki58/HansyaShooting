using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Life : MonoBehaviour
{
    //敵とプレイヤーについているバリア

    int Life = 5;//今の耐久力
    int Max_Life = 5;//耐久力の最大値

    public bool Player_Wall = false;//プレイヤーの壁かどうか

    GameObject Player;

    Audio audio_script;

    // Start is called before the first frame update
    void Start()
    {
        Life = Max_Life;//耐久値を最大値と同じにする

        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();

        if (Player_Wall == true)//プレイヤーの壁ならプレイヤーのオブジェクトを取得
        {
            Player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null) //プレイヤーの壁ならそれに追従
        {
            this.transform.position = new Vector2(Player.transform.position.x,Player.transform.position.y + 1);
        }
    }

    public void Max_Life_kaihuku()//耐久力を全回復
    {
        Life = Max_Life;
    }

    public void Life_Damage()//被弾
    {
        Life--;

        if (Life == 0 && this.transform.parent.gameObject.tag == "Enemy")//敵のバリアなら
        {
            audio_script.Sound("Baria_Break");//バリア破壊の音を鳴らす

            this.transform.parent.gameObject.GetComponent<Enemy>().Wall_Break(this.gameObject.transform);//親オブジェクトの弾の発射位置を増やす

            //コンポーネントを消去(オブジェクトは親オブジェクトの弾の発射位置に必要になるため残す)
            Destroy(this.GetComponent<BoxCollider2D>());
            Destroy(this.GetComponent<SpriteRenderer>());
            Destroy(this);
        }
        else if (Life == 0)//プレイヤーのバリアならオブジェクトを消去
        {
            audio_script.Sound("Baria_Break");//バリア破壊の音を鳴らす
            Destroy(this.gameObject);
        }
    }
}
