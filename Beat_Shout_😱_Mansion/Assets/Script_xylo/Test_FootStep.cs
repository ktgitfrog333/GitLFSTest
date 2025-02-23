using UnityEngine;
using System.Collections;

public class Test_FootStep : MonoBehaviour
{
    public float minInterval = 1.5f;  // 最初の足音間隔（秒）
    public float maxInterval = 0.9f;  // 最後の足音間隔（秒）
    public float accelerationTime = 4.0f;  // 何秒かけて加速するか

    private Coroutine footstepCoroutine;
    private bool isPlaying = false;

    void Update()
    {
        // Fキーが押されたとき
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartFootsteps();
        }
        // Fキーが離されたとき
        if (Input.GetKeyUp(KeyCode.F))
        {
            StopFootsteps();
        }
    }

    void StartFootsteps()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            footstepCoroutine = StartCoroutine(FootstepRoutine());
        }
    }

    void StopFootsteps()
    {
        if (isPlaying)
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
            }
            isPlaying = false;
        }
    }

    IEnumerator FootstepRoutine()
    {
        float elapsedTime = 0f;

        while (true)
        {
            // 経過時間に基づいて間隔を計算
            float t = Mathf.Clamp01(elapsedTime / accelerationTime);
            float currentInterval = Mathf.Lerp(minInterval, maxInterval, t);

            // 足音を再生
            PlayFootStep();

            // 計算された間隔だけ待機
            yield return new WaitForSeconds(currentInterval);

            elapsedTime += currentInterval;
        }
    }

    void PlayFootStep()
    {
        SE_Picker.Instance.PlayFootStep(1);
    }
}