using UnityEngine;
using UnityEngine.UI; // Textを使用するため

public class Timer : MonoBehaviour
{
    public Text timerText; // UIのTextコンポーネント
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime; // 経過時間を更新
        timerText.text = elapsedTime.ToString("F2") + "秒"; // 画面に表示
    }
}
