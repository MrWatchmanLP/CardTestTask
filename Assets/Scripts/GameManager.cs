using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform HandTransform;
    public Transform PanelTransform;
    public Transform PointerTransform;

    public static float CardDistanceFromPivot = 400f;
    public static float PivotOffset = -650f;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
