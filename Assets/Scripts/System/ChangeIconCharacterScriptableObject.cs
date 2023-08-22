using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

// モンスターデータスクリプタブルオブジェクトのアイコンを変更するエディター
[CustomEditor(typeof(CharacterData))]
public class ChangeIconCharacterScriptableObject : Editor
{
    public override Texture2D RenderStaticPreview
    ( 
        string assetPath, 
        Object[] subAssets, 
        int width, 
        int height 
    )
    {
        var obj = target as CharacterData;
        var icon = obj.icon;

        if ( icon == null )
        {
            return base.RenderStaticPreview( assetPath, subAssets, width, height );
        }

        var preview = AssetPreview.GetAssetPreview( icon );
        var final = new Texture2D( width, height );

        EditorUtility.CopySerialized( preview, final );

        return final;
    }
}
#endif