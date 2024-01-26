using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellButtonManager : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------
    [SerializeField, Tooltip("キャラクターデータ")] private CharacterData _characterData;
    [SerializeField, Tooltip("呪文ボタンリスト")] private List<SpellButton> _spellButtonList = default;
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
        // 呪文ボタン初期化
        int count = _spellButtonList.Count;
        for(int i = 0; i < count; i++)
        {
            SpellButton spellButton = _spellButtonList[i];
            spellButton.Initialize();
            spellButton.SetCharacterData(_characterData);
            spellButton.SetOnButton(value=>{
                InGameManager.instance.CreateCharacter(value, true, -1);
            });
        }
    }
    // ---------- Private関数 ----------
}
