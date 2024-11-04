using UnityEngine;

public class ShotGunTrigger : MonoBehaviour
{
    private bool isGroundClose;  // 地面が近いかどうかのフラグ

    // 現在の地面近接フラグを返し、取得後にリセットするメソッド
    public bool CheckGroundClose()
    {
        bool currentGroundClose = isGroundClose;
        isGroundClose = false;  // 一度取得したらリセット
        return currentGroundClose;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 判定対象が「Ground」タグならフラグを立てる
        if (other.CompareTag("Ground"))
        {
            isGroundClose = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 判定対象が「Ground」タグならフラグを立てる
        if (other.CompareTag("Ground"))
        {
            isGroundClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 判定対象が「Ground」タグならフラグを立てる
        if (other.CompareTag("Ground"))
        {
            isGroundClose = false;
        }
    }
}
