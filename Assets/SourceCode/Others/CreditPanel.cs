using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button _infoButton; 
    public GameObject _infoPanel; 

    public void OnPointerDown(PointerEventData eventData)
    {
		_infoPanel.SetActive(true);
		print("0");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPress == _infoButton.gameObject)
        {
			_infoPanel.SetActive(false);
			print("1");
		}
    }
	
}