using UnityEngine;
using Cinemachine;

public class StageConfiner : MonoBehaviour
{
    // ステージのコリジョンを保持するCollider2D (PolygonCollider2D, BoxCollider2Dなど何でも利用可能)
    public Collider2D stageConfiner;

    // Cinemachineのバーチャルカメラ
    public CinemachineVirtualCamera virtualCamera;

    // カメラのConfinerコンポーネント
    public CinemachineConfiner2D confiner;

    // プレイヤーのリスポーンポイントとして使用するGameObject
    public GameObject respawnPoint;

    private void Start()
    {
        // Confinerコンポーネントを取得
        if (virtualCamera != null)
        {
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーがこのコリジョンに入ったか確認
        if (other.CompareTag("Player")) // Playerタグを持つオブジェクト
        {
            UpdateCameraConfiner(); // カメラのConfinerを更新

            // 接触したプレイヤーからリスポーンスクリプトを取得
            Respawn respawnScript = other.GetComponent<Respawn>();

            if (respawnScript != null && respawnPoint != null)
            {
                // リスポーンポイントの位置をリスポーン位置に設定
                Vector3 newRespawnPosition = respawnPoint.transform.position; // リスポーンポイントの位置
                respawnScript.SetRespawnPosition(newRespawnPosition);
                Debug.Log("リスポーン位置をリスポーンポイントに更新しました: " + newRespawnPosition);
            }
            else if (respawnPoint == null)
            {
                Debug.LogWarning("リスポーンポイントが設定されていません。");
            }
        }
    }

    // コンテクストメニューで呼び出せるようにするメソッド
    [ContextMenu("Update Camera Confiner")]
    private void UpdateCameraConfiner()
    {
        if (confiner != null && stageConfiner != null)
        {
            confiner.m_BoundingShape2D = stageConfiner; // ステージのコリジョンをカメラに設定
            Debug.Log("カメラのConfinerを更新しました: " + stageConfiner.name);
        }
    }

    [ContextMenu("Set Default Respawn Point")]
    private void SetDefaultRespawnPoint()
    {
        if (respawnPoint != null)
        {
            respawnPoint.transform.position = transform.position; // 自身の位置をリスポーンポイントに設定
            Debug.Log("リスポーンポイントをデフォルト位置に設定しました: " + transform.position);
        }
        else
        {
            Debug.LogWarning("リスポーンポイントが設定されていません。");
        }
    }
}
