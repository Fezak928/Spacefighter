using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverMenuManager : MonoBehaviour
{
    [SerializeField] private int _currentHighscore = 0;
    [SerializeField] private GameObject _highscoreMenu;
    [SerializeField] private TextMeshProUGUI _highscoreDisplay, _yourScore, _scoreDisplay;

    [SerializeField] private GameObject _gameOverFirstSelected;

    private void OnEnable()
    {
        _currentHighscore = PlayerPrefs.GetInt("Highscore");

        if (GameManager.Instance == null)
            return;

        EventSystem.current.SetSelectedGameObject(_gameOverFirstSelected);

        if (GameManager.Instance.CurrentScore > _currentHighscore)
        {
            PlayerPrefs.SetInt("Highscore", GameManager.Instance.CurrentScore);
            _highscoreMenu.SetActive(false);
            _yourScore.text = "New Higscore!";
        }
        else
        {
            _highscoreMenu.SetActive(true);
            _highscoreDisplay.text = _currentHighscore.ToString();
            _yourScore.text = "Your Score";
        }

        _scoreDisplay.text = GameManager.Instance.CurrentScore.ToString();

        PlayerPrefs.Save();
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
