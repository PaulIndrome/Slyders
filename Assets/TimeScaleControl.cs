using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleControl : MonoBehaviour {

	public static IEnumerator Freeze(float duration){
		float oldTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		yield return new WaitForSecondsRealtime(duration);
		Time.timeScale = oldTimeScale;
	}

	public static IEnumerator ScaleTimeFor(float scale, float duration){
		float oldTimeScale = Time.timeScale;
		Time.timeScale = scale;
		yield return new WaitForSecondsRealtime(duration);
		Time.timeScale = oldTimeScale;
	}

}
