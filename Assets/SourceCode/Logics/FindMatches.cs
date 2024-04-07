using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
	public List<GameObject> _currentMatches = new List<GameObject>();
	
	public GameObject _adjacentBombParticle;
	public GameObject _arrowRowParticle;
	public GameObject _arrowColumnParticle;
	private Board _board;


    void Start() => _board = FindObjectOfType<Board>();

    public void FindAllMatch()
	{
		StartCoroutine(FindAllMatchesCo());
	}

	private List<GameObject> IsAdjacentBomb(Fruit _fruit_1, Fruit _fruit_2, Fruit _fruit_3)
	{
		List<GameObject> _currentFruits = new List<GameObject>();
		if(_fruit_1._isAdjacentBomb) { _currentMatches.Union(GetAdjacentPieces(_fruit_1.column, _fruit_1.row)); }
		if(_fruit_2._isAdjacentBomb) { _currentMatches.Union(GetAdjacentPieces(_fruit_2.column, _fruit_2.row)); }
		if(_fruit_3._isAdjacentBomb) { _currentMatches.Union(GetAdjacentPieces(_fruit_3.column, _fruit_3.row)); }
		return _currentFruits;
	}

	private List<GameObject> IsRowBomb(Fruit _fruit_1, Fruit _fruit_2, Fruit _fruit_3)
	{
		List<GameObject> _currentFruits = new List<GameObject>();
		if(_fruit_1._isRowBomb) { _currentMatches.Union(GetRowPieces(_fruit_1.row)); _board.BombRow(_fruit_1.row); }
		if(_fruit_2._isRowBomb) { _currentMatches.Union(GetRowPieces(_fruit_2.row)); _board.BombRow(_fruit_2.row); }
		if(_fruit_3._isRowBomb) { _currentMatches.Union(GetRowPieces(_fruit_3.row)); _board.BombRow(_fruit_3.row); }
		return _currentFruits;
	}
	private List<GameObject> IsColumnBomb(Fruit _fruit_1, Fruit _fruit_2, Fruit _fruit_3)
	{
		List<GameObject> _currentFruits = new List<GameObject>();
		if(_fruit_1._isColumnBomb) { _currentMatches.Union(GetColumnPieces(_fruit_1.column)); _board.BombColumn(_fruit_1.column); }
		if(_fruit_2._isColumnBomb) { _currentMatches.Union(GetColumnPieces(_fruit_2.column)); _board.BombColumn(_fruit_2.column); }
		if(_fruit_3._isColumnBomb) { _currentMatches.Union(GetColumnPieces(_fruit_3.column)); _board.BombColumn(_fruit_3.column); }
		return _currentFruits;
	}

	private void AddToListAndMatch(GameObject _fruit)
	{
		if(!_currentMatches.Contains(_fruit)) { _currentMatches.Add(_fruit); }
		_fruit.GetComponent<Fruit>()._isMatched = true;
		
	}

	private void GetNearByPieces(GameObject _fruit_1, GameObject _fruit_2, GameObject _fruit_3)
	{
		AddToListAndMatch(_fruit_1);
		AddToListAndMatch(_fruit_2);
		AddToListAndMatch(_fruit_3);
	}

