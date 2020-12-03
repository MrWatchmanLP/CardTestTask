using UnityEngine;

public class PointerFollower : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
