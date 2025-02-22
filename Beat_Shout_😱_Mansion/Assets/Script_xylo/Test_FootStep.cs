using UnityEngine;
using System.Collections;

public class Test_FootStep : MonoBehaviour
{
    public float minInterval = 1.5f;  // �ŏ��̑����Ԋu�i�b�j
    public float maxInterval = 0.9f;  // �Ō�̑����Ԋu�i�b�j
    public float accelerationTime = 4.0f;  // ���b�����ĉ������邩

    private Coroutine footstepCoroutine;
    private bool isPlaying = false;

    void Update()
    {
        // F�L�[�������ꂽ�Ƃ�
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartFootsteps();
        }
        // F�L�[�������ꂽ�Ƃ�
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
            // �o�ߎ��ԂɊ�Â��ĊԊu���v�Z
            float t = Mathf.Clamp01(elapsedTime / accelerationTime);
            float currentInterval = Mathf.Lerp(minInterval, maxInterval, t);

            // �������Đ�
            PlayFootStep();

            // �v�Z���ꂽ�Ԋu�����ҋ@
            yield return new WaitForSeconds(currentInterval);

            elapsedTime += currentInterval;
        }
    }

    void PlayFootStep()
    {
        SE_Picker.Instance.PlayFootStep(1);
    }
}