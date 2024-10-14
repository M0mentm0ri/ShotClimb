using UnityEngine;

public class ShotGun : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public Transform weaponTransform;

    [SerializeField] private float recoilForce = 5f; // Inspectorで調整可能
    [SerializeField] private float shotCooldown = 0.5f; // Inspectorで調整可能
    private float cooldownTimer = 0f;
    public ParticleSystem shotEffect;
    public GameObject[] ammoDisplayObjects;

    [SerializeField] private int maxAmmo = 5; // Inspectorで調整可能
    private int currentAmmo;
    [SerializeField] private float reloadTime = 2f; // Inspectorで調整可能
    private float reloadTimer = 0f;
    public bool isGrounded = false;

    [Header("Movement Settings")]
    [SerializeField] private float keyHoldTime = 0f; // Inspectorで調整可能
    [SerializeField] private float maxHoldDuration = 1f; // Inspectorで調整可能
    [SerializeField] private float movementForceMultiplier = 2f; // Inspectorで調整可能
    private float adjustedRecoilForce;

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

       /* HandleMovement()*/;
    }

    //void HandleMovement()
    //{
    //    Vector2 moveDirection = Vector2.zero;

    //    // 押されているキーに応じた移動方向を決定
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        moveDirection = Vector2.left;
    //    }
    //    else if (Input.GetKey(KeyCode.D))
    //    {
    //        moveDirection = Vector2.right;
    //    }

    //    if (isGrounded)
    //    {
    //        // 地上にいる場合の動き
    //        if (moveDirection != Vector2.zero)
    //        {
    //            // 押されている方向に一定の速度で移動
    //            playerRb.velocity = new Vector2(moveDirection.x * movementForceMultiplier, playerRb.velocity.y);
    //        }
    //        else
    //        {
    //            // 動かないときは速度をゼロにする
    //            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
    //        }
    //    }
    //    else
    //    {
    //        // 空中にいる場合
    //        if (moveDirection != Vector2.zero)
    //        {
    //            // 空中でのふわっとした動き
    //            if (playerRb.velocity.x < 5f && playerRb.velocity.x > -5f) // 横方向の速度を制限
    //            {
    //                playerRb.AddForce(moveDirection * movementForceMultiplier * 0.5f, ForceMode2D.Force); // 力を半分に
    //            }
    //        }

    //        // 空中で加速しないように抵抗を加える
    //        playerRb.velocity = new Vector2(playerRb.velocity.x * 0.98f, playerRb.velocity.y); // 空中での減速
    //    }
    //}


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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // 水平方向の反動にも対応するように力を調整
        float recoilForceX = adjustedRecoilForce * Mathf.Cos(Mathf.Atan2(direction.y, direction.x));
        float recoilForceY = adjustedRecoilForce * Mathf.Sin(Mathf.Atan2(direction.y, direction.x));

        // 正しい反動を与える
        playerRb.AddForce(new Vector2(-recoilForceX, -recoilForceY), ForceMode2D.Impulse);

        // 地面や壁との距離を測定
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f);

        // リコイルを強化する条件
        if (hit.collider != null && hit.distance <= 0.5f)
        {
            adjustedRecoilForce = recoilForce * 1.6f;  // 近い場合リコイル力を強化
        }
        else
        {
            adjustedRecoilForce = recoilForce;  // デフォルトのリコイル力
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
}
