using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WaypointMarker : MonoBehaviour
{
    // Indicator icon
    [SerializeField] private Image m_image;
    [SerializeField] private RectTransform m_imageRect;
    // The target (location, enemy, etc..)

    public Transform TargetWaypoint;
    // UI Text to display the distance
    //public Text meter;
    // To adjust the position of the icon
    [SerializeField] private Vector3 m_offset;
    Sequence seq;

    private void OnEnable()
    {
        seq = DOTween.Sequence();
        AnimateMarker();
    }
    private void Update()
    {
        if (TargetWaypoint != null)
        {   if(!m_image.gameObject.activeSelf) m_image.gameObject.SetActive(true);
            Marking();
        }
        else if(TargetWaypoint == null)
        {
            if(m_image.gameObject.activeSelf) m_image.gameObject.SetActive(false);
        }
        
    }

    public void AnimateMarker()
    {
        seq.Append(m_imageRect.DOScale(new Vector2(1.2f, 1.2f), 0.3f).SetDelay(0.3f));
        seq.Append(m_imageRect.DOScale(Vector2.one, 0.3f));
        seq.SetLoops(-1, LoopType.Restart);
    }


    private void Marking()
    {
        // Giving limits to the icon so it sticks on the screen
        // Below calculations witht the assumption that the icon anchor point is in the middle
        // Minimum X position: half of the icon width
        float minX = m_image.GetPixelAdjustedRect().width / 2;
        // Maximum X position: screen width - half of the icon width
        float maxX = Screen.width - minX;

        // Minimum Y position: half of the height
        float minY = m_image.GetPixelAdjustedRect().height / 2;
        // Maximum Y position: screen height - half of the icon height
        float maxY = Screen.height - minY;

        // Temporary variable to store the converted position from 3D world point to 2D screen point
        Vector2 pos = Camera.main.WorldToScreenPoint(TargetWaypoint.position + m_offset);

        // Check if the target is behind us, to only show the icon once the target is in front
        if (Vector3.Dot((TargetWaypoint.position - transform.position), transform.forward) < 0)
        {
           // m_image.enabled = false;
            // Check if the target is on the left side of the screen
            if(pos.x < Screen.width / 2)
            {
                // Place it on the right (Since it's behind the player, it's the opposite)
                pos.x = maxX;
            }
            else
            {
                // Place it on the left side
                pos.x = minX;
            }
        }
        else
            //m_image.enabled = true;


        // Limit the X and Y positions
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Update the marker's position
        m_image.transform.position = pos;
        // Change the meter text to the distance with the meter unit 'm'
        //meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }

    
}