using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class laststage : MonoBehaviour
{
    public string playerTag = "Player";        // プレイヤーのタグ
    public GameObject whiteOutPanel;           // ホワイトアウト用のPanel
    public float fadeSpeed = 1.0f;             // フェード速度
    public string gameClearScene = "GameClear1";  // ゲームクリア画面のシーン名

    private bool isClearing = false;
    private float alpha = 0;
    private Image panelImage;

    void Start()
    {
        // PanelのImageコンポーネントを取得
        if (whiteOutPanel != null)
        {
            panelImage = whiteOutPanel.GetComponent<Image>();
            panelImage.color = new Color(1, 1, 1, 0);  // アルファを0に設定
        }
        else
        {
            Debug.LogError("WhiteOut Panel is not assigned.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーと接触した場合
        if (collision.CompareTag(playerTag) && !isClearing)
        {
            isClearing = true;
            Time.timeScale = 0;  // ゲームの時間を止める
        }
    }

    void Update()
    {
        if (isClearing && panelImage != null)
        {
            // ホワイトアウトエフェクト
            alpha += fadeSpeed * Time.unscaledDeltaTime;
            panelImage.color = new Color(1, 1, 1, alpha);

            // ホワイトアウトが完了したら次のシーンに遷移
            if (alpha >= 1)
            {
                SceneManager.LoadScene(gameClearScene);
            }
        }
    }
}
