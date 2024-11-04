using UnityEngine;

public class InfinityPathCamera : MonoBehaviour
{
    public float speed = 1.0f;      // カメラが移動する速さ
    public float width = 5.0f;      // 横方向の幅
    public float height = 3.0f;     // 縦方向の高さ
    public Vector3 centerPosition = Vector3.zero; // 移動の中心位置

    private float time;

    void Update()
    {
        // Spaceキーが押されたときにスクリプトを無効化
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.enabled = false;  // スクリプトを無効化
            return;
        }
        time += Time.deltaTime * speed;

        // カメラの位置を∞の形に設定
        float x = width * Mathf.Sin(time);
        float y = height * Mathf.Sin(2 * time);

        // カメラの新しい位置
        transform.position = centerPosition + new Vector3(x, y, transform.position.z);
    }
}
