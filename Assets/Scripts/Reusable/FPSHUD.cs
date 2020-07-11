using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Attach this to a Text to make a frames/second indicator.
///
/// It calculates frames/second over each updateInterval, so the display does not keep changing wildly.
///
/// It is also fairly accurate at very low FPS counts (<10).
/// We do this not by simply counting frames per interval, but by accumulating FPS for each frame. This way we end up with
/// correct overall FPS even if the interval renders something like 5.5 frames.
/// 
/// Updated to new Unity UI.
/// Version 1.2
/// </summary>
[RequireComponent(typeof(Text))]
public class FPSHUD : MonoBehaviour {
    [System.Serializable]
    public class MinValueColorPair {
        public float fps;
        public Color color;

        public MinValueColorPair(){}

        public MinValueColorPair(float fps, Color color){
            this.fps = fps;
            this.color = color;
        }
    }

	public  float updateInterval = 0.5f;
    public string stringFormat = "{0:F2} FPS";
    public MinValueColorPair[] colorRanges = new[] {
        new MinValueColorPair(0, Color.red), new MinValueColorPair(10, Color.yellow),  new MinValueColorPair(20, Color.green)
    };

    private Text  text;
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval

	void Start(){
		text = GetComponent<Text>();
        colorRanges = colorRanges.OrderBy(cr => cr.fps).ToArray();
        if ( !text ){
	        Debug.Log("UtilityFramesPerSecond needs a Text component!");
	        enabled = false;
	        return;
	    }
	    timeleft = updateInterval;
	}

	void Update(){
	    timeleft -= Time.deltaTime;
	    accum += Time.timeScale/Time.deltaTime;
	    ++frames;

	    // Interval ended - update GUI text and start new interval
	    if( timeleft <= 0.0 ){
		    float fps = accum/frames;
		    string format = System.String.Format(stringFormat, fps);
			text.text = format;
            if(colorRanges.Length > 0)
                text.color = GetColorPerFPS(fps);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
	}

    Color GetColorPerFPS(float fps) {
        for (int i = 0; i < colorRanges.Length; i++) {
            if (fps < colorRanges[i].fps) {
                if (i != 0)
                    return colorRanges[i-1].color;
            }
        }
        return colorRanges[colorRanges.Length - 1].color;
    }
}