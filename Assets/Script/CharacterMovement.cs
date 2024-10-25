using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // X方向への移動速度
    public float targetX = 10f;       // X方向の最終目標座標
    public float startClimbingX = 7f; // Y方向への移動を開始するX座標
    public float climbSpeed = 3f;     // Y方向（上方向）への移動速度
    public float climbTime = 2f;      // Y方向への移動にかかる時間

    private bool isMoving = false;    // キャラクターが移動中かどうか
    private bool isClimbing = false;  // Y方向に移動中かどうか
    private float climbTimer = 0f;    // Y方向移動のタイマー

    void Update()
    {
        // Spaceキーが押された場合に移動を開始
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = true; // 移動フラグをオン
        }

        // キャラクターのX座標への移動処理
        if (isMoving)
        {
            // X座標が目標位置に達するまで右に移動
            if (transform.position.x < targetX)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

                // Y方向への移動を開始するX座標に達した場合
                if (transform.position.x >= startClimbingX && climbTimer <= 0)
                {
                    isClimbing = true; // Y方向への移動フラグをオン
                    climbTimer = climbTime; // タイマーを設定
                }
            }
            else
            {
                isMoving = false; // X座標への移動を終了
                Debug.Log("X方向への移動完了");
            }
        }

        // Y方向に移動中の処理（X座標と同時にY座標を移動）
        if (isClimbing && climbTimer > 0)
        {
            transform.Translate(Vector3.up * climbSpeed * Time.deltaTime); // Y方向に移動
            climbTimer -= Time.deltaTime; // タイマーを減少

            // 指定の時間が経過したらY方向の移動を停止
            if (climbTimer <= 0)
            {
                isClimbing = false;
                Debug.Log("Y方向への移動完了");
            }
        }
    }
}
