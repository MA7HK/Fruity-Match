using UnityEngine;

public class PanelController : MonoBehaviour
{
	public Animator _planelAnimation;
	public Animator _panelInfoAnimation;

	public void OK()
	{
		if(_planelAnimation != null && _panelInfoAnimation != null) 
		{ 
			_planelAnimation.SetBool("IsFade", true);
			_panelInfoAnimation.SetBool("IsDown", true);
		}
	}

}