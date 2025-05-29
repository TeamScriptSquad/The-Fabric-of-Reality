using UnityEngine;

public class Hole : MonoBehaviour
{
    public Transform holeTransform; // Точка, куда перемещается камера
    public bool isBeingShored = false;

    public void HideHole()
    {
        Destroy(gameObject); // полностью удалить объект
    }
}
