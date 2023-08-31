using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターの処理４：ダメージ計算後の処理
public class CharacterProcess4 : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    private void Update()
    {
        if(InGameManager.instance == null)
            return;

        List<Character> allyCharacterList = InGameManager.instance.GetAllyCharacterList();
        List<Character> enemyCharacterList = InGameManager.instance.GetEnemyCharacterList();

        foreach(Character character in allyCharacterList)
            UpdateCharacter(character);
        foreach(Character character in enemyCharacterList)
            UpdateCharacter(character);
    }
    // ---------- Public関数 ----------
    // ---------- Private関数 ----------
    private void UpdateCharacter(Character character)
    {
        int Hp = character.GetHP();
        int beforeHp = character.GetBeforeHP();
        bool isDamage = character.GetIsDamage();

    }
}
