using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject characterPrefab;
    public GameObject playerPrefab;
    public GameObject paddleLight;
    public GameObject ballLight;
    public Text scoreText;
    public Text ballText;
    public Text levelText;
    public Text highScoreText;
    public Text inputText;
    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;
    public GameObject panelOption;
    public GameObject panelOptionMenu;
    public GameObject panelMainMenu;
    public GameObject panelHighscore;
    public GameObject buttonResume;

    public GameObject[] levels;

    public static GameManager Instance
    {
        get;
        private set;
    }

    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
    private State _state;
    private GameObject _currentBall;
    private GameObject _currentLevel;
    private bool _isSwitchingState;
    private GameObject _currentCharacter;
    private GameObject _currentPlayer;
    private Character scriptCharacter;
    [SerializeField] private AudioClip _ballLosingSoundEffect;
    [SerializeField] private AudioClip _gameLosingSoundEffect;
    [SerializeField] private AudioClip _gameWinningSoundEffect;
    [SerializeField] private AudioClip _themeSong;

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            scoreText.text = "SCORE: " + _score;
        }
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

    private int _balls;

    public int Balls
    {
        get { return _balls; }
        set
        {
            _balls = value;
            ballText.text = "Balls: " + _balls;
        }
    }

    private bool _playerInput = true;
    public bool PlayerInput
    {
        get { return _playerInput; }
        set { _playerInput = value; }
    }

    private bool _characterAlive = true;
    public bool CharacterAlive
    {
        get { return _characterAlive; }
        set { _characterAlive = value; }
    }

    public static bool GameIsPaused = false;

    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }

    public void ChangePlayerInput()
    {
        if (inputText.text == "KEYBOARD")
        {
            inputText.text = "MOUSE";
            _playerInput = true;
        }
        else
        {
            inputText.text = "KEYBOARD";
            _playerInput = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }

    void setPauseOption()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        panelMenu.SetActive(false);
        panelOption.SetActive(false);
        panelOptionMenu.SetActive(false);
        panelMainMenu.SetActive(true);
        panelHighscore.SetActive(true);
        buttonResume.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    void Pause()
    {
        Cursor.visible = true;
        panelMenu.SetActive(true);
        panelOption.SetActive(true);
        panelOptionMenu.SetActive(true);
        panelMainMenu.SetActive(false);
        panelHighscore.SetActive(false);
        buttonResume.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void SwitchState(State newState, float delay = 1f)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                highScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
                panelMenu.SetActive(true);
                SoundManager.Instance.PlayMusic(_themeSong, true);
                break;
            case State.INIT:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Balls = 3;
                CharacterAlive = true;
                if (_currentLevel != null)
                {
                    Destroy(_currentLevel);
                }
                _currentPlayer = Instantiate(playerPrefab);

                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(_currentBall);
                Destroy(_currentLevel);
                Destroy(_currentCharacter);
                Level++;
                SwitchState(State.LOADLEVEL, 2f);
                panelLevelCompleted.SetActive(true);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    _currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if (Score > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                Destroy(_currentBall);
                Destroy(_currentLevel);
                Destroy(_currentCharacter);
                Destroy(_currentPlayer);
                panelGameOver.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                setPauseOption();
                if (_currentBall == null)
                {
                    if (Balls > 0)
                    {
                        SoundManager.Instance.PlaySoundOnce(_ballLosingSoundEffect);
                        _currentBall = Instantiate(ballPrefab);
                        Instantiate(paddleLight);
                        Instantiate(ballLight);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySoundOnce(_gameLosingSoundEffect);
                        SwitchState(State.GAMEOVER);
                    }
                }

                if (_currentCharacter == null)
                {
                    if (_characterAlive)
                    {
                        _currentCharacter = Instantiate(characterPrefab);
                        scriptCharacter = _currentCharacter.GetComponent<Character>();
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }

                if (_currentLevel != null && scriptCharacter.touchingWithPaddle && !_isSwitchingState)
                {
                    SoundManager.Instance.PlaySoundOnce(_gameWinningSoundEffect);
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }
}