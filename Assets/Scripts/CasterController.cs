using UnityEngine;
using UnityEngine.UI;

public class CasterController : MonoBehaviour
{
    [SerializeField] private Image casterBarFill;
    private RectTransform casterTransform;
    private float defaultBarWidth;

    private CasterTargeter targeter;


    private void OnEnable()
    {
        targeter = GetComponent<CasterTargeter>();
        casterTransform = casterBarFill.GetComponent<RectTransform>();
        defaultBarWidth = casterTransform.sizeDelta.x;
    }

    private void Update()
    {
        if (targeter.isCasting)
        {
            targeter.currentCastTime += Time.deltaTime;
            casterTransform.sizeDelta = new Vector2(Mathf.Min(targeter.currentCastTime / targeter.castTime, 1) * defaultBarWidth, casterTransform.sizeDelta.y);
        }
        else
        {
            casterTransform.sizeDelta = new Vector2(0, casterTransform.sizeDelta.y);
        }
    }
}
