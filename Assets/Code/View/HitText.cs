using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class HitText : MonoBehaviour
{
    public Text label;

    public float duration, flyHeight;
    private Vector3 point;

    public void Show(float value, Vector3 flyFrom)
    {
        gameObject.SetActive(true);
        point = flyFrom;
                
        label.text = value.ToString("+#;-#");
        label.color = value > 0 ? Color.green : Color.red;

        StartCoroutine(animCoro());
    }

    IEnumerator animCoro()
    {
        var iterations = duration / Time.deltaTime;

        var dp = Vector3.up * flyHeight / iterations;
        var da = 1.0f / iterations;

        for(var i = 0; i < iterations; i++)
        {
            point += dp;

            var color = label.color;
            color.a -= da;

            label.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(point);
    }
}
