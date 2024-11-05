using System.Collections;
using UnityEngine;

public class ShotGun : MonoBehaviour
{
    // --- Weapon Settings ---
    [Header("Weapon Settings")]
    public Transform weaponTransform;                  // �����Transform
    [SerializeField] private float recoilForce = 5f;    // ������ (Inspector�Œ����\)
    [SerializeField] private float shotCooldown = 0.5f; // ���˃N�[���_�E�� (Inspector�Œ����\)
    private float cooldownTimer = 0f;                   // �N�[���_�E���̃^�C�}�[
    public ParticleSystem shotEffect;                   // ���˃G�t�F�N�g
    public ParticleSystem powerEffect;                  // ���˃G�t�F�N�g
    public GameObject[] ammoDisplayObjects;             // �e��̕\���p�I�u�W�F�N�g
    public GameObject flashObject; // ��: �e���̃t���b�V��
    public Shaker shaker;
    public ParticleSystem DamageEffect;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 5;           // �ő�e�� (Inspector�Œ����\)
    private int currentAmmo;                            // ���݂̒e��
    [SerializeField] private float reloadTime = 2f;     // �����[�h���� (Inspector�Œ����\)
    private float reloadTimer = 0f;                     // �����[�h�^�C�}�[

    // --- Movement Settings ---
    [Header("Movement Settings")]
    public Rigidbody2D playerRb;                        // �v���C���[�̃��W�b�g�{�f�B
    [SerializeField] private float groundForce = 10f;   // �n��ł̈ړ���
    [SerializeField] private float airForce = 5f;       // �󒆂ł̈ړ���
    [SerializeField] private float accelFactor = 1.5f;  // �����W�� (���x���Ɋ�Â�����)
    public bool isGrounded = false;                     // �n��ɂ��邩�ǂ����̃t���O
    private bool isDamage = false;

    [Header("Recoil Settings")]
    public ShotGunTrigger shotGunTrigger;  // ����p�g���K�[�I�u�W�F�N�g�̎Q��
    [SerializeField] private float recoilMultiplier = 1.5f; // �n�ʂɋ߂��ꍇ�̃��R�C���{��
    public GameObject respawnlight;

    public Respawn respawn;

    // Animator�̎Q�Ƃ�ǉ�
    public Animator animator;

    void Start()
    {
        respawnlight.SetActive(false);
        // shotGunTrigger�̎Q�Ƃ�ݒ肷��
        if (shotGunTrigger == null)
        {
            shotGunTrigger = GetComponentInChildren<ShotGunTrigger>();
        }
        // �I�u�W�F�N�g���A�N�e�B�u�ɂ���
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

        if(animator == null)
        {
            // Animator�R���|�[�l���g�̎擾
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        AimWeapon();
        cooldownTimer -= Time.deltaTime;

        float speed = playerRb.velocity.magnitude;

        // �A�j���[�V�����̏�Ԃ��X�V
        UpdateAnimationStates();

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
            
            PlayShotEffect();
            ApplyRecoil();
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
        // �R���g���[���[�܂��̓L�[�{�[�h�̓��͂��擾
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

    // 0.1�b�ԃI�u�W�F�N�g���A�N�e�B�u�ɂ���
    private IEnumerator FlashActiveObject()
    {
        // �I�u�W�F�N�g���A�N�e�B�u�ɂ���
        flashObject.SetActive(true);

        // 0.1�b�ҋ@
        yield return new WaitForSeconds(0.1f);

        // �I�u�W�F�N�g���A�N�e�B�u�ɂ���
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
        // ���������̌v�Z
        playerRb.velocity = Vector2.zero;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        float adjustedRecoilForce = recoilForce;

        // �n�ʂ��߂��ꍇ�A���R�C���{���𑝂₷
        if (shotGunTrigger != null && shotGunTrigger.CheckGroundClose())
        {
            Debug.Log("Ground!");
            // �n�ʂ��߂��̂Ń��R�C���{����K�p
             powerEffect.Play();
            adjustedRecoilForce *= recoilMultiplier;
        }

        // �v�Z�������R�C���̗͂�������
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
            // �e��𑝉������A�\�����X�V
            if (currentAmmo < maxAmmo)
            {
                currentAmmo++;
                UpdateAmmoDisplay();
            }

            // �Փ˂����e��I�u�W�F�N�g���폜
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Needle"))
        {
            if (!isDamage)
                // 0.2�b��Ƀv���C���[�����X�|�[��
                StartCoroutine(RespawnAfterDelay(0.2f));
        }
    }

    // �v���C���[��x����Ƀ��X�|�[��������R���[�`��
    private IEnumerator RespawnAfterDelay(float delay)
    {
        shaker.Shake();
        // �v���C���[�̑��x���[���ɐݒ肵�A���������Z�b�g

        respawnlight.SetActive(true);
        playerRb.velocity = Vector2.zero;

        playerRb.simulated = false;
        isDamage = true;

        yield return new WaitForSeconds(delay);

        DamageEffect.Play();

        // �w�肳�ꂽ���ԑҋ@
        yield return new WaitForSeconds(delay);

        respawnlight.SetActive(false);
        // �v���C���[�����X�|�[��
        respawn.RespawnPlayer();
        playerRb.simulated = true;
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

    // �A�j���[�V�����̏�Ԃ��X�V���郁�\�b�h
    private void UpdateAnimationStates()
    {
        // �󒆂��ǂ������`�F�b�N
        bool isInAir = !isGrounded;
        animator.SetBool("IsJump", isInAir);

        // ���s��Ԃ�ݒ�
        float moveInput = Input.GetAxis("Horizontal");
        bool isWalking = Mathf.Abs(moveInput) > 0.1f;
        animator.SetBool("IsWalk", isWalking);
    }
}
