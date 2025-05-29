using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Transform cameraFixedPosition; // ������� ������ � ����-����
    public Transform cameraFixedRotation; // ���� ������ (����� ������������ Quaternion)
    public GameObject player; // ������ ������
    public PlayerScript playerController; // ���������, ���������� �� ���������� ������� (��������, ������ ����������)
    public Camera mainCamera;
    public float cameraMoveSpeed = 2f;
    public CameraController cameraController;
    public Canvas miniGameCanvas;
    public Text interactionHintText; 

    public Text key1Text;
    public Text key2Text;
    public Text key3Text;
    public Text key4Text;

    private Hole currentHole;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;

    private List<KeyCode> requiredKeys;
    private List<KeyCode> pressedKeys;

    private bool isMiniGameActive = false;
    private bool isLookingAtHole = false; // ������� �� ����� �� ����

    void Start()
    {
        miniGameCanvas.gameObject.SetActive(false);
        interactionHintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isMiniGameActive)
        {
            CheckPlayerLookingAtHole();

            if (isLookingAtHole && Input.GetKeyDown(KeyCode.E))
            {
                MoveCameraToHole(currentHole.holeTransform.position);
            }
        }
    }

    private void CheckPlayerLookingAtHole()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        // ������� ����, �� ������� ��������� ����, ����� ����������� ���
        int layerMask = LayerMask.GetMask("HoleLayer"); // �������� ���� "HoleLayer" � ��������� ������

        if (Physics.Raycast(ray, out hit, 10f, layerMask))
        {
            // ���������, ��� ������ �� �����
            if (hit.transform.CompareTag("Hole")) // ��� �� ����
            {
                isLookingAtHole = true;
                currentHole = hit.transform.GetComponent<Hole>();
                interactionHintText.gameObject.SetActive(true);
                interactionHintText.text = "E";
                return;
            }
        }

        // ���� �� ������ � ������� ���������
        isLookingAtHole = false;
        interactionHintText.gameObject.SetActive(false);
    }

    private void MoveCameraToHole(Vector3 holePosition)
    {
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;
        // Запоминаем текущие позиции, чтобы их можно было восстановить
        Vector3 startCamPos = mainCamera.transform.position;
        Quaternion startCamRot = mainCamera.transform.rotation;

        mainCamera.transform.position = cameraFixedPosition.position;
        mainCamera.transform.rotation = cameraFixedRotation.rotation;

        if (playerController != null)
            playerController.enabled = false;

        var cameraController = mainCamera.GetComponent<CameraController>();
        if (cameraController != null)
            cameraController.enabled = false;

        StartCoroutine(StartMiniGame());
    }
    
    private IEnumerator StartMiniGame()
    {
        playerController.StopMovement();
        isMiniGameActive = true;
        miniGameCanvas.gameObject.SetActive(true);

        interactionHintText.gameObject.SetActive(false);
        // ��������� ������
        List<KeyCode> allKeys = new List<KeyCode> {
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
            KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
            KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
            KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
            KeyCode.Y, KeyCode.Z,
        };

        requiredKeys = new List<KeyCode>();
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, allKeys.Count);
            requiredKeys.Add(allKeys[index]);
            allKeys.RemoveAt(index);
        }

        // Назначение клавиш в отдельные Text поля
        key1Text.text = requiredKeys[0].ToString();
        key2Text.text = requiredKeys[1].ToString();
        key3Text.text = requiredKeys[2].ToString();
        key4Text.text = requiredKeys[3].ToString();


        pressedKeys = new List<KeyCode>();

        while (pressedKeys.Count < requiredKeys.Count)
        {
            foreach (KeyCode key in requiredKeys)
            {
                if (Input.GetKeyDown(key) && !pressedKeys.Contains(key))
                {
                    pressedKeys.Add(key);
                }
            }
            yield return null;
        }

        EndMiniGame();
        StartCoroutine(MoveCameraBack());
    }

    private void EndMiniGame()
    {
        if (currentHole != null)
        {
            Destroy(currentHole.gameObject); // полностью удаляем дырку
        }
        playerController.ResumeMovement();
        // Восстановить управление камерой и игроком
        if (playerController != null)
            playerController.enabled = true;
        if (cameraController != null)
        {
            cameraController.enabled = true; // возвращаем управление
        }
    }

    private IEnumerator MoveCameraBack()
    {
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;
        
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, originalCameraPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, originalCameraRotation, t);
            yield return null;
        }

        Destroy(gameObject);
        isMiniGameActive = false;
        currentHole.isBeingShored = false;
    }
}