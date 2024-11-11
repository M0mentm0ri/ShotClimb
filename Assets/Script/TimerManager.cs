using UnityEngine;
using UnityEngine.UI;


public class TimerManager : MonoBehaviour
{
    public Text timerText;  // タイマーを表示するテキストUI
    private float elapsedTime;  // 経過時間を保持
    private bool isTiming = true;  // タイマーを動かすフラグ
    private static float totalElapsedTime = 0f;  // 全体の経過時間を記録

    void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;  // 経過時間を更新
            timerText.text = elapsedTime.ToString("F2") + "秒";  // タイマーをUIに表示
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("timerTag"))
        {
            isTiming = false;  // タイマーを停止
            totalElapsedTime += elapsedTime;  // 合計経過時間に加算
            GameClear();  // ゲームクリア画面へ遷移
        }
    }

    private void GameClear()
    {
        PlayerPrefs.SetFloat("StageTime", elapsedTime);  // ステージタイムを保存
        PlayerPrefs.SetFloat("TotalTime", totalElapsedTime);  // 合計タイムを保存
        
    }
}
