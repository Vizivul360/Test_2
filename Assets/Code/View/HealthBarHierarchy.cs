using UnityEngine.UI;
using UnityEngine;

public class HealthBarHierarchy : MonoBehaviour, IHealthBar
{
    public HitText delta;
    public Image bar;

    private Vector3 anchor;

    public void OnChange(Health value, float delta)
    {
        gameObject.SetActive(value.Alive);

        bar.fillAmount = value.Current / value.Max;
        showDelta(delta);
    }

    private void showDelta(float value)
    {
        if ((int)value == 0) return;

        var obj = Instantiate(delta);

        obj.transform.SetParent(transform.parent);
        obj.Show(value, anchor);
    }

    public void SetAnchor(Vector3 value)
    {
        anchor = value;
    }

    void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(anchor);
    }
}

public interface IHealthBar
{
    void OnChange(Health value, float delta);

    void SetAnchor(Vector3 value);
}
