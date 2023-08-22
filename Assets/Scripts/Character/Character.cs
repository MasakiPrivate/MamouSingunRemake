using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //　セットアップ
    public Character SetUp(int characterId, bool isEnemy ,int pos_z = -1)
    {
        // 奥行に多少のばらつきを持たせる
        if(pos_z == -1)
            pos_z = Random.Range(0, 10);

        // データベースからパラメータを読み込んで反映させる

        return this;
    }
}
