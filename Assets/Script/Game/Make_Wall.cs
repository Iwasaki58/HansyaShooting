using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Make_Wall : MonoBehaviour
{
    //左右に配置する壁を作る

    public List<GameObject> Wall = new List<GameObject>();//壁のリスト

    GameObject Kabe_Parent;//作成した壁を子要素にしてまとめるオブジェクト

    bool Make_On = false;//false=壁を作らない,true=壁を作る

    float Make_Wall_Kankaku = 3.5f;//壁を作る間隔
    float nowtime = 0;

    // Start is called before the first frame update
    void Start()
    {
        Kabe_Parent = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Make_On == true)//Make_Onがtrueなら左右にある壁を作る
        {
            nowtime += Time.deltaTime;

            if (nowtime > Make_Wall_Kankaku)//一定時間ごとに作成
            {
                nowtime = 0;

                int num = Random.Range(0, Wall.Count);//作成する壁の添え字

                GameObject a = Instantiate(Wall[num], new Vector2(0, 6), Quaternion.identity);//ランダムな壁を作成

                a.transform.parent = Kabe_Parent.transform;
            }
        }
    }

    public void On_Suitti()//壁を作るためのMake_Onを切り替え
    {
        nowtime = 0;

        if (Make_On == true)//trueならfalseに,falseならtrueに
        {
            Make_On = false;
        }
        else
        {
            Make_On = true;
        }
        
    }
}
