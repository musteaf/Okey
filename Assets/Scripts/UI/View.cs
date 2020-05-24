using UnityEngine;

public abstract class View : MonoBehaviour
{
    protected RectTransform rectTransform;

    public void Init()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public abstract void UpdateRect(float width, float height);
    public abstract void Hide();
    public abstract void Show();
}
