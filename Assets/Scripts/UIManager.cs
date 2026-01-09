using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void OnEnable()
    {
        instance = this;
        EventHub.OnHPChange += ChangeHPBarUI;
        EventHub.OnLose += RaiseLoseUI;
    }
    private void OnDisable()
    {
        instance = null;
        EventHub.OnHPChange -= ChangeHPBarUI;
        EventHub.OnLose -= RaiseLoseUI;
    }

    [SerializeField]Slider hpBar;
    [SerializeField] GameObject loseUI;

    private void Start()
    {
        
    }
    private void ChangeHPBarUI()
    {
        if (Crystal.instance != null)
        {
            hpBar.value = Crystal.instance.CurrentHP;
        }
    }
    private void RaiseLoseUI()
    {
        loseUI.SetActive(true);
        Time.timeScale = 0;
    }
}
