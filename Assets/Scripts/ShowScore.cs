using TMPro;
using UnityEngine;

public class ShowScore : MonoBehaviour
{
    private void Start()
    {
        int score = GameStatusManager.Instance.score;
        int bestScore = GameStatusManager.Instance.bestScore;
        GetComponent<TextMeshProUGUI>().text = "Score: " + score + "\nBest Score: " + bestScore;
    }
}
