using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu (fileName = "World", menuName = "Level")]
public class Level : ScriptableObject
{
	[Header("Board Dimensions")]
	public int width;
	public int height;
	public float cameraDistance;

	[Header("Starting Tiles")]
	public TileType[] boardLayout;

	[Header("Available Fruits")]
	public GameObject[] fruits;

	[Header("Score Goals")]
	public int[] scoreGoals;

	[Header("End Game Requirements")]
	public EndGameRequirement endGameRequirement;
	public BlankGoal[] levelGoals;
}