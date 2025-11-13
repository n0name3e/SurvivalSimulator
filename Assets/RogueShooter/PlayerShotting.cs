using UnityEngine;

public class PlayerShotting : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GameObject laser;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // 4. Rotate the player
            Vector3 lookDirection = (hit.point - transform.position).normalized;
            //lookDirection.y = 0f; // Keep the player upright!
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            Vector3 euler = targetRotation.eulerAngles;
            euler.x = 0;
            Quaternion finalRotation = Quaternion.Euler(euler);
            GameObject laserObject = Instantiate(laser, transform.position, finalRotation);
            laserObject.transform.Translate(0, 5, 0);

            TimeLapse laserTL = laserObject.GetComponent<TimeLapse>();
            laserTL.recordTransform = false;
            laserTL.recordAnimator = false;
            laserTL.destroyOnLapseEnd = true;

            animator.CrossFade("Swing", 0.2f, 0, 0);
        }
    }
}
