using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BTManager : MonoBehaviour {

    public static BTManager Instance;

    private bool audioConfig, playerConfig;
    
    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void SetConfigAudio(bool state) {
        audioConfig = state;
        TryStart();
    }

    public void SetConfigPlayer(bool state) {
        playerConfig = state;
        TryStart();
    }

    private void TryStart() {
        SetupUI.Instance.start.interactable = audioConfig && playerConfig;
    }

    public void LoadScene(string sceneName) {
        StopAllCoroutines();
        StartCoroutine(LoadingScene(sceneName));
    }
    
    private IEnumerator LoadingScene(string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        if (sceneName.Equals("Game")) yield return StartCoroutine(AudioManager.Instance.LoadSong());
        while (!operation.isDone) {
            yield return null;
        }

        if (sceneName.Equals("Game")) AudioManager.Instance.PlayNextSong();
    }
}
