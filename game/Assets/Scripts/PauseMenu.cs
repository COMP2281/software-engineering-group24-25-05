using UnityEngine;

public class PauseMenu : MonoBehaviour {
    private bool isPaused = false;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    void Resume() {
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    void Pause() {
        Time.timeScale = 0.0f;
        isPaused = true;
    }
}
