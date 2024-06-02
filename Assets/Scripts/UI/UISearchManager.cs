using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISearchManager : MonoBehaviour
{
    public Slider searchSlider;
    public Image[] img;
    float _timer;
    float _timerMax;
    public TMP_Text timeText;
    public GameManager gameManager;

    public void Init()
    {
        searchSlider.value = 0;
        for (int i = 0; i < img.Length; i++)
        {
            img[i].color = Color.white;
        }
    }
    public void StartTimer(float time)
    {
        _timer = time;
        InfoManager.Instance.GameInfo.searchLeftTime = time;
        _timerMax = InfoManager.Instance.GameInfo.GetSearchTime();
        ChangeToTime(time);
        StartCoroutine(Search());
    }

    public void ChangeToTime(float time)
    {
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        timeText.text = "남은 시간 : " + minutes + ":" + seconds;
    }
    public IEnumerator Search()
    {
        int count = 0;
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            InfoManager.Instance.GameInfo.searchLeftTime = _timer;
            searchSlider.value = (_timerMax - _timer) / _timerMax;
            ChangeToTime(_timer);
            if (searchSlider.value >= 0.25*(count+1)&& searchSlider.value<1)
            {
                img[count++].color = new Color(0, 0, 0, 0);
            }

            yield return null;
        }
        _timer = 0;
        ChangeToTime(_timer);
        InfoManager.Instance.GameInfo.searchLeftTime = 0;
        gameManager.GetSeveralItem(count);
        gameManager.mainController.ChangeState(CharacterController.State.None);
    }
}
