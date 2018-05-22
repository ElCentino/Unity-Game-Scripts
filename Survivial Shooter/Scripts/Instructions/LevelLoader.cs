using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    private Slider slider;

    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        StartCoroutine("LoadSceneAsync", 1);
    }

    void Update()
    {
    
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            float progress = map(operation.progress, 0, 0.9f, 0, 100);

            slider.value = progress;

            print(operation.progress);

            yield return null;
        }
    }

    float map(float source, float low, float high, float min, float max)
    {
        float percentageLow = (source - low) / 100;
        float percentageHigh = high / 100;

        float conversion = (percentageLow * (max / percentageHigh)) + min;

        return conversion;
    }
}
