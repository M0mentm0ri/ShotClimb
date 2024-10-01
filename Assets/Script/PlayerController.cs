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

    public LineRenderer lineRenderer; // LineRendererをInspectorで設定
    public float maxGrappleDistance = 10f; // グラップルの最大距離
    public float pullSpeed = 5f; // 引き寄せる速度
    private Vector2 grappleTarget;
    private bool isGrappling = false;
    internal string ammo;

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

        // 弾を発射
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Shoot((targetPoint - (Vector2)muzzle.position).normalized);
        }

        // グラップルを発射または解除
        if (Input.GetMouseButtonDown(1))
        {
            if (isGrappling)
            {
                ReleaseGrapple();
            }
            else
            {
                FireGrapple(targetPoint);
            }
        }

        // 線を更新
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grappleTarget);
            PullPlayer();
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position); // グラップルしていない時はプレイヤーの位置を表示
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

    void FireGrapple(Vector2 targetPoint)
    {
        // グラップルを発射
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (targetPoint - (Vector2)transform.position).normalized, maxGrappleDistance);

        if (hit.collider != null)
        {
            // ターゲットに刺さった場合の処理
            grappleTarget = hit.point;
            lineRenderer.SetPosition(1, grappleTarget);
            isGrappling = true; // グラップル中フラグを設定
        }
        else
        {
            lineRenderer.SetPosition(1, targetPoint);
            isGrappling = false; // グラップル中フラグをリセット
        }
    }

    void PullPlayer()
    {
        Vector2 playerPosition = transform.position;

        // プレイヤーをターゲット位置に向かって引き寄せる
        if (Vector2.Distance(playerPosition, grappleTarget) > 0.1f)
        {
            Vector2 newPosition = Vector2.MoveTowards(playerPosition, grappleTarget, pullSpeed * Time.deltaTime);
            transform.position = newPosition;
        }
        else
        {
            ReleaseGrapple(); // 到達したらグラップルを終了
        }
    }

    void ReleaseGrapple()
    {
        isGrappling = false; // グラップルを解除
        lineRenderer.SetPosition(1, transform.position); // 線の終点をプレイヤーの位置に戻す
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
