using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce = 10f;
    public int maxAmmo = 2;
    private int currentAmmo;
    public Transform muzzle;  // ���ˌ�
    public GameObject bulletPrefab;  // �e�̃v���n�u
    public float bulletSpeed = 20f;  // �e�̑��x
    public Camera mainCamera;  // �}�E�X�̈ʒu���擾���邽�߂̃J����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;  // ���C���J�������擾
        currentAmmo = maxAmmo;  // �����e����ݒ�
    }

    void Update()
    {
        // �}�E�X�̈ʒu���擾
        Vector3 mousePos = Input.mousePosition;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        Vector2 targetPoint = new Vector2(mousePos.x, mousePos.y);

        // ���N���b�N�Œe�𔭎�
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Shoot((targetPoint - (Vector2)muzzle.position).normalized);
        }
    }

    void Shoot(Vector2 direction)
    {
        currentAmmo--;

        // �e�𐶐����Ĕ���
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;

        // �v���C���[�ɔ�����ǉ��i��Ԃ��߂̗́j
        rb.AddForce(-direction * jumpForce, ForceMode2D.Impulse);

        // �_���[�W����
        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, direction);
        if (hit.collider != null && hit.collider.CompareTag("Destructible"))
        {
            hit.collider.GetComponent<DestructibleObject>().TakeDamage(1);  // ��Q���ւ̃_���[�W
        }
    }

    public void Reload(int ammoPickedUp)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammoPickedUp, maxAmmo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AmmoPickup"))
        {
            Reload(2);
            Destroy(collision.gameObject);
        }
    }
}
