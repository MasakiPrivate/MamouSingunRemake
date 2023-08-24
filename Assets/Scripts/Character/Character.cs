using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Character : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    [SerializeField, Tooltip("キャラクターデータ")] private CharacterData _characterData = default;
    [SerializeField, Tooltip("画像上書き")] private OverrideSprite _overrideSprite = default;
    // ---------- プロパティ ----------
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    void Start()
    {
        SetUp(_characterData, true, -1);
    }

    void Update()
    {
        
    }
    // ---------- Public関数 ----------
    //　セットアップ
    public Character SetUp(CharacterData characterData, bool isAlly ,int pos_z = -1)
    {
        // 奥行に多少のばらつきを持たせる
        if(pos_z == -1)
            pos_z = Random.Range(0, 10);

        // データチェック
        if(characterData != null)
            _characterData = characterData;
        if(_characterData == null)
        {
            Debug.Log("キャラクターデータがありません");
            return null;
        }

        _overrideSprite.overrideTexture = _characterData.texture;

        // データベースからパラメータを読み込んで反映させる

        return this;
    }
    // ---------- Private関数 ----------
    // ---------- protected関数 ----------
}
