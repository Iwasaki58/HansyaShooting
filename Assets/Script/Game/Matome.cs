using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Matome : MonoBehaviour
{
    //テキストの処理,ゲームオーバーの処理,ポーズの処理,アイテムのドロップ

    bool Now_ScoreAttack = false;//今のシーンがスコアアタックモードかどうか(false=タイムアタック,true=スコアアタック)

    GameObject Tama_Matome;//作成された弾を子要素にしてまとめているオブジェクト

    public GameObject Over_Object;//ゲームオーバー時に表示されるオブジェクトのプレハブ

    public GameObject Pause_Object;//ポーズ時に表示されるオブジェクトのプレハブ
    GameObject Now_Pause = null;//ポーズで表示されるオブジェクト
    bool stop = false;//ポーズ中かどうか

    GameObject canvas;//キャンバス

    public GameObject[] Items;//敵からドロップされるアイテム

    //スコアアタックの子要素のテキストのオブジェクト
    Text Score_Text;
    Text HighScore_Text;
    Text Life_Text;
    Text Bomu_Text;

    //タイムアタックの子要素のテキストのオブジェクト
    Text Time_Text;
    Text High_Time_Text;

    int Score = 0;//スコアアタックの得点
    int HighScore = 0;//最高得点

    int Max_Bomu = 3;//ボムの最大値
    int Bomu = 0;//ボムの所持数

    int Max_Life = 5;//プレイヤーの体力の最大値
    int Life = 0;//プレイヤーの体力

    float Time_Score = 0;//タイムアタックで計測する時間
    float High_Time = 0;//最高時間
    int Time_Sevent_Koeta = 1;//経過時間が70の倍数になるたびに増える(経過時間に応じた、強化された敵を出す数を決めることに使用)
    int Time_Fift_Koeta = 1;//経過時間が50の倍数になるたびに増える(経過時間に応じて、左右に壁を出現させる処理に使用)

    Enemy_Dasu Enemy_Dasu;
    Make_Wall Make_Wall;
    Audio audio_script;//タイトルを参照して音を鳴らす

    bool GameEnd = false;//false=ゲームが始まっている,true=ゲームが終わっている

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");

        if (SceneManager.GetActiveScene().name.Equals("ScoreAttack"))//今のシーンがスコアアタックなら
        {
            Now_ScoreAttack = true;

            //子要素のテキストのオブジェクトを取得
            Score_Text = this.transform.Find("Score_Text").GetComponent<Text>();
            HighScore_Text = this.transform.Find("HighScore_Text").GetComponent<Text>();
            Life_Text = this.transform.Find("Life_Text").GetComponent<Text>();
            Bomu_Text = this.transform.Find("Bomu_Text").GetComponent<Text>();

            Life = Max_Life;//体力を最大値に設定
            Bomu = Max_Bomu;//ボムの所持数を最大値に設定

            HighScore = PlayerPrefs.GetInt("HighScore", 0);//最高得点をロード,データがなければ0を返す

            //テキストを変更
            Hyouzi_Life();
            Hyouzi_Bomu();
            Hyouzi_Score(false);
        }
        else
        {
            High_Time = PlayerPrefs.GetFloat("HighTime", 0);

            Time_Text = this.transform.Find("Time_Text").GetComponent<Text>();
            High_Time_Text = this.transform.Find("High_Time_Text").GetComponent<Text>();

            Hyouzi_Time(false);//経過時間を表示
        }

        Tama_Matome = GameObject.Find("Tama_Matome");

        Enemy_Dasu = GameObject.Find("Enemy_Syutugen").GetComponent<Enemy_Dasu>();
        Make_Wall = GameObject.Find("Kabe_Parent").GetComponent<Make_Wall>();
        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEnd == false)//ゲームがまだ終わっていない
        {
            if (Input.GetMouseButtonDown(1) && Now_ScoreAttack == true)//スコアアタック中なら、右クリックでボムを使える
            {
                Use_Bomu();
            }
            else if (Input.GetKeyDown(KeyCode.Space) )//ポーズ
            {
                Pause();
            }

            if (Now_ScoreAttack == false)//タイムアタックなら時間を加算する
            {
                Time_Score_Plus();
            }
        }
        else if (GameEnd == true && Input.GetKeyDown(KeyCode.Space))//ゲーム終了後,もう一度遊ぶ
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if ((stop == true || GameEnd == true) && Input.GetKeyDown(KeyCode.Return))//ポーズ中かゲーム終了後
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Title");//タイトルに戻る
        }
    }

    void Pause()//ポーズ
    {
        if (stop == false)//ポーズ状態にする
        {
            Now_Pause = Canvas_Text_Make(Pause_Object);//ポーズ中に出すテキストを表示

            Time.timeScale = 0;//時間を止める
            stop = true;
        }
        else//ポーズ状態を解除
        {
            Destroy(Now_Pause);//ポーズ中に出すテキストを消去

            Time.timeScale = 1;
            stop = false;
        }

        audio_script.Sound("Pause");//ポーズの効果音
    }

    void Time_Score_Plus()//時間を加える
    {
        Time_Score += Time.deltaTime;

        Hyouzi_Time(true);

        float Bun = Kirisute(Time_Score);//小数点第1位までに切り下げる

        if (Bun == 70 * Time_Sevent_Koeta)//70の倍数になるたびに強化版の敵が出る数を増やす(最大3)
        {
            Time_Sevent_Koeta++;
            Enemy_Dasu.Elite_On_Num();
        }
        if (Bun == 50 * Time_Fift_Koeta && Make_Wall != null)
        {
            Time_Fift_Koeta++;
            Make_Wall.On_Suitti();
        }

    }

    void Hyouzi_Time(bool GameStart)//経過時間を表示
    {
        Time_Text.text = "タイム : " + Kirisute(Time_Score);

        if (Time_Score > High_Time)//最高タイムを更新したら、文字を更新
        {
            High_Time_Text.text = "ハイスコア : " + Kirisute(Time_Score);
        }else if (GameStart == false)//初期設定
        {
            High_Time_Text.text = "ハイスコア : " + Kirisute(High_Time);
        }
    }

    public void Life_Kaihuku()//プレイヤーの体力を回復
    {
        if (Max_Life > Life)//体力が最大値未満なら回復
        {
            Life++; //体力を増やす
            Hyouzi_Life();
        }
    }

    public void Life_Damage()//プレイヤーの体力を減らす
    {
        if(Now_ScoreAttack == true)//スコアアタック
        {
            Life--;//体力を減らす

            Hyouzi_Life();

            if (Life == 0)//ゲームオーバー
            {
                string Gameover_Text = "撃破数 : " + Score + "体";//最終結果のテキスト

                GameOver(Gameover_Text);

                if (Score > HighScore)//ハイスコアを更新したら
                {
                    PlayerPrefs.SetInt("HighScore", Score);//ハイスコアを保存する
                    PlayerPrefs.Save();
                }

            }
        }
        else//タイムアタックなら一度でも被弾したら終了
        {
            string Gameover_Text = "タイム : " + Kirisute(Time_Score) + "秒";//最終結果のテキスト

            GameOver(Gameover_Text);

            if (Time_Score > High_Time)//ハイスコアを更新したら
            {
                PlayerPrefs.SetFloat("HighTime", Time_Score);//ハイスコアを保存する
                PlayerPrefs.Save();
            }
        }
    }

    void Hyouzi_Life()//体力の残数を更新
    {
        Life_Text.text = "体力 : " + Life;//体力のテキストを更新
    }

    void GameOver(string Gameover_Text)//ゲームオーバーの処理
    {
        GameEnd = true;
        Time.timeScale = 0;//時間を止める

        GameObject Player = GameObject.Find("Player");//プレイヤーのオブジェクトを取得
        Player.GetComponent<Player>().Destroy_Wall();//プレイヤーのバリアを消去
        Destroy(Player);//プレイヤーを消去

        AudioSource BGM_Audio = GameObject.Find("BGM").GetComponent<AudioSource>();
        BGM_Audio.Stop();//BGMを止める
        audio_script.Sound("Gameover");//ゲームオーバーの効果音

        GameObject obje = Canvas_Text_Make(Over_Object);//ゲームオーバー時のテキストを表示
        obje.transform.Find("Score_Text").GetComponent<Text>().text = Gameover_Text;//テキストに得点を表示

    }

    GameObject Canvas_Text_Make(GameObject a)//キャンバスにテキストオブジェクトを作り,キャンバスの子要素に移動させる
    {
        GameObject Obje = Instantiate(a);//引数のオブジェクトを作る

        Obje.transform.parent = canvas.transform;//キャンバスの子要素にする

        if (Now_ScoreAttack == true)//スコアアタック
        {
            Obje.transform.localPosition = new Vector3(-215, 100, 0);//場所を変更

            Obje.transform.localScale = new Vector3(1, 1, 1);//スケールを変更
        }
        else//タイムアタック
        {
            Obje.transform.localPosition = new Vector3(0, 100, 0);//場所を変更

            Obje.transform.localScale = new Vector3(1, 1, 1);//スケールを変更

        }

        return Obje;//作ったオブジェクトを返す
    }

    public void Plus_Score(Vector2 enemy_point)//敵を撃破した時、得点を増やす
    {
        if (Now_ScoreAttack == true)//スコアアタック
        {
            Score++;//得点を増やす

            Return_RandItem(enemy_point);//アイテムをドロップ

            Hyouzi_Score(true);

            if (Score % 18 == 0 && Score != 0)//得点に応じてエリートを出す数を増やす
            {
                Enemy_Dasu.Elite_On_Num();
            }

            if (Score % 12 == 0 && Score != 0 && Make_Wall != null)//得点に応じて左右の壁を出し入れ
            {
                Make_Wall.On_Suitti();
            }

        }
    }

    void Hyouzi_Score(bool GameStart)//得点を表示
    {
        Score_Text.text = "撃破\n" + Score;

        if (Score > HighScore)//ハイスコアを超えたら、テキストを変更
        {
            HighScore_Text.text = "ハイスコア\n" + Score;
        }else if (GameStart == false)
        {
            HighScore_Text.text = "ハイスコア\n" + HighScore;
        }
    }

    void Return_RandItem(Vector2 enemy_point)//アイテムをドロップさせる
    {
        int rand = Random.Range(0, 3);//３分の1でアイテムをドロップ

        if (rand == 0)//ランダムなアイテムを倒した敵の位置に作成
        {
            int num = Random.Range(0, Items.Length);

            GameObject a = Instantiate(Items[num], enemy_point, Quaternion.identity);
            a.name = Items[num].name;
        }
    }

    public void Plus_Bomb()//ボムを増やす
    {
        if (Bomu < Max_Bomu)//ボムの所持数が最大値未満なら
        {
            Bomu++;//ボムを増やす

            Hyouzi_Bomu();
        }        
    }

    void Use_Bomu()//ボムを使う
    {
        if (Bomu > 0 && stop == false)//ポーズ中ではなく、ボムが残っているのなら
        {
            Bomu--;//ボムを減らす

            Hyouzi_Bomu();

            foreach (Transform transform in Tama_Matome.transform)//子要素の弾を全て消す
            {
                Destroy(transform.gameObject);
            }

            audio_script.Sound("Use_Bomu");//ボム使用時の効果音
        }
    }
    
    void Hyouzi_Bomu()//ボムの残数を更新
    {
        Bomu_Text.text = "ボム : " + Bomu;
    }

    float Kirisute(float num)//小数を小数点第一位まで切り捨てる
    {
        float Result = Mathf.Floor(num * 10) / 10;

        return Result;
    }
}
