using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 移動速度
    public float climbSpeed = 3f; // 階段を登る速度
    public float targetX = 10f;   // 目標X座標
    public bool isOnStairs = false; // 階段にいるかどうか

    private bool isMoving = false;   // キャラクターが移動中かどうか

    void Update()
    {
        // Spaceキーが押された場合
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = true; // 移動を開始
        }

        // キャラクターが移動中の処理
        if (isMoving)
        {
            // 目標のX座標に到達するまで移動
            if (!isOnStairs)
            {
                if (transform.position.x < targetX)
                {
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
                else
                {
                    isOnStairs = true; // 階段に到達したと見なす
                }
            }
            // 階段を登る処理
            else
            {
                // 階段の高さに応じて移動
                transform.Translate(Vector3.up * climbSpeed * Time.deltaTime);
                // 階段の高さに到達したら移動を完了
                //if (transform.position.y >= /* 階段の高さ */)
                //{
                //    Debug.Log("移動完了");
                //    isMoving = false; // 移動完了後、フラグをリセット
                //    isOnStairs = false; // 階段を降りるなどの処理を考慮
                //}
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stairs")) // 階段に設定したタグ
        {
            isOnStairs = true; // 階段に接触したらフラグを立てる
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stairs"))
        {
            isOnStairs = false; // 階段から離れたらフラグを下ろす
        }
    }
}
