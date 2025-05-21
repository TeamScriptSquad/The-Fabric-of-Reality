using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;        // ������ ������
    public float mouseSensitivity = 100f; // ���������������� ����
    public float maxPitch = 80f;    // ������������ ���� ������� �� ���������
    public float minPitch = -80f;   // ����������� ���� ������� �� ���������

    private float yaw = 0f;
    private float pitch = 0f;


    // ������ ��������� ������� ������ � ��������� ���� (��������, 30 �������� ����)
    public Vector3 fixedRotation = new Vector3(90f, 0f, 0f);
    public Vector3 fixedPosition; // ����� ������ � ���������� ��� � Start
    void Start()
    {
        // ������������� ������ � ����� � ������ ��� ��������� (�� �������)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update()
    {
        if (PlayerScript.isMiniGameActive)
        {
            // ��������� �������� � ��������� ������
            transform.localRotation = Quaternion.Euler(fixedRotation);
            // ����� ��� ������������� �������, ���� �����
            return;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // ������� ������
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}