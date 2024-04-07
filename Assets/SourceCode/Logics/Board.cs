using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	wait,
	move,
	win,
	lose,
	pause
}

public enum TileKind
{
	Breakable,
	Blank,
	Basket,
	CoveredBasket,
	Slime,
	Normal,
}

[System.Serializable]
public class MatchType
{
	public int type;
	public string fruit; 
}


[System.Serializable]
public class TileType
{
	public int _x;
	public int _y;
	public TileKind _tileKind;
}

public class Board : MonoBehaviour
{
	[Header("Scriptable Object")]
	public World _world;
	public int _level;

	public GameState _currentState = GameState.move;
	
	[Header("Board Dimension")]
	public int _width;
	public int _height;
	public int _offSet;
	public float _cameraDistace;

	[Header("Prefabs")]
	public GameObject _tilePrefab;
	public GameObject _breakableTilePrefabs;
	public GameObject _basketTilePrefabs;
	public GameObject _coveredBasketTilePrefabs;
	public GameObject _destoryEffect;
	public GameObject[] Fruits;

	[Header("LayOut")]
	public TileType[] _boardLayout;
	private BackgroundTile[,] _breakableTiles;
	public BackgroundTile[,] _basketTiles;
	public BackgroundTile[,] _coveredBasketTiles;
	private bool[,] _blankSpaces;
	public GameObject[,] allFruits;

	[Header("Match")] 
	public MatchType matchType;
	public Fruit _currentFruit;
	private FindMatches _findMatches;
	private ScoreManager _scoreManager;
	private GoalManager _goalManager;
	public int _basePieceValue = 20;
	public int _streakValue = 1;
	public float _refillDelay = 0.5f;
	public int[] _scoreGoals;

	// Start is called before the first frame update

	void Awake()
	{
		if(PlayerPrefs.HasKey("currentLevel"))
		{
			_level = PlayerPrefs.GetInt("currentLevel");
		}
		if(_world != null)
		{
			if(_level < _world.levels.Length)
			{
				if(_world.levels[_level] != null)
				{
					_width = _world.levels[_level].width;
					_height = _world.levels[_level].height;
					Fruits = _world.levels[_level].fruits;
					_scoreGoals = _world.levels[_level].scoreGoals;
					_boardLayout = _world.levels[_level].boardLayout;
					_cameraDistace = _world.levels[_level].cameraDistance;
				}
			}
		}
	}

	void Start() => StartUp();

	public void StartUp()
	{
		_scoreManager = FindObjectOfType<ScoreManager>();
		_findMatches = FindObjectOfType<FindMatches>();
		_goalManager = FindObjectOfType<GoalManager>();
		_breakableTiles = new BackgroundTile[_width, _height];
		_basketTiles = new BackgroundTile[_width, _height];
		_coveredBasketTiles = new BackgroundTile[_width, _height];
		_blankSpaces = new bool[_width, _height];
		allFruits = new GameObject[_width, _height];
		SetUp();
		_currentState = GameState.pause;
	}

	public void GenerateTiles()
	{
		// Cache prefab references
		for (int i = 0; i < _boardLayout.Length; i++)
		{
			Vector2 tempPosition = new Vector2(_boardLayout[i]._x, _boardLayout[i]._y);

			switch (_boardLayout[i]._tileKind)
			{
				case TileKind.Blank:
					_blankSpaces[_boardLayout[i]._x, _boardLayout[i]._y] = true;
					break;
				case TileKind.Breakable:
					GameObject breakableTile = Instantiate(_breakableTilePrefabs, tempPosition, Quaternion.identity);
					_breakableTiles[_boardLayout[i]._x, _boardLayout[i]._y] = breakableTile.GetComponent<BackgroundTile>();
					break;
				case TileKind.Basket:
					GameObject basketTile = Instantiate(_basketTilePrefabs, tempPosition, Quaternion.identity);
					_basketTiles[_boardLayout[i]._x, _boardLayout[i]._y] = basketTile.GetComponent<BackgroundTile>();
					break;
				case TileKind.CoveredBasket:
					GameObject coverbasketTile = Instantiate(_coveredBasketTilePrefabs, tempPosition, Quaternion.identity);
					_basketTiles[_boardLayout[i]._x, _boardLayout[i]._y] = coverbasketTile.GetComponent<BackgroundTile>();
					break;
				// Add more cases for other tile kinds if needed
				default:
					// Handle any other cases here
					break;
			}
		}
	}



