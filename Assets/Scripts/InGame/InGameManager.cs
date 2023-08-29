using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    // ---------- 定数宣言 ----------
    public const int PLAYER_UNIT_MAX_NUM = 100; //プレイヤーキャラクターの一度に出撃できる最大体数
    public const int ENEMY_UNIT_MAX_NUM = 100;  //敵キャラクターの一度に出撃できる最大体数
    public const float ALLY_CHARACTER_START_POS_X = 3.0f;
    public const float ENEMY_CHARACTER_START_POS_X = 19.0f;
    // ---------- ゲームオブジェクト参照変数宣言 ----------
    // ---------- プレハブ ----------
    [SerializeField, Tooltip("キャラクタープレハブ")] private Character _characterPrefab;
    [SerializeField, Tooltip("キャラクターデータ")] private CharacterData _characterData;
    [SerializeField, Tooltip("戦場")] private Transform _battleField;
    // ---------- プロパティ ----------
    // ---------- クラス変数宣言 ----------
    public static InGameManager instance = new InGameManager();
    // ---------- インスタンス変数宣言 ----------
    private List<Character> _allyCharacterList = default;
    private List<Character> _enemyCharacterList = default;
    // ---------- Unity組込関数 ----------
    private void Awake()
    {
        if(instance == null)
            instance = this;
        if(instance != this)
            Destroy(this);
    }
    // ---------- Public関数 ----------
    public void Initialize()
    {
        RefreshCharacterList();
        CreateCharacter(_characterData, true, -1);
        CreateCharacter(_characterData, false, -1);
    }

    // キャラクター生成
    public void CreateCharacter(CharacterData characterData, bool isAlly ,float fpos_z = -1)
    {
        if(isAlly && _allyCharacterList.Count >= PLAYER_UNIT_MAX_NUM)
        {
            return;
        }
        if(!isAlly && _enemyCharacterList.Count >= ENEMY_UNIT_MAX_NUM)
        {
            return;
        }
        Character character = Instantiate(_characterPrefab);
        character.transform.parent = _battleField;

        // 初期位置設定
        character.transform.localPosition = Vector3.zero;
        character.SetUp(characterData, isAlly, fpos_z);

        // 敵か味方かで処理が変わる
        if(isAlly)
        {   
            character.transform.Translate(ALLY_CHARACTER_START_POS_X, 0, 0);    // 初期位置設定
            character.SetGetTargetCharacterListFunc(()=>{ return _enemyCharacterList; }); // 敵テーブルを返す
            _allyCharacterList.Add(character);                                  // 味方テーブルに登録
        }
        else
        {
            character.transform.Translate(ENEMY_CHARACTER_START_POS_X, 0, 0);    // 初期位置設定
            character.SetGetTargetCharacterListFunc(()=>{ return _allyCharacterList; }); // 味方テーブルを返す
            _enemyCharacterList.Add(character);                                  // 敵テーブルに登録
        }
    }

    public List<Character> GetAllyCharacterList() { return _allyCharacterList; }
    public List<Character> GetEnemyCharacterList() { return _enemyCharacterList; }
    // ---------- Private関数 ----------
    private void RefreshCharacterList()
    {
        if(_allyCharacterList != null)
        {
            foreach(Character character in _allyCharacterList)
            {
                Destroy(character);
            }
        }
        if(_enemyCharacterList != null)
        {
            foreach(Character character in _enemyCharacterList)
            {
                Destroy(character);
            }
        }

        _allyCharacterList = new List<Character>();
        _enemyCharacterList = new List<Character>();
    }
}
