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

    [Header("Movement Settings")]
    private float keyHoldTime = 0f;
    public float maxHoldDuration = 1f; // �������̍ő厞��
    public float movementForceMultiplier = 2f; // �������ɉ������ړ��͂̔{��


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

<<<<<<< HEAD
        // ���̐ݒu�A�e���|�[�g�@�\
        HandleFlagActions();

        // �����ݒu����Ă���ꍇ�A�͈͂�`��
        if (placedFlag != null)
        {
            DrawCircle(teleportLineRenderer, teleportDistance, placedFlag.transform.position);
            DrawCircle(pickupLineRenderer, flagPickupRange, placedFlag.transform.position);
        }
        else
        {
            // �����Ȃ��ꍇ��LineRenderer�𖳌���
            teleportLineRenderer.enabled = false;
            pickupLineRenderer.enabled = false;
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
=======
        HandleMovement(); // �ړ�������ǉ�
>>>>>>> a60e37c603de3805e3e4718c5a9102400c58c952
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
<<<<<<< HEAD
                                                                           // ����̌������X�V
=======
>>>>>>> a60e37c603de3805e3e4718c5a9102400c58c952
            weaponTransform.rotation = Quaternion.Euler(0, 180, 180 - angle);
        }
        else
        {
<<<<<<< HEAD
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0); // �������̂���X���𔽓]
                                                                         // ����̌������X�V
=======
            weaponTransform.parent.rotation = Quaternion.Euler(0, 0, 0); // �E����
>>>>>>> a60e37c603de3805e3e4718c5a9102400c58c952
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
<<<<<<< HEAD

    // LineRenderer��������
    private void CreateLineRenderer(ref LineRenderer lineRenderer, Color color)
    {
        GameObject lineObj = new GameObject("LineRenderer");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.positionCount = circleSegmentCount + 1;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.enabled = false; // �f�t�H���g�͖�����
    }

    // �~��`��
    private void DrawCircle(LineRenderer lineRenderer, float radius, Vector3 center)
    {
        float angle = 0f;
        for (int i = 0; i <= circleSegmentCount; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x + center.x, y + center.y, 0));
            angle += (360f / circleSegmentCount);
        }
    }
}
=======
}
>>>>>>> a60e37c603de3805e3e4718c5a9102400c58c952
