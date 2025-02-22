using System.Collections;
using System.Collections.Generic;
using CriWare;
using UnityEngine;

public class CRIWARE_AisacChange : MonoBehaviour
{
    public static CRIWARE_AisacChange Instance { get; private set; } // �V���O���g����ݒ�

    private CriAtomSource currentSource;
    private CriAtomExPlayer atomExPlayer;
    private bool ChangeLock = false;
    //�c�̑J�ڂ͂S�p�^�[���܂œo�^�\�B�K�v�ɉ����Ēǉ�����
    public string AisacControl_00;
    public string AisacControl_01;
    public string AisacControl_02;
    public string AisacControl_03;

    [HideInInspector] public bool OnNoBgm = false; // BGM�ύX�����b�N���邩�ǂ���

    // AISAC�R���g���[���̕ύX�ɂ����鎞�Ԃ��C���X�y�N�^�[����ݒ�\�ɂ���
    public float transitionDuration = 3.0f;
    public float transitionDuration2 = 0.05f;

    // AISAC�R���g���[���̌��ݒl��ǐՂ��邽�߂̕ϐ�
    private float currentAisacValue_00 = 0.9f; // �����l
    private float currentAisacValue_01 = 0.0f; // �����l
    private float currentAisacValue_02 = 0.0f; // �����l
    private float currentAisacValue_03 = 0.0f; // �����l
    public bool isAutoStart = false;
    private Dictionary<string, Coroutine> currentCoroutines = new Dictionary<string, Coroutine>();

    // �C���X�y�N�^�[����ݒ肷��BPM�ƊJ�n��
    public float bpm = 120.0f;
    public int startBeat = 0; // �����l�̓[�����ڂ���

