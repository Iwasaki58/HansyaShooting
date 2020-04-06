using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Matome : MonoBehaviour
{
    //テキストの処理,ゲームオーバーの処理,ポーズの処理,アイテムのドロップ

    GameObject Tama_Matome;//作成された弾を子要素にしてまとめているオブジェクト

    public GameObject Over_Object;//ゲームオーバー時に表示されるオブジェクトのプレハブ

    public GameObject Pause_Object;//ポーズ時に表示されるオブジェクトのプレハブ
    GameObject Now_Pause = null;//ポーズで表示されるオブジェクト
    bool stop = false;//ポーズ中かどうか

    GameObject canvas;//キャンバス

    public GameObject[] Items;//敵からドロップされるアイテム

    //子要素のテキストのオブジェクト
    Text Score_Text;
    Text HighScore_Text;
    Text Life_Text;
    Text Bomu_Text;

    int Score = 0;//得点
    int HighScore = 0;//最高得点

    int Max_Bomu = 3;//ボムの最大値
    int Bomu = 0;//ボムの所持数

    int Max_Life = 5;//プレイヤーの体力の最大値
    int Life = 0;//プレイヤーの体力

    Enemy_Dasu Enemy_Dasu;

    Make_Wall Make_Wall;

    bool GameEnd = false;//false=ゲームが始まっている,true=ゲームが終わっている

    Audio audio_script;//タイトルを参照して音を鳴らす

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");

        //子要素のテキストのオブジェクトを取得
        Score_Text = this.transform.Find("Score_Text").GetComponent<Text>();
        HighScore_Text = this.transform.Find("HighScore_Text").GetComponent<Text>();
        Life_Text = this.transform.Find("Life_Text").GetComponent<Text>();
        Bomu_Text = this.transform.Find("Bomu_Text").GetComponent<Text>();

        Tama_Matome = GameObject.Find("Tama_Matome");

        Enemy_Dasu = GameObject.Find("Enemy_Syutugen").GetComponent<Enemy_Dasu>();

        Make_Wall = GameObject.Find("Kabe_Parent").GetComponent<Make_Wall>();

        Life = Max_Life;//体力を最大値に設定
        Bomu = Max_Bomu;//ボムの所持数を最大値に設定

        HighScore = PlayerPrefs.GetInt("HighScore", 0);//最高得点をロード,データがなければ0を返す

        //テキストを変更
        Life_Text.text = "体力 : " + Life;
        HighScore_Text.text = "ハイスコア : " + HighScore;
        Score_Text.text = "撃破 : " + Score;
        Bomu_Text.text = "ボム : " + Bomu;

        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEnd == false)//ゲームがまだ終わっていない
        {
            if (Input.GetMouseButtonDown(1))//右クリックでボム使う
            {
                Use_Bomu();
            }
            else if (Input.GetKeyDown(KeyCode.Space) )//ポーズ
            {
                Pause();
            } 
        }else if (GameEnd == true && Input.GetKeyDown(KeyCode.Space))//ゲーム終了後,もう一度遊ぶ
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

    private void Pause()//ポーズ
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

    public void Life_Kaihuku()//プレイヤーの体力を回復
    {
        if (Max_Life > Life)//体力が最大値未満なら回復
        {
            Life++; //体力を増やす
            Life_Text.text = "体力 : " + Life;//体力のテキストを更新
        }
        
    }

    public void Life_Damage()//プレイヤーの体力を減らす
    {
        Life--;//体力を減らす

        Life_Text.text = "体力 : " + Life;//体力のテキストを更新

        if (Life == 0)//ゲームオーバー
        {
            GameEnd = true;
            Time.timeScale = 0;//時間を止める

            GameObject Player = GameObject.Find("Player");//プレイヤーのオブジェクトを取得
            Player.GetComponent<Player>().Destroy_Wall();//プレイヤーのバリアを消去
            Destroy(Player);//プレイヤーを消去

            if (Score > HighScore)//ハイスコアを更新したら
            {
                PlayerPrefs.SetInt("HighScore", Score);//ハイスコアを保存する
                PlayerPrefs.Save();
            }

            AudioSource BGM_Audio = GameObject.Find("BGM").GetComponent<AudioSource>();
            BGM_Audio.Stop();//BGMを止める
            audio_script.Sound("Gameover");//ゲームオーバーの効果音

            GameObject obje = Canvas_Text_Make(Over_Object);//ゲームオーバー時のテキストを表示
            obje.transform.Find("Score_Text").GetComponent<Text>().text = "撃破数 : " + Score + "体";//テキストに得点を表示
        }
    }

    GameObject Canvas_Text_Make(GameObject a)//キャンバスにテキストオブジェクトを作り,キャンバスの子要素に移動させる
    {
        GameObject Obje = Instantiate(a);//引数のオブジェクトを作る

        Obje.transform.parent = canvas.transform;//キャンバスの子要素にする

        Obje.transform.localPosition = new Vector3(0, 100, 0);//場所を変更

        Obje.transform.localScale = new Vector3(1, 1, 1);//スケールを変更

        return Obje;//作ったオブジェクトを返す
    }

    public void Plus_Score(Vector2 enemy_point)//得点を増やす
    {
        Score++;//得点を増やす

        Return_RandItem(enemy_point);//アイテムをドロップ

        Score_Text.text = "撃破 : " + Score;

        if (Score > HighScore)//ハイスコアを超えたら、テキストを変更
        {
            HighScore_Text.text = "ハイスコア : " + Score;
        }

        if (Score % 18 == 0 && Score != 0)//得点に応じてエリートを出す数を増やす
        {
            Enemy_Dasu.Elite_On_Num();
        }

        if (Score % 12 == 0 && Score != 0 && Make_Wall != null)//得点に応じて左右の壁を出し入れ
        {
            Make_Wall.On_Suitti();
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

            Bomu_Text.text = "ボム : " + Bomu;//テキストを更新
        }        
    }

    public void Use_Bomu()//ボムを使う
    {
        if (Bomu > 0 && Time.deltaTime != 0)//ポーズ中ではなく、ボムが残っているのなら
        {
            Bomu--;//ボムを減らす

            Bomu_Text.text = "ボム : " + Bomu;//テキストを更新

            foreach (Transform transform in Tama_Matome.transform)//子要素の弾を全て消す
            {
                Destroy(transform.gameObject);
            }

            audio_script.Sound("Use_Bomu");//ボム使用時の効果音
        }
    }
}
