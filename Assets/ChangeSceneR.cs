using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public void StartTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}