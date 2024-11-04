using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float floatSpeed = 1.0f;  // ふわふわするスピード
    public float floatHeight = 0.5f; // ふわふわする高さ

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // 初期位置を記録
    }

    void Update()
    {
        // 上下にふわふわする動きを設定
        transform.position = startPos + new Vector3(0.0f, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0.0f);
    }
}
