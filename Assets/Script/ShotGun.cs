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
    public float stopReloadTime = 2f; // �����[�h�܂ł̒�~����
    private float stopTimer = 0f; // ��~���Ԃ��v������^�C�}�[
    public GameObject[] ammoDisplayObjects; // �e��\���p�I�u�W�F�N�g
    public float reloadTime = 2f; // �����[�h�Ԋu
    private float reloadTimer = 0f; // �����[�h�p�̃^�C�}�[
    public bool isGrounded = false; // �n�ʂɐڐG���Ă��邩�ǂ���

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

        // �����[�h�^�C�}�[���X�V�i�n�ʂɐڐG���Ă���ꍇ�̂݁j
        if (isGrounded || speed <= 0.1f) // �v���C���[����~���Ă���ꍇ
        {
            reloadTimer += Time.deltaTime; // �����[�h�^�C�}�[��i�߂�
            if (reloadTimer >= reloadTime)
            {
                ReloadAmmo();
                reloadTimer = 0f; // �����[�h��A�^�C�}�[�����Z�b�g
            }
        }
        else
        {
            reloadTimer = 0f; // �v���C���[�������Ă���ꍇ�A�^�C�}�[�����Z�b�g
        }

        // �e�����邩�m�F���āA���ˉ\���`�F�b�N
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f && currentAmmo > 0)
        {
            ApplyRecoil();
            PlayShotEffect(); // �p�[�e�B�N���Đ�
            currentAmmo--; // �c�e�������炷
            UpdateAmmoDisplay(); // �c�e���\�����X�V
            cooldownTimer = shotCooldown; // �N�[���_�E���^�C�}�[�����Z�b�g
        }

        // �e��0�̏ꍇ�͔��˂ł��Ȃ�
        if (currentAmmo <= 0)
        {
            Debug.Log("�c�e������܂���B�����[�h���Ă��������B");
        }
    }

    private void ReloadAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo++; // �c�e����1�����₷
            UpdateAmmoDisplay(); // �c�e���\�����X�V
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
        playerRb.AddForce(direction.normalized * recoilForce, ForceMode2D.Impulse); // �����͂��̂܂܂̕����ɗ͂�������
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
            isGrounded = true; // �n�ʂɐڐG��
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false; // �n�ʂ��痣�ꂽ
        }
    }
}
