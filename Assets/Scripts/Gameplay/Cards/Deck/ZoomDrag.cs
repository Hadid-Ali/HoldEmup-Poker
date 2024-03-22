using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomDrag : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [Header("Component References")]
    
    [SerializeField] private RectTransform m_RectTransform;
    
    private Vector2 mousePosition = new Vector2();
    private Vector2 startPosition = new Vector2();
    private Vector2 differencePoint = new Vector2();

    private bool m_IsDragging = false;

    Vector3 position;

    float currentX;
    float currentY;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            UpdateMousePosition();
        }

        if (Input.GetMouseButtonDown(0))
        {
            UpdateStartPosition();
            UpdateDifferencePoint();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = mousePosition - differencePoint;
    }

    public void SetPositionManual()
    {
        m_RectTransform.DOAnchorPos(new Vector2(-290f, -269f), 0);
    }
    
    private void UpdateMousePosition()
    {
        mousePosition.y = Input.mousePosition.y;
    }

    private void UpdateStartPosition()
    {
        Vector3 currPosition = transform.position;
        
        startPosition.x = currPosition.x;
        startPosition.y = currPosition.y;
    }

    private void UpdateDifferencePoint()
    {
        differencePoint = mousePosition - startPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_IsDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_IsDragging = false;
    }
}
