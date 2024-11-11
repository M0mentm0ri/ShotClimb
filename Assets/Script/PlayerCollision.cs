using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // タイマーを表示するTextMeshProのUI
    public TimerManager timerManager;  // TimerManagerの参照（Inspectorから割り当て）

    private Color originalColor;  // 元のタイマー色を保存
    private HashSet<int> collidedObjectIDs = new HashSet<int>();  // 衝突済みオブジェクトIDを保存

    private void Start()
    {
        // タイマーの元の色を保存
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
            int objectID = other.GetInstanceID();  // オブジェクトのユニークなIDを取得

            // 衝突済みであれば処理をスキップ
            if (collidedObjectIDs.Contains(objectID))
                return;

            // 新規衝突の場合、IDをセットに追加
            collidedObjectIDs.Add(objectID);

            Debug.Log("タイマーが衝突しました");

            // タイマーを赤くする
            StartCoroutine(ChangeTimerColor());

            // ステージ時間を保存
            timerManager.SaveStageTime();
        }
    }

    private IEnumerator ChangeTimerColor()
    {
        // タイマーを赤く変更
        timerText.color = Color.red;
        yield return new WaitForSeconds(1f);  // 1秒間赤いまま
        timerText.color = originalColor;  // 元の色に戻す
    }
}
