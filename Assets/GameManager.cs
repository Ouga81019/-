using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool canMove = false;

    void Awake()
    {
        Instance = this;
    }
}