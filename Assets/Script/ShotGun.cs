using UnityEngine;

public class ShotGun : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public Transform weaponTransform;

    [SerializeField] private float recoilForce = 5f; // Inspector�Œ����\
    [SerializeField] private float shotCooldown = 0.5f; // Inspector�Œ����\
    private float cooldownTimer = 0f;
    public ParticleSystem shotEffect;
    public GameObject[] ammoDisplayObjects;

    [SerializeField] private int maxAmmo = 5; // Inspector�Œ����\
    private int currentAmmo;
    [SerializeField] private float reloadTime = 2f; // Inspector�Œ����\
    private float reloadTimer = 0f;
    public bool isGrounded = false;

    [Header("Movement Settings")]
    [SerializeField] private float keyHoldTime = 0f; // Inspector�Œ����\
    [SerializeField] private float maxHoldDuration = 1f; // Inspector�Œ����\
    [SerializeField] private float movementForceMultiplier = 2f; // Inspector�Œ����\

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

        // �����[�h�^�C�}�[���X�V
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

        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 moveDirection = Vector2.zero;

        // ������Ă���L�[�ɉ������ړ�����������
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = Vector2.right;
        }

        // ���݂̑��x���擾
        float currentSpeed = playerRb.velocity.x;

        // ��s�����n��ɂ��邩�𔻒f
        if (isGrounded)
        {
            // �n��ɂ���ꍇ�̓���
            if (moveDirection != Vector2.zero)
            {
                // ������Ă�������Ɉ��̑��x�ňړ�
                playerRb.velocity = new Vector2(moveDirection.x * movementForceMultiplier, playerRb.velocity.y);
            }
            else
            {
                // �����Ȃ��Ƃ��͑��x���[���ɂ���
                playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            }
        }
        else
        {
            // ��s���̓���
            if (moveDirection != Vector2.zero)
            {
                // ���������̃L�[�������ƈ��̑��x�Ŕ�ё�����
                if (moveDirection.x == Mathf.Sign(currentSpeed))
                {
                    playerRb.velocity = new Vector2(moveDirection.x * movementForceMultiplier, playerRb.velocity.y);
                }
                // �t�����̃L�[�������ƁA��s���������̑��x�Ō���������
                else
                {
                    playerRb.velocity = new Vector2(moveDirection.x * movementForceMultiplier, playerRb.velocity.y);
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

        // �e�I�u�W�F�N�g�̃X�P�[���𔽓]
        if (angle > 90 || angle < -90) // ���������p�x�͈�
        {
            weaponTransform.parent.rotation = Quaternion.Euler(0, 180, 0); // �������̂���X���𔽓]
            // ����̌������X�V
            weaponTransform.rotation = Quaternion.Euler(0, 180, 180 - angle);
        }
        else
        {
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0); // �E�����̂���X���𔽓]
            // ����̌������X�V
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
}
