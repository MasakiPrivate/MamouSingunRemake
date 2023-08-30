using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターの処理１：アクション変更判定
public class CharacterProcess1 : MonoBehaviour
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
        // ゲーム開始演出中は棒立ちさせる（主に敵用）
        if(false)
        {
            character.ChangeAction(CharacterAction.stand);
            return;
        }
        //　敵の城がないステージで勝利したら棒立ちさせる（主に味方用）
        if(false)
        {
            character.ChangeAction(CharacterAction.stand);
            return;
        }

        bool isTargetInRange = false;
        bool isCanAtk = false;

        //攻撃クールタイムを減らす
        character.UpdateAtkCoolTImeCounter();

        //歩きか棒立ち中、攻撃可能な敵がいるか探す
        if( character.GetCurrentAction() == CharacterAction.walk || 
            character.GetCurrentAction() == CharacterAction.stand )
        {
            List<Character> targetList = character.GetTargetTable();
            int count = targetList.Count;
            // 相手テーブル走査
            for(int i = 0; i < targetList.Count; i++)
            {
                Character target = targetList[i];

                // 自身の背後にいるなら無視
                if(character.CheckTargetIsFront(target) == false)
                    continue;

                Anti anti = character.GetAnti();    // 自身の攻撃層を取得
                bool isTargetFly = target.IsFly();  // 相手が飛行しているか

                // 自分が対地で相手が飛行なら無視
                if(anti == Anti.antiGround && isTargetFly == true )
                    continue;

                // 相手との距離を取得
                float targetDistanceX = character.GetDistanceX(target);
                // 自身の射程を取得
                float range = character.GetRange();

                // 自分が対空で相手が地上ならギリギリまで無視
                if(anti == Anti.antiAir && isTargetFly == false )
                    range = 20 * character.GetAwayCorrection();

                // そいつが射程内にいるか
                if( targetDistanceX <= range )
                {
                    // 敵が射程内にいるフラグを立てる
                    isTargetInRange = true;
                    // 攻撃可能かチェック
                    // 自身が飛行しているなら攻撃可能
                    if(character.IsFly() == true)
                    {
                        isCanAtk = true;
                    }
                    // 自身が地上の場合、相手が自身の攻撃層にいるなら攻撃可能
                    else    
                    {
                        if(anti == Anti.antiGround && isTargetFly == false )
                            isCanAtk = true;
                        if(anti == Anti.antiAir && isTargetFly == true )
                            isCanAtk = true;
                        if(anti == Anti.antiBoth)
                            isCanAtk = true;
                    }
                    // 攻撃可能ならループを抜ける
                    if(isCanAtk)
                        break;
                }
            }

                // 敵が射程内にいる
            if(isTargetInRange)
            {
                // 攻撃可能なら攻撃
                if(character.isZeroAtkCoolTImeCounter() && isCanAtk == true )
                    character.ChangeAction(CharacterAction.attack);
                // 攻撃不可なら棒立ち
                else
                    character.ChangeAction(CharacterAction.stand);
            }
            // 敵が射程内にいないなら歩く
            else
            {
                character.ChangeAction(CharacterAction.walk);
            }
        }

        // 「直前のHP」更新
        character.UpdateBeforeHP();
        // 「ダメージを受けたフラグ」更新
        character.SetIsDamage(false);
    }
}