	// public void GenerateBlankSpaces()
	// {
	// 	for (int i = 0; i < _boardLayout.Length; i++)
	// 	{
	// 		if(_boardLayout[i]._tileKind == TileKind.Blank)
	// 		{
	// 			_blankSpaces[_boardLayout[i]._x, _boardLayout[i]._y] = true;
	// 		}
	// 	}
	// }

	// public void GenerateBreakableTile()
	// {
	// 	for (int i = 0; i < _boardLayout.Length; i++)
	// 	{
	// 		if( _boardLayout[i]._tileKind == TileKind.Breakable)
	// 		{
	// 			Vector2 _tempPosition = new Vector2(_boardLayout[i]._x, _boardLayout[i]._y);
	// 			GameObject _tile = Instantiate(_breakableTilePrefabs, _tempPosition, Quaternion.identity);
	// 			_breakableTiles[_boardLayout[i]._x, _boardLayout[i]._y] = _tile.GetComponent<BackgroundTile>();
	// 		}
	// 	}
	// }

	// private void GenerateBasketTiles()
	// {
	// 	for (int i = 0; i < _boardLayout.Length; i++)
	// 	{
	// 		if( _boardLayout[i]._tileKind == TileKind.Basket)
	// 		{
	// 			Vector2 _tempPosition = new Vector2(_boardLayout[i]._x, _boardLayout[i]._y);
	// 			GameObject _tile = Instantiate(_basketTilePrefabs, _tempPosition, Quaternion.identity);
	// 			_basketTiles[_boardLayout[i]._x, _boardLayout[i]._y] = _tile.GetComponent<BackgroundTile>();
	// 		}
	// 	}
	// }
	// private void GenerateCoveredbasketTiles()
	// {
	// 	for (int i = 0; i < _boardLayout.Length; i++)
	// 	{
	// 		if( _boardLayout[i]._tileKind == TileKind.CoveredBasket)
	// 		{
	// 			Vector2 _tempPosition = new Vector2(_boardLayout[i]._x, _boardLayout[i]._y);
	// 			GameObject _tile = Instantiate(_coveredBasketTilePrefabs, _tempPosition, Quaternion.identity);
	// 			_coveredBasketTiles[_boardLayout[i]._x, _boardLayout[i]._y] = _tile.GetComponent<BackgroundTile>();
	// 		}
	// 	}
	// }
	