    void Start()
    {
        // �V���O���g���̏���
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ��ڂ̃C���X�^���X���쐬���ꂽ�ꍇ�͔j��
        }
    }

    public void AutoStart()
    {
        if (isAutoStart)
        {
            // BGM��SE�̉��ʂ�Data�N���X����擾���Đݒ肷�郁�\�b�h�������ɒǋL
            //    float bgmVolume = 
            //    float seVolume = 

            // �擾�������ʂ�CriAtom�̃J�e�S���ɐݒ�
            //   CriAtom.SetCategoryVolume("BGM", bgmVolume);
            //   CriAtom.SetCategoryVolume("SE", seVolume);


            float startTimeInSeconds = BeatToSeconds(startBeat, bpm);
            atomExPlayer.SetStartTime((long)(startTimeInSeconds * 1000)); // �~���b�P�ʂŐݒ�
            BGM0();
            currentSource.Play();
        }

        if (OnNoBgm)
        {
            ChangeAisacControl(AisacControl_00, 0.0f, transitionDuration2);
            ChangeAisacControl(AisacControl_01, 0.0f, transitionDuration2);
            ChangeAisacControl(AisacControl_02, 0.0f, transitionDuration2);
            ChangeAisacControl(AisacControl_03, 0.0f, transitionDuration2);
        }
    }

    public void SetSource(CriAtomSource atomSource)
    {
        currentSource = atomSource;
        atomExPlayer = currentSource.player; // CriAtomExPlayer ���擾
    }

    public void PlayStart()
    {
        float startTimeInSeconds = BeatToSeconds(startBeat, bpm);
        atomExPlayer.SetStartTime((long)(startTimeInSeconds * 1000)); // �~���b�P�ʂŐݒ�
        currentSource.Play();
        Debug.Log("�Đ��J�n");
    }

    private void OnDisable()
    {
        // �V�[���J�ڎ��ɉ������~
        if (currentSource != null)
        {
            currentSource.Stop();
        }
    }

    public void PauseOn()
    //�ꎞ��~���̓r�[�g�������~������BBGM�ł͂Ȃ��P��SE�Ƃ����`�ŉ��y���Đ�����
    //CRIWARE���̐ݒ�ŃJ�e�S����BGM�Ƃ��Ă����΁A���ʒ�����BGM�̂��̂��K�p�����
    {
        if (currentSource != null)
        {
            currentSource.Pause(true);
        }
    }

    public void PauseOff()
    {
        if (currentSource != null)
        {
            currentSource.Pause(false);
        }
    }

    public void ApplyAisac(CriAtomSource source)
    {
        currentSource = source;

        if (currentSource == null) return;

        ChangeAisacControl(AisacControl_00, currentAisacValue_00, transitionDuration * 2);
        ChangeAisacControl(AisacControl_01, currentAisacValue_01, transitionDuration * 2);
        ChangeAisacControl(AisacControl_02, currentAisacValue_02, transitionDuration * 2);
        ChangeAisacControl(AisacControl_03, currentAisacValue_03, transitionDuration * 2);
    }

    //����������Aisac�̕ύX�ƕύX�b���Ɋւ��郁�\�b�h�B�K�v�ɉ����č쐬����
    public void BGM0()�@//�������BGM_A��
    {
        if (ChangeLock || OnNoBgm) return;
        ChangeAisacControlIfExists(AisacControl_00, 0.9f, transitionDuration * 2);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, transitionDuration * 2);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, transitionDuration * 2);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, transitionDuration * 2);
    }

    public void BGM0Now()�@//�u����BGM_A��
    {
        if (ChangeLock || OnNoBgm) return;
        ChangeAisacControlIfExists(AisacControl_00, 0.9f, 0.1f);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, 0.1f);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, 0.1f);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, 0.1f);
    }

    public void BGM1()
    {
        if (ChangeLock || OnNoBgm) return;
        ChangeAisacControlIfExists(AisacControl_00, 0.0f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_01, 0.9f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, transitionDuration);
    }

    public void BGM1Now()
    {
        if (ChangeLock || OnNoBgm) return;
        ChangeAisacControlIfExists(AisacControl_00, 0.0f, 0.1f);
        ChangeAisacControlIfExists(AisacControl_01, 0.9f, 0.1f);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, 0.1f);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, 0.1f);
    }

    public void BGM2()
    {
        if (ChangeLock || OnNoBgm) return;
        ChangeAisacControlIfExists(AisacControl_00, 0.0f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_02, 0.9f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, transitionDuration2);
    }

    public void BGM3()
    {
        if (ChangeLock || OnNoBgm) return;
        ChangeAisacControlIfExists(AisacControl_00, 0.0f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_03, 0.9f, transitionDuration);
    }

    public void BGM4()
    {
        if (OnNoBgm) return;
        ChangeLock = true; // BGM�ύX�����b�N
        ChangeAisacControlIfExists(AisacControl_00, 0.0f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, transitionDuration2);
    }

    public void BGM5()
    {
        if (OnNoBgm) return;
        ChangeLock = false; // BGM�ύX���b�N������
        ChangeAisacControlIfExists(AisacControl_00, 0.9f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, transitionDuration2);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, transitionDuration2);
    }

    public void BGM6()
    {
        if (OnNoBgm) return;
        ChangeLock = false; // BGM�ύX���b�N������
        ChangeAisacControlIfExists(AisacControl_00, 0.9f, transitionDuration / 2);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, transitionDuration / 2);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, transitionDuration / 2);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, transitionDuration / 2);
    }

    public void BGM7() // 1�b�����ăt�F�[�h�A�E�g
    {
        if (OnNoBgm) return;
        ChangeLock = false; // BGM�ύX���b�N������
        ChangeAisacControlIfExists(AisacControl_00, 0.0f, transitionDuration / 3);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, transitionDuration / 3);
        ChangeAisacControlIfExists(AisacControl_02, 0.0f, transitionDuration / 3);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, transitionDuration / 3);
    }

    public void BGM8() // 1�b�����ăt�F�[�h�A�E�g
    {
        if (OnNoBgm) return;
        ChangeLock = true; // BGM�ύX���b�N
        ChangeAisacControlIfExists(AisacControl_00, 0.0f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_01, 0.0f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_02, 0.9f, transitionDuration);
        ChangeAisacControlIfExists(AisacControl_03, 0.0f, transitionDuration);
    }

    private bool HasAisacControl(string aisacName)
    {
        try
        {
            // �ꎞ�I��AISAC�R���g���[����ݒ肵�悤�Ƃ���
            atomExPlayer.SetAisacControl(aisacName, 0.0f);
            return true;
        }
        catch (System.Exception)
        {
            // ���s�����ꍇ��AISAC�R���g���[�������݂��Ȃ��Ɣ��f
            return false;
        }
    }

    private void ChangeAisacControlIfExists(string aisacName, float targetValue, float Setduration)
    {
        if (HasAisacControl(aisacName))
        {
            ChangeAisacControl(aisacName, targetValue, Setduration);
        }
        else
        {
            Debug.LogWarning("�w�肳�ꂽAISAC�����݂��܂���: " + aisacName);
        }
    }

    private void ChangeAisacControl(string aisacName, float targetValue, float Setduration)
    {
        // ���Ɏ��s���̃R���[�`��������Β�~
        if (currentCoroutines.TryGetValue(aisacName, out Coroutine runningCoroutine))
        {
            StopCoroutine(runningCoroutine);
            currentCoroutines.Remove(aisacName);
        }

        Coroutine newCoroutine = StartCoroutine(ChangeAisacControlValue(aisacName, targetValue, Setduration));
        currentCoroutines[aisacName] = newCoroutine;
    }

    IEnumerator ChangeAisacControlValue(string aisacName, float targetValue, float Setduration)
    {
        float startTime = Time.time;
        float startValue = GetCurrentAisacValue(aisacName);
        while (Time.time - startTime < Setduration)
        {
            float t = (Time.time - startTime) / Setduration;
            float newValue = Mathf.Lerp(startValue, targetValue, t);
            currentSource.SetAisacControl(aisacName, newValue);
            SetCurrentAisacValue(aisacName, newValue);
            yield return null;
        }
        currentSource.SetAisacControl(aisacName, targetValue);
        SetCurrentAisacValue(aisacName, targetValue);

        // �R���[�`���̎��s�����������玫������폜
        if (currentCoroutines.ContainsKey(aisacName))
        {
            currentCoroutines.Remove(aisacName);
        }
    }

    private float GetCurrentAisacValue(string aisacName)
    {
        switch (aisacName)
        {
            case "AisacControl_00": return currentAisacValue_00;
            case "AisacControl_01": return currentAisacValue_01;
            case "AisacControl_02": return currentAisacValue_02;
            case "AisacControl_03": return currentAisacValue_03;
            default: return 0.0f; // �s����AISAC���̏ꍇ��0.0f��Ԃ�
        }
    }

    private void SetCurrentAisacValue(string aisacName, float newValue)
    {
        switch (aisacName)
        {
            case "AisacControl_00": currentAisacValue_00 = newValue; break;
            case "AisacControl_01": currentAisacValue_01 = newValue; break;
            case "AisacControl_02": currentAisacValue_02 = newValue; break;
            case "AisacControl_03": currentAisacValue_03 = newValue; break;
        }
    }

    // ��������b���ւ̕ϊ����\�b�h
    private float BeatToSeconds(int beat, float bpm)
    {
        return (beat * 60.0f) / bpm;
    }

    // transitionDuration�̃Z�b�^�[
    public void SetTransitionDuration(float duration)
    {
        transitionDuration = duration;
    }
}
