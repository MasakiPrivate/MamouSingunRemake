using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAnimationEvents : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------
    // ---------- クラス変数宣言 ----------
    private Action _onDoudleAttack = ()=>{};
    private Action _onAttack = ()=>{};
    private Action _onAttackEnd = ()=>{};
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    // ---------- Public関数 ----------
    public void OnDoudleAttack(){ _onDoudleAttack(); }
    public void OnAttack(){ _onAttack(); }
    public void OnAttackEnd(){ _onAttackEnd(); }

    public void SetOnDoudleAttack(Action action){ _onDoudleAttack = action; }
    public void SetOnAttack(Action action){ _onAttack = action; }
    public void SetOnAttackEnd(Action action){ _onAttackEnd = action; }
    // ---------- Private関数 ----------
}
