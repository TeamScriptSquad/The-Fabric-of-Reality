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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBeingShored)
        {
            isBeingShored = true;
            gameManager.StartShoring(this);
        }
    }
}