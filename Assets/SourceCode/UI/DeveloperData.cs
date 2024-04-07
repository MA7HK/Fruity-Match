using TMPro;
using UnityEngine;

public class DeveloperData : MonoBehaviour
{
	public TMP_Text versionText;

    void Start()
    {
        versionText.text = "Version: " + Application.version;
    }
}