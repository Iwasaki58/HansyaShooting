using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Manager : MonoBehaviour//カーソルの処理
{
    //カーソルとプレイヤーの弾の発射を管理

    public GameObject Bullet;//弾のプレハブ

    GameObject Player;//プレイヤー

    float HassyaKannkaku = 0.5f;//連射するときの間隔
    float NowTime = 0;//左クリックをしてから押し続ける時間

    Audio audio_script;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");

        Cursor.lockState = CursorLockMode.Confined;//カーソルがウィンドウから出ないようにする

        audio_script = GameObject.Find("Audio_Matome").GetComponent<Audio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.deltaTime != 0)//左クリックで発射
        {
            Shot();
        }else if (Input.GetMouseButtonUp(0))//離すと連射をやめる
        {
            NowTime = 0;
        }else if (Input.GetMouseButton(0))//左クリックを押し続けると等間隔に連射
        {
            NowTime += Time.deltaTime;
            if (NowTime > HassyaKannkaku)//押し続けた時間が決まった時間を過ぎたら
            {
                Shot();
            }
        }
        
    }

    void Shot()//球を放つ
    {
        NowTime = 0;

        Vector3 Player_Vector = new Vector3(Player.transform.position.x, Player.transform.position.y + 1.5f, 0);//プレイヤーの発射位置の場所を求める

        Vector2 dif = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player_Vector;//カーソルとプレイヤーの位置から、球が動く方向を求める

        GameObject bullet = Instantiate(Bullet, Player_Vector, Quaternion.identity);//弾を発射

        bullet.GetComponent<Tama>().Set_Kakudo(dif, true, false);//弾の初期の方向を設定(方向,プレイヤーの弾かどうか,強化版の敵の弾か)

        audio_script.Sound("shot");//発射の音を鳴らす
    }
}
