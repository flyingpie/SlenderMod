using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 200, 20), "Load"))
        {
            Application.LoadLevelAsync("Death");
        }
    }
}
