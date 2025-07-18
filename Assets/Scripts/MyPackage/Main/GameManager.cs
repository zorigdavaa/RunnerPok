﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using ZPackage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
#if ANALYTICS_SDKS
using GameAnalyticsSDK;
#endif

namespace ZPackage
{
    public enum GameState { Starting, Playing, Pause, LevelCompleted, GameOver, Settings, Flying }
    public class GameManager : GenericSingleton<GameManager>, ISave
    {
        // [SerializeField] GroundSpawner groundSpawner;
        // [SerializeField] ProgressBar progressBar;
        #region Events
        public event EventHandler<GameState> StateChanged;
        public event EventHandler OnGameStart;
        public event EventHandler OnGamePlay;
        public event EventHandler OnGamePaused;
        public event EventHandler OnGameResumed;
        public event EventHandler<LevelCompletedEventArgs> LevelCompleted;
        public event EventHandler GameOverEvent;
        public event EventHandler Settings;
        public event EventHandler Flying;
        #endregion
        #region Properties
        private GameState _state;

        public GameState State
        {
            get { return _state; }
            set
            {
                _state = value;
                StateChanged?.Invoke(this, value);
            }
        }

        private bool _isPaused;

        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                if (value)
                {
                    //Time.timeScale = 0;
                    OnGamePaused?.Invoke(this, EventArgs.Empty);
                    State = GameState.Pause;

                }
                else
                {
                    //Time.timeScale = 1;
                    OnGameResumed?.Invoke(this, EventArgs.Empty);
                    PlayGame();
                    //State = GameState.Playing;
                }
            }
        }
        int level = 1;
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                PlayerPrefs.SetInt("level", value);
                Z.CanM.HudLevel(value.ToString());
                // if (progressBar)
                // {
                //     progressBar.UpdateLevel(value);
                // }
            }
        }
        private int coin;
        public int Coin
        {
            get { return coin; }
            set
            {

                coin = value;
                // PlayerPrefs.SetInt("coin", value);
                Z.CanM.HudCoin(value.ToString());
            }
        }
        private int score;
        public int Score
        {
            get { return score; }
            set
            {

                score = value;
                // PlayerPrefs.SetInt("score", value);
                Z.CanM.HudScore(value.ToString());
            }
        }

        private int throwCount;
        public int ThrowCount
        {
            get { return throwCount; }
            set
            {

                throwCount = value;
                // PlayerPrefs.SetInt("throwCount", value);
                Z.CanM.HudThrowCount(value.ToString());
            }
        }
        private int nextLvlScore;
        public int NextLvlScore
        {
            get { return nextLvlScore; }
            set
            {
                nextLvlScore = value;
            }
        }

        #endregion

        #region Methods
        public void Awake()
        {

            // groundSpawner = FindObjectOfType<GroundSpawner>();
            // //canvasManager = FindObjectOfType<CanvasManager>();
            // progressBar = FindObjectOfType<ProgressBar>();

        }

        private void Start()
        {
            StartGame();
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 60;
            CreateGitIgnore();
        }
        public void Save()
        {
            PlayerPrefs.SetInt("coin", Coin);
            PlayerPrefs.SetInt("level", Level);
            PlayerPrefs.GetInt("nextLevelScore", NextLvlScore);
            // Score = PlayerPrefs.GetInt("score",0);
        }

        public void Load()
        {
            // Debug.Log("Load from GameManager");
            Coin = PlayerPrefs.GetInt("coin", 0);
            Level = PlayerPrefs.GetInt("level", 1);
            // Score = 0;
            ThrowCount = 3;
            NextLvlScore = PlayerPrefs.GetInt("nextLevelScore", 20);
            // Score = PlayerPrefs.GetInt("score",0);
        }
        private void CreateGitIgnore()
        {
            if (!File.Exists(".gitignore"))
            {
                print(Directory.GetCurrentDirectory() + ".gitignore File Created");
                File.WriteAllText(".gitignore", File.ReadAllText(Directory.GetCurrentDirectory() + "/Assets/Scripts/Data/gitignore"));
            }
        }

        private void NextLevel()
        {
            print(Level);
            Level++;
            Debug.Log("Level Completed " + Level);
        }
        private void StartGame()
        {
            SaveManager.Load();
            GAStartEvent();
            State = GameState.Starting;
            OnGameStart?.Invoke(this, EventArgs.Empty);
        }
        public void Fly()
        {
            State = GameState.Flying;
            Flying?.Invoke(this, EventArgs.Empty);
        }
        public void PlayGame()
        {
            GAPlayEvent();
            OnGamePlay?.Invoke(this, EventArgs.Empty);
            State = GameState.Playing;
        }

        public void PauseGame(bool value)
        {
            IsPaused = value;
        }
        public void LevelComplete(object sender, float counter)
        {
            if (State != GameState.LevelCompleted)
            {
                LevelCompletedEventArgs eventArgs = new LevelCompletedEventArgs { level = Level, score = Score };
                LevelCompleted?.Invoke(sender, eventArgs);
                State = GameState.LevelCompleted;
                NextLevel();
                GALevelCompleteEvent();
            }
        }
        public void GameOver(object sender, EventArgs e)
        {
            GameOverEvent?.Invoke(sender, e);
            State = GameState.GameOver;
            GAGameOverEvent();
        }
        public void Setting(object sender, EventArgs e)
        {
            Settings?.Invoke(sender, e);
            State = GameState.Settings;
            PauseGame(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void GAStartEvent()
        {
#if ANALYTICS_SDKS
            if (PlayerPrefs.GetInt("LevelZero", 0) == 0)
            {
                PlayerPrefs.SetInt("LevelZero", 1);
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_00000");
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_00000");
            }
#endif
        }
        void GAPlayEvent()
        {
#if ANALYTICS_SDKS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_" + Level.ToString("00000"));
#endif
        }
        void GAGameOverEvent()
        {
#if ANALYTICS_SDKS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level_" + Level.ToString("00000"), Score);
#endif
        }
        void GALevelCompleteEvent()
        {
#if ANALYTICS_SDKS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_" + Level.ToString("00000"), Score);
#endif
        }
        #endregion

    }
}
