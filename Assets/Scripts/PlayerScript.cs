using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    public float speed = 6f;               // Скорость движения
    public Transform cameraTransform;      // Ссылка на камеру (назначьте в инспекторе)

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            // Получение входных данных
            float h = Input.GetAxisRaw("Horizontal");
            float b = Input.GetAxisRaw("Vertical");

            // Получаем направления на основе ориентации камеры
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Обнуляем вертикальную компоненту
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            // Рассчитываем направление движения
            Vector3 moveDir = (forward * b + right * h).normalized;

            if (h == 0 && b == 0)
            {
                // Если движение не происходит, мгновенно останавливаемся
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            }
            else
            {
                // В противном случае задаем скорость
                Vector3 horizontalVelocity = moveDir * speed;
                rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
            }
        }
    }
    public void StopMovement()
    {
        rb.isKinematic = true;
    }
    public void ResumeMovement()
    {
        rb.isKinematic = false;
    }
}