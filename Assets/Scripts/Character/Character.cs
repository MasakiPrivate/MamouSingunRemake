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
    private const float KNOCK_BACK_VEC_X = -5f;
    private const float KNOCK_BACK_ADD_X = 0.2f;
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
    private CharacterShot _shot_flg;            // 使用する射撃弾
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
    private DieAction _dieAction;           // 死亡アクション
    private Ability _abiliry;         // 能力
    private bool _isAlly = true;
    private CharacterAction _action;    // 行動
    private bool _isInitialize = false;
    private bool _isDamage = false;     // ダメージを受けたフラグ
    private int _beforeHp = 0;          // 直前のHP
    private bool _isAttack = false;     // 攻撃判定を出したフラグ
    private bool _isKnockBack = false;  // ノックバック予定フラグ
    private float _vecX = 0f;
    private int _counter = 0;
    private Func<List<Character>> _getTargetCharacterListFunc = default;
    private Action<Character> _onCharacterDead = default;
    // ---------- クラス変数宣言 ----------
    // ---------- インスタンス変数宣言 ----------
    // ---------- Unity組込関数 ----------
    private void Start()
    {
        if(_isInitialize)
            SetUp(_characterData, _isAlly, 0);

        // 攻撃モーション：２段攻撃キャラクターの攻撃判定発生
        _AnimationEvents.SetOnDoudleAttack(()=>
        {
            if(_isDoubleAtk)
                _isAttack = true;
        });
        // 攻撃モーション：攻撃判定発生
        _AnimationEvents.SetOnAttack(()=>
        {
            _isAttack = true;
            _atkCoolTImeCounter = _atkCoolTIme;
        });
        // 攻撃モーション：攻撃終了処理
        _AnimationEvents.SetOnAttackEnd(()=>
        {
            ChangeAction(CharacterAction.stand);
        });
        // ダメージモーション：モーション終了時の処理
        _AnimationEvents.SetOnDamageEnd(()=>
        {
            if(0 < _hp)
                ChangeAction(CharacterAction.walk);
            else
            {
                // キャラクター死亡処理
                _onCharacterDead?.Invoke(this);
                // 死亡
                switch(_dieAction)
                {
                    case DieAction.vanish:
                        ChangeAction(CharacterAction.vanish);
                        break;
                    case DieAction.run:
                        ChangeAction(CharacterAction.run);
                        break;
                    case DieAction.dead:
                        ChangeAction(CharacterAction.dead);
                        break;
                }
            }
        });
    }

    private void FixedUpdate()
    {
        // ノックバック
        if(_action == CharacterAction.damage && _vecX < 0)
        {
            if(_isAlly)
                transform.Translate(_vecX, 0, 0);
            else
                transform.Translate(-_vecX, 0, 0);
            _vecX += KNOCK_BACK_ADD_X * AWAY_CORRECTION;
        }
  
        if(_action == CharacterAction.run )
        {
            // 画面外へ行った判定
            bool isOut = true;
            if(isOut)
                CharacterVanish();
        }
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
            return null;
        }

        // テクスチャ更新
        if(_characterData.texture != null)
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
                _vecX = KNOCK_BACK_VEC_X * AWAY_CORRECTION;
                break;
            case CharacterAction.run:
                Vector3 scaleX = transform.localScale;
                scaleX.x *= -1f;
                transform.localScale = scaleX;
                _animator.SetBool("Run", true);
                break;
            case CharacterAction.dead:
                _animator.SetBool("Dead", true);
                break;
            //　死んで消えるアクション
            case CharacterAction.vanish:
                CharacterVanish();
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
    //  今のHPを返す
    public int GetHP(){ return _hp; }
    //  HP更新
    public void SetHP(int hp){ _hp = hp; }
    //  最大HPを返す
    public int GetMaxHP(){ return _maxHp; }
    //  攻撃力を返す
    public int GetAtk(){ return _atk; }
    // 「ダメージを受けたフラグ」更新
    public void SetIsDamage(bool isDamage){ _isDamage = isDamage; }
    // 「ダメージを受けたフラグ」を返す
    public bool IsDamage(){ return _isDamage; }
    // 相手キャラクターのテーブルを返す
    public List<Character> GetTargetTable(){ return _getTargetCharacterListFunc(); }
    public void SetGetTargetCharacterListFunc(Func<List<Character>> func)
    {
        _getTargetCharacterListFunc = func;
    }
    public bool IsAlly(){ return _isAlly; }
    // 攻撃層（対地、対空、対両方）を返す
    public Anti GetAnti(){ return _anti; }
    // 射程を返す
    public float GetRange(){ return _range; }
    public bool IsFly(){ return _isFly; }
    // 攻撃判定を出しているか
    public bool IsAttack(){ return _isAttack; }
    // 攻撃判定をおろす
    public void UnAttack(){ _isAttack = false; }
    // 複数攻撃フラグを返す
    public bool IsWideAtk(){ return _isWideAtk; } 
    // 攻撃時弾発射フラグを返す
    public CharacterShot GetShot(){ return _shot_flg;}
    public bool IsKnockBack(){ return _isKnockBack; }
    public void SetIsKnockBack(bool isKnockBack){ _isKnockBack = isKnockBack; }
    public int GetKbNum(){ return _kbNum; }
    public int GetKbNumCounter(){ return _kbNumCounter; }
    public void SetKbNumCounter(int kbNumCounter){ _kbNumCounter = kbNumCounter; }
    // 弾発射
    public void FireShot()
    {

    }

    // 相手と自身の距離を求める
    public float GetDistanceX(Character target)
    {
        float distance = transform.localPosition.x - target.transform.localPosition.x;
        return Mathf.Abs(distance);
    }

    // 相手が自身の前方にいるかチェック　前方にいる：true、背後にいる：false
    public bool CheckTargetIsFront(Character target)
    {
        float characterPosX = transform.localPosition.x;
        float targetPosX = target.transform.localPosition.x;

        // 味方の場合
        if(_isAlly == true)
        {
            if(characterPosX <= targetPosX )
                return true;
            else
                return false;
        }
        // 敵の場合
        else
        {
            if(targetPosX <= characterPosX )
                return true;
            else
                return false;
        }
    }
    // 消失時のコールバック設定
    public void SetOnCharacterDead(Action<Character> action)
    {
        _onCharacterDead = action;
    }
    // ---------- Private関数 ----------
    // キャラクターの消滅処理
    private void CharacterVanish()
    {
        Destroy(this.gameObject);
    }
    // ---------- protected関数 ----------
}
