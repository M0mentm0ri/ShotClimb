using System.Collections;
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
    public ParticleSystem powerEffect;                  // 発射エフェクト
    public GameObject[] ammoDisplayObjects;             // 弾薬の表示用オブジェクト
                                                        // 撃つ瞬間にアクティブにするオブジェクト
    public GameObject flashObject; // 例: 銃口のフラッシュ
    public Shaker shaker;
    public ParticleSystem DamageEffect;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 5;           // 最大弾薬数 (Inspectorで調整可能)
    private int currentAmmo;                            // 現在の弾薬数
    [SerializeField] private float reloadTime = 2f;     // リロード時間 (Inspectorで調整可能)
    private float reloadTimer = 0f;                     // リロードタイマー

    //--- Movement Settings ---
    [Header("Movement Settings")]
    public Rigidbody2D playerRb;                        // プレイヤーのリジットボディ
    [SerializeField] private float groundForce = 10f;   // 地上での移動力
    [SerializeField] private float airForce = 5f;       // 空中での移動力
    [SerializeField] private float accelFactor = 1.5f;  // 加速係数 (速度差に基づく調整)
    public bool isGrounded = false;                     // 地上にいるかどうかのフラグ
    private bool isDamage = false;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilMultiplier = 1.5f; // 地面に近い場合のリコイル倍率
    [SerializeField] private float distanceThreshold = 0.5f; // 地面との距離の閾値

    public Respawn respawn;

    void Start()
    {
        // オブジェクトをアクティブにする
        flashObject.SetActive(false);
        if (shaker == null)
        {
            shaker = GetComponent<Shaker>();
        }
        if (respawn == null)
        {
            respawn = GetComponent<Respawn>();
        }

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
            shaker.Shake();
            UpdateAmmoDisplay();
            cooldownTimer = shotCooldown;
            StartCoroutine(FlashActiveObject());

        }

        HandleMovement();
    }

    void HandleMovement()
    {
        // コントローラーまたはキーボードの入力を取得
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(moveInput, 0);
        float currentSpeed = playerRb.velocity.x;

        if (isGrounded)
        {
            if (moveDirection.x != 0)
            {
                float targetSpeed = moveDirection.x * groundForce;
                float speedDifference = targetSpeed - currentSpeed;
                float forceToAdd = speedDifference * accelFactor;
                playerRb.AddForce(new Vector2(forceToAdd, 0), ForceMode2D.Force);
            }
        }
        else
        {
            if (moveDirection != Vector2.zero)
            {
                if (moveDirection.x == Mathf.Sign(currentSpeed))
                {
                    playerRb.AddForce(new Vector2(moveDirection.x * airForce, 0), ForceMode2D.Force);
                }
                else
                {
                    playerRb.AddForce(new Vector2(moveDirection.x * airForce, 0), ForceMode2D.Force);
                }
            }
        }
    }

    // 0.1秒間オブジェクトをアクティブにする
    private IEnumerator FlashActiveObject()
    {
        // オブジェクトをアクティブにする
        flashObject.SetActive(true);

        // 0.1秒待機
        yield return new WaitForSeconds(0.1f);

        // オブジェクトを非アクティブにする
        flashObject.SetActive(false);
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

        if (angle > 90 || angle < -90)
        {
            weaponTransform.parent.rotation = Quaternion.Euler(0, 180, 0);
            weaponTransform.rotation = Quaternion.Euler(0, 180, 180 - angle);
        }
        else
        {
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0);
            weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void ApplyRecoil()
    {
        playerRb.velocity = Vector2.zero;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        Vector2 offset = direction * 2.5f;
        Vector2 rayOrigin = (Vector2)transform.position + offset;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distanceThreshold);
        Debug.DrawRay(rayOrigin, direction * distanceThreshold, Color.red, 5f);

        float adjustedRecoilForce = recoilForce;
        if (hit.collider != null)
        {
            powerEffect.Play();
            adjustedRecoilForce *= 1.5f;
            if (hit.collider.CompareTag("Ground"))
            {
                Debug.Log("Ground!");
            }
            else
            {
                Debug.Log($"その他のタグ: {hit.collider.tag}");
            }
        }

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
        else if (other.CompareTag("Ammo"))
        {
            // 弾薬を増加させ、表示を更新
            if (currentAmmo < maxAmmo)
            {
                currentAmmo++;
                UpdateAmmoDisplay();
            }

            // 衝突した弾薬オブジェクトを削除
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Needle"))
        {
            if(!isDamage)
            // 0.2秒後にプレイヤーをリスポーン
            StartCoroutine(RespawnAfterDelay(0.2f));
        }
            
    }

    // プレイヤーを遅延後にリスポーンさせるコルーチン
    private IEnumerator RespawnAfterDelay(float delay)
    {
        isDamage = true;

        yield return new WaitForSeconds(delay);

        DamageEffect.Play();

        // 指定された時間待機
        yield return new WaitForSeconds(delay);

        // プレイヤーをリスポーン
        respawn.RespawnPlayer();

        isDamage = false;
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
