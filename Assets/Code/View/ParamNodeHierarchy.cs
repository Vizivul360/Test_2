using UnityEngine.UI;
using UnityEngine;

public class ParamNodeHierarchy : MonoBehaviour, IBuffView, IStatView
{
    public Image icon;
    public Text label;

    public void SetData(Stat stat)
    {
        setIcon(stat.icon);
    }

    public void UpdateValue(float newValue)
    {
        label.text = newValue.ToString("0.##");
    }

    public void SetData(Buff buff)
    {
        label.text = buff.title;
        setIcon(buff.icon);
    }

    private void setIcon(string name)
    {
        icon.sprite = Resources.Load<Sprite>("Icons/" + name);
    }
}

public interface IStatView
{
    void SetData(Stat stat);

    void UpdateValue(float newValue);
}

public interface IBuffView
{
    void SetData(Buff buff);
}