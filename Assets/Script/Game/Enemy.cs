using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //敵の操作

    public List<Transform> ShotPoint = new List<Transform>();//弾を発射する場所

    public bool Zikinerai = false;//自機狙いか
    bool Escape = false;//画面外に撤退するか

    float NowTime = 0;//弾を発射してから経過した時間
    float ShotKankaku = 5;//弾を放つ間隔

    int Life = 6;//体力

    Rigidbody2D rigid;

    GameObject Player;

    public GameObject Bullet;//発射する弾のプレハブ
    public bool Elite = false;//強化版かどうか

    Vector3 Syokiiti;//初期位置
    bool StartMove = false;//初期位置への移動が終わったどうか

    Enemy_Dasu Enemy_Dasu;

    Audio audio_script;

    // Start is called before the first frame update
    void Start()
    {
        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();

        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");

        Syokiiti = new Vector3(this.transform.position.x, 3.8f,0);//初期位置を設定

        Enemy_Dasu = GameObject.Find("Enemy_Syutugen").GetComponent<Enemy_Dasu>();

        Invoke("Hassya",1);//1秒後に球を発射
    }

    // Update is called once per frame
    void Update()
    {
        if(Escape == false)//通常時
        {
            NowTime += Time.deltaTime;

            if (NowTime > ShotKankaku)//一度発射してから発射間隔を超えれば発射
            {
                Hassya();   
            }
        }else if (Escape == true)//時間切れ時の撤退
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(this.transform.position.x,10f), Time.deltaTime * 8);//少しずつ下に移動
        }

        if (StartMove == false)//初期位置への移動中
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, Syokiiti, Time.deltaTime * 5);

            if (this.transform.position == Syokiiti)//初期位置に到達したら
            {
                StartMove = true;
            }
        }
        
    }

    public void Hassya()//敵の名前と強化版かどうかで、参照して連射する数を判別
    {
        NowTime = 0;

        if(ShotPoint.Count != 0)//発射口になる子要素があるなら
        {
            switch (this.gameObject.name)//敵の名前と強化版かどうかで、連射する数を判別
            {
                case "Enemy_sikaku":
                    StartCoroutine(Make_X_Bullet(3));
                    break;
                case "Enemy_sikaku_Elite":
                    StartCoroutine(Make_X_Bullet(5));
                    break;
                default:
                    if (Elite == true)
                    {
                        StartCoroutine(Make_X_Bullet(2));
                    }
                    else
                    {
                        Shot();//単発
                    }
                    break;
            }
        }
    }

    void Shot()//弾の発射(発射口ごとに1発ずつ)
    {
        for (int i = 0; i < ShotPoint.Count; i++)//場所に応じた角度に発射
        {
            bool Shot_Zikinerai = false;//この弾は自機狙いかどうか

            if ( (ShotPoint[i].gameObject.tag == "Wall" && Zikinerai == false) || (Zikinerai == true))//Zikineraiがture,または発射口の子要素がWallなら
            {
                Shot_Zikinerai = true;
            }

            Vector2 dif;//発射に使う方向
            if (Shot_Zikinerai == true)//自機狙い
            {
                dif = Player.gameObject.transform.position - ShotPoint[i].position;//方向を求める
            }
            else//固定
            {
                dif = ShotPoint[i].position - this.gameObject.transform.position;//方向を求める
            }

            GameObject bullet = Instantiate(Bullet, ShotPoint[i].position, ShotPoint[i].rotation);//弾を発射

            bullet.GetComponent<Tama>().Set_Kakudo(dif, false, Elite);

        }
        audio_script.Sound("shot");//弾を発射するときの音
    }

    IEnumerator Make_X_Bullet(int num)//num 連射
    {
        for (int i=0;i<num;i++)//引数の数だけ発射口から発射する
        {
            Shot();
            yield return new WaitForSeconds(0.5f);//0.5秒間隔
        }
        
    }
    
    public void Escape_On()//時間切れになったとき、画面外に撤退
    {
        Escape = true;//撤退用の移動のフラグ

        this.gameObject.GetComponent<Collider2D>().isTrigger = true;//当たり判定を消す

        foreach (Transform child in this.gameObject.transform)//子要素の当たり判定を消す
        {
            Collider2D collider2D = child.GetComponent<Collider2D>();

            if (collider2D != null)
            {
                collider2D.isTrigger = true;
            }

        }
    }

    public void Life_Damage()//被弾すれば体力を減らす
    {
        Life--;

        if (Life == 0)//体力がなくなれば
        {
            Gekiha(true);
        }
    }

    public void Wall_Break(Transform a)//子要素のバリアが破壊されれば、それがあった場所を発射口に設定
    {
        ShotPoint.Add(a);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Delete")//画面外に出たらこれを消去
        {
            Gekiha(false);
        }
    }

    void Gekiha(bool a)//trueなら得点を増やす,falseなら増やさない(画面外に出たときの消去)
    {
        Debug.Log("大破");

        Vector2 vector2 = this.gameObject.transform.position;//自分の位置

        Enemy_Dasu.Gekiha(vector2,a);//trueなら得点を増やす,falseなら増やさない

        Destroy(this.gameObject);//このオブジェクトを消去
    }
}
