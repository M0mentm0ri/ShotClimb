using System.Collections;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    // プレハブ（復活時に生成するオブジェクト）
    public GameObject objectPrefab;

    // プレハブの生成位置
    public Transform spawnPosition;

    // 復活に必要な時間
    public float respawnTime = 5f;

    // 破壊時のパーティクル
    public ParticleSystem destructionParticles;

    // 復活時のパーティクル
    public ParticleSystem respawnParticles;

    // 現在のオブジェクト参照
    public GameObject currentObject;

    // 一定時間後に復活するためのフラグ
    private bool isRespawning = false;

    // 初期化処理
    private void Start()
    {
        // ゲーム開始時にオブジェクトをスポーン
        currentObject = Instantiate(objectPrefab, spawnPosition.position, spawnPosition.rotation, transform);
    }

    // 毎フレームの更新処理
    private void Update()
    {
        // 1秒に1回チェック
        if (!isRespawning && currentObject == null)
        {
            // 破壊を検知した時点で復活のコルーチンを開始
            StartCoroutine(RespawnObjectAfterDelay());
        }
    }

    // 一定時間後にオブジェクトを復活させるコルーチン
    private IEnumerator RespawnObjectAfterDelay()
    {
        isRespawning = true;

        // 復活時間の待機
        yield return new WaitForSeconds(respawnTime);

        // 復活時のパーティクルを再生
        if (respawnParticles != null)
        {
            respawnParticles.transform.position = spawnPosition.position; // スポーン位置にパーティクルを再生
            respawnParticles.Play();
        }

        // プレハブを生成し、現在のオブジェクトに設定
        currentObject = Instantiate(objectPrefab, spawnPosition.position, spawnPosition.rotation, transform);

        // 破壊パーティクルを再生する位置に移動
        if (destructionParticles != null && currentObject != null)
        {
            destructionParticles.transform.position = currentObject.transform.position;
        }

        // 復活が完了したのでフラグをリセット
        isRespawning = false;
    }
}
