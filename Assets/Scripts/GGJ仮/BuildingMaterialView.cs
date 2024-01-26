using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMaterialView : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    // ---------- プロパティ ----------

    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    private MaterialType _materialType = MaterialType.none;
    // ---------- Unity組込関数 ----------
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    // ---------- Public関数 ----------
    public void SetUp(MaterialType materialType)
    {
        _materialType = materialType;
    }
    // ---------- Private関数 ----------
}

