using UnityEngine;
using System.Collections;

public class Test_BeatObj : MonoBehaviour
{
    private Vector3 originalScale; // ���̃T�C�Y��ۑ�
    private Coroutine scaleCoroutine; // �R���[�`�����Ǘ�

    public bool Beat01 = false;
    public bool Beat02 = false;
    public bool Beat03 = false;
    public bool Beat04 = false;

    private void OnEnable()
    {
        // ���̃T�C�Y��ۑ�
        originalScale = transform.localScale;

        CRIWARE_conductor.TempoMethodEvent1 += TempoMethod1;
        CRIWARE_conductor.TempoMethodEvent2 += TempoMethod2;
        CRIWARE_conductor.TempoMethodEvent3 += TempoMethod3;
        CRIWARE_conductor.TempoMethodEvent4 += TempoMethod4;
        CRIWARE_conductor.TempoMethodEvent5 += TempoMethod5;
        CRIWARE_conductor.TempoMethodEvent6 += TempoMethod6;
        CRIWARE_conductor.TempoMethodEvent7 += TempoMethod7;
        CRIWARE_conductor.TempoMethodEvent8 += TempoMethod8;
    }

    private void OnDisable()
    {
        CRIWARE_conductor.TempoMethodEvent1 -= TempoMethod1;
        CRIWARE_conductor.TempoMethodEvent2 -= TempoMethod2;
        CRIWARE_conductor.TempoMethodEvent3 -= TempoMethod3;
        CRIWARE_conductor.TempoMethodEvent4 -= TempoMethod4;
        CRIWARE_conductor.TempoMethodEvent5 -= TempoMethod5;
        CRIWARE_conductor.TempoMethodEvent6 -= TempoMethod6;
        CRIWARE_conductor.TempoMethodEvent7 -= TempoMethod7;
        CRIWARE_conductor.TempoMethodEvent8 -= TempoMethod8;
    }

    private void TempoMethod1()
    {
        if(!Beat01)
            return;

        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine); // �r���ŌĂ΂ꂽ�ꍇ�͑O�̃R���[�`�����~
        }
        scaleCoroutine = StartCoroutine(ShrinkAndRestore());
    }
    private void TempoMethod2()
    {
        if (!Beat02)
            return;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine); // �r���ŌĂ΂ꂽ�ꍇ�͑O�̃R���[�`�����~
        }
        scaleCoroutine = StartCoroutine(ShrinkAndRestore());
    }
    private void TempoMethod3()
    {
        if (!Beat03)
            return;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine); // �r���ŌĂ΂ꂽ�ꍇ�͑O�̃R���[�`�����~
        }
        scaleCoroutine = StartCoroutine(ShrinkAndRestore());
    }
    private void TempoMethod4()
    {
        if (!Beat04)
            return;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine); // �r���ŌĂ΂ꂽ�ꍇ�͑O�̃R���[�`�����~
        }
        scaleCoroutine = StartCoroutine(ShrinkAndRestore());
    }
    private void TempoMethod5()
    {
        if (!Beat01)
            return;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine); // �r���ŌĂ΂ꂽ�ꍇ�͑O�̃R���[�`�����~
        }
        scaleCoroutine = StartCoroutine(ShrinkAndRestore());
    }
    private void TempoMethod6()
    {
        if (!Beat02)
            return;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine); // �r���ŌĂ΂ꂽ�ꍇ�͑O�̃R���[�`�����~
        }
        scaleCoroutine = StartCoroutine(ShrinkAndRestore());
    }
    private void TempoMethod7()
    {
        if (!Beat03)
            return;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine); // �r���ŌĂ΂ꂽ�ꍇ�͑O�̃R���[�`�����~
        }
        scaleCoroutine = StartCoroutine(ShrinkAndRestore());
    }
    private void TempoMethod8()
    {
        if (!Beat04)
            return;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine); // �r���ŌĂ΂ꂽ�ꍇ�͑O�̃R���[�`�����~
        }
        scaleCoroutine = StartCoroutine(ShrinkAndRestore());
    }


    private IEnumerator ShrinkAndRestore()
    {
        // ��u�ŏk��
        transform.localScale = originalScale * 0.5f; // 50%�̃T�C�Y�ɏk��

        // 0.2�b�����Č��̃T�C�Y�ɖ߂�
        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale * 0.5f, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale; // �Ō�Ɋm���Ɍ��̃T�C�Y�ɖ߂�
    }
}
