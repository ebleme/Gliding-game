using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField]
    private Text distance;

    public void FinishGame(float distanceFromStartPoint)
    {
        distance.text = distanceFromStartPoint.ToString("F");

        float maxDistance = PlayerPrefs.GetFloat(Constants.MaxDistance, 0);

        if (distanceFromStartPoint > maxDistance)
            PlayerPrefs.SetFloat(Constants.MaxDistance, distanceFromStartPoint);

        gameObject.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
