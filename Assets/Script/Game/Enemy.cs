using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    //敵の操作

    public List<int> ShotKakudo = new List<int>();//球を発射する角度
    public GameObject Bullet;//発射する弾のプレハブ

    public bool Zikinerai = false;//自機狙いか
    bool Escape = false;//画面外に撤退するか
    bool BariaBreak = false;//バリアを持っている敵キャラクターがバリアを壊されたかどうか(true=バリアを壊された , false=壊されていない,バリアを持っていない)

    float NowTime = 0;//弾を発射してから経過した時間
    float ShotKankaku = 5;//弾を放つ間隔
    float Shot_Point_Kyori = 1.05f;//自分の場所と球を発射する場所の距離

    int Life = 6;//体力

    Rigidbody2D rigid;

    Enemy_Dasu Enemy_Dasu;
    Audio audio_script;

    GameObject Player;

    bool Elite = false;//強化版かどうか(false=通常,true=強化版)

    Vector3 Syokiiti;//初期位置
    bool StartMove = false;//初期位置への移動が終わったどうか(false=移動中,true=移動終了)

    // Start is called before the first frame update
    void Start()
    {
        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();
        Enemy_Dasu = GameObject.Find("Enemy_Syutugen").GetComponent<Enemy_Dasu>();
        rigid = this.gameObject.GetComponent<Rigidbody2D>();

        Player = GameObject.Find("Player");

        //シーンがタイトルかタイムアタックだったら
        if (SceneManager.GetActiveScene().name.Equals("Title") == true || SceneManager.GetActiveScene().name.Equals("TimeAttack") )
        {
            Vector2 Scale = this.gameObject.transform.localScale; //scallを0.3増やす
            this.gameObject.transform.localScale = new Vector2(Scale.x + 0.3f,Scale.y + 0.3f);

            Shot_Point_Kyori += 0.2f;
        }

        if (this.gameObject.name.Contains("Elite") == true)//名前にEliteとついていたら
        { 
            Elite = true; //強化版に設定
        }

        Syokiiti = new Vector3(this.transform.position.x, 3.8f,0);//初期位置を設定

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
            this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(this.transform.position.x,10f), Time.deltaTime * 8);//少しずつ上に移動
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

        if(ShotKakudo.Count != 0)//発射口になる子要素があるなら
        {
            //敵の名前と強化版かどうかで、連射する数を判別
            if (this.gameObject.name.Contains("Enemy_Rhombus"))//ひし形の敵なら
            {
                if (Elite == true)//強化版なら
                {
                    StartCoroutine(Make_X_Bullet(5));
                }
                else
                {
                    StartCoroutine(Make_X_Bullet(3));
                }
            }
            else//ひし形以外の敵なら
            {
                if (Elite == true)//強化版なら
                {
                    StartCoroutine(Make_X_Bullet(2));
                }
                else
                {
                    Shot();//単発
                }
            }
        }
    }

    IEnumerator Make_X_Bullet(int num)//num 連射
    {
        for (int i = 0; i < num; i++)//引数の数だけ発射口から発射する
        {
            Shot();
            yield return new WaitForSeconds(0.5f);//0.5秒間隔
        }

    }

    void Shot()//弾の発射(発射口ごとに1発ずつ)
    {
        for (int i = 0; i < ShotKakudo.Count; i++)//場所に応じた角度に発射
        {
            bool Shot_Zikinerai = false;//この弾は自機狙いかどうか

            if ( (Escape == false && BariaBreak == true && ShotKakudo[i] == 0 && Zikinerai == false) || (Escape == false && Zikinerai == true))//Zikineraiがture,またはバリアが壊されたのなら
            {
                Shot_Zikinerai = true;
            }

            //  三角関数で球を作る場所を求める(後でこの設定に対応させ、自機狙いの設定をこれに対応させる)
            float radian = ShotKakudo[i] * Mathf.Deg2Rad;

            float x = this.gameObject.transform.position.x + (Shot_Point_Kyori * Mathf.Sin(radian));
            float y = this.gameObject.transform.position.y - (Shot_Point_Kyori * Mathf.Cos(radian));

            Vector3 Ball_Make_Position = new Vector3(x, y, 0);//球を作る座標

            Vector2 dif;//発射に使う方向ベクトル
            if (Shot_Zikinerai == true)//自機狙い
            {
                dif = Player.gameObject.transform.position - Ball_Make_Position;//方向ベクトルを求める
            }
            else//固定
            {
                dif = Ball_Make_Position - this.gameObject.transform.position;//方向ベクトルを求める
            }

            GameObject bullet = Instantiate(Bullet, Ball_Make_Position, Quaternion.identity);//弾を発射

            bullet.GetComponent<Tama>().Set_Kakudo(dif, false, Elite);

        }
        audio_script.Sound("shot");//弾を発射するときの音
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

        ShotKakudo = new List<int>() { -80, -45, 0, 45, 80 };//最後に多くの弾を発射する
        Shot();
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
        BariaBreak = true;
        ShotKakudo.Add(0);
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
        Vector2 vector2 = this.gameObject.transform.position;//自分の位置

        Enemy_Dasu.Gekiha(vector2,a);//trueなら得点を増やす,falseなら増やさない

        Destroy(this.gameObject);//このオブジェクトを消去
    }
}
