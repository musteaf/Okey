using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class TileHolder : MonoBehaviour
{
    private float width;
    private float height;
    private float ratio;
    [SerializeField] private GameObject tilePref;
    [SerializeField] private GameObject remainsPref;
    [SerializeField] private GameObject indicatorPref;
    private Transform transform;
    private Vector2[] tilePositions = new Vector2[28];
    private TileView[] tileSlots = new TileView[28];
    Dictionary<int, TileTuple> tileTuples = new Dictionary<int, TileTuple>();
    private List<TileView> tileViews = new List<TileView>();
    // use as pool -> do not delete
    private IndicatorView indicatorView;
    private RemainsView remainsView;
    private int distributedTiles = 14;
    //maybe we can separate view calculations and view inits -> boardviewcontroller and tileholder

    public bool isDistributed()
    {
        return distributedTiles == 14;
    }

    void Awake()
    {
        transform = gameObject.transform;
        RectTransform rowRectTransform = gameObject.GetComponent<RectTransform>();
        width = rowRectTransform.rect.width;
        height = rowRectTransform.rect.height;

        float tileWidth = width / 14;
        float tileHeight = height / 2;
        float paddingWidth = tileWidth / 20;
        float paddingHeight = tileHeight / 6;

        float offSetX = tileWidth / 2 - tileWidth * 7;
        float offSetY = tileHeight / 2;
        
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 14; i++)
            {
                tilePositions[j*14 + i] = new Vector2(offSetX + i * tileWidth, offSetY - j * tileHeight);
            }
        }

        for (int i = 0; i < 14; i++)
        {
            GameObject tileGameObject = Instantiate(tilePref) as GameObject;
            TileView tileView = tileGameObject.GetComponent<TileView>();
            tileView.Init();
            tileGameObject.GetComponent<TileDragHandler>().SetTileHolder(this);
            tileGameObject.GetComponent<TileLongPressHandler>().SetTileView(tileView, this);
            tileGameObject.transform.SetParent(transform);
            tileGameObject.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
            tileView.UpdateRect(tileWidth - paddingWidth, tileHeight - paddingHeight);
            tileView.UpdatePosition(tilePositions[i], i);
            tileSlots[i] = tileView;
            tileViews.Add( tileView);
        }
        GameObject indicatorGameObject = Instantiate(indicatorPref) as GameObject;
        indicatorView = indicatorGameObject.GetComponent<IndicatorView>();
        indicatorView.transform.SetParent(transform);
        indicatorView.Init();
        indicatorView.UpdateRect(tileWidth - paddingWidth, tileHeight - paddingHeight);
        indicatorView.SetPosition(new Vector2(0-tileWidth-2*paddingWidth, Screen.height / 2));
        
        GameObject remainsGameObject = Instantiate(remainsPref) as GameObject;
        remainsView = remainsGameObject.GetComponent<RemainsView>();
        remainsView.transform.SetParent(transform);
        remainsView.Init();
        remainsView.UpdateRect(tileWidth - paddingWidth, tileHeight - paddingHeight);
        remainsView.SetPosition(new Vector2(0, Screen.height / 2));
    }

    public void UpdateBoard(List<Tile> tiles, Tile indicator)
    {
        distributedTiles = 0;
        tileTuples = new Dictionary<int, TileTuple>();
        for (int i = 0; i < tileSlots.Length; i++)
        {
            tileSlots[i] = null;
        }
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile currentTile = tiles[i];
            TileView currentTileView = tileViews[i];
            tileViews[i].UpdateTile(currentTile);
            tileTuples[currentTile.Id] = new TileTuple(currentTileView, currentTile);
            tileSlots[i] = currentTileView;
            currentTileView.Hide();
        }
        indicatorView.UpdateTile(indicator);
        for (int i = 0; i < tiles.Count; i++)
        {
            TileView currentTileView = tileViews[i];
            StartCoroutine(DistributionAnimation(0+i*0.10f, currentTileView, i));
        }
        
    }

    public void Sorted(List<TileGroup> tileGroups)
    {
        for (int i = 0; i < tileSlots.Length; i++)
        {
            tileSlots[i] = null;
        }

        int slotPos = 0;
        foreach (TileGroup tileGroup in tileGroups)
        {
            if (slotPos<14 && slotPos + tileGroup.Tiles.Count > 14)
            {
                slotPos = 14;
            }
            foreach (Tile tile in tileGroup.Tiles)
            {
                tileSlots[slotPos] = tileTuples[tile.Id].TileView;
                tileSlots[slotPos].UpdatePosition(tilePositions[slotPos], slotPos);
                slotPos++;
            }
            slotPos++;
        }
    }

    IEnumerator DistributionAnimation(float s, TileView tileView, int position)
    {
        yield return new WaitForSeconds(s);
        int y = (Screen.height / 2);
        Vector2 startPosition = new Vector2(0, y);
        tileView.MoveTargetPosition(startPosition);
        tileView.Show();
        tileView.DistributionAnimation(tilePositions[position],startPosition, position);
        distributedTiles++;
    }

    public void OnDrop(TileView tileView, RectTransform tileRect)
    {
        //tileView.
        float closest = float.MaxValue;
        int closestPos = -1;
        tileSlots[tileView.SlotPos] = null;
        for (int i = 0; i < tilePositions.Length; i++)
        {
            float currentDistance = Vector2.Distance(new Vector2(tileRect.localPosition.x, tileRect.localPosition.y),
                tilePositions[i]);
            if (currentDistance < closest)
            {
                closestPos = i;
                closest = currentDistance;
            }
        }
       
        if (tileSlots[closestPos] == null)
        {
            tileSlots[closestPos] = tileView;
            tileView.UpdatePosition(tilePositions[closestPos], closestPos);
        }
        else
        {
            if (CheckCanSlideRight(closestPos))
            {
                SlideRight(closestPos);
            }
            else
            {
                SlideLeft(closestPos);
            }
            tileSlots[closestPos] = tileView;
            tileView.UpdatePosition(tilePositions[closestPos], closestPos);
        }

    }
    
    public void SlideRight(int startPos)
    {
        int row = startPos / 14;
        TileView lastView = tileSlots[startPos]; 
        for (int i = startPos; i < (row+1) * 14 ; i++)
        {
            if (tileSlots[i + 1] == null)
            {
                tileSlots[i + 1] = lastView;
                tileSlots[i + 1].UpdatePosition(tilePositions[i+1], i+1);
                break;
            }
            TileView temp = tileSlots[i + 1];
            tileSlots[i + 1] = lastView;
            tileSlots[i + 1].UpdatePosition(tilePositions[i+1], i+1);
            lastView = temp;
        }
        
        tileSlots[startPos] = null;
    }
    public void SlideLeft(int startPos)
    {
        int row = startPos / 14;
        TileView lastView = tileSlots[startPos]; 
        for (int i = startPos; i > row * 14; i--)
        {
            if (tileSlots[i - 1] == null)
            {
                tileSlots[i - 1] = lastView;
                tileSlots[i - 1].UpdatePosition(tilePositions[i-1], i-1);
                break;
            }
            TileView temp = tileSlots[i - 1];
            tileSlots[i - 1] = lastView;
            tileSlots[i - 1].UpdatePosition(tilePositions[i-1], i-1);
            lastView = temp;
        }
        tileSlots[startPos] = null;
    }

    public bool CheckCanSlideRight(int closestPos)
    {
        int row = closestPos / 14;
        for (int i = closestPos+1; i < (row+1)*14; i++)
        {
            if (tileSlots[i] == null)
            {
                return true;
            }
        }
        return false;
    }
}