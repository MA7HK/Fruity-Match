using UnityEngine;
using UnityEngine.UI;

public class GoalPanel : MonoBehaviour
{
	public Image _thisImage;
	public Sprite _thisSprite;
	public Text _thisText;
	public string _thisString;
    // Start is called before the first frame update
    void Start() => SetUp();

    void SetUp()
	{
		_thisImage.sprite = _thisSprite;
		_thisText.text = _thisString;
	}
}