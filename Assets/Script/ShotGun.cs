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

    public float walkSpeed = 2f; // �������x
    public Animator animator; // �A�j���[�^�[�̎Q��

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

        // �����[�h�^�C�}�[���X�V
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

        // �e�����邩�m�F���Ĕ���
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f && currentAmmo > 0)
        {
            ApplyRecoil();
            PlayShotEffect();
            currentAmmo--;
            UpdateAmmoDisplay();
            cooldownTimer = shotCooldown;
        }

        HandleMovement(); // �ړ�������ǉ�
    }

    private void HandleMovement()
    {

    }

    void ApplyRecoil()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // ������^����ۂ�X����Y���ɗ͂�������B�����͗͂ŉ��Z
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

        // �e�I�u�W�F�N�g�̃X�P�[���𔽓]
        if (angle > 90 || angle < -90) // ���������p�x�͈�
        {
            weaponTransform.parent.rotation = Quaternion.Euler(0, 180, 0); // �������̂���X���𔽓]
            weaponTransform.rotation = Quaternion.Euler(0, 180, 180 - angle);
        }
        else
        {
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0); // �E����
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
}
