using System;
using UnityEngine.UI;
using UnityEngine;

public class MainPanelHierarchy : MonoBehaviour, IMainView
{
    public Button buttonWithBuffs, buttonNoBuffs;

    public void setRestart(Action<bool> action)
    {
        buttonWithBuffs.onClick.AddListener(() => action(true));
        buttonNoBuffs.onClick.AddListener(() => action(false));
    }
}

public interface IMainView
{
    void setRestart(Action<bool> action);
}
