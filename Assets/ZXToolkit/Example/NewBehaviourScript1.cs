using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewBehaviourScript1 : MonoBehaviour {

    public Image image;
    /// <summary>
    /// 周期（默认每2秒呼吸一次）
    /// </summary>
    public float Cycle = 2f;
    private float a;
    private Color color;
    private float time;
    // Use this for initialization
    void Start () {
        color = image.color;
        time = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Q))
        {
            color = image.color;
            time += Time.deltaTime;
            var Alpha = Mathf.Abs(Mathf.Cos((1 / Cycle) * time));
            color.a = Alpha;
            image.color = color;
        }
    }
}
