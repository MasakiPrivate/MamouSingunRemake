using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpellButton : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------
    [SerializeField, Tooltip("ボタン")] private Button _button;
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    private Action<CharacterData> _onButton;
    private CharacterData _characterData;
    // ---------- Unity組込関数 ----------
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    // ---------- Public関数 ----------
    public void SetOnButton(Action<CharacterData> onButton){ _onButton = onButton; }
    public void SetCharacterData(CharacterData characterData){ _characterData = characterData; }
    public void Initialize()
    {
        _button.onClick.AddListener(()=>{ _onButton?.Invoke(_characterData); });
    }
    // ---------- Private関数 ----------
}
