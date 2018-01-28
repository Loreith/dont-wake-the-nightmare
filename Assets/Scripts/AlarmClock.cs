using UnityEngine;
using UnityEngine.UI;

public class AlarmClock : MonoBehaviour {

    public Text timeText;

    Vector2 currentTime = new Vector2(3, 0);
    Vector2 endTime = new Vector2(6, 0);

    float seconds5Timer = 0;
    float next5Seconds = 0;
    int increment5Seconds = 0;
	
	// Update is called once per frame
	void Update () {
        currentTime.y += Time.deltaTime;

        // Check Minutes
        if (currentTime.y >= 90) {
            currentTime.y = Mathf.Clamp(currentTime.y - 90, 0, 90);

            currentTime.x += 1;

            Vector2 difference = endTime - currentTime;

            if (difference.x <= 0) {
                Debug.Log("Times Up");
            }
        }

        Seconds5Check();
        DisplayTime();

    }

    void DisplayTime() {
        string time = "";

        if (currentTime.x < 10) 
            time += "0";
        
        time += currentTime.x.ToString() + ":";

        float newSeconds = currentTime.y * (2f / 3f);

        if (newSeconds < 10) 
            time += "0";
        
        time += Mathf.FloorToInt(newSeconds).ToString();

        timeText.text = time;
    }

    void Seconds5Check() {
        seconds5Timer += Time.deltaTime;

        if (seconds5Timer >= next5Seconds && Mathf.FloorToInt(seconds5Timer) % 5 == 0) {
            next5Seconds += 5;

            Seconds5Event();
        }
    }

    void Seconds5Event() {
        increment5Seconds++;
    }

    public void ReduceTimer(float amount) {
        currentTime.y += amount * 3f / 2f;
    }
}
