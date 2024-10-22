using UnityEngine;

public class Title : MonoBehaviour
{
    public Vector3 startPosition = new Vector3(0f, 0.31f, -10f);
    public Vector3 endPosition = new Vector3(0f, -27.83f, -10f);
    public float acceleration = 2.0f;
    public float maxSpeed = 20.0f;
    public float decelerationDistance = 5.0f; // この距離に近づくと減速が始まる
    private float currentSpeed = 0f;
    private bool startSliding = false;

    void Start()
    {
        // カメラの初期位置を設定
        transform.position = startPosition;
    }

    void Update()
    {
        // Spaceキーが押されたらスライド開始
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startSliding = true;
        }

        // スライド処理
        if (startSliding)
        {
            float distanceToEnd = Vector3.Distance(transform.position, endPosition);

            // 終了地点に近づいたら減速を開始
            if (distanceToEnd < decelerationDistance)
            {
                // 距離に応じて減速
                currentSpeed = Mathf.Lerp(0, maxSpeed, distanceToEnd / decelerationDistance);
            }
            else
            {
                // 通常の加速
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            }

            // カメラをy方向に移動
            transform.position = Vector3.MoveTowards(transform.position, endPosition, currentSpeed * Time.deltaTime);

            // 終了地点に到達したらスライドを停止し、ゲーム開始
            if (transform.position == endPosition)
            {
                startSliding = false;
                StartGame();
            }
        }
    }

    void StartGame()
    {
        // ゲーム開始の処理をここに追加
        Debug.Log("Game Started!");
    }
}
