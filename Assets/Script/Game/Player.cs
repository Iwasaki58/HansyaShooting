using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //プレイヤー

    float Idouryou = 300f;//移動量
    Rigidbody2D Rigidbody2D;

    public GameObject Wall;//バリアのプレハブ
    GameObject Made_Wall;//バリアのオブジェクトを保存

    Matome matome;//体力などはここで管理

    SpriteRenderer Core;//当たり判定があるマルのスプライト

    float Muteki = 1.5f;//被弾後の無敵になれる時間
    float NowTime = 0;//無敵になってからの時間
    bool Now_Muteki = false;//今が無敵状態か

    bool Now_ScoreAttack = false;//今のシーンはスコアアタックか

    Audio audio_script;

    float BariaKyori = 0.8f;//バリアを張るとき、プレイヤーとバリアとの距離

    // Start is called before the first frame update
    void Start()
    {
        Core = this.transform.Find("Core").gameObject.GetComponent<SpriteRenderer>();
        Core.color = Color.green;//色を緑(通常状態)

        Rigidbody2D = this.GetComponent<Rigidbody2D>();

        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "ScoreAttack":
                this.gameObject.transform.position = new Vector2(-2, -3);//初期位置
                matome = GameObject.Find("UI_Ground").GetComponent<Matome>();

                Now_ScoreAttack = true;
                Make_Wall();//初期でバリアを張る
                break;
            case "TimeAttack":
                this.gameObject.transform.position = new Vector2(0, -2);//初期位置
                matome = GameObject.Find("UI_Ground").GetComponent<Matome>();
                break;
            case "Title":
                this.gameObject.transform.position = new Vector2(0, -2);//初期位置
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Now_Muteki == true)//無敵状態なら
        {
            NowTime += Time.deltaTime;

            if (NowTime > Muteki)//無敵時間を過ぎたら
            {
                Core.color = Color.green;
                Now_Muteki = false;
                NowTime = 0;
            }
        }
    }

    void Move()//移動
    {
        float x = Input.GetAxisRaw("Horizontal") * Idouryou * Time.deltaTime;//横の移動
        float y = Input.GetAxisRaw("Vertical") * Idouryou * Time.deltaTime;//縦の移動

        Rigidbody2D.velocity = new Vector2( x,y);//移動

        //操作できる上限
        float hidari = -7.6f;
        float migi = 3.64f;
        float ue = 4;
        float sita = -4.5f;

        if (Now_ScoreAttack == false)//スコアアタックではないなら、それ用の上限に変える
        {
            hidari = -7.35f;
            migi = 7.35f;
            sita = -4.3f;
            ue = 4.2f;
        }

        if (this.transform.position.x < hidari)//左右で規定の座標を過ぎた移動をさせないようにする
        {
            this.transform.position = new Vector2(hidari, this.transform.position.y);
        }
        else if (this.transform.position.x > migi)
        {
            this.transform.position = new Vector2(migi, this.transform.position.y);
        }

        if (this.transform.position.y < sita)//上下で規定の座標を過ぎた移動をさせないようにする
        {
            this.transform.position = new Vector2(this.transform.position.x, sita);
        }
        else if (this.transform.position.y > ue)
        {
            this.transform.position = new Vector2(this.transform.position.x, ue);
        }

    }

    public void Life_Damage()//体力を減らす
    {
        if(Now_Muteki == false)//無敵状態ではないなら
        {
            audio_script.Sound("Hit_Player");//プレイヤーが被弾時の音を鳴らす

            Now_Muteki = true;//無敵状態にする

            Core.color = Color.blue;//当たり判定があるマルの色を青(無敵状態) 

            if (matome != null)//matomeがnullではないなら被弾
            {
                matome.Life_Damage();//1ダメージ
            }
        }
    }

    void Make_Wall()//バリアを張る
    {
        if (Made_Wall == null)//今バリアを張っていないなら作成
        {
            Made_Wall = Instantiate(Wall,new Vector2(this.transform.position.x,this.transform.position.y + BariaKyori),Quaternion.identity);//プレイヤーの少し上
            Made_Wall.GetComponent<Wall_Life>().Set_PlayerKyori(BariaKyori);
            Made_Wall.name = Wall.name;
            Made_Wall.transform.parent = GameObject.Find("Kabe_Parent").transform;
        }
        else//バリアを張っているなら耐久力を全回復
        {
            Made_Wall.GetComponent<Wall_Life>().Max_Life_kaihuku();
        }
    }

    public void Destroy_Wall()//バリアを破壊(ゲームオーバー用)
    {
        if (Made_Wall != null)
        {
            Destroy(Made_Wall);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")//アイテムに触ればその効果を発動
        {
            switch (collision.gameObject.name)//アイテムの種類は名前で判別
            {
                case "Kabe_Item"://壁を張る
                    Make_Wall();
                    break;
                case "Life_Item"://体力を1回復
                    matome.Life_Kaihuku();
                    break;
                case "Bomu_Item"://ボムを1回復
                    matome.Plus_Bomb();
                    break;
            }
            Destroy(collision.gameObject);//アイテムのオブジェクトを削除

            audio_script.Sound("Item");//アイテム取得の音を鳴らす
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")//敵に直接触れたら被弾
        {
            Life_Damage();
        }
    }
}
