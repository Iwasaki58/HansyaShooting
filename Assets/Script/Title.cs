using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    //タイトル画面のボタン

    public void GameEnd()//ゲームを終わらせる(ウインドウを閉じる)
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
              UnityEngine.Application.Quit();
        #endif
    }

    public void ScoreAttackStart()//スコアアタック
    {
        SceneManager.LoadScene("ScoreAttack");
    }

    public void TimeAttackStart()//タイムアタック
    {
        SceneManager.LoadScene("TimeAttack");
    }
}
