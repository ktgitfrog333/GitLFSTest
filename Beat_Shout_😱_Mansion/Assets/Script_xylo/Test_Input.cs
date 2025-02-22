using UnityEngine;
using System.Collections;

public class Test_Input : MonoBehaviour
{
    public GameObject targetCube;
    private Material cubeMaterial;
    private Color originalColor;
    private Coroutine colorChangeCoroutine;

    // �N�[���_�E���֘A�̕ϐ���ǉ�
    private float cooldownDuration = 0.2f; // �N�[���_�E�����ԁi�b�j
    private float lastInputTime = -1f;     // �Ō�̓��͎���

    private void Start()
    {
        if (targetCube != null)
        {
            Renderer renderer = targetCube.GetComponent<Renderer>();
            if (renderer != null)
            {
                cubeMaterial = renderer.material;
                originalColor = cubeMaterial.color;
            }
        }
    }

    private void Update()
    {
        // �X�y�[�X�L�[�������ꂽ���̂ݔ�����s��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �N�[���_�E�����łȂ����`�F�b�N
            if (Time.time - lastInputTime >= cooldownDuration)
            {
                CheckBeatAndChangeColor();
                lastInputTime = Time.time; // ���͎��Ԃ��X�V
            }
        }
    }

    private void CheckBeatAndChangeColor()
    {
        CRIWARE_conductor.BeatResult result = CRIWARE_conductor.Instance.JustBeatTick();
        if (result != CRIWARE_conductor.BeatResult.Miss)
        {
            ChangeColor(Color.green * 0.8f); // ���邢�ΐF
        }
        else
        {
            ChangeColor(Color.red); // �ԐF
        }
    }

    private void ChangeColor(Color newColor)
    {
        if (cubeMaterial != null)
        {
            if (colorChangeCoroutine != null)
            {
                StopCoroutine(colorChangeCoroutine);
            }
            colorChangeCoroutine = StartCoroutine(LerpColor(newColor));
        }
    }

    private IEnumerator LerpColor(Color targetColor)
    {
        float elapsedTime = 0f;
        float duration = 0.3f;
        Color startColor = cubeMaterial.color;

        cubeMaterial.color = targetColor;
        yield return null;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            cubeMaterial.color = Color.Lerp(targetColor, originalColor, t);
            yield return null;
        }

        cubeMaterial.color = originalColor;
        colorChangeCoroutine = null;
    }
}