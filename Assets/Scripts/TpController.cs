using UnityEngine;
using UnityEngine.UI;

public class TpController : MonoBehaviour
{
    public float currentTp;
    public int maxTp = 300;
    public float tpRequiredForSkill = 100f;

    [SerializeField] private Image tpBarFill;
    private RectTransform tpTransform;
    private float defaultBarWidth;

    private void OnEnable()
    {
        tpTransform = tpBarFill.GetComponent<RectTransform>();
        defaultBarWidth = tpTransform.sizeDelta.x;
    }

    public void IncreaseTp(float amount)
    {
        currentTp = Mathf.Min(currentTp + Mathf.Abs(amount), maxTp);
    }

    public void DescreaseTp(float amount)
    {
        currentTp = Mathf.Max(currentTp - Mathf.Abs(amount), 0);
    }

    private void Update()
    {
        tpTransform.sizeDelta = new Vector2(currentTp / tpRequiredForSkill * defaultBarWidth, tpTransform.sizeDelta.y);
    }
}
