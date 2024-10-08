using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [SerializeField] private float grappleSpeed = 10f;
    [SerializeField] private float maxGrappleDistance = 15f;
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private Rigidbody playerRb;
    private bool pullingPlayer = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 右クリックで発射
        {
            StartGrapple();
        }

        if (Input.GetKey(KeyCode.Space) && pullingPlayer) // Spaceキーで引き寄せ
        {
            PullPlayer();
        }

        if (Input.GetMouseButtonDown(1) && isGrappling) // 再度右クリックで回収
        {
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxGrappleDistance))
        {
            grapplePoint = hit.point;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
            isGrappling = true;
            pullingPlayer = true;
        }
    }

    void PullPlayer()
    {
        Vector3 direction = (grapplePoint - transform.position).normalized;
        playerRb.MovePosition(Vector3.Lerp(transform.position, grapplePoint, grappleSpeed * Time.deltaTime));

        // 到達チェック
        if (Vector3.Distance(transform.position, grapplePoint) < 0.5f)
        {
            StopGrapple();
        }
    }

    void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        isGrappling = false;
        pullingPlayer = false;
    }
}
