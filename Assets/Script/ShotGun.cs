using UnityEngine;

public class ShotGun : MonoBehaviour
{
    //--- Weapon Settings ---
    [Header("Weapon Settings")]
    public Transform weaponTransform;                  // 武器のTransform
    [SerializeField] private float recoilForce = 5f;    // 反動力 (Inspectorで調整可能)
    [SerializeField] private float shotCooldown = 0.5f; // 発射クールダウン (Inspectorで調整可能)
    private float cooldownTimer = 0f;                   // クールダウンのタイマー
    public ParticleSystem shotEffect;                   // 発射エフェクト
    public GameObject[] ammoDisplayObjects;             // 弾薬の表示用オブジェクト

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 5;           // 最大弾薬数 (Inspectorで調整可能)
    private int currentAmmo;                            // 現在の弾薬数
    [SerializeField] private float reloadTime = 2f;     // リロード時間 (Inspectorで調整可能)
    private float reloadTimer = 0f;                     // リロードタイマー

    //--- Movement Settings ---
    [Header("Movement Settings")]
    public Rigidbody2D playerRb;                        // プレイヤーのリジットボディ
    [SerializeField] private float groundForce = 10f; // 地上での移動力
    [SerializeField] private float airForce = 5f;    // 空中での移動力
    [SerializeField] private float accelFactor = 1.5f; // 加速係数 (速度差に基づく調整)
    public bool isGrounded = false;             // 地上にいるかどうかのフラグ

    [Header("Recoil Settings")]
    [SerializeField] private float recoilMultiplier = 1.5f; // 地面に近い場合のリコイル倍率
    [SerializeField] private float distanceThreshold = 0.5f; // 地面との距離の閾値

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

        HandleMovement();
    }

    void HandleMovement()
    {
        // コントローラーまたはキーボードの入力を取得 (Horizontal軸は "A"、"D" およびコントローラーの左スティックに対応)
        float moveInput = Input.GetAxis("Horizontal");

        // 移動方向を決定（-1 左、0 停止、1 右）
        Vector2 moveDirection = new Vector2(moveInput, 0);

        // 現在の速度を取得
        float currentSpeed = playerRb.velocity.x;

        // 地上にいるかどうかを確認
        if (isGrounded)
        {
            // 入力がある場合のみ処理を行う
            if (moveDirection.x != 0)
            {
                // 目標とする移動速度
                float targetSpeed = moveDirection.x * groundForce;

                // 現在の速度との差を求める
                float speedDifference = targetSpeed - currentSpeed;

                // 加える力を速度の差に基づいて調整（速度が遅いときは強く、速いときは弱く）
                float forceToAdd = speedDifference * accelFactor;

                // AddForceで速度を一定に保つ
                playerRb.AddForce(new Vector2(forceToAdd, 0), ForceMode2D.Force);
            }
        }
        else
        {
            // 空中にいる場合はAddForceを使って移動を制御
            if (moveDirection != Vector2.zero)
            {
                // 進んでいる方向と同じ場合は力を加える
                if (moveDirection.x == Mathf.Sign(currentSpeed))
                {
                    playerRb.AddForce(new Vector2(moveDirection.x * airForce, 0), ForceMode2D.Force);
                }
                // 逆方向の場合も力を加えて、速度を減少させる
                else
                {
                    playerRb.AddForce(new Vector2(moveDirection.x * airForce, 0), ForceMode2D.Force);
                }
            }
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
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0); // 右向きのためX軸を反転
            // 武器の向きを更新
            weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void ApplyRecoil()
    {
        // プレイヤーの速度をゼロにリセット
        playerRb.velocity = Vector2.zero;

        // マウスの位置を取得し、ワールド座標に変換
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // 射撃方向にオフセットを追加
        Vector2 offset = direction * 2.5f; // ここでオフセットの長さを調整
        Vector2 rayOrigin = (Vector2)transform.position + offset;

        // 地面との距離を計測
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distanceThreshold);

        // レイキャストを視覚的に表示
        Debug.DrawRay(rayOrigin, direction * distanceThreshold, Color.red, 5f);

        // 加える反動を調整
        float adjustedRecoilForce = recoilForce;
        if (hit.collider != null)
        {
            // 当たったオブジェクトのタグを確認
            if (hit.collider.CompareTag("Ground"))
            {
                Debug.Log("Ground!");
                adjustedRecoilForce *= 1.5f; // Groundタグの場合、反動を1.5倍に
            }
            else
            {
                Debug.Log($"その他のタグ: {hit.collider.tag}"); // hit.collider.tagでタグを取得
            }
        }

        // 正しい方向に反動を加える
        playerRb.AddForce(-direction.normalized * adjustedRecoilForce, ForceMode2D.Impulse);
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

    private void OnTriggerStay2D(Collider2D other)
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
}
