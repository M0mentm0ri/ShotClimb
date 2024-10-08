using UnityEngine;

public class ShotGun : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public Transform weaponTransform;
    public float recoilForce = 5f;
    public float shotCooldown = 0.5f;
    private float cooldownTimer = 0f;
    public ParticleSystem shotEffect;

    public int maxAmmo = 5;
    private int currentAmmo;
    public float stopReloadTime = 2f;
    private float stopTimer = 0f;
    public GameObject[] ammoDisplayObjects;
    public float reloadTime = 2f;
    private float reloadTimer = 0f;
    public bool isGrounded = false;

    public GameObject flagPrefab; // 旗のプレハブ
    private GameObject placedFlag; // 設置された旗
    public Transform flagParent; // 旗を配置する親オブジェクト
    public float teleportDistance = 10f; // テレポートする距離のしきい値
    public float flagPickupRange = 2f; // 旗を回収する範囲
    private bool hasFlag = true; // 手持ちに旗があるかどうか
    public float holdTime = 1.5f; // 旗を回収するための長押し時間
    private float holdTimer = 0f;

    private LineRenderer teleportLineRenderer; // テレポート距離の表示用
    private LineRenderer pickupLineRenderer; // 旗回収範囲の表示用
    public int circleSegmentCount = 50; // 円を構成する頂点の数

    [Header("Movement Settings")]
    private float keyHoldTime = 0f;
    public float maxHoldDuration = 1f; // 長押しの最大時間
    public float movementForceMultiplier = 2f; // 長押しに応じた移動力の倍率


    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();

        // LineRendererを作成
        CreateLineRenderer(ref teleportLineRenderer, Color.red);
        CreateLineRenderer(ref pickupLineRenderer, Color.green);
    }

    void Update()
    {
        AimWeapon();
        cooldownTimer -= Time.deltaTime;

        float speed = playerRb.velocity.magnitude;

        // リロードタイマーを更新
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
    }

    private void HandleFlagActions()
    {
        if (placedFlag != null)
        {
            float distanceToFlag = Vector2.Distance(transform.position, placedFlag.transform.position);

            // 旗の範囲外で自動的にテレポート
            if ((distanceToFlag > teleportDistance) || ((Input.GetKey(KeyCode.R) && !(distanceToFlag <= flagPickupRange))))
            {
                TeleportToFlag();
            }

            // 旗の近くでRを長押しすると旗を回収
            if (distanceToFlag <= flagPickupRange && Input.GetKey(KeyCode.R))
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= holdTime)
                {
                    PickupFlag();
                    holdTimer = 0f;
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }

        // 旗が手持ちにある場合、設置可能
        if (isGrounded && hasFlag && Input.GetKeyDown(KeyCode.R) && placedFlag == null)
        {
            PlaceFlag();
        }
    }

    private void PlaceFlag()
    {
        Vector3 placePosition = transform.position;
        placedFlag = Instantiate(flagPrefab, placePosition, Quaternion.identity, flagParent);
        hasFlag = false;

        // LineRendererを有効化
        teleportLineRenderer.enabled = true;
        pickupLineRenderer.enabled = true;
    }

    private void PickupFlag()
    {
        if (placedFlag != null)
        {
            Destroy(placedFlag); // 旗を削除
            placedFlag = null;
            hasFlag = true; // 旗を手持ちに戻す
        }
    }

    private void TeleportToFlag()
    {
        if (placedFlag != null)
        {
            transform.position = placedFlag.transform.position; // 旗の位置にテレポート
        }
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
                                                                           // 武器の向きを更新
            weaponTransform.rotation = Quaternion.Euler(0, 180, 180 - angle);
        }
        else
        {
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0); // 左向きのためX軸を反転
                                                                         // 武器の向きを更新
            weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }


    void ApplyRecoil()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        playerRb.AddForce(-direction.normalized * recoilForce, ForceMode2D.Impulse);
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