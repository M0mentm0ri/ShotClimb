using UnityEngine;

public class ShotGun : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public Transform weaponTransform;
    public float recoilForce = 5f;
    public float shotCooldown = 0.5f;
    private float cooldownTimer = 0f;
    public ParticleSystem shotEffect;
    public GameObject[] ammoDisplayObjects;

    public int maxAmmo = 5;
    private int currentAmmo;
    public float reloadTime = 2f;
    private float reloadTimer = 0f;
    public bool isGrounded = false;

    public float walkSpeed = 2f; // 歩き速度
    public Animator animator; // アニメーターの参照

    [Header("Movement Settings")]
    private float keyHoldTime = 0f;
    public float maxHoldDuration = 1f; // 長押しの最大時間
    public float movementForceMultiplier = 2f; // 長押しに応じた移動力の倍率


    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
    }

    void Update()
    {
        AimWeapon();
        cooldownTimer -= Time.deltaTime;

        // リロードタイマーを更新
        float speed = playerRb.velocity.magnitude;
        if (isGrounded || speed <= 0.1f)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime)
            {
                ReloadAmmo();
                reloadTimer = 0f;
            }
        }
        else
        {
            reloadTimer = 0f;
        }

        // 弾があるか確認して発射
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f && currentAmmo > 0)
        {
            ApplyRecoil();
            PlayShotEffect();
            currentAmmo--;
            UpdateAmmoDisplay();
            cooldownTimer = shotCooldown;
        }

<<<<<<< HEAD
        // 旗の設置、テレポート機能
        HandleFlagActions();

        // 旗が設置されている場合、範囲を描画
        if (placedFlag != null)
        {
            DrawCircle(teleportLineRenderer, teleportDistance, placedFlag.transform.position);
            DrawCircle(pickupLineRenderer, flagPickupRange, placedFlag.transform.position);
        }
        else
        {
            // 旗がない場合はLineRendererを無効化
            teleportLineRenderer.enabled = false;
            pickupLineRenderer.enabled = false;
        }

        HandleMovement();

    }

    void HandleMovement()
    {
        Vector2 moveDirection = Vector2.zero;

        // 押されているキーに応じた移動方向を決定
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = Vector2.right;
        }

        // 現在の速度を取得
        float currentSpeed = playerRb.velocity.x;

        // 飛行中か地上にいるかを判断
        if (isGrounded)
        {
            // 地上にいる場合の動き
            if (moveDirection != Vector2.zero)
            {
                // 押されている方向に一定の速度で移動
                playerRb.velocity = new Vector2(moveDirection.x * movementForceMultiplier, playerRb.velocity.y);
            }
            else
            {
                // 動かないときは速度をゼロにする
                playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            }
        }
        else
        {
            // 飛行中の動き
            if (moveDirection != Vector2.zero)
            {
                // 同じ方向のキーを押すと一定の速度で飛び続ける
                if (moveDirection.x == Mathf.Sign(currentSpeed))
                {
                    playerRb.velocity = new Vector2(moveDirection.x * movementForceMultiplier, playerRb.velocity.y);
                }
                // 逆方向のキーを押すと、飛行距離を一定の速度で減少させる
                else
                {
                    playerRb.velocity = new Vector2(moveDirection.x * movementForceMultiplier, playerRb.velocity.y);
                }
            }
        }
=======
        HandleMovement(); // 移動処理を追加
>>>>>>> a60e37c603de3805e3e4718c5a9102400c58c952
    }

    private void HandleMovement()
    {

    }

    void ApplyRecoil()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // 反動を与える際はX軸とY軸に力を加える。反動は力で加算
        playerRb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
    }

    private void ReloadAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo++;
            UpdateAmmoDisplay();
        }
    }

    void AimWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 親オブジェクトのスケールを反転
        if (angle > 90 || angle < -90) // 左を向く角度範囲
        {
            weaponTransform.parent.rotation = Quaternion.Euler(0, 180, 0); // 左向きのためX軸を反転
<<<<<<< HEAD
                                                                           // 武器の向きを更新
=======
>>>>>>> a60e37c603de3805e3e4718c5a9102400c58c952
            weaponTransform.rotation = Quaternion.Euler(0, 180, 180 - angle);
        }
        else
        {
<<<<<<< HEAD
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0); // 左向きのためX軸を反転
                                                                         // 武器の向きを更新
=======
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0); // 右向き
>>>>>>> a60e37c603de3805e3e4718c5a9102400c58c952
            weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void PlayShotEffect()
    {
        if (shotEffect != null)
        {
            shotEffect.Play();
        }
    }

    void UpdateAmmoDisplay()
    {
        for (int i = 0; i < ammoDisplayObjects.Length; i++)
        {
            ammoDisplayObjects[i].SetActive(i < currentAmmo);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
<<<<<<< HEAD

    // LineRendererを初期化
    private void CreateLineRenderer(ref LineRenderer lineRenderer, Color color)
    {
        GameObject lineObj = new GameObject("LineRenderer");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.positionCount = circleSegmentCount + 1;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.enabled = false; // デフォルトは無効化
    }

    // 円を描画
    private void DrawCircle(LineRenderer lineRenderer, float radius, Vector3 center)
    {
        float angle = 0f;
        for (int i = 0; i <= circleSegmentCount; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x + center.x, y + center.y, 0));
            angle += (360f / circleSegmentCount);
        }
    }
}
=======
}
>>>>>>> a60e37c603de3805e3e4718c5a9102400c58c952
