using UnityEngine;
using TMPro;

public class Timer: MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 3f;

    float timer;
    bool isCounting = true;

    void Start()
    {
        timer = countdownTime;

        // Å‰‚Í‘€ì‹Ö~
        GameManager.Instance.canMove = false;
    }

    void Update()
    {
        if (!isCounting) return;

        timer -= Time.deltaTime;

        int displayTime = Mathf.CeilToInt(timer);
        countdownText.text = displayTime.ToString();

        if (timer <= 0)
        {
            countdownText.text = "GO!";
            isCounting = false;

            // ‘€ì‰ğ‹Ö
            GameManager.Instance.canMove = true;

            Invoke("HideText", 1f);
        }
    }

    void HideText()
    {
        countdownText.gameObject.SetActive(false);
    }
}