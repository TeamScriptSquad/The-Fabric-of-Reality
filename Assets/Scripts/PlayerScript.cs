using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int speed = 6;
    public int RT = 10;
    public static bool isMiniGameActive = false;
    private Rigidbody rb;
    private Transform cameraTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (PlayerScript.isMiniGameActive)
        {
            // В момент активности мини-игры останавливаем игрок
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * b + right * h).normalized;

        // Получаем текущую скорость
        Vector3 currentVelocity = rb.linearVelocity;

        // Обнуляем вертикальную составляющую, чтобы не мешала движению
        currentVelocity.y = 0;

        if (moveDirection.magnitude > 0)
        {
            // Если есть ввод, задаем новую скорость
            rb.linearVelocity = moveDirection * speed + new Vector3(0, currentVelocity.y, 0);
        }
        else
        {
            // Если клавиши не нажаты, сбрасываем горизонтальную скорость
            rb.linearVelocity = new Vector3(0, currentVelocity.y, 0);
        }
    }
}