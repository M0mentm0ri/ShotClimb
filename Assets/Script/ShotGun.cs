using UnityEngine;

public class ShotGun : MonoBehaviour
{
    //--- Weapon Settings ---
    [Header("Weapon Settings")]
    public Transform weaponTransform;                  // �����Transform
    [SerializeField] private float recoilForce = 5f;    // ������ (Inspector�Œ����\)
    [SerializeField] private float shotCooldown = 0.5f; // ���˃N�[���_�E�� (Inspector�Œ����\)
    private float cooldownTimer = 0f;                   // �N�[���_�E���̃^�C�}�[
    public ParticleSystem shotEffect;                   // ���˃G�t�F�N�g
    public GameObject[] ammoDisplayObjects;             // �e��̕\���p�I�u�W�F�N�g

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 5;           // �ő�e�� (Inspector�Œ����\)
    private int currentAmmo;                            // ���݂̒e��
    [SerializeField] private float reloadTime = 2f;     // �����[�h���� (Inspector�Œ����\)
    private float reloadTimer = 0f;                     // �����[�h�^�C�}�[

    //--- Movement Settings ---
    [Header("Movement Settings")]
    public Rigidbody2D playerRb;                        // �v���C���[�̃��W�b�g�{�f�B
    [SerializeField] private float groundForce = 10f; // �n��ł̈ړ���
    [SerializeField] private float airForce = 5f;    // �󒆂ł̈ړ���
    [SerializeField] private float accelFactor = 1.5f; // �����W�� (���x���Ɋ�Â�����)
    public bool isGrounded = false;             // �n��ɂ��邩�ǂ����̃t���O

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
        // �R���g���[���[�܂��̓L�[�{�[�h�̓��͂��擾 (Horizontal���� "A"�A"D" ����уR���g���[���[�̍��X�e�B�b�N�ɑΉ�)
        float moveInput = Input.GetAxis("Horizontal");

        // �ړ�����������i-1 ���A0 ��~�A1 �E�j
        Vector2 moveDirection = new Vector2(moveInput, 0);

        // ���݂̑��x���擾
        float currentSpeed = playerRb.velocity.x;

        // �n��ɂ��邩�ǂ������m�F
        if (isGrounded)
        {
            // �ڕW�Ƃ���ړ����x
            float targetSpeed = moveDirection.x * groundForce;

            // ���݂̑��x�Ƃ̍������߂�
            float speedDifference = targetSpeed - currentSpeed;

            // ������͂𑬓x�̍��Ɋ�Â��Ē����i���x���x���Ƃ��͋����A�����Ƃ��͎キ�j
            float forceToAdd = speedDifference * accelFactor;

            // AddForce�ő��x�����ɕۂ�
            playerRb.AddForce(new Vector2(forceToAdd, 0), ForceMode2D.Force);
        }
        else
        {
            // �󒆂ɂ���ꍇ��AddForce���g���Ĉړ��𐧌�
            if (moveDirection != Vector2.zero)
            {
                // �i��ł�������Ɠ����ꍇ�͗͂�������
                if (moveDirection.x == Mathf.Sign(currentSpeed))
                {
                    playerRb.AddForce(new Vector2(moveDirection.x * airForce, 0), ForceMode2D.Force);
                }
                // �t�����̏ꍇ���͂������āA���x������������
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
        playerRb.velocity = Vector2.zero;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        playerRb.AddForce(-direction.normalized * recoilForce, ForceMode2D.Impulse);
<<<<<<< HEAD

=======
>>>>>>> 0ee53c589557f912673d9ae4713cd2bf00136e89
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
