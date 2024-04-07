using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public GameObject hintParticlePrefab; // Reference to the hint particle prefab
    public GameObject currentHint; // The currently active hint particle
    private Board board; // Reference to the game board
    public float hintDelay; // Delay between hints in seconds
    private float hintDelaySeconds;

     private void Start()
    {
        board = FindObjectOfType<Board>();
        hintDelaySeconds = hintDelay;
    }

    private void Update()
    {
        hintDelaySeconds -= Time.deltaTime;
        if (hintDelaySeconds <= 0 && currentHint == null)
        {
            MarkHint();
            hintDelaySeconds = hintDelay;
        }
    }

    private List<GameObject> FindAllMatches()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        for (int i = 0; i < board._width; i++)
        {
            for (int j = 0; j < board._height; j++)
            {
                if (board.allFruits[i, j] != null)
                {
                    if (i < board._width - 1 && board.SwitchAndCheck(i, j, Vector2.right))
                    {
                        possibleMoves.Add(board.allFruits[i, j]);
                    }
                    if (j < board._height - 1 && board.SwitchAndCheck(i, j, Vector2.up))
                    {
                        possibleMoves.Add(board.allFruits[i, j]);
                    }
                }
            }
        }
        return possibleMoves;
    }

    private GameObject PickOneRandomly()
    {
        List<GameObject> possibleMoves = FindAllMatches();
        if (possibleMoves.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleMoves.Count); // Corrected index range
            return possibleMoves[randomIndex];
        }
        return null;
    }

    private void MarkHint()
    {
        GameObject move = PickOneRandomly();
        if (move != null)
        {
            currentHint = Instantiate(hintParticlePrefab, move.transform.position, Quaternion.identity);
            currentHint.GetComponent<ParticleSystem>().Play();
        }
    }

    public void DestroyHint()
    {
        if (currentHint != null)
        {
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;
        }
    }
}