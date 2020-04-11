using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Dasu : MonoBehaviour
{
    //敵を出すスクリプト

    public List<GameObject> Enemys = new List<GameObject>();//敵のリスト
    public List<GameObject> Enemys_Elite = new List<GameObject>();//敵の強化版のリスト

    Matome matome;

    float Spawn_Kankaku = 30;//敵を再出現させるための間隔
    float Nowtime = 0;

    int Enemy_Count = 0;//場にいる敵の数(数が0になればその場で再出現)

    int Elite_Dasu = 0;//強化版を出す数

    Audio audio_script;

    bool Game_Now = false;//今のシーンがTitleかどうか(false=タイトル,true=それ以外)

    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)//今のシーンがタイトル画面ではないなら
        {
            case "ScoreAttack"://スコアアタック
                matome = GameObject.Find("UI_Ground").GetComponent<Matome>();
                Game_Now = true;
                break;
            case "TimeAttack"://タイムアタック
                matome = GameObject.Find("UI_Ground").GetComponent<Matome>();
                break;
        }

        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();

        Enemy_Spawn();//敵を出す
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime != 0)
        {
            Nowtime += Time.deltaTime;

            if ( (Nowtime > Spawn_Kankaku) || (Enemy_Count == 0) )//敵の数が0,もしくは敵が出現してから一定期間時間が経過したら
            {
                Enemy_Spawn();//敵を出す
            }
        }
    }

    void Enemy_Spawn()//ランダムに敵を出す
    {
        if (Enemys.Count != 0)//敵のプレハブのリストに要素があれば
        {
            Nowtime = 0;

            foreach (Transform child in this.gameObject.transform)//子要素内の敵を撤退させる
            {
                child.gameObject.GetComponent<Enemy>().Escape_On();
            }

            float X = -5.5f;//配置する敵のxの座標
            float X_plus = 3.5f;//次に配置する敵のx座標の間隔

            if (Game_Now == false)//今のシーンがスコアアタックではないなら、x座標を変更
            {
                X = -5.5f;
                X_plus = 5.5f;
            }

            bool[] Elite_bool = { false, false, false };//各敵が強化版かどうか
            for (int i = 0; i < Elite_Dasu; i++)
            {
                List<int> false_List = new List<int>();//Elite_boolのfalseになっている場所

                for (int n=0;n<Elite_bool.Length;n++)//Elite_boolのfalseになっている場所を探す
                {
                    if (Elite_bool[n] == false)
                    {
                        false_List.Add(n); //false_Listに加える
                    }
                }

                int num = false_List[Random.Range(0,false_List.Count)];//ランダムのfalse_Listの添え字

                Elite_bool[num] = true;//添え字の場所をtrueにする(強化版が出るようにする)
            }
            

            for (int i = 0; i < 3; i++) {//左から敵を生成
                if (Elite_bool[i] == false)//通常の敵
                {
                    Make_Enemy(Enemys, X);//敵を作る
                }
                else//強化版
                {
                    Make_Enemy(Enemys_Elite, X);//敵を作る
                }

                X += X_plus;//次に作成する敵のx座標を求める
            }

            Enemy_Count = 0;//敵の数を初期化
            foreach (Transform child in this.gameObject.transform)//子要素の数を数える
            {
                Enemy_Count++;
            }
        }
    }

    void Make_Enemy(List<GameObject> gameObjects,float X)//敵を作る
    {
        int num = Random.Range(0, gameObjects.Count);//敵のリストの添え字を乱数で求める
        GameObject ene = Instantiate(gameObjects[num], new Vector2(X, 6.6f),gameObjects[num].transform.rotation);//敵を作る
        ene.name = gameObjects[num].name;
        ene.transform.parent = this.gameObject.transform;//このオブジェクトの子要素にする
    }

    public void Elite_On_Num()//強化版の敵を作る数を増やす
    {
        if (Elite_Dasu < 3)
        {
            Elite_Dasu++;
        }
        
    }

    public void Gekiha(Vector2 enemy_point, bool Gekiha_boll)//敵を撃破した時の処理(撃破された敵の位置,撃破されているか撤退か)
    {
        Enemy_Count--;//場にいる敵の数を減らす

        if (Gekiha_boll == true)//弾に被弾して倒されたのなら
        {
            audio_script.Sound("Enemy_Gekiha");//敵が撃破されたときの音を鳴らす

            if (matome != null)
            {
                matome.Plus_Score(enemy_point);//得点を増やす
            }
        }
    }
}
