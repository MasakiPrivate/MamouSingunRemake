using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターの処理２：移動
public class CharacterProcess2 : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    private void FixedUpdate()
    {
        if(InGameManager.instance == null)
            return;

        List<Character> allyCharacterList = InGameManager.instance.GetAllyCharacterList();
        List<Character> enemyCharacterList = InGameManager.instance.GetEnemyCharacterList();

        foreach(Character character in allyCharacterList)
            FixedUpdateCharacter(character);
        foreach(Character character in enemyCharacterList)
            FixedUpdateCharacter(character);
    }
    // ---------- Public関数 ----------
    // ---------- Private関数 ----------
    private void FixedUpdateCharacter(Character character)
    {
        float spd = character.GetSPD();
        CharacterAction action = character.GetCurrentAction();

        // 逃げ足は速い
        if( action == CharacterAction.run)
            spd = character.GetSPD() + 2.0f * character.GetAwayCorrection();
        // 向きが逆なら移動も逆に
        if(character.transform.localScale.x < 0)
            spd *= -1.0f;

        // 移動
        if(action == CharacterAction.walk || action == CharacterAction.run)
        {
            character.transform.Translate(spd, 0, 0);
        }
    }
}
