using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shot
{
    shotNone = -1,
    shotSlime = 1,
    shotWater = 2,
    shotMagic = 3,
    shotBigMagic = 4,
}
public enum Anti
{
    antiGround = 1,
    antiAir = 2,
    antiBoth = 3
}

[CreateAssetMenu(menuName = "MyScriptable/Create CharacterData")]
public class CharacterData : ScriptableObject {
	public string characterName;    // 名前
    public Texture texture;          // テクスチャ
    public Sprite icon;            // アイコン
    public int summonCost;          // 召喚コスト
	public int maxHp;               // HP
	public int atk;                 // 攻撃力
	public int atkCoolTIme;         // 攻撃のクールタイム
	public int shot_flg;            // 使用する射撃弾
	public bool isWideAtk;          // 複数攻撃
    public bool isDoubleAtk;        // ２段攻撃
    public int range;               // 射程
    public Anti anti;                // 対地or対空or対両方
    public float spd;                 // 移動速度
    public bool isFly;              // 飛行
    public bool isFloat;            // 浮き。棒立ち時に歩きのアニメーションをさせるか
    public int waterSpd;            // 水辺での速度
    public int kbNum;               // ノックバック数
    public int dieAction;           // 死亡アクション
    public int buyCost;             // 購入コスト
    public string description;      // 説明
    public Ability abiliry;         // 能力
}
