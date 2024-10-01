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

    public LineRenderer lineRenderer; // LineRenderer��Inspector�Őݒ�
    public float maxGrappleDistance = 10f; // �O���b�v���̍ő勗��
    public float pullSpeed = 5f; // �����񂹂鑬�x
    private Vector2 grappleTarget;
    private bool isGrappling = false;
    internal string ammo;

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

        // �e�𔭎�
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Shoot((targetPoint - (Vector2)muzzle.position).normalized);
        }

        // �O���b�v���𔭎˂܂��͉���
        if (Input.GetMouseButtonDown(1))
        {
            if (isGrappling)
            {
                ReleaseGrapple();
            }
            else
            {
                FireGrapple(targetPoint);
            }
        }

        // �����X�V
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grappleTarget);
            PullPlayer();
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position); // �O���b�v�����Ă��Ȃ����̓v���C���[�̈ʒu��\��
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

    void FireGrapple(Vector2 targetPoint)
    {
        // �O���b�v���𔭎�
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (targetPoint - (Vector2)transform.position).normalized, maxGrappleDistance);

        if (hit.collider != null)
        {
            // �^�[�Q�b�g�Ɏh�������ꍇ�̏���
            grappleTarget = hit.point;
            lineRenderer.SetPosition(1, grappleTarget);
            isGrappling = true; // �O���b�v�����t���O��ݒ�
        }
        else
        {
            lineRenderer.SetPosition(1, targetPoint);
            isGrappling = false; // �O���b�v�����t���O�����Z�b�g
        }
    }

    void PullPlayer()
    {
        Vector2 playerPosition = transform.position;

        // �v���C���[���^�[�Q�b�g�ʒu�Ɍ������Ĉ����񂹂�
        if (Vector2.Distance(playerPosition, grappleTarget) > 0.1f)
        {
            Vector2 newPosition = Vector2.MoveTowards(playerPosition, grappleTarget, pullSpeed * Time.deltaTime);
            transform.position = newPosition;
        }
        else
        {
            ReleaseGrapple(); // ���B������O���b�v�����I��
        }
    }

    void ReleaseGrapple()
    {
        isGrappling = false; // �O���b�v��������
        lineRenderer.SetPosition(1, transform.position); // ���̏I�_���v���C���[�̈ʒu�ɖ߂�
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
