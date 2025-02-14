using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreUI : MonoBehaviour
{
    private int score;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Events.Instance.AddListener(GameEvents.CubeStacked, HandleCubeStacked);
        Events.Instance.AddListener(GameEvents.GameStarted, HandleGameStarted);
    }

    private void HandleGameStarted(IGameEvent eventParam)
    {
        score = 0;
        text.text = score.ToString();
    }

    private void HandleCubeStacked(IGameEvent eventParam)
    {
        score++;
        text.text = score.ToString();
    }

    private void OnDestroy()
    {
        Events.Instance.RemoveListener(GameEvents.CubeStacked, HandleCubeStacked);
        Events.Instance.RemoveListener(GameEvents.GameStarted, HandleGameStarted);
    }
}
