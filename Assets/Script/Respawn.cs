using UnityEngine;

public class Respawn : MonoBehaviour
{
    // リスポーン位置を保持する変数
    public Vector3 respawnPosition;

    // リスポーンを実行するメソッド
    public void RespawnPlayer()
    {
        // プレイヤーをリスポーン位置にテレポートさせる
        transform.position = respawnPosition;
        Debug.Log("プレイヤーをリスポーン位置にテレポートしました: " + respawnPosition);
    }

    // リスポーン位置を設定するメソッド
    public void SetRespawnPosition(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
        Debug.Log("新しいリスポーン位置を設定しました: " + respawnPosition);
    }

    private void Update()
    {
        // Rキーが押されたか確認
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer(); // リスポーンメソッドを呼び出す
        }
    }
}
