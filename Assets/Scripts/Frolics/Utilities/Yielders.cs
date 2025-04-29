using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Yielders {

	static Dictionary<float, WaitForSeconds> waitTimes = new Dictionary<float, WaitForSeconds>();
	static Dictionary<float, WaitForSecondsRealtime> waitTimesReal = new Dictionary<float, WaitForSecondsRealtime>();

	static WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
	public static WaitForEndOfFrame waitForEndOfFrame { get { return endOfFrame; } }

	static WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
	public static WaitForFixedUpdate waitForFixedUpdate { get { return fixedUpdate; } }

	public static WaitForSeconds waitForSeconds(float seconds) {
		if (!waitTimes.ContainsKey(seconds))
			waitTimes.Add(seconds, new WaitForSeconds(seconds));
		
		return waitTimes[seconds];
	}
	
	public static WaitForSecondsRealtime waitForSecondsRealtime(float seconds) {
		if (!waitTimesReal.ContainsKey(seconds))
			waitTimesReal.Add(seconds, new WaitForSecondsRealtime(seconds));
		
		return waitTimesReal[seconds];
	}

}