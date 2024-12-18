using ZPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : GenericSingleton<CanvasManager>
{
    public GameObject beforeStartMenu, afterLostMenu, afterWinMenu, Coin, Level, Hud, BoardMenu;
    [SerializeField] TMP_Text CoinText, ScoreText, ScoreMultipText, LevelText, ThrowCount;
    public GameObject plusOnePF;
    private void OnEnable()
    {
        CoinText = Coin.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        LevelText = Level.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        GameManager.Instance.LevelCompleted += OnLevelCompleted;
        GameManager.Instance.OnGamePlay += OnGamePlay;
        GameManager.Instance.GameOverEvent += OnGameOver;
        GameManager.Instance.OnGameStart += OnGameStart;
    }
    private void Start()
    {
        Hud.SetActive(false);
    }
    Canvas damageCanvas;
    public void SetLastDamageCanvas(Canvas canv)
    {
        if (damageCanvas)
        {
            damageCanvas.sortingOrder = 0;
        }
        damageCanvas = canv;
        damageCanvas.sortingOrder = 1;
    }

    private void OnGamePlay(object sender, EventArgs e)
    {
        beforeStartMenu.SetActive(false);
        Hud.SetActive(true);
    }

    private void OnGameStart(object sender, EventArgs e)
    {
        beforeStartMenu.SetActive(true);
    }
    public void HudCoin(string value)
    {
        CoinText.text = value;
    }
    public void HudScore(string value)
    {
        ScoreText.text = value;
    }
    public void HudThrowCount(string value)
    {
        ThrowCount.text = value;
    }
    public void HudScoreMult(int value)
    {
        ScoreMultipText.text = value.ToString() + "X";
    }
    public void HudLevel(string value)
    {
        LevelText.text = value;
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        Hud.SetActive(false);
        Invoke(nameof(ShowAfterLostMenu), 1);
    }
    void ShowAfterLostMenu()
    {
        afterLostMenu.SetActive(true);
    }

    private void OnLevelCompleted(object sender, LevelCompletedEventArgs e)
    {
        Hud.SetActive(false);
        ShowWinMenu(e);
    }
    public void ShowPlusOne(Vector3 worldPosition, string text, Color color)
    {
        GameObject obj = Instantiate(plusOnePF, Vector3.zero, Quaternion.identity, transform);
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);

        // Set the screen position as the anchored position of the RectTransform
        RectTransform objRect = obj.GetComponent<RectTransform>();
        objRect.position = screenPosition;
        Debug.Log(worldPosition);
        obj.GetComponent<PlusOne>().Set(text, color);
        Destroy(obj, 2);
    }

    private void ShowWinMenu(LevelCompletedEventArgs e)
    {
        afterWinMenu.SetActive(true);
        Transform nextButton = afterWinMenu.transform.Find("ButtonNext");
        nextButton.gameObject.SetActive(false);
        StartCoroutine(LocalFunc());
        IEnumerator LocalFunc()
        {
            float time = 0;
            float duration = 1;
            // float scoreToCoin = e.score * 0.3f - e.level * 5;
            // int coinFrom = GameManager.Instance.Coin;
            // int coinTo = coinFrom + Mathf.CeilToInt(scoreToCoin);
            while (time < duration)
            {
                // GameManager.Instance.Coin = (int)Mathf.Lerp(coinFrom, coinTo, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            nextButton.gameObject.SetActive(true);
        }
    }
}
