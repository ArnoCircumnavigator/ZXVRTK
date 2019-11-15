using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    private int i = 0;
    private List<NpcWord> npcWords;
	// Use this for initialization
	void Start () {
        npcWords = ZXTK_Analysis.Instance.LoadNpcWorld(Application.dataPath + "/ZXToolkit/TextFolder/小亚.xml");

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(npcWords[i].Text);
            i++;
        }
	}
}
