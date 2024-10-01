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
    public float stopReloadTime = 2f; // リロードまでの停止時間
    private float stopTimer = 0f; // 停止時間を計測するタイマー
    public GameObject[] ammoDisplayObjects; // 弾薬表示用オブジェクト
    public float reloadTime = 2f; // リロード間隔
    private float reloadTimer = 0f; // リロード用のタイマー
    public bool isGrounded = false; // 地面に接触しているかどうか

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

        // リロードタイマーを更新（地面に接触している場合のみ）
        if (isGrounded || speed <= 0.1f) // プレイヤーが停止している場合
        {
            reloadTimer += Time.deltaTime; // リロードタイマーを進める
            if (reloadTimer >= reloadTime)
            {
                ReloadAmmo();
                reloadTimer = 0f; // リロード後、タイマーをリセット
            }
        }
        else
        {
            reloadTimer = 0f; // プレイヤーが動いている場合、タイマーをリセット
        }

        // 弾があるか確認して、発射可能かチェック
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f && currentAmmo > 0)
        {
            ApplyRecoil();
            PlayShotEffect(); // パーティクル再生
            currentAmmo--; // 残弾数を減らす
            UpdateAmmoDisplay(); // 残弾数表示を更新
            cooldownTimer = shotCooldown; // クールダウンタイマーをリセット
        }

        // 弾薬が0の場合は発射できない
        if (currentAmmo <= 0)
        {
            Debug.Log("残弾がありません。リロードしてください。");
        }
    }

    private void ReloadAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo++; // 残弾数を1発増やす
            UpdateAmmoDisplay(); // 残弾数表示を更新
        }
    }

    void AimWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void ApplyRecoil()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        playerRb.AddForce(direction.normalized * recoilForce, ForceMode2D.Impulse); // ここはそのままの方向に力を加える
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
            isGrounded = true; // 地面に接触中
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false; // 地面から離れた
        }
    }
}
