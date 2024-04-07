using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
	public void GoToMainMenu()
	{
		SceneManager.LoadSceneAsync("Main_Menu");
	}
}