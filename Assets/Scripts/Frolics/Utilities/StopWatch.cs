using UnityEngine;
using System.Collections.Generic;

public class StopWatch {
	float seconds;

	float startTime;
	float endTime;

	public StopWatch() {
		this.startTime = 0f;
		this.endTime = 0f;
		this.seconds = 0f;
	}

	public void start() {
		startTime = Time.time;
		endTime = startTime;
	}

	public void pause() {
		endTime = Time.time;
		seconds += endTime - startTime;
		startTime = endTime;
	}

	public void togglePause(bool isPaused) {
		if (isPaused)
			pause();
		else
			start();
	}

	public void reset() { startTime = endTime = 0f; }

	public float getSeconds() {
		return seconds;
	}

	public float getMinutes() { return getSeconds() / 60f; }

	public string getFormattedTime() {
		System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(getSeconds());
		return timeSpan.ToString(@"mm\:ss");
	}
}