using UnityEngine;
using Cinemachine;

public class StageConfiner : MonoBehaviour
{
    // ステージのコリジョンを保持するPolygonCollider2D
    public PolygonCollider2D stageConfiner;

    // Cinemachineのバーチャルカメラ
    public CinemachineVirtualCamera virtualCamera;

    // カメラのConfinerコンポーネント
    public CinemachineConfiner2D confiner;

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
        }
    }

    private void UpdateCameraConfiner()
    {
        if (confiner != null && stageConfiner != null)
        {
            confiner.m_BoundingShape2D = stageConfiner; // ステージのコリジョンをカメラに設定
            Debug.Log("カメラのConfinerを更新しました: " + stageConfiner.name);
        }
    }
}
