using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Networking;
using System.IO;
using Unity.EditorCoroutines.Editor;
using System.Linq;

[CustomEditor(typeof(CsvImporter))]
public class CsvImpoterEditor : Editor {

    const string SHEET_ID = "1PntUhlkoirwhOQqnC15riFKArfMV7VVQD2VE9ZfUtn0";
    const string SHEET_NAME_CHARACTER_DATA = "CharacterData";
	public override void OnInspectorGUI(){
		DrawDefaultInspector();

		if (GUILayout.Button("スプレッドシートからデータ作成")){
            // スプレッドシートから読み込み
            EditorCoroutineUtility.StartCoroutine(Method(SHEET_NAME_CHARACTER_DATA), this);
		}
	}

    IEnumerator Method(string _SHEET_NAME){
        UnityWebRequest request = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/"+SHEET_ID+"/gviz/tq?tqx=out:csv&sheet="+_SHEET_NAME);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log(request.error);
        }
        else{
            string csvText = request.downloadHandler.text;           
            SetCsvDataToScriptableObject(csvText);
        }
    }

    void SetCsvDataToScriptableObject(string _csvText ){
        // ボタンを押されたらパース実行
		// 改行ごとにパース
		string[] afterParse = _csvText.Split('\n');


        // ヘッダー行を除いてインポート
		for (int i = 1; i < afterParse.Length; i++){
			string[] parseByComma = afterParse[i].Split(',');

			int column = 0;

			// 先頭の列が空であればその行は読み込まない
			if (parseByComma[column] == ""){
				continue;
			}

			// 行数をIDとしてファイルを作成
            string dataName = "characterData" + i.ToString("D4");
			string fileName = "characterData" + i.ToString("D4") + ".asset";
			string path = "Assets/Database/Character/" + fileName;

			// CharacterDataのインスタンスをメモリ上に作成
			CharacterData characterData = CreateInstance<CharacterData>();
            characterData.name = dataName;

            string value = "";

			// 名前
            value = TrimString(parseByComma[column]);
			characterData.characterName = value;
            
            // 召喚コスト
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.summonCost = int.Parse(value);

            // HP
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.maxHp = int.Parse(value);

            // 攻撃力
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.atk = int.Parse(value);
            
            // 攻撃のクールタイム
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.atkCoolTIme = int.Parse(value);
            
            // 使用する射撃弾
            column += 1;
            value = TrimString(parseByComma[column]);
            int intShot = int.Parse(value);
            characterData.shot_flg = (CharacterShot)Enum.ToObject(typeof(CharacterShot), intShot);

            // 複数攻撃
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.isWideAtk = bool.Parse(value);

            // ２段攻撃
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.isDoubleAtk = bool.Parse(value);

            // 射程
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.range = float.Parse(value);
            
            // 対地or対空or対両方
            column += 1;
            value = TrimString(parseByComma[column]);
            int intAnti = int.Parse(value);
            characterData.anti = (Anti)Enum.ToObject(typeof(Anti), intAnti);

            // 移動速度
            column += 1;
            value = TrimString(parseByComma[column]);
            float spd = float.Parse(value) / 10.0f;
            characterData.spd = spd;
            
            // 飛行
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.isFly = bool.Parse(value);
            
            // 浮き。棒立ち時に歩きのアニメーションをさせるか
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.isFloat = bool.Parse(value);
            
            // 水辺での速度
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.waterSpd = int.Parse(value);
            
            // ノックバック数
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.kbNum = int.Parse(value);

            // 死亡アクション
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.dieAction = int.Parse(value);

            // 購入コスト
            column += 1;
            value = TrimString(parseByComma[column]);
            characterData.buyCost = int.Parse(value);

            // テクスチャとアイコン取得
            column += 1;
            value = TrimString(parseByComma[column]);
            string texturePath = "Assets/Texture/Character/" + value + ".png";
            Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();
            Sprite icon = sprites[0];
            
            // 説明
            column += 1;
            value = TrimString(value);
            characterData.description = value;

            // 能力
            column += 1;
            value = TrimString(value);
            characterData.texture = null;

			// インスタンス化したものをアセットとして保存
			var asset = (CharacterData)AssetDatabase.LoadAssetAtPath(path, typeof(CharacterData));
			if (asset == null){
				// 指定のパスにファイルが存在しない場合は新規作成
				AssetDatabase.CreateAsset(characterData, path);
                asset = (CharacterData)AssetDatabase.LoadAssetAtPath(path, typeof(CharacterData));
			} else {
				// 指定のパスに既に同名のファイルが存在する場合は更新
				EditorUtility.CopySerialized(characterData, asset);
				AssetDatabase.SaveAssets();
			}

            //　実体を作ってからテクスチャを反映させる
            asset.texture = texture;
            asset.icon = icon;
			AssetDatabase.Refresh();
		}

        Debug.Log("データの作成が完了しました。");
    }

    string TrimString(string value)
    {
        return value.TrimStart('"').TrimEnd('"');
    }
}
