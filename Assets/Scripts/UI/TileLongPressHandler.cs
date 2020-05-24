using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileLongPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private TileHolder tileHolder;
    private TileView tileView;
    private bool pointed;
    public void SetTileView(TileView tileView, TileHolder tileHolder)
    {
        this.tileHolder = tileHolder;
        this.tileView = tileView;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (tileHolder.isDistributed())
        {
            tileView.Holded();
            pointed = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointed)
        {
            tileView.StopHolded();
        }
    }
}
