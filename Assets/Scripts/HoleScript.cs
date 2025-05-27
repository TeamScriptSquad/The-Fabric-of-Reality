using UnityEngine;

public class Hole : MonoBehaviour
{
    public Transform holeTransform; // Точка, куда перемещается камера
    public bool isBeingShored = false;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
}
