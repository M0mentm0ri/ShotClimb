using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // タイマーを表示するTextMeshProのUI
    public TimerManager timerManager;  // TimerManagerの参照（Inspectorから割り当て）

    private Color originalColor;

    private void Start()
    {
        originalColor = timerText.color;

        // TimerManagerをインスペクタから割り当てていない場合は、FindObjectOfTypeで検索
        if (timerManager == null)
        {
            timerManager = FindObjectOfType<TimerManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("timerTag"))
        {
            Debug.Log("タイマーが衝突しました");

            // タイマーを赤くする
            StartCoroutine(ChangeTimerColor());

            // タイマーの色を変更したり、ステージ時間を保存する
            timerManager.SaveStageTime();
        }
    }

    private IEnumerator ChangeTimerColor()
    {
        // タイマーを赤くする
        timerText.color = Color.red;
        yield return new WaitForSeconds(1f);  // 1秒間赤くする
        timerText.color = originalColor;  // 元の色に戻す
    }
}
