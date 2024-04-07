using UnityEngine;

public class CameraController : MonoBehaviour
{	
	private Board _board;
	public float _cameraOffset;
	public float _aspectRatio = 0.625f;
	public float _padding = 2;
	public float _yOffset = 1;

    // Start is called before the first frame update
    void Start()
    {
        _board = FindObjectOfType<Board>();
		if(_board != null)
		{
			RepositionCamera(_board._width - 1, _board._height);
		}
		Time.timeScale = 1.0f;
		Camera.main.orthographicSize = _board._cameraDistace;
   }

    void RepositionCamera(float _x, float _y)
	{
		Vector3 _tempPosition = new Vector3((_x/2),(_y/2) + _yOffset, _cameraOffset);
		transform.position = _tempPosition;
		if(_board._width >= _board._height)
		{
			Camera.main.orthographicSize = (_board._width/2 + _padding) / _aspectRatio;
		}
		else { Camera.main.orthographicSize = _board._height/2 + _padding; }
	}


}