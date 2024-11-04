using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlatformStretch : MonoBehaviour
{
    public enum StretchDirection
    {
        Right,
        Left
    }

    [Header("Stretch Settings")]
    public StretchDirection direction = StretchDirection.Right; // 伸ばす方向
    public float stretchAmount = 2.0f;  // 伸ばす長さ
    public float stretchSpeed = 1.0f;   // 伸ばす速度
    public float resetSpeed = 1.0f;     // 元に戻る速度

    private SpriteRenderer spriteRenderer;
    private Vector2 originalSize;       // 元のスプライトサイズ
    private Vector2 targetSize;         // 伸ばす目標サイズ
    private Vector3 originalPosition;   // 元の位置
    private bool isStretching = false;  // 伸ばすフラグ
    private bool isResetting = false;   // 元に戻すフラグ

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSize = spriteRenderer.size;
        originalPosition = transform.position;  // 初期位置を保存
    }

    // エディター上での変更を可視化する
    private void OnDrawGizmosSelected()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 previewSize = spriteRenderer.size; // プレビューサイズは元のサイズを使用
        Vector3 previewPosition = transform.position; // プレビュー位置は元の位置を使用

        // 伸ばす方向に基づいてプレビューサイズと位置を変更
        if (direction == StretchDirection.Left)
        {
            previewSize.x += stretchAmount; // 左に伸ばす場合、幅を増やす
            previewPosition = transform.position - Vector3.right * (stretchAmount / 2); // プレビュー位置を左に移動
        }
        else if (direction == StretchDirection.Right)
        {
            previewSize.x += stretchAmount; // 右に伸ばす場合、幅を増やす
            previewPosition = transform.position + Vector3.right * (stretchAmount / 2); // プレビュー位置を右に移動
        }

        Gizmos.color = Color.green; // Gizmosの色を緑に設定
        Gizmos.DrawWireCube(previewPosition, previewSize); // プレビューの表示
    }

    // ゆっくり伸ばす処理
    public void StretchPlatform()
    {
        isStretching = true;  // 伸ばす動作を開始
        isResetting = false;  // 元に戻す動作を停止
        SetTargetSizeAndPosition();
    }

    // ゆっくり元に戻す処理
    public void ResetPlatform()
    {
        isStretching = false; // 伸ばす動作を停止
        isResetting = true;   // 元に戻す動作を開始
        targetSize = originalSize;
    }

    private void Update()
    {
        if (isStretching)
        {
            // 伸ばす動作
            spriteRenderer.size = Vector2.Lerp(spriteRenderer.size, targetSize, stretchSpeed * Time.deltaTime);

            // サイズの変化に基づいて位置を更新
            UpdatePositionOnStretch();

            // 目標サイズに近づいたらフラグを解除
            if (Vector2.Distance(spriteRenderer.size, targetSize) < 0.01f)
            {
                spriteRenderer.size = targetSize;
                isStretching = false;
            }
        }
        else if (isResetting)
        {
            // 元に戻す動作
            spriteRenderer.size = Vector2.Lerp(spriteRenderer.size, originalSize, resetSpeed * Time.deltaTime);

            UpdatePositionOnStretch();

            // 元の位置に戻す
            if (Vector2.Distance(spriteRenderer.size, originalSize) < 0.01f)
            {
                spriteRenderer.size = originalSize;
                transform.position = originalPosition;  // 元の位置に戻す
                isResetting = false;
            }
        }
    }

    // 伸ばす動作の際に位置を更新するメソッド
    private void UpdatePositionOnStretch()
    {
        float stretchChange = spriteRenderer.size.x - originalSize.x;  // 伸ばしの変化量

        if (direction == StretchDirection.Left)
        {
            // 右方向に伸ばす場合は右に移動
            transform.position = originalPosition - Vector3.right * (stretchChange / 2);
        }
        else if (direction == StretchDirection.Right) 
        {
            // 左方向に伸ばす場合は左に移動
            transform.position = originalPosition + Vector3.right * (stretchChange / 2);
        }
    }


    // 伸ばす方向に基づいて目標サイズと位置を設定
    private void SetTargetSizeAndPosition()
    {
        targetSize = originalSize;

        if (direction == StretchDirection.Left)
        {
            targetSize.x += stretchAmount; // 右方向に伸ばす
            transform.position = originalPosition + Vector3.right * (stretchAmount / 2); // 位置を右に移動
        }
        else if (direction == StretchDirection.Right)
        {
            targetSize.x += stretchAmount; // 左方向に伸ばす
            transform.position = originalPosition - Vector3.right * (stretchAmount / 2); // 位置を左に移動
        }
    }
}
