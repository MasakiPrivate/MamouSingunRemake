using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターの処理３：ヒットチェックとダメージ計算
public class CharacterProcess3 : MonoBehaviour
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
        if(character.IsAttack() == false)
            return;
        // 攻撃判定をおろす
        character.UnAttack();

        // 弾を撃つタイプなら発射してオワリ
        if( character.GetShot() != CharacterShot.none)
        {
            character.FireShot();
            return;
        }
        // 単体攻撃用、一番近い敵を記録
        float target_min_range = -1.0f;
        Character deside_target = null;

        // 攻撃が当たったフラグ
        bool isAttackHit = false;

        List<Character> targetList = character.GetTargetTable();
        int count = targetList.Count;
        // 相手テーブル走査
        for(int i = 0; i < targetList.Count; i++)
        {
            Character target = targetList[i];

            // 相手のライフはもうとっくに０なら虫
            if(target.GetHP() <= 0)
                continue;

            // 相手との距離を取得
            float targetDistanceX = character.GetDistanceX(target);
            // 自身の射程を取得
            float range = character.GetRange();
            // 射程外なら無視
            if(range < targetDistanceX)
                continue;

            // 自身が地上の場合
            if(character.IsFly() == false)
            {
                Anti anti = character.GetAnti();    // 自身の攻撃層を取得
                bool isTargetFly = target.IsFly();  // 相手が飛行しているか
                // 自分が対地で相手が飛行なら無視
                if(anti == Anti.antiGround && isTargetFly == true )
                    continue;
                    
                // 自分が対空で相手が地上なら無視
                if(anti == Anti.antiAir && isTargetFly == false )
                    continue;
                    
            }
            // 自身の背後にいるなら無視
            if(character.CheckTargetIsFront(target) == false)
                continue;
                
            // 相手が怯み中なら無視
            if(target.GetCurrentAction() == CharacterAction.damage)
                continue;
                
            // 単体攻撃なら一番距離の短い相手をターゲットにする
            if(character.IsWideAtk() == false)
            {
                if(target_min_range >= targetDistanceX || target_min_range < 0)
                {
                    target_min_range = targetDistanceX;
                    deside_target = target;
                }
            }
            // 複数攻撃なら相手にダメージを与える
            else
            {
                DamageProcess(character, target);
                isAttackHit = true;
            }
        }
        // 単体攻撃なら、ターゲットにした１体にダメージを与える
        if( character.IsWideAtk() == false && deside_target != null )
        {
            DamageProcess(character, deside_target);
            isAttackHit = true;
        }

        // 攻撃がヒットしたなら
        if(isAttackHit == true && character.GetAtk() < 0)
        {
            // ヒットエフェクト発生
            // ヒット効果音発生
        }
    }

    // ダメージ処理
    private void DamageProcess(Character character, Character target)
    {
        int dmg = character.GetAtk();
        int targetHp = target.GetHP();

        targetHp -= dmg;
        target.SetHP(targetHp);
        target.SetIsDamage(true);
    }
}