private IEnumerator FindAllMatchesCo()
{
    //yield return new WaitForSeconds(0.2f);
	yield return null;
    for (int i = 0; i < _board._width; i++)
    {
        for (int j = 0; j < _board._height; j++)
        {
            GameObject currentFruit = _board.allFruits[i, j];
            if(currentFruit != null)
            {
                Fruit _currentFruit = currentFruit.GetComponent<Fruit>();
                if(_currentFruit != null)
                {
                    if(i > 0 && i < _board._width - 1)
                    {
                        GameObject leftfruit = _board.allFruits[i - 1, j];
                        GameObject rightfruit = _board.allFruits[i + 1, j];
                        if(leftfruit != null && rightfruit != null)
                        {
                            Fruit _leftfruit = leftfruit.GetComponent<Fruit>();
                            Fruit _rightfruit = rightfruit.GetComponent<Fruit>();
                            if(_leftfruit != null && _rightfruit != null)
                            {
                                if(leftfruit.tag == currentFruit.tag && rightfruit.tag == currentFruit.tag)
                                {
                                    _currentMatches.Union(IsRowBomb(_leftfruit, _currentFruit, _rightfruit));
                                    _currentMatches.Union(IsColumnBomb(_leftfruit, _currentFruit, _rightfruit));
                                    _currentMatches.Union(IsAdjacentBomb(_leftfruit, _currentFruit, _rightfruit));
                                    GetNearByPieces(leftfruit, currentFruit, rightfruit);
                                }
                            }
                        }
                    }

                    if(j > 0 && j < _board._height - 1)
                    {
                        GameObject upfruit = _board.allFruits[i, j + 1];
                        GameObject downfruit = _board.allFruits[i, j - 1];
                        if(upfruit != null && downfruit != null)
                        {
                            Fruit _upfruit = upfruit.GetComponent<Fruit>();
                            Fruit _downfruit = downfruit.GetComponent<Fruit>();
                            if(_upfruit != null && _downfruit != null)
                            {
                                if(upfruit.tag == currentFruit.tag && downfruit.tag == currentFruit.tag)
                                {
                                    _currentMatches.Union(IsColumnBomb(_upfruit, _currentFruit, _downfruit));
                                    _currentMatches.Union(IsRowBomb(_upfruit, _currentFruit, _downfruit));
                                    _currentMatches.Union(IsAdjacentBomb(_upfruit, _currentFruit, _downfruit));
                                    GetNearByPieces(upfruit, currentFruit, downfruit);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

	public void MatchPieceOfColor(string _fruitName)
	{
		for (int i = 0; i < _board._width; i++)
		{
			for (int j = 0; j < _board._height; j++)
			{
				if(_board.allFruits[i, j] != null)
				{
					if(_board.allFruits[i, j].tag == _fruitName)
					{
						_board.allFruits[i, j].GetComponent<Fruit>()._isMatched = true;
					}
				}
			}
		}
	}

	List<GameObject> GetAdjacentPieces(int column, int row)
	{
		List<GameObject> fruits = new List<GameObject>();
		for (int i = column - 1; i <= column + 1; i++)
		{
			for (int j = row - 1; j <= row + 1; j++)
			{
				if( i >= 0 && i < _board._width && j >= 0 && j < _board._height)
				{
					if(_board.allFruits[i, j] != null)
					{
						fruits.Add(_board.allFruits[i, j]);
						_board.allFruits[i, j].GetComponent<Fruit>()._isMatched = true;
						GameObject _adjacentBomb = Instantiate(_adjacentBombParticle, _board.allFruits[i, j].transform.position, Quaternion.identity);
						_adjacentBomb.transform.parent = _board.allFruits[i ,j].transform;
						print("adjacent Bomb");
					}
				}
			}
		}
		return fruits;
	}

	List<GameObject> GetColumnPieces(int column)
	{
		List<GameObject> fruits = new List<GameObject>();
		for (int i = 0; i < _board._height; i++)
		{
			if(_board.allFruits[column, i] != null)
			{
				Fruit fruit = _board.allFruits[column, i].GetComponent<Fruit>();
				if(fruit._isRowBomb)
				{
					fruits.Union(GetRowPieces(i)).ToList();
				}
				fruits.Add(_board.allFruits[column, i]);
				fruit._isMatched = true;
				StartCoroutine(DestoryParticle(_arrowColumnParticle, fruit));
				SoundManager.soundfx.PlayCollapsedSound();
				print("Column Bomb");
			}
		}
		return fruits;
	}

	List<GameObject> GetRowPieces(int row)
	{
		List<GameObject> fruits = new List<GameObject>();
		for (int i = 0; i < _board._width; i++)
		{
			if(_board.allFruits[i,row] != null)
			{
				Fruit fruit = _board.allFruits[i, row].GetComponent<Fruit>();
				if(fruit._isColumnBomb)
				{
					fruits.Union(GetColumnPieces(i)).ToList();
				}
				fruits.Add(_board.allFruits[i, row]);
				fruit._isMatched = true;
				StartCoroutine(DestoryParticle(_arrowRowParticle, fruit));
				SoundManager.soundfx.PlayCollapsedSound();
				print("Row Bomb");
			}
		}
		return fruits;
	}

	//	make a corutine for bombs to destory some delay
	//	use that code comment on getcolumnpices
	private IEnumerator DestoryParticle(GameObject _particle, Fruit _parent)
	{
		yield return new WaitForSeconds(0.2f);
		GameObject _columnDestoryParticle = Instantiate(_particle, _parent.transform.position, Quaternion.identity);
		_columnDestoryParticle.transform.parent = _parent.transform;
		Destroy(_columnDestoryParticle, 1f);
	}


	public void CheckBombs(MatchType matchType)
	{	//	did player move something
		if(_board._currentFruit != null)
		{	//	is the piece they matched
			if(_board._currentFruit._isMatched && _board._currentFruit.tag == matchType.fruit)
			{	
				//	make it unmatched
				_board._currentFruit._isMatched= false;
				// int typeOfBomb = Random.Range(0, 100);
				// if(typeOfBomb < 50) { _board._currentFruit.MakeRowBomb(); }
				// else if(typeOfBomb >= 50) { _board._currentFruit.MakeColumnBomb(); }

				if((_board._currentFruit.swipeAngle > -45 && _board._currentFruit.swipeAngle <= 45)
					|| (_board._currentFruit.swipeAngle < -135 && _board._currentFruit.swipeAngle >= 135))
				{ _board._currentFruit.MakeRowBomb(); }
				else { _board._currentFruit.MakeColumnBomb(); }
			}
			//	is the other piece matched
			else if(_board._currentFruit._otherFruit != null) 
			{
				Fruit _otherfruit = _board._currentFruit._otherFruit.GetComponent<Fruit>();
				if(_otherfruit._isMatched && _otherfruit.tag == matchType.fruit)
				{
					_otherfruit._isMatched = false;
					// int typeOfBomb = Random.Range(0, 100);
					// if(typeOfBomb < 50) { _otherfruit.MakeRowBomb(); }
					// else if(typeOfBomb >= 50) { _otherfruit.MakeColumnBomb(); }

					if((_board._currentFruit.swipeAngle > -45 && _board._currentFruit.swipeAngle <= 45)
						|| (_board._currentFruit.swipeAngle < -135 && _board._currentFruit.swipeAngle >= 135))
					{ _otherfruit.MakeRowBomb(); }
					else { _otherfruit.MakeColumnBomb(); }
				}
			}
		}
	}
}