	public void SetUp()
	{	
		GenerateTiles();
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				if (!_blankSpaces[i, j])
				{
					Vector2 tilePosition = new Vector2(i, j);
					GameObject backgroundTile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity, this.transform);
					backgroundTile.name = $"({i},{j})";

					int fruitToUse = Random.Range(0, Fruits.Length);
					int maxIteration = 0;
					while (maxIteration < 100)
					{
						if (MatchesAt(i, j, Fruits[fruitToUse]))
						{
							fruitToUse = Random.Range(0, Fruits.Length);
							maxIteration++;
						}
						else
						{
							break;
						}
					}

					Vector2 fruitPosition = new Vector2(i, j + _offSet);
					GameObject fruitObject = Instantiate(Fruits[fruitToUse], fruitPosition, Quaternion.identity, this.transform);
					Fruit fruitComponent = fruitObject.GetComponent<Fruit>();
					if (fruitComponent != null)
					{
						fruitComponent.row = j;
						fruitComponent.column = i;
						allFruits[i, j] = fruitObject;
						fruitObject.name = $"({i},{j})";
					}
				}
			}
		}


		// GenerateBlankSpaces();
		// GenerateBreakableTile();
		// GenerateBasketTiles();
		//GenerateCoveredbasketTiles();
		// for (int i = 0; i < _width; i++)
		// {
		// 	for (int j = 0; j < _height; j++)
		// 	{
		// 		if(!_blankSpaces[i,j] )
		// 		{
		// 			Vector2 _tempTilePosition = new Vector2(i , j);
		// 			GameObject _backgroundTile = Instantiate(_tilePrefab, _tempTilePosition ,Quaternion.identity) as GameObject;
		// 			_backgroundTile.transform.parent = this.transform;
		// 			_backgroundTile.name = "(" + i + "," + j + ")";

		// 			int _fruitToUse = Random.Range(0, Fruits.Length);
		// 			int _maxIteration = 0;
		// 			while (_maxIteration < 100)
		// 			{
		// 				if (MatchesAt(i, j, Fruits[_fruitToUse]))
		// 				{
		// 					_fruitToUse = Random.Range(0, Fruits.Length);
		// 					_maxIteration++;
		// 				}
		// 				else { break; }
		// 			}
		// 			_maxIteration = 0;

		// 			Vector2 _tempFruitPosition = new Vector2(i , j + _offSet);
		// 			GameObject fruits = Instantiate(Fruits[_fruitToUse], _tempFruitPosition, Quaternion.identity);
		// 			Fruit fruitComponent = fruits.GetComponent<Fruit>();
		// 			if (fruitComponent != null)
		// 			{
		// 				fruitComponent.row = j;
		// 				fruitComponent.column = i;
		// 				fruits.transform.parent = this.transform;
		// 				fruits.name = "(" + i + "," + j + ")";
		// 				allFruits[i, j] = fruits;
		// 			}
		// 		}
		// 	}
		// }
	}


	private bool MatchesAt(int column, int row, GameObject piece)
	{
		if(column > 1 && row > 1)
		{
			if(allFruits[column - 1, row] != null && allFruits[column -2, row] != null )
			{
				if(allFruits[column - 1, row].tag == piece.tag && allFruits[column - 2, row].tag == piece.tag) { return true; }
			}
			if(allFruits[column, row - 1] != null && allFruits[column, row - 2] != null)
			{
				if(allFruits[column, row - 1].tag == piece.tag && allFruits[column, row - 2].tag == piece.tag) { return true; }
			}
		}
		else if(column <= 1 || row <= 1)
		{
			if(row > 1 ) 
			{
				if(allFruits[column, row -1] != null && allFruits[column, row - 2] != null)
				{
					if(allFruits[column, row - 1].tag == piece.tag && allFruits[column, row - 2].tag == piece.tag) { return true; }
				} 
			}
			if(column > 1 ) 
			{	
				if(allFruits[column - 1, row] != null && allFruits[column - 2, row] != null)
				{
					if(allFruits[column - 1, row].tag == piece.tag && allFruits[column - 2, row].tag == piece.tag) { return true; }
				}
			}
		}
		return false;
	}

	private MatchType ColumnOrRow()
	{

		// 	make a copy of current matches
		List<GameObject> matchCopy = _findMatches._currentMatches as List<GameObject>;
		matchType.type = 0;
		matchType.fruit = "";

		//	cycle through all of match copy and decide if a bomb need it
		for (int i = 0; i < matchCopy.Count; i++)
		{
			//	store this fruit
			Fruit thisfruit = matchCopy[i].GetComponent<Fruit>();
			string fruit = matchCopy[i].tag;
			int column = thisfruit.column;
			int row = thisfruit.row;
			int columnMatch = 0;
			int rowMatch = 0;
			//	cycle through the rest of the pieces and compare
			for (int j = 0; j < matchCopy.Count; j++)
			{
				Fruit nextfruit = matchCopy[j].GetComponent<Fruit>();
				if(nextfruit == thisfruit) { continue; }
				if(nextfruit.column == thisfruit.column && nextfruit.tag == fruit) { columnMatch++; }
				if(nextfruit.row == thisfruit.row && nextfruit.tag == fruit) { rowMatch++; }
			}
			//	return 3 if column or row bomb
			//	return 2 if adjacent bomb
			//	return 1 if its a color bomb
			if(columnMatch == 4 || rowMatch == 4) { matchType.type = 1; matchType.fruit = fruit; return matchType; }
			else if(columnMatch == 2 || rowMatch == 2) { matchType.type = 2; matchType.fruit = fruit; return matchType; }
			else if(columnMatch == 3 || rowMatch == 3) { matchType.type = 3; matchType.fruit = fruit; return matchType; }
		}
		matchType.type = 0;
		matchType.fruit = "";
		return matchType;

	}

	private void CheckToMakeBombs()
	{
		// // how many objects are in findmatch currentMatches
		// if (_findMatches._currentMatches.Count > 3)
		// {
		// 	MatchType typeOfMatch = ColumnOrRow();
		// 	if (typeOfMatch.type == 1)
		// 	{
		// 		MakeBomb(typeOfMatch.fruit, "FruitBomb");
		// 	}
		// 	else if (typeOfMatch.type == 2)
		// 	{
		// 		MakeBomb(typeOfMatch.fruit, "AdjacentBomb");
		// 	}
		// 	else if (typeOfMatch.type == 3)
		// 	{
		// 		_findMatches.CheckBombs(typeOfMatch);
		// 	}
		// }

		

		//	how many obejct are in findmatch currentMatches
		if(_findMatches._currentMatches.Count > 3)
		{
			MatchType typeOfMatch = ColumnOrRow();
			if(typeOfMatch.type == 1)
			{
				//print("Make a fruit Bomb");
				if(_currentFruit != null && _currentFruit._isMatched && _currentFruit.tag == typeOfMatch.fruit)
				{
					_currentFruit._isMatched = false;
					_currentFruit.MakeFruitBomb();
				}
				else
				{
					if(_currentFruit._otherFruit != null)
					{
						Fruit _otherFruit = _currentFruit._otherFruit.GetComponent<Fruit>();
						if(_otherFruit._isMatched && _otherFruit.tag == typeOfMatch.fruit)
						{
							_otherFruit._isMatched = false;
							_otherFruit.MakeFruitBomb();
						}
					}
				}
			}
			else if(typeOfMatch.type == 2)
			{
				//print("Make a Adjacent Bomb");
				if(_currentFruit != null && _currentFruit._isMatched && _currentFruit.tag == typeOfMatch.fruit)
				{
					_currentFruit._isMatched = false;
					_currentFruit.MakeAdjacentBomb();

				}
				else if(_currentFruit._otherFruit != null)
				{
					Fruit _otherFruit = _currentFruit._otherFruit.GetComponent<Fruit>();
					if(_otherFruit._isMatched && _otherFruit.tag == typeOfMatch.fruit)
					{
						_otherFruit._isMatched = false;
						_otherFruit.MakeAdjacentBomb();
					}
				}
			}
			else if(typeOfMatch.type == 3) { _findMatches.CheckBombs(typeOfMatch); }

		}
	
		// if(_findMatches._currentMatches.Count == 4 || _findMatches._currentMatches.Count == 7)
		// {
		// 	_findMatches.CheckBombs();
		// }

		// if(_findMatches._currentMatches.Count == 5 || _findMatches._currentMatches.Count == 8)
		// {
		// 	if(ColumnOrRow())
		// 	{
		// 		//print("Make a fruit Bomb");
		// 		if(_currentFruit != null)
		// 		{
		// 			if(_currentFruit._isMatched)
		// 			{
		// 				if(!_currentFruit._isFruitBomb)
		// 				{
		// 					_currentFruit._isMatched = false;
		// 					_currentFruit.MakeFruitBomb();
		// 				}
		// 			}
		// 			else
		// 			{
		// 				if(_currentFruit._otherFruit != null)
		// 				{
		// 					Fruit _otherFruit = _currentFruit._otherFruit.GetComponent<Fruit>();
		// 					if(_otherFruit._isMatched)
		// 					{
		// 						if(!_otherFruit._isColumnBomb)
		// 						{
		// 							_otherFruit._isMatched = false;
		// 							_otherFruit.MakeFruitBomb();
		// 						}
		// 					}
		// 				}
		// 			}
		// 		}
		// 	}
		// 	else
		// 	{
		// 		//print("Make a Adjacent Bomb");
		// 		if(_currentFruit != null)
		// 		{
		// 			if(_currentFruit._isMatched)
		// 			{
		// 				if(!_currentFruit._isAdjacentBomb)
		// 				{
		// 					_currentFruit._isMatched = false;
		// 					_currentFruit.MakeAdjacentBomb();
		// 				}
		// 			}
		// 			else
		// 			{
		// 				if(_currentFruit._otherFruit != null)
		// 				{
		// 					Fruit _otherFruit = _currentFruit._otherFruit.GetComponent<Fruit>();
		// 					if(_otherFruit._isMatched)
		// 					{
		// 						if(!_otherFruit._isAdjacentBomb)
		// 						{
		// 							_otherFruit._isMatched = false;
		// 							_otherFruit.MakeAdjacentBomb();
		// 						}
		// 					}
		// 				}
		// 			}
		// 		}
		// 	}
		// }

	}

	// private void MakeBomb(string fruitTag, string bombType)
	// {
	// 	Fruit fruitToCheck = _currentFruit;
	// 	if (fruitToCheck == null || !fruitToCheck._isMatched || fruitToCheck.tag != fruitTag)
	// 	{
	// 		fruitToCheck = _currentFruit._otherFruit.GetComponent<Fruit>();
	// 	}

	// 	if (fruitToCheck != null && fruitToCheck._isMatched && fruitToCheck.tag == fruitTag)
	// 	{
	// 		fruitToCheck._isMatched = false;
	// 		if (bombType == "FruitBomb")
	// 		{
	// 			fruitToCheck.MakeFruitBomb();
	// 		}
	// 		else if (bombType == "AdjacentBomb")
	// 		{
	// 			fruitToCheck.MakeAdjacentBomb();
	// 		}
	// 	}
	// }

	// public void BombRow(int _row)
	// {
	// 	for (int i = 0; i < _width; i++)
	// 	{
	// 		if(_basketTiles[i, _row])
	// 		{
	// 			_basketTiles[i, _row].TakeDamage(1);
	// 			if(_basketTiles[i, _row]._hitPoints <= 0)
	// 			{
	// 				_basketTiles[i, _row] = null;
	// 			}
	// 		}
	// 	}
	// }
	// public void BombColumn(int _column)
	// {
	// 	for (int i = 0; i < _width; i++)
	// 	{
	// 		if(_basketTiles[_column, i])
	// 		{
	// 			_basketTiles[_column, i].TakeDamage(1);
	// 			if(_basketTiles[_column, i]._hitPoints <= 0)
	// 			{
	// 				_basketTiles[_column, i] = null;
	// 			}
	// 		}
	// 	}
	// }

	public void BombRow(int _row)
	{
		for (int i = 0; i < _width; i++)
		{
			var tile = _basketTiles[i, _row];
			if (tile != null)
			{
				tile.TakeDamage(1);
				if (tile._hitPoints <= 0)
				{
					_basketTiles[i, _row] = null;
				}
			}
		}
	}

	public void BombColumn(int _column)
	{
		for (int i = 0; i < _width; i++)
		{
			var tile = _basketTiles[_column, i];
			if (tile != null)
			{
				tile.TakeDamage(1);
				if (tile._hitPoints <= 0)
				{
					_basketTiles[_column, i] = null;
				}
			}
		}
	}


	private void DestroyMatchAt(int column, int row)
	{
		if(allFruits[column, row].GetComponent<Fruit>()._isMatched)
		{
			if(_breakableTiles[column,row] != null) 
			{ 
				_breakableTiles[column, row].TakeDamage(1);
				if(_breakableTiles[column, row]._hitPoints <= 0) { _breakableTiles[column, row] = null; }
			}
			if(_basketTiles[column,row] != null) 
			{ 
				_basketTiles[column, row].TakeDamage(1);
				if(_basketTiles[column, row]._hitPoints <= 0) { _basketTiles[column, row] = null; }
			}
			DamageCoveredBasket(column, row);
			if( _goalManager != null) 
			{
				_goalManager.CompareGoal(allFruits[column,row].tag.ToString());
				_goalManager.UpdateGoals();
			}
				
			SoundManager.soundfx.PlayRandomDestorySound();

			//_findMatches._currentMatches.Remove(allFruits[column, row]);
			GameObject _destoryParticle = Instantiate(_destoryEffect, allFruits[column, row].transform.position, Quaternion.identity);
			Destroy(_destoryParticle, 0.5f);
			Destroy(allFruits[column, row]);
			_scoreManager.IncreaseScore(_basePieceValue * _streakValue);
			allFruits[column, row] = null;
		}
	}

	// private void DestroyMatchAt(int column, int row)
	// {
	// 	if (allFruits[column, row].GetComponent<Fruit>()._isMatched)
	// 	{
	// 		// Handle breakable tiles
	// 		if (_breakableTiles[column, row] != null)
	// 		{
	// 			_breakableTiles[column, row].TakeDamage(1);
	// 			if (_breakableTiles[column, row]._hitPoints <= 0)
	// 				_breakableTiles[column, row] = null;
	// 		}

	// 		// Handle basket tiles
	// 		if (_basketTiles[column, row] != null)
	// 		{
	// 			_basketTiles[column, row].TakeDamage(1);
	// 			if (_basketTiles[column, row]._hitPoints <= 0)
	// 				_basketTiles[column, row] = null;
	// 		}

	// 		// Damage covered basket
	// 		DamageCoveredBasket(column, row);

	// 		// Update goals if goal manager exists
	// 		_goalManager?.CompareGoal(allFruits[column, row].tag.ToString());
	// 		_goalManager?.UpdateGoals();

	// 		// Play destroy sound
	// 		SoundManager.soundfx.PlayRandomDestorySound();

	// 		// Create and destroy particle effect
	// 		GameObject destroyParticle = Instantiate(_destoryEffect, allFruits[column, row].transform.position, Quaternion.identity);
	// 		Destroy(destroyParticle, 0.5f);

	// 		// Destroy the fruit
	// 		Destroy(allFruits[column, row]);
	// 		allFruits[column, row] = null;

	// 		// Increase score
	// 		_scoreManager.IncreaseScore(_basePieceValue * _streakValue);
	// 	}
	// }


	public void DestroyMatches()
	{
		if(_findMatches._currentMatches.Count >= 4) { CheckToMakeBombs(); }
		_findMatches._currentMatches.Clear();
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				if(allFruits[i, j] != null) { DestroyMatchAt(i, j); }
			}
		}
		StartCoroutine(DecreaseRowCo2());

	}


