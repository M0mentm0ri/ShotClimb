using UnityEngine;

public class ShotGun : MonoBehaviour
{
    // プレイヤーのRigidbody2D
    private Rigidbody2D playerRb;

    // 武器（プレイヤーの子オブジェクト）への参照
    public Transform weaponTransform;

    // 反動の力
    public float recoilForce = 5f;

    // 発射のクールダウンタイム
    public float shotCooldown = 0.5f;

    // 内部クールダウンタイマー
    private float cooldownTimer = 0f;

    // パーティクルシステム（ショットガンの発射エフェクト）
    public ParticleSystem shotEffect;

    // 残弾数の変数
    public int maxAmmo = 5; // 最大弾数
    private int currentAmmo; // 現在の弾数

    // 弾薬表示用のオブジェクト
    public GameObject[] ammoDisplayObjects; // 弾薬を表示するためのオブジェクト

    void Start()
    {
        // Rigidbody2D コンポーネントを取得
        playerRb = GetComponent<Rigidbody2D>();

        // 残弾数を初期化
        currentAmmo = maxAmmo;

        // 残弾数の表示を初期化
        UpdateAmmoDisplay();
    }

    void Update()
    {
        // 常に武器の方向をマウスに向ける
        AimWeapon();

        // クールダウンのタイマーを更新
        cooldownTimer -= Time.deltaTime;

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

    // マウスの方向に武器を向ける
    void AimWeapon()
    {
        // マウス位置を取得し、プレイヤーからの方向を計算
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // 子オブジェクト（武器）の方向を設定（常にマウスを向く）
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 発射後に反動を加える
    void ApplyRecoil()
    {
        // マウス位置を取得し、プレイヤーからの方向を計算
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // プレイヤーに反動を加える（発射方向とは逆）
        playerRb.AddForce(direction.normalized * recoilForce, ForceMode2D.Impulse);

        // 発射時のロジック（例えば、弾丸生成など）はここに追加可能
    }

    // パーティクルエフェクトを再生する
    void PlayShotEffect()
    {
        // パーティクルシステムが設定されている場合のみ再生
        if (shotEffect != null)
        {
            shotEffect.Play();
        }
    }

    // 残弾数の表示を更新する
    void UpdateAmmoDisplay()
    {
        // ammoDisplayObjects の長さと maxAmmo が一致していることが前提
        for (int i = 0; i < ammoDisplayObjects.Length; i++)
        {
            // 残弾数がまだある場合は表示し、それ以外は非表示にする
            if (i < currentAmmo)
            {
                ammoDisplayObjects[i].SetActive(true);
            }
            else
            {
                ammoDisplayObjects[i].SetActive(false);
            }
        }
    }

    // 弾薬をリロードする（特定のオブジェクトに触れた時に呼ばれる）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            // 残弾数を最大にリロード
            currentAmmo = maxAmmo;

            // 弾薬表示を更新
            UpdateAmmoDisplay();
        }
    }
}
