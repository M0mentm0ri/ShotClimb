using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shaker : MonoBehaviour
{
    // Cinemachineのインパルスソース
    public CinemachineImpulseSource impulseSource;

    // Start is called before the first frame update
    void Start()
    {
        // インパルスソースが未設定の場合、同じオブジェクトから取得
        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }
    }

    // シェイクを実行するメソッド
    public void Shake()
    {
        // インパルスを生成してカメラを揺らす
        impulseSource.GenerateImpulse();
    }
}
