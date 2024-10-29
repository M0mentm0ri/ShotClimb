using UnityEngine;

public class CloudMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 direction = Vector3.right; // 雲の動く方向（インスペクターで設定可能）
    public float speed = 1.0f;                // 雲の速度（インスペクターで設定可能）

    [Header("Position Settings")]
    public Vector3 respawnPosition;           // ワープ先位置（ワールド座標）
    public Vector3 endPosition;               // 終了位置（ワールド座標）

    void Update()
    {
        // 雲を指定された方向に速度分だけ移動
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // 終了位置を通過した場合、ワープ位置に移動
        if (IsBeyondEndPosition())
        {
            Warp();
        }
    }

    void Warp()
    {
        // ワールド座標でワープ先位置に移動
        transform.position = respawnPosition;
        Debug.Log("ワープ先の位置: " + transform.position);
    }

    // ワールド座標で終了位置を通過したかどうかを確認するメソッド
    bool IsBeyondEndPosition()
    {
        Vector3 worldPosition = transform.position;

        // direction によって、どの軸を超えたかをチェックする
        if (direction.x > 0 && worldPosition.x >= endPosition.x) return true;
        if (direction.x < 0 && worldPosition.x <= endPosition.x) return true;
        if (direction.y > 0 && worldPosition.y >= endPosition.y) return true;
        if (direction.y < 0 && worldPosition.y <= endPosition.y) return true;
        if (direction.z > 0 && worldPosition.z >= endPosition.z) return true;
        if (direction.z < 0 && worldPosition.z <= endPosition.z) return true;

        return false;
    }
}
