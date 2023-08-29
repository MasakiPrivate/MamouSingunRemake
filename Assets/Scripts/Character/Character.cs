using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum CharacterAction
{
    walk,
    attack,
    stand,
    damage,
    run,
    dead,
    vanish
}

public class Character : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    private const float AWAY_CORRECTION = 0.0225f;
    private string[] character_anim_parameter = {"Attack", "Walk", "Stand", "Damage", "Run", "Dead"};
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    [SerializeField, Tooltip("キャラクターデータ")] private CharacterData _characterData = default;
    [SerializeField, Tooltip("画像上書き")] private OverrideSprite _overrideSprite = default;
    [SerializeField, Tooltip("アニメーション")] private Animator _animator = default;
    [SerializeField, Tooltip("アニメーションコールバック")] private CharacterAnimationEvents _AnimationEvents;
    // ---------- プロパティ ----------
    private string _characterName;    // 名前
    private Texture _texture;          // テクスチャ
    private int _maxHp;               // 最大HP
    private int _hp;                  // 現在HP
    private int _atk;                 // 攻撃力
    private int _atkCoolTIme;         // 攻撃のクールタイム
    private int _atkCoolTImeCounter;  // 攻撃のクールタイムカウンター
    private int _shot_flg;            // 使用する射撃弾
    private bool _isWideAtk;          // 複数攻撃
    private bool _isDoubleAtk;        // ２段攻撃
    private float _range;             // 射程
    private Anti _anti;               // 対地or対空or対両方
    private float _spd;               // 移動速度
    private bool _isFly;              // 飛行
    private bool _isFloat;            // 浮き。棒立ち時に歩きのアニメーションをさせるか
    private int _waterSpd;            // 水辺での速度
    private int _kbNum;               // ノックバック数
    private int _kbNumCounter;        // ノックバック数カウンター
    private int _dieAction;           // 死亡アクション
    private Ability _abiliry;         // 能力
    private bool _isAlly = true;
    private CharacterAction _action;    // 行動
    private bool _isInitialize = false;
    private bool _isDamage = false;     // ダメージを受けたフラグ
    private int _beforeHp = 0;          // 直前のHP
    private bool _isAttack = false;     // 攻撃判定を出したフラグ
    private Func<List<Character>> _getTargetCharacterListFunc = default;
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    private void Start()
    {
        if(_isInitialize)
            SetUp(_characterData, _isAlly, 0);

        _AnimationEvents.SetOnDoudleAttack(()=>
        {
            if(_isDoubleAtk)
                _isAttack = true;
        });
        _AnimationEvents.SetOnAttack(()=>
        {
            _isAttack = true;
            _atkCoolTImeCounter = _atkCoolTIme;
        });
        _AnimationEvents.SetOnAttackEnd(()=>
        {
            ChangeAction(CharacterAction.stand);
        });
    }
    // ---------- Public関数 ----------
    //　セットアップ
    public Character SetUp(CharacterData characterData, bool isAlly ,float fpos_z = -1)
    {
        // 奥行に多少のばらつきを持たせる
        if(fpos_z == -1)
            fpos_z = UnityEngine.Random.Range(0, 0.02f);
        transform.Translate(0, -fpos_z, -fpos_z);

        // データチェック
        if(characterData != null)
            _characterData = characterData;
        if(_characterData == null)
        {
            Debug.Log("キャラクターデータがありません");
            return null;
        }

        // テクスチャ更新
        _overrideSprite.overrideTexture = _characterData.texture;
        // 敵味方フラグ
        _isAlly = isAlly;
        
        // 敵なら左右反転
        if(_isAlly == false)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }

        // データベースからパラメータを読み込んで反映させる
        _characterName      = _characterData.characterName;     // 名前
        _texture            = _characterData.texture;           // テクスチャ
        _maxHp              = _characterData.maxHp;             // 最大HP
        _hp                 = _characterData.maxHp;             // 現在HP
        _atk                = _characterData.atk;               // 攻撃力
        _atkCoolTIme        = _characterData.atkCoolTIme;       // 攻撃のクールタイム
        _atkCoolTImeCounter = _characterData.atkCoolTIme;       // 攻撃のクールタイムカウンター
        _shot_flg           = _characterData.shot_flg;          // 使用する射撃弾
        _isWideAtk          = _characterData.isWideAtk;         // 複数攻撃
        _isDoubleAtk        = _characterData.isDoubleAtk;       // ２段攻撃
        _range              = _characterData.range * AWAY_CORRECTION;             // 射程
        _anti               = _characterData.anti;              // 対地or対空or対両方
        _spd                = _characterData.spd * AWAY_CORRECTION;    // 移動速度
        _isFly              = _characterData.isFly;             // 飛行
        _isFloat            = _characterData.isFloat;           // 浮き。棒立ち時に歩きのアニメーションをさせるか
        _waterSpd           = _characterData.waterSpd;          // 水辺での速度
        _kbNum              = _characterData.kbNum;             // ノックバック数
        _kbNumCounter       = _characterData.kbNum;             // ノックバック数カウンター
        _dieAction          = _characterData.dieAction;         // 死亡アクション
        _abiliry            = _characterData.abiliry;           // 能力

        ChangeAction(CharacterAction.walk);

        _isInitialize = true;
        return this;
    }

    // アクション変更
    public void ChangeAction(CharacterAction action)
    {
        if(_action == action)
            return;

        _action = action;
        foreach(string animParamin in character_anim_parameter)
        {
            _animator.SetBool(animParamin, false);
        }
        switch(action)
        {
            case CharacterAction.walk:
                // 水の上で、自分が水の上だと歩けないなら棒立ちさせる

                _animator.SetBool("Walk", true);
                break;
            case CharacterAction.attack:
                _animator.SetBool("Attack", true);
                break;
            case CharacterAction.stand:
                if(!_isFloat)
                    _animator.SetBool("Stand", true);
                else
                    _animator.SetBool("Walk", true);
                break;
            case CharacterAction.damage:
                _animator.SetBool("Damage", true);
                break;
            case CharacterAction.run:
                _animator.SetBool("Run", true);
                break;
            case CharacterAction.dead:
                _animator.SetBool("Dead", true);
                break;
            //　死んで消えるアクション
            case CharacterAction.vanish:
                _animator.SetBool("Damage", true);
                break;
        }
    }

    public float GetSPD(){ return _spd; }
    public CharacterAction GetCurrentAction(){ return _action; }
    public float GetAwayCorrection(){ return AWAY_CORRECTION; }
    //攻撃クールタイムを減らす
    public void UpdateAtkCoolTImeCounter()
    {
        _atkCoolTImeCounter--;
        if(_atkCoolTImeCounter <= 0)
            _atkCoolTImeCounter = 0;
    }
    // 攻撃クールタイムがなくなっているか
    public bool isZeroAtkCoolTImeCounter()
    {
        if(_atkCoolTImeCounter <= 0)
            return true;
        return false;
    }
    // 「直前のHP」更新
    public void UpdateBeforeHP(){ _beforeHp = _hp; }
    // 「直前のHP」を返す
    public int GetBeforeHP(){ return _beforeHp; }
    // 「ダメージを受けたフラグ」更新
    public void UpdateIsDamage(){ _isDamage = false; }
    // 「ダメージを受けたフラグ」を返す
    public bool GetIsDamage(){ return _isDamage; }
    // 相手キャラクターのテーブルを返す
    public List<Character> GetTargetTable(){ return _getTargetCharacterListFunc(); }
    public void SetGetTargetCharacterListFunc(Func<List<Character>> func)
    {
        _getTargetCharacterListFunc = func;
    }
    public bool IsAlly(){ return _isAlly; }
    // 攻撃層（対地、対空、対両方）を取得
    public Anti GetAnti(){ return _anti; }
    // 射程を取得
    public float GetRange(){ return _range; }
    public bool IsFly(){ return _isFly; }
    // ---------- Private関数 ----------
    // ---------- protected関数 ----------
}
