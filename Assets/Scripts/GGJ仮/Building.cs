using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    private const int BUILDING_MATERIAL_MAX_NUM = 10;
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    [SerializeField, Tooltip("建材プレハブ")] private BuildingMaterialView _buildingMaterialPrefab;
    // ---------- プロパティ ----------
    private List<BuildingMaterial> _BuildingMaterialList = default;
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    // ---------- Public関数 ----------
    // 建材を生成
    public void CreateBuildingMaterial(MaterialType materialType)
    {
        BuildingMaterialView buildingMaterialView = Instantiate(_buildingMaterialPrefab);
        buildingMaterialView.SetUp(materialType);
        BuildingMaterial buildingMaterial = new BuildingMaterial(materialType, buildingMaterialView);

        AddBuildingMaterialList(buildingMaterial);
    }

    // 建材を積み立てる
    public void AddBuildingMaterialList(BuildingMaterial buildingMaterial)
    {
        _BuildingMaterialList.Add(buildingMaterial);
    }
    // ---------- Private関数 ----------
    
}

public enum MaterialType{
    none,
    straw,
    wood,
    charcoal,
    brick
}

public class BuildingMaterial
{
     // ---------- 定数宣言 ----------
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    private BuildingMaterialView _view;
    private MaterialType _materialType = MaterialType.none;
    private float _hp;
    // ---------- コンストラクタ・デストラクタ ----------
    // ---------- Public関数 ----------
    public BuildingMaterial(MaterialType materialType, BuildingMaterialView view)
    {
        _materialType = materialType;
        _view = view;
    }
    // ---------- protected関数 ----------
    // ---------- Private関数 ----------

}