using UnityEngine;
using UnityEngine.EventSystems;

public class TileDragHandler : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    private Vector2 startPosition;
    private TileHolder tileHolder;
    private RectTransform holderRect;
    private RectTransform rectTransform;
    private TileView tileView;
    private bool dragged = false;
    
    public void SetTileHolder(TileHolder tileHolder)
    {
        this.tileHolder = tileHolder;
        holderRect = tileHolder.GetComponent<RectTransform>();
        tileView = gameObject.GetComponent<TileView>();
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragged)
        {
            tileView.StopAllAnimations();
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragged)
        {
            tileView.StopAllAnimations();
            if (RectTransformUtility.RectangleContainsScreenPoint(holderRect, Input.mousePosition))
            {
                tileHolder.OnDrop(tileView, rectTransform);
            }
            else
            {
                tileView.UpdatePosition(startPosition);
            }
            dragged = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (tileHolder.isDistributed())
        {
            tileView.StopAllAnimations();
            startPosition = transform.localPosition;
            dragged = true;
        }
    }
}
