using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public float cameraMoveSpeed = 2f;

    public Canvas miniGameCanvas;
    public Text keysToPressText;

    private Hole currentHole;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private List<KeyCode> requiredKeys;
    private List<KeyCode> pressedKeys;

    private bool isMiniGameActive = false;

    private Vector3 playerPosition;
    private Quaternion playerRotation;
    public Transform playerTransform;
    void Start()
    {
        miniGameCanvas.gameObject.SetActive(false);
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;
    }

    public void StartShoring(Hole hole)
    {
        if (isMiniGameActive) return;

        // сохран€ем позицию и поворот игрока
        playerPosition = playerTransform.position;
        playerRotation = playerTransform.rotation;

        currentHole = hole;
        StartCoroutine(MoveCameraToHole(hole.holeTransform.position));
    }

    private IEnumerator MoveCameraToHole(Vector3 targetPosition)
    {
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        Vector3 targetPos = targetPosition + new Vector3(0, 2, -2);
        Quaternion targetRot = Quaternion.LookRotation(targetPosition - targetPos);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        StartCoroutine(StartMiniGame());
    }

    private IEnumerator StartMiniGame()
    {
        PlayerScript.isMiniGameActive = true;
        isMiniGameActive = true;
        miniGameCanvas.gameObject.SetActive(true);

        List<KeyCode> allKeys = new List<KeyCode> {
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
            KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
            KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
            KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
            KeyCode.Y, KeyCode.Z,
            KeyCode.Space
        };

        requiredKeys = new List<KeyCode>();
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, allKeys.Count);
            requiredKeys.Add(allKeys[index]);
            allKeys.RemoveAt(index);
        }

        

        // ‘ормируем строку дл€ UI
        string keysString = string.Join(", ", requiredKeys.Select(k => k.ToString()));


        // Ќазначаем текст
        keysToPressText.text = "Press these keys in any order:\n" + keysString;

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
        TeleportCameraToPlayer();
    }

    private void EndMiniGame()
    {
        PlayerScript.isMiniGameActive = false;
    }

    private void TeleportCameraToPlayer()
    {
        mainCamera.transform.position = playerPosition;
        mainCamera.transform.rotation = playerRotation;

        // отключите мини-игру или выполните любые дополнительные действи€
        miniGameCanvas.gameObject.SetActive(false);
        isMiniGameActive = false;
        currentHole.isBeingShored = false;
    }
}