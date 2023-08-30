using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameMode
{
    title,
    inGame,
    stageSelect,
    evolution,
    edit
}

public class GameMainManager : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------
    [SerializeField, Tooltip("インゲームマネージャー")] private InGameManager _inGameManager = default;
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    void Start()
    {
        Application.targetFrameRate = 60;
        _inGameManager.Initialize();
    }

    void Update()
    {
        
    }
    // ---------- Public関数 ----------
    // ---------- Private関数 ----------
}
