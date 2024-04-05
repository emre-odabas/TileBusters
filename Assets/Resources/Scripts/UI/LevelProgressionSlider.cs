using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Controllers;
using TMPro;
using UnityEngine.UI;
using GameCore.Managers;
using DG.Tweening;
public class LevelProgressionSlider : MonoBehaviour
{

    public TextMeshProUGUI m_CurrentLevelText;
    public TextMeshProUGUI m_NextLevelText;
    public Slider m_Slider;
    private void Awake()
    {
        m_Slider = GetComponent<Slider>();
        GameManager.Instance.onStartPlay += OnStartPlay;
    }

    private void OnStartPlay()
    {
        SetCurrentLevel(GameManager.Instance.m_CurrentLevelIndex, false);
    }

    private void Start()
    {
        
    }
    public void Setup(int _maxValue)
    {
        m_Slider.maxValue =_maxValue;
        m_Slider.value = 0;
    }
    public void SetCurrentLevel(int _levelIndex, bool _isBonus)
    {
        if (_isBonus)
        {
            m_CurrentLevelText.text = "B";
        }
        else
        {
            m_CurrentLevelText.text = (_levelIndex + 1).ToString();
        }
        m_NextLevelText.text = (_levelIndex + 2).ToString();
    }

    public void UpdateValue(int _newValue)
    {
        DOVirtual.Float(m_Slider.value, _newValue, 0.2f, (x) =>
        {
            m_Slider.value = x;
        });
    }
}