// 	private void DamageCoveredBasket(int column, int row)
// 	{
// 		if(column > 0)
// 		{
// 			if(_coveredBasketTiles[column - 1, row])
// 			{
// 				_coveredBasketTiles[column - 1, row].TakeDamage(1);
// ;				if(_coveredBasketTiles[column - 1, row]._hitPoints <=  0)
// 				{
// 					_coveredBasketTiles[column - 1, row] = null;
// 				}
// 			}
// 		}
// 		if(column < _width - 1)
// 		{
// 			if(_coveredBasketTiles[column + 1, row])
// 			{
// 				_coveredBasketTiles[column + 1, row].TakeDamage(1);
// 				if(_coveredBasketTiles[column + 1, row]._hitPoints <=  0)
// 				{
// 					_coveredBasketTiles[column + 1, row] = null;
// 				}
// 			}
// 		}
// 		if(row > 0)
// 		{
// 			if(_coveredBasketTiles[column, row - 1])
// 			{
// 				_coveredBasketTiles[column, row - 1].TakeDamage(1);
// 				if(_coveredBasketTiles[column, row - 1]._hitPoints <=  0)
// 				{
// 					_coveredBasketTiles[column, row - 1] = null;
// 				}
// 			}
// 		}
// 		if(row < _height - 1)
// 		{
// 			if(_coveredBasketTiles[column, row + 1])
// 			{
// 				_coveredBasketTiles[column, row + 1].TakeDamage(1);
// 				if(_coveredBasketTiles[column, row + 1]._hitPoints <=  0)
// 				{
// 					_coveredBasketTiles[column, row + 1] = null;
// 				}
// 			}
// 		}
// 	}

	private void DamageCoveredBasket(int column, int row)
	{
		void HandleTile(int col, int r)
		{
			if (_coveredBasketTiles[col, r] != null)
			{
				_coveredBasketTiles[col, r].TakeDamage(1);
				if (_coveredBasketTiles[col, r]._hitPoints <= 0)
					_coveredBasketTiles[col, r] = null;
			}
		}

		if (column > 0) HandleTile(column - 1, row);
		if (column < _width - 1) HandleTile(column + 1, row);
		if (row > 0) HandleTile(column, row - 1);
		if (row < _height - 1) HandleTile(column, row + 1);
	}


	public IEnumerator DecreaseRowCo2()
	{
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				if(!_blankSpaces[i, j] && allFruits[i, j] == null && !_coveredBasketTiles[i, j])
				{
					for (int k = j + 1; k < _height; k++)
					{
						if(allFruits[i, k] != null)
						{
							allFruits[i, k].GetComponent<Fruit>().row = j;
							allFruits[i, k] = null;
							break;
						}
					}
				}
			}
		}
		yield return new WaitForSeconds(_refillDelay * 0.5f);
		StartCoroutine(FillBoard());
	}


	// private IEnumerator DecreaseRowCo()
	// {
	// 	int _nullCounter = 0;
	// 	for (int i = 0; i < _width; i++)
	// 	{
	// 		for (int j = 0; j < _height; j++)
	// 		{
	// 			if(allFruits[i, j] == null) { _nullCounter++; }
	// 			else if(_nullCounter > 0)
	// 			{
	// 				allFruits[i, j].GetComponent<Fruit>().row -= _nullCounter;
	// 				allFruits[i, j] = null;
	// 			}
	// 		}
	// 		_nullCounter = 0;
	// 	}
	// 	yield return new WaitForSeconds(_refillDelay * 0.5f);
	// 	StartCoroutine(FillBoard());
	// }

	private void RefillBoard()
	{
		for (int i = 0; i < _width; i++)
		{
			for(int j = 0; j < _height; j++)
			{
				if(allFruits[i, j] == null && !_blankSpaces[i, j] && !_coveredBasketTiles[i, j])
				{
					Vector2 _tempPosition = new Vector2(i, j + _offSet);
					int _fruitToUse = Random.Range(0, Fruits.Length);
					int maxIteration = 0;
					while (MatchesAt(i, j, Fruits[_fruitToUse]) && maxIteration < 100)
					{
						maxIteration++;
						_fruitToUse = Random.Range(0, Fruits.Length);
					}
					maxIteration = 0;
					GameObject piece = Instantiate(Fruits[_fruitToUse], _tempPosition, Quaternion.identity);
					allFruits[i, j] = piece;
					piece.GetComponent<Fruit>().row = j; 
					piece.GetComponent<Fruit>().column = i; 
				}
			}
		}
	}


	private bool MatchesOnBoard()
	{
		_findMatches.FindAllMatch();
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				if (allFruits[i, j]?.GetComponent<Fruit>()._isMatched == true)
					return true; // Exit early if a match is found
			}
		}
		return false;
	}


	private IEnumerator FillBoard()
	{
		yield return new WaitForSeconds(_refillDelay);
		RefillBoard();
		yield return new WaitForSeconds(_refillDelay);
		while(MatchesOnBoard())
		{
			_streakValue++;
			DestroyMatches();
			yield break;
			//yield return new WaitForSeconds(_refillDelay * 2);
		}
		//_findMatches._currentMatches.Clear();
		_currentFruit = null;
		if(IsDeadlocked()) 
		{ 
			StartCoroutine(ShuffleBoard()); 
			print("Dead Locked"); 
		}
		yield return new WaitForSeconds(_refillDelay);
		_currentState = GameState.move;
		_streakValue = 1;
	}

	private Vector2 CheckForAdjacent(int column, int row)
	{
		if(allFruits[column + 1, row] && column < _width -1) 	{ return Vector2.right; }
		if(allFruits[column - 1, row] && column > 0) 			{ return Vector2.left; }
		if(allFruits[column, row + 1] && row  < _height -1) 	{ return Vector2.up; }
		if(allFruits[column, row - 1] && row > 0)  				{ return Vector2.down; }
		return Vector2.zero;
	}

	private void SwitchPieces(int _column, int _row, Vector2 _direction)
	{
		if(allFruits[_column + (int)_direction.x, _row + (int)_direction.y] != null)
		{
			//	take the second piece and save it in a holder
			GameObject _holder = allFruits[_column + (int)_direction.x, _row + (int)_direction.y] as GameObject;
			//	switching the first fruit to be the second position
			allFruits[_column + (int)_direction.x, _row + (int)_direction.y] = allFruits[_column, _row];
			//	set the first fruit to be the second fruit
			allFruits[_column, _row] = _holder;
		}
	}

	private bool CheckForMatches()
	{
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				if(allFruits[i, j] != null)
				{
					if( i < _width - 2)
					{
						//	Check if the fruits to the right and two to the right
						if(allFruits[i + 1, j] != null && allFruits[i + 2, j] != null)
						{
							if(allFruits[i + 1, j].tag == allFruits[i, j].tag && allFruits[i + 2, j].tag == allFruits[i, j].tag) { return true; }
						}
					}

					if(j < _height - 2)
					{
						//	check if the fruits above exist
						if(allFruits[i , j + 1] != null && allFruits[i, j + 2] != null)
						{
							if(allFruits[i , j + 1].tag == allFruits[i, j].tag && allFruits[i, j + 2].tag == allFruits[i, j].tag) { return true; }
						}
					}
				}
			}
		}
		return false;
	}

	public bool SwitchAndCheck(int _column, int _row, Vector2 _direction)
	{
		SwitchPieces(_column, _row, _direction);
		if(CheckForMatches())
		{
			SwitchPieces(_column, _row, _direction);
			return true;
		}
		SwitchPieces(_column, _row, _direction);
		return false;
	}

	private bool IsDeadlocked()
	{
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				if(allFruits[i ,j] != null)
				{
					if(i < _width - 1) 
					{ 
						if(SwitchAndCheck(i , j, Vector2.right)) { return false; }
					}
					if(j < _height - 1) 
					{ 
						if(SwitchAndCheck(i , j, Vector2.up)) { return false; }
					}
				}
			}
		}
		return true;
	}

	private IEnumerator ShuffleBoard()
	{
		yield return new WaitForSeconds(0.4f);
		List<GameObject> _newBoard = new List<GameObject>();
		for(int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				if(allFruits[i, j] != null)
				{
					_newBoard.Add(allFruits[i, j]);
				}
			}
		}

		yield return new WaitForSeconds(0.4f);
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				if(!_blankSpaces[i, j] && !_basketTiles[i, j])
				{
					//	pipck a random
					int _pieceToUse = Random.Range(0, _newBoard.Count);
					//	make a container for the piece
					int _maxIteration = 0;
					while (_maxIteration < 100)
					{
						if (MatchesAt(i, j, _newBoard[_pieceToUse]))
						{
							_pieceToUse = Random.Range(0, _newBoard.Count);
							_maxIteration++;
							//print(_maxIteration);
						}
						else { break; }
					}
					_maxIteration = 0;
					Fruit  _piece = _newBoard[_pieceToUse].GetComponent<Fruit>();
					//	assign the colum to the piece
					_piece.column = i;
					//	assign the row to the piece
					_piece.row = j;
					//	Fill in the fruits arraty with this new piece
					allFruits[i, j] = _newBoard[_pieceToUse];
					//	remove if form the list
					_newBoard.Remove(_newBoard[_pieceToUse]);
				}
			}
		}
		//	check if it's still deadlock
		if(IsDeadlocked()) { StartCoroutine(ShuffleBoard()); }
	}



}