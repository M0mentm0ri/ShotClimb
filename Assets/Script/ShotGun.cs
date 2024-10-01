using UnityEngine;

public class ShotGun : MonoBehaviour
{
    // �v���C���[��Rigidbody2D
    private Rigidbody2D playerRb;

    // ����i�v���C���[�̎q�I�u�W�F�N�g�j�ւ̎Q��
    public Transform weaponTransform;

    // �����̗�
    public float recoilForce = 5f;

    // ���˂̃N�[���_�E���^�C��
    public float shotCooldown = 0.5f;

    // �����N�[���_�E���^�C�}�[
    private float cooldownTimer = 0f;

    // �p�[�e�B�N���V�X�e���i�V���b�g�K���̔��˃G�t�F�N�g�j
    public ParticleSystem shotEffect;

    // �c�e���̕ϐ�
    public int maxAmmo = 5; // �ő�e��
    private int currentAmmo; // ���݂̒e��

    // �e��\���p�̃I�u�W�F�N�g
    public GameObject[] ammoDisplayObjects; // �e���\�����邽�߂̃I�u�W�F�N�g

    void Start()
    {
        // Rigidbody2D �R���|�[�l���g���擾
        playerRb = GetComponent<Rigidbody2D>();

        // �c�e����������
        currentAmmo = maxAmmo;

        // �c�e���̕\����������
        UpdateAmmoDisplay();
    }

    void Update()
    {
        // ��ɕ���̕������}�E�X�Ɍ�����
        AimWeapon();

        // �N�[���_�E���̃^�C�}�[���X�V
        cooldownTimer -= Time.deltaTime;

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

    // �}�E�X�̕����ɕ����������
    void AimWeapon()
    {
        // �}�E�X�ʒu���擾���A�v���C���[����̕������v�Z
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // �q�I�u�W�F�N�g�i����j�̕�����ݒ�i��Ƀ}�E�X�������j
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // ���ˌ�ɔ�����������
    void ApplyRecoil()
    {
        // �}�E�X�ʒu���擾���A�v���C���[����̕������v�Z
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // �v���C���[�ɔ�����������i���˕����Ƃ͋t�j
        playerRb.AddForce(direction.normalized * recoilForce, ForceMode2D.Impulse);

        // ���ˎ��̃��W�b�N�i�Ⴆ�΁A�e�ې����Ȃǁj�͂����ɒǉ��\
    }

    // �p�[�e�B�N���G�t�F�N�g���Đ�����
    void PlayShotEffect()
    {
        // �p�[�e�B�N���V�X�e�����ݒ肳��Ă���ꍇ�̂ݍĐ�
        if (shotEffect != null)
        {
            shotEffect.Play();
        }
    }

    // �c�e���̕\�����X�V����
    void UpdateAmmoDisplay()
    {
        // ammoDisplayObjects �̒����� maxAmmo ����v���Ă��邱�Ƃ��O��
        for (int i = 0; i < ammoDisplayObjects.Length; i++)
        {
            // �c�e�����܂�����ꍇ�͕\�����A����ȊO�͔�\���ɂ���
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

    // �e��������[�h����i����̃I�u�W�F�N�g�ɐG�ꂽ���ɌĂ΂��j
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            // �c�e�����ő�Ƀ����[�h
            currentAmmo = maxAmmo;

            // �e��\�����X�V
            UpdateAmmoDisplay();
        }
    }
}
