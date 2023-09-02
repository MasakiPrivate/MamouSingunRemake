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
        // 今のHPと直前のHPを取得
        int hp = character.GetHP();
        int beforeHp = character.GetBeforeHP();
        // ダメージを受けたフラグ
        bool isDamage = character.IsDamage();
        //　現状の「ノックバックするか」フラグを取得
        bool isKnockBack = character.IsKnockBack();
        
        if(hp <= 0)
            character.SetHP(0);

        // ダメージを受けてないなら終了
        if(isDamage == false)
            return;
        // 「ダメージを受けたフラグ」更新
        character.SetIsDamage(false);
        
        int maxHP = character.GetMaxHP();               // 最大HP
        int kbNum = character.GetKbNum();               // 最大ノックバック回数
        int kbCounter = character.GetKbNumCounter();    // 今のノックバック回数
        int kbLineHp = 0;                               // ノックバックするHP

        if(isKnockBack == false)
        {
            // HPが一定値を下回ったらノックバック
            // 例えば、ノックバック回数が３回なら、HPが2/3,1/3,0/3になったときにノックバックする
            for( int i = kbCounter - 1; i >= 0; i-- )
            {
                kbLineHp = maxHP * i / kbNum;
                if(hp <= kbLineHp && kbLineHp < beforeHp)
                {
                    isKnockBack = true;
                    character.SetKbNumCounter( kbCounter - 1 );
                    break;
                }
            }
        }
        if(isKnockBack == true)
        {
            character.ChangeAction(CharacterAction.damage);
        }
    }
}
