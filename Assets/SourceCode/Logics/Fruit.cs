using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour
{
	[Header("Board variables")]
	public int column;
	public int row;
	public int _preColumn;
	public int _preRow;
	public int _targetX;
	public int _targetY;
	public bool _isMatched = false;

	private Board _board;
	public GameObject _otherFruit;
	private Vector2 _firstTouchPosition = Vector2.zero;
	private Vector2 _finalTouchPosition = Vector2.zero;
	private Vector2 _tempPosition;
	private FindMatches _findMatches;
	private HintManager _hintManager;
	private EndGameManager _endGameManager;

	[Header("Swipe")]
	public float swipeAngle = 0;
	public float _swipeResist = 1.0f;

	[Header("Power ups")]
	public bool _isFruitBomb;
	public bool _isColumnBomb;
	public bool _isRowBomb;
	public bool _isAdjacentBomb;
	public GameObject _adjacentBomb;
	public GameObject _arrowRow;
	public GameObject _arrowColumn;
	public GameObject _fruitBomb;

	// Start is called before the first frame update
	void Start()
	{

		_isColumnBomb = false;
		_isRowBomb = false;
		_isFruitBomb = false;
		_isAdjacentBomb = false;

		_board = GameObject.FindWithTag("Board").GetComponent<Board>();
		_endGameManager = FindObjectOfType<EndGameManager>();
		_findMatches = FindObjectOfType<FindMatches>();
		_hintManager = FindObjectOfType<HintManager>();

	}

	private void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(1))
		{
			_isAdjacentBomb = true;
			GameObject marker = Instantiate(_adjacentBomb, transform.position, Quaternion.identity);
			marker.transform.parent = this.transform;
		}
	}


	// Update is called once per frame
	void Update()
	{
		_targetX = column;
		_targetY = row;
		if(Mathf.Abs(_targetX - transform.position.x) > 0.1)
		{
			// Move towards the target
			_tempPosition = new Vector2(_targetX, transform.position.y);
			transform.position = Vector2.Lerp(transform.position, _tempPosition, 0.5f);
			if(_board.allFruits[column, row] != this.gameObject)
			{
				_board.allFruits[column, row] = this.gameObject; 
				_findMatches.FindAllMatch();
			}
		}
		else
		{
			// Directly set the position
			_tempPosition = new Vector2(_targetX, transform.position.y);
			transform.position = _tempPosition;
		}

		if(Mathf.Abs(_targetY - transform.position.y) > 0.1)
		{
			// Move towards the target
			_tempPosition = new Vector2(transform.position.x, _targetY);
			transform.position = Vector2.Lerp(transform.position, _tempPosition, 0.5f);
			if(_board.allFruits[column, row] != this.gameObject)
			{
				_board.allFruits[column, row] = this.gameObject; 
				_findMatches.FindAllMatch();		
			}
		}
		else
		{
			// Directly set the position
			_tempPosition = new Vector2(transform.position.x, _targetY);
			transform.position = _tempPosition;
		}

	}

	public IEnumerator CheckMove()
	{
		
		if(_isFruitBomb)
		{ 
			_findMatches.MatchPieceOfColor(_otherFruit.tag); 
			_isMatched = true; 
			print("Fruit Bomb");
		}
		else if(_otherFruit.GetComponent<Fruit>()._isFruitBomb)
		{ 
			_findMatches.MatchPieceOfColor(this.gameObject.tag); 
			_otherFruit.GetComponent<Fruit>()._isMatched = true;
		}
		
		yield return new WaitForSeconds(.5f);
		if(_otherFruit != null)
		{
			if(!_isMatched && !_otherFruit.GetComponent<Fruit>()._isMatched)
			{
				_otherFruit.GetComponent<Fruit>().row = row;
				_otherFruit.GetComponent<Fruit>().column = column;
				row = _preRow;
				column = _preColumn;
				yield return new WaitForSeconds(0.5f);
				_board._currentFruit = null;
				_board._currentState = GameState.move;
			}
			else
			{
				if(_endGameManager != null)
				{
					if(_endGameManager._requirements._gameType == GameType.Moves) { _endGameManager.DecreaseCounterValue(); }
				}
				_board.DestroyMatches();
			}
			_otherFruit = null;
		}
		
	}

	private void OnMouseDown()
	{
		if(_hintManager != null) { _hintManager.DestroyHint(); }
		if(_board._currentState == GameState.move)
		{
			_firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Debug.Log(_firstTouchPosition);
		}
	}

	private void OnMouseUp()
	{
		if(Utilitites.IsMouseOverUIObject()) return;
		if(_board._currentState == GameState.move)
		{
			_finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			CalculateAngle();
		}
	}

	void CalculateAngle()
	{
		if(Mathf.Abs(_finalTouchPosition.y - _firstTouchPosition.y) > _swipeResist 
			|| Mathf.Abs(_finalTouchPosition.x - _firstTouchPosition.x) > _swipeResist)
		{
			_board._currentState = GameState.wait;
			swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, 
								 _finalTouchPosition.x - _firstTouchPosition.x) * 180 / Mathf.PI;
			//Debug.Log(swipeAngle);
			MovePieces();
			_board._currentFruit = this;
		}
		else _board._currentState = GameState.move;
	}

	void MoveActualPieces(Vector2 _direction)
	{
		_otherFruit = _board.allFruits[column + (int)_direction.x, row + (int)_direction.y];
		_preColumn = column;
		_preRow = row;
		if(_board._basketTiles[column, row] == null && _board._basketTiles[column + (int)_direction.x, row + (int)_direction.y] == null)
		{
			if(_otherFruit != null)
			{
				_otherFruit.GetComponent<Fruit>().column += -1 * (int)_direction.x;
				_otherFruit.GetComponent<Fruit>().row += -1 * (int)_direction.y;
				column += (int)_direction.x;
				row += (int)_direction.y;
				StartCoroutine(CheckMove());
			}
			else _board._currentState = GameState.move;
		}
		else _board._currentState = GameState.move;
	}

	void MovePieces()
	{
		if(swipeAngle > -45 && swipeAngle <= 45 && column < _board._width - 1)
		{
			MoveActualPieces(Vector2.right);
		}
		else if(swipeAngle > 45 && swipeAngle <= 135 && row < _board._height - 1)
		{
			MoveActualPieces(Vector2.up);
		}
		else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
		{
			MoveActualPieces(Vector2.left);
		}
		else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0)
		{
			MoveActualPieces(Vector2.down);
		}
		else
		{
			_board._currentState = GameState.move;
		}
			
	}

	public void MakeRowBomb()
	{
		if(!_isColumnBomb && !_isFruitBomb && !_isAdjacentBomb)
		{
			_isRowBomb = true;
			GameObject arrow = Instantiate(_arrowRow, transform.position, Quaternion.identity);
			arrow.transform.parent = this.transform;
		}
	}

	public void MakeColumnBomb()
	{
		if(!_isRowBomb && !_isFruitBomb && !_isAdjacentBomb)
		{
			_isColumnBomb = true;
			GameObject arrow = Instantiate(_arrowColumn, transform.position, Quaternion.identity);
			arrow.transform.parent = this.transform;
		}
	}

	public void MakeFruitBomb()
	{
		if(!_isRowBomb && !_isColumnBomb && !_isAdjacentBomb)
		{
			_isFruitBomb = true;
			GameObject fruit = Instantiate(_fruitBomb, transform.position, Quaternion.identity);
			fruit.transform.parent = this.transform;
			gameObject.tag = "FruitBomb";
		}
	}

	public void MakeAdjacentBomb()
	{
		if(!_isRowBomb && !_isColumnBomb && !_fruitBomb)
		{
			_isAdjacentBomb = true;
			GameObject _adjacentbomb = Instantiate(_adjacentBomb, transform.position, Quaternion.identity);
			_adjacentbomb.transform.parent = this.transform;
		}
	}

}