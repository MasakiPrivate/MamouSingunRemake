using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------
    [SerializeField, Tooltip("呪文ボタンリスト")] private SpellButtonManager _spellButtonManager;
    [SerializeField, Tooltip("マナUI")] private ManaUIManager _manaUIManager;
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    // ---------- Public関数 ----------
    public void Initialize()
    {
        _spellButtonManager.Initialize();
        _manaUIManager.Initialize();
    }
    // ---------- Private関数 ----------
}
