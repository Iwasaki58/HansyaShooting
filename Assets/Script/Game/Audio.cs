using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    //オーディオで鳴らす効果音を纏める

    public AudioClip[] audioClip;//音源
    AudioSource audioSource;//音を出すコンポーネント

    /*audioclip
        *Tama
            *弾が反射したとき
            *プレイヤーに当たったとき
        
        *Enemy_Dasu
            *敵が撃破されたとき
        
        *matome
            *プレイヤーが撃破されたとき
            *ポーズのオンオフ
        
        *Cursor_Manager
            *弾を発射するとき
        
        *Player
            *アイテムの取得
        
        *Wall_Life
            *バリアが壊される
    */

    /*
     *効果音
         *効果音ラボ
         *魔王魂
         * 
     *BGM
        *魔王魂 
     */

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void Sound(string Sname)//ファイル名を参照して効果音を鳴らす
    {
        for (int i=0;i<audioClip.Length;i++)
        {
            if (audioClip[i] != null && audioClip[i].name.Equals(Sname) == true)
            {
                audioSource.PlayOneShot(audioClip[i]);
                break;
            }
        }
    }
}
