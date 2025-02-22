using UnityEngine;
using TMPro;
using UnityEngine.UI; // �X���C�_�[���g�p
using CriWare;
using System.Collections;

public class MicInput_Criware : MonoBehaviour
{
    public int sampleSize = 1024; // FFT��͗p�T���v���T�C�Y
    public float requiredDuration = 1.0f; // ��莞�ԉ��ʂ𒴂�������K�v����
    public float startThreshold = 0.2f; // ���ʂ������l

    public float[] volumeThresholds = new float[4]; // ���ʕ]���p��臒l
    public TextMeshProUGUI text; // ���ʕ\���p��TextMeshPro
    public Slider volumeSlider; // ���ʃQ�[�W�p�̃X���C�_�[

    private CriAtomExMic mic; // CRIWARE�̃}�C�N����
    private float volumeAccumulation = 0f; // ���ʍ��v
    private float volumeAccumulationStartTime = 0f; // ���ʌv���J�n����
    private bool isMeasuring = false; // �v�������ǂ���

    void Start()
    {
        StartCoroutine(InitializeMicrophoneWithDelay());

        // �X���C�_�[�͈̔͂�ݒ�
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f; // �ŏ��l
            volumeSlider.maxValue = 2f; // ���ʂ�1�𒴂���\�����l������2�ɐݒ�
            volumeSlider.value = 0f;
        }
    }

    private IEnumerator InitializeMicrophoneWithDelay()
    {
        yield return null; // 1�t���[���҂�

        // CRIWARE�̃}�C�N���W���[����������
        CriAtomExMic.InitializeModule();

        InitializeMicrophone();
    }

    void Update()
    {
        CheckVolume();
    }

    void InitializeMicrophone()
    {
        // �}�C�N�̃f�o�C�X�����p�\���m�F
        if (!CriAtomExMic.isInitialized)
        {
            Debug.LogError("CRIWARE�̃}�C�N���W���[��������������Ă��܂���I");
            return;
        }

        var devices = CriAtomExMic.GetDevices();
        if (devices == null || devices.Length == 0)
        {
            Debug.LogError("���p�\�ȃ}�C�N�f�o�C�X��������܂���I");
            return;
        }

        // �}�C�N�̐ݒ���쐬
        var config = CriAtomExMic.Config.Default;
        config.deviceId = devices[0].deviceId; // �ŏ��̃f�o�C�X���g�p
        config.numChannels = 1; // ���m����
        config.samplingRate = 44100;
        config.frameSize = (uint)sampleSize;

        // CRI AtomExMic �̏�����
        mic = CriAtomExMic.Create(config);

        if (mic == null)
        {
            Debug.LogError("CRIWARE�̃}�C�N���͂̏������Ɏ��s���܂����I");
            return;
        }

        mic.Start(); // �}�C�N���͂��J�n
        Debug.Log("CRIWARE�̃}�C�N���͂��J�n���܂����I");
    }

    void CheckVolume()
    {
        if (mic == null) return;

        float[] micBuffer = new float[sampleSize];
        uint samplesRead = mic.ReadData(micBuffer, (uint)sampleSize);

        if (samplesRead > 0)
        {
            float volume = CalculateRMS(micBuffer, (int)samplesRead);

            // ���ʂ��X���C�_�[�ƃe�L�X�g�ɔ��f
            if (text != null)
                text.text = $"Vol: {volume:F2}";
            if (volumeSlider != null)
                volumeSlider.value = volume; // �X���C�_�[�ɔ��f

            if (volume > startThreshold)
            {
                if (!isMeasuring)
                {
                    isMeasuring = true;
                    volumeAccumulation = 0f;
                    volumeAccumulationStartTime = Time.time;
                }

                volumeAccumulation += volume;

                if (Time.time - volumeAccumulationStartTime >= requiredDuration)
                {
                    float averageVolume = volumeAccumulation / (Time.time - volumeAccumulationStartTime);
                    int volumeLevel = EvaluateVolumeLevel(averageVolume);
                    text.text = $"Vol: {averageVolume:F2} (Lv {volumeLevel})";
                    Debug.Log($"�ݒ�l�𒴂��鉹�ʂ����o: ���ω��� {averageVolume}, ���x�� {volumeLevel}");

                    isMeasuring = false;
                }
            }
            else
            {
                isMeasuring = false;
            }
        }
    }

    int EvaluateVolumeLevel(float averageVolume)
    {
        for (int i = 0; i < volumeThresholds.Length; i++)
        {
            if (averageVolume <= volumeThresholds[i])
            {
                return i + 1; // 1�`4�̕]��
            }
        }
        return volumeThresholds.Length; // �ő�l�𒴂����ꍇ�͍ō��]��
    }

    float CalculateRMS(float[] samples, int length)
    {
        float sum = 0f;
        for (int i = 0; i < length; i++)
        {
            sum += samples[i] * samples[i];
        }
        return Mathf.Sqrt(sum / length);
    }

    void OnDestroy()
    {
        if (mic != null)
        {
            mic.Stop();
            mic.Dispose();
            mic = null;
        }

        // CRIWARE�̃}�C�N���W���[�����I��
        CriAtomExMic.FinalizeModule();
    }
}
