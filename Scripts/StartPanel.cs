using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject maxDistanceRow;

    [SerializeField]
    private Text maxDistance;

    private void Start()
    {
        maxDistanceRow.SetActive(PlayerPrefs.HasKey(Constants.MaxDistance));
        maxDistance.text = PlayerPrefs.GetFloat(Constants.MaxDistance, 0).ToString("F");
    }

    public void StartGame()
    {
        TouchManager.Instance.GameStarted = true;
        
        gameObject.SetActive(false);
    }

}
