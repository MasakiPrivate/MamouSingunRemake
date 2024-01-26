using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManaUIManager : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------
    [SerializeField, Tooltip("所持マナ")] private TextMeshProUGUI _currentMana;
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    // ---------- Public関数 ----------
    public void Initialize()
    {
        _currentMana.text = "0";
    }
    // ---------- Private関数 ----------
}
