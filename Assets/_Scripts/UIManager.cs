using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text pointsText;
    private static int m_points;

    public static void AddPoints(int pointsToAdd)
    {
        m_points += pointsToAdd;
    }

    void Update()
    {
        pointsText.text = m_points.ToString();
    }
}
