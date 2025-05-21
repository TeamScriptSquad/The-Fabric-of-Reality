using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;        // Объект игрока
    public float mouseSensitivity = 100f; // Чувствительность мыши
    public float maxPitch = 80f;    // Максимальный угол наклона по вертикали
    public float minPitch = -80f;   // Минимальный угол наклона по вертикали

    private float yaw = 0f;
    private float pitch = 0f;


    // Жестко фиксируем позицию камеры в положении вниз (например, 30 градусов вниз)
    public Vector3 fixedRotation = new Vector3(90f, 0f, 0f);
    public Vector3 fixedPosition; // Можно задать в инспекторе или в Start
    void Start()
    {
        // Устанавливаем курсор в центр и делаем его невидимым (по желанию)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update()
    {
        if (PlayerScript.isMiniGameActive)
        {
            // Отключаем вращение и фиксируем камеру
            transform.localRotation = Quaternion.Euler(fixedRotation);
            // Можно ещё зафиксировать позицию, если нужно
            return;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Вращаем камеру
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}