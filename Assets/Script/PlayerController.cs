using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce = 10f;
    public int maxAmmo = 2;
    private int currentAmmo;
    public Transform muzzle;  // 発射口
    public GameObject bulletPrefab;  // 弾のプレハブ
    public float bulletSpeed = 20f;  // 弾の速度
    public Camera mainCamera;  // マウスの位置を取得するためのカメラ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;  // メインカメラを取得
        currentAmmo = maxAmmo;  // 初期弾数を設定
    }

    void Update()
    {
        // マウスの位置を取得
        Vector3 mousePos = Input.mousePosition;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        Vector2 targetPoint = new Vector2(mousePos.x, mousePos.y);

        // 左クリックで弾を発射
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Shoot((targetPoint - (Vector2)muzzle.position).normalized);
        }
    }

    void Shoot(Vector2 direction)
    {
        currentAmmo--;

        // 弾を生成して発射
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;

        // プレイヤーに反動を追加（飛ぶための力）
        rb.AddForce(-direction * jumpForce, ForceMode2D.Impulse);

        // ダメージ処理
        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, direction);
        if (hit.collider != null && hit.collider.CompareTag("Destructible"))
        {
            hit.collider.GetComponent<DestructibleObject>().TakeDamage(1);  // 障害物へのダメージ
        }
    }

    public void Reload(int ammoPickedUp)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammoPickedUp, maxAmmo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AmmoPickup"))
        {
            Reload(2);
            Destroy(collision.gameObject);
        }
    }
}
