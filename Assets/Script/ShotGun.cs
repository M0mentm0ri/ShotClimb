using System.Collections;
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
    public ParticleSystem powerEffect;                  // ���˃G�t�F�N�g
    public GameObject[] ammoDisplayObjects;             // �e��̕\���p�I�u�W�F�N�g
                                                        // ���u�ԂɃA�N�e�B�u�ɂ���I�u�W�F�N�g
    public GameObject flashObject; // ��: �e���̃t���b�V��
    public Shaker shaker;
    public ParticleSystem DamageEffect;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 5;           // �ő�e�� (Inspector�Œ����\)
    private int currentAmmo;                            // ���݂̒e��
    [SerializeField] private float reloadTime = 2f;     // �����[�h���� (Inspector�Œ����\)
    private float reloadTimer = 0f;                     // �����[�h�^�C�}�[

    //--- Movement Settings ---
    [Header("Movement Settings")]
    public Rigidbody2D playerRb;                        // �v���C���[�̃��W�b�g�{�f�B
    [SerializeField] private float groundForce = 10f;   // �n��ł̈ړ���
    [SerializeField] private float airForce = 5f;       // �󒆂ł̈ړ���
    [SerializeField] private float accelFactor = 1.5f;  // �����W�� (���x���Ɋ�Â�����)
    public bool isGrounded = false;                     // �n��ɂ��邩�ǂ����̃t���O
    private bool isDamage = false;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilMultiplier = 1.5f; // �n�ʂɋ߂��ꍇ�̃��R�C���{��
    [SerializeField] private float distanceThreshold = 0.5f; // �n�ʂƂ̋�����臒l

    public Respawn respawn;

    void Start()
    {
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
                Debug.Log($"���̑��̃^�O: {hit.collider.tag}");
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
            if(!isDamage)
            // 0.2�b��Ƀv���C���[�����X�|�[��
            StartCoroutine(RespawnAfterDelay(0.2f));
        }
            
    }

    // �v���C���[��x����Ƀ��X�|�[��������R���[�`��
    private IEnumerator RespawnAfterDelay(float delay)
    {
        isDamage = true;

        yield return new WaitForSeconds(delay);

        DamageEffect.Play();

        // �w�肳�ꂽ���ԑҋ@
        yield return new WaitForSeconds(delay);

        // �v���C���[�����X�|�[��
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
