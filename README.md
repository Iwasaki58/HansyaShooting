# HansyaShooting

## HansyaShootingの概要

Unityを使用して作成した、弾が反射する2Dのシューティングゲームです。

## 使用したUnityのバージョン

Unity 2019.2.3f1

## 遊び方

- このゲームの弾はプレイヤーや敵、そして他の弾に当たると反射してしまい、縦横無尽に動きます。
- プレイヤーが放った弾に当たっても、プレイヤーはダメージを受けてしまいます。それは敵も同じです。そのため、自分が打った弾は避けなくてはならない弾幕の 一部に変化します。
- プレイヤーはその弾幕を避けるか、被弾しそうな弾に弾を放って跳ね返してください。
- 多くの敵を倒して高得点を目指すことがこのゲームの目的です。

## 操作方法

このゲームはキーボードとマウスを使います。コントローラー等の操作は対応していません。
- WASDキー,方向キー : 移動
- 左クリック : 弾をマウスカーソルの方向に放つ
- 右クリック : 画面上の弾を全て消去する(ボムの残数が0の場合は使用できません)
- スペースキー : ポーズ

## プロジェクトのファイル構成

- HansyaShooting : ゲームのプロジェクトのファイル
  - Assets : Unityプロジェクトで使用するコンテンツのディレクトリ
    - Image : ゲームに使用した画像ファイルのディレクトリ
      - Item : アイテムの画像のディレクトリ
        1. bomu.png : ボムの残数を回復させるアイテムの画像
        2. Life.png : 体力を回復させるアイテムの画像
      - Sonota : アイテム、スプライト以外の画像ファイルのディレクトリ
        1. Cursor.png : マウスカーソルの画像
        2. Hansya_Icon_1024.png : exeファイルのアイコンの画像
        3. WhiteBlock.png : ゲーム中の得点やプレイヤーの情報を表示する部分に表示する画像
      - Sprite : Unityで用意されている図形の画像のディレクトリ
        1. Circle.png : 丸の画像
        2. Square.png : 四角形の画像
        3. Triangle.png : 三角形の画像
    - Object : 使用したオブジェクトのディレクトリ
      - Chara : プレイヤー、敵のオブジェクトのディレクトリ
        - Title_You : タイトル画面用にサイズを調整した敵のオブジェクトのディレクトリ
          1. Enemy_Circle 1.prefab : 丸の敵
          2. Enemy_Circle_Elite 1.prefab : 丸の敵の強化版
          3. Enemy_Sankaku 1.prefab : 三角形の敵
          4. Enemy_Sankaku_Elite 1.prefab : 三角形の敵の強化版
          5. Enemy_sikaku.prefab : 四角形の敵
          6. Enemy_sikaku_Elite.prefab : 四角形の敵の強化版
          7. Enemy_sikaku_Wall 1.prefab : バリアを張っている四角形の敵
          8. sikaku_Wall_Elite 1.prefab : バリアを張っている四角形の敵の強化版
        - Enemy : 敵のオブジェクトのディレクトリ
          1. Enemy_Circle.prefab : 丸の敵
          2. Enemy_Circle_Elite.prefab : 丸の敵の強化版
          3. Enemy_Sankaku.prefab : 三角形の敵
          4. Enemy_Sankaku_Elite.prefab : 三角形の敵の強化版
          5. Enemy_sikaku.prefab : 四角形の敵
          6. Enemy_sikaku_Elite.prefab : 四角形の敵の強化版
          7. Enemy_sikaku_Wall.prefab : バリアを張っている四角形の敵
          8. sikaku_Wall_Elite.prefab : バリアを張っている四角形の敵の強化版
        - Player.prefab : プレイヤーのオブジェクト
      - Item : アイテムのオブジェクトのディレクトリ
        1. Bomu_Item.prefab : ボムを１回復させるアイテムKabe_Item.prefab : プレイヤーの正面にバリアを張るアイテム
        2. Life_Item.prefab : 体力を１回復させるアイテム
      - Tama : 弾のオブジェクトのディレクトリ
        - Title_You : タイトル画面用にサイズを調整した弾のディレクトリ
          1. Kuro_Circle.prefab : 丸の弾のオブジェクト
          2. Sankaku_kuro.prefab : 三角形の弾のオブジェクト
          3. Sikaku_kuro.prefab : 四角形の弾のオブジェクト
        - Kuro_Circle 1.prefab : 丸の弾のオブジェクト
        - Sankaku_kuro 1.prefab : 三角形の弾のオブジェクト
        - Sikaku_kuro 1.prefab : 四角形の弾のオブジェクト
      - UI : 画面上に表示するテキストのオブジェクトのディレクトリ
        1. GameOver.prefab : ゲームが終了した時に表示するテキスト
        2. Pause.prefab : ポーズ中に表示するテキスト
      - Wall : ゲーム中に登場する壁のオブジェクトのディレクトリ
        1. Player_Kabe.prefab : プレイヤー、敵に追従するバリア
        2. Maru_kabe.prefab : 上から下に移動する丸の壁
        3. Sankaku_kabe.prefab : 上から下に移動する三角の壁
        4. Sikaku_kabe.prefab : 上から下に移動する四角の壁
        5. Sikaku_Wall.prefab : 左右にある壁
    - Scene : ゲームのシーンのディレクトリ
      1. Title : タイトル画面のシーン
      2. Game : ゲームを遊ぶ画面のシーン
    - Script : ゲームで使用するプログラムのディレクトリ
      - Game : ゲームを遊ぶシーンで使用するプログラムのディレクトリ
        1. Audio.cs : 効果音のファイルをまとめ、必要な効果音を鳴らすプログラム
        2. Cursor_Manager.cs : マウスカーソルとプレイヤーの弾の発射を管理
        3. Enemy.cs : 敵の行動のプログラム
        4. Enemy_Dasu.cs : 敵を生成するプログラム
        5. Item.cs : アイテムのプログラム
        6. Make_Wall.cs : 上から下に動く動く壁を定期的に生成するプログラム
        7. Matome.cs : プレイヤーの残りの体力とボムの管理、テキストの処理,ゲームオーバーの処理,ポーズの処理,アイテムのドロップの処理を行うプログラム
        8. Move_Wall.cs : 上から下に動く壁を移動させるプログラム
        9. Player.cs : プレイヤーの操作のプログラム
        10. Tama.cs : 弾の移動を行うプログラム
        11. Wall_Life.cs : バリアの体力を管理するプログラム
      - Title.cs : タイトル画面で使用するプログラム
    - Sound : ゲーム中に使用するBGM、効果音のディレクトリ
      1. BGM_Title.mp3 : タイトル画面のBGM
      2. Game_BGM.mp3 : 遊ぶ画面のBGM
      3. Enemy_Gekiha.mp3 : 敵を撃破した時の効果音
      4. Baria_Break.mp3 : バリアが破壊されたときの効果音
      5. Gameover.mp3 : ゲームが終了した時の効果音
      6. Hansya.mp3 : 弾が反射した時の効果音
      7. Hit_Player.mp3 : プレイヤーに球が命中した時の効果音
      8. Item.mp3 : アイテムを取得した時の効果音
      9. Pause.mp3 : ポーズを行った時、解除したときの効果音
      10. shot.mp3 : 弾を打ったときの効果音
      11. Use_Bomu.mp3 : ボムを使用した時の効果音
  - ProjectSettings : プロジェクトの各種設定を保存するディレクトリ
  - Packages : パッケージのディレクトリ
  


## タイトル画面のスクリーンショット

![タイトル画面](./Readme_Screenshot/HansyaShooting_SS_Title.png)

## ゲーム画面のスクリーンショット

![ゲーム画面](./Readme_Screenshot/HansyaShooting_SS_Play.png)
