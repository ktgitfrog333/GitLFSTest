using System;
using System.Collections;
using System.Collections.Generic;
using CriWare;
using UnityEngine;
using static CriWare.CriAtomExBeatSync;

public class CRIWARE_conductor : MonoBehaviour
{
    public int ThisStageNumber = 0;

    [HideInInspector] public static CRIWARE_conductor Instance { get; private set; }//�V���O���g����ݒ�

    public CriAtomSource atomSourceA;//BGM�̉���A
    public CriAtomSource atomSourceB;//BGM�̉���B
    public CriAtomSource atomSourceC;//BGM�̉���C ���l�̌`�ő��₷���Ƃ��\

    [HideInInspector] public CriAtomSource currentSource;
    private bool isInitializedAfterChange = false; // �y�ȕύX��ɏ��������K�v���ǂ����̃t���O

    [HideInInspector] public float lastBeatSyncTime = -1f; // �Ō�Ƀr�[�g�����C�x���g��������������

    private float lastBeatSyncTime01 = -1f; // �Ō�Ɉꔏ�ړ����C�x���g��������������
    private float lastBeatSyncTime02 = -1f; // �Ō�Ɉꔏ�ړ����C�x���g��������������
    private float lastBeatSyncTime03 = -1f; // �Ō�Ɉꔏ�ړ����C�x���g��������������
    private float lastBeatSyncTime04 = -1f; // �Ō�Ɉꔏ�ړ����C�x���g��������������


    public float BeatFuzzy = 20f; // �r�[�g�̃Y�����e�͈�
    public float BeatOffSet = 0.0f; // �r�[�g�̃Y���̃I�t�Z�b�g�B�}�C�i�X�l�O��B�b���̎���
    private float beatFuzzySet; // �r�[�g�̃Y�����e�͈͂̌���l
    [HideInInspector] public float BasicBeat; // �S�������̕b��
    [HideInInspector] public float frameRate;  // �t���[�����[�g��ێ�����v���p�e�B

    public static event Action TempoSet;

    //����
    public static event Action TempoMethodEvent1;
    public static event Action TempoMethodEvent2;
    public static event Action TempoMethodEvent3;
    public static event Action TempoMethodEvent4;
    public static event Action TempoMethodEvent5;
    public static event Action TempoMethodEvent6;
    public static event Action TempoMethodEvent7;
    public static event Action TempoMethodEvent8;

    //�����̂P�t���[����B���U�����p
    public static event Action TempoMethodDelay1_1;
    public static event Action TempoMethodDelay1_2;
    public static event Action TempoMethodDelay1_3;
    public static event Action TempoMethodDelay1_4;
    public static event Action TempoMethodDelay1_5;
    public static event Action TempoMethodDelay1_6;
    public static event Action TempoMethodDelay1_7;
    public static event Action TempoMethodDelay1_8;

    //�����̂Q�t���[����B���U�����p
    public static event Action TempoMethodDelay2_1;
    public static event Action TempoMethodDelay2_2;
    public static event Action TempoMethodDelay2_3;
    public static event Action TempoMethodDelay2_4;
    public static event Action TempoMethodDelay2_5;
    public static event Action TempoMethodDelay2_6;
    public static event Action TempoMethodDelay2_7;
    public static event Action TempoMethodDelay2_8;

    //16�r�[�g�̏����p
    public static event Action TempoMethod16Beat2;
    public static event Action TempoMethod16Beat3;
    public static event Action TempoMethod16Beat4;

    private Coroutine invoke16BeatCoroutine; // �R���[�`���̎Q�Ƃ�ێ�

    //BGM�؂�ւ��p��bool�X�C�b�`
    [HideInInspector] public bool BGM_A_Sw = false;
    [HideInInspector] public bool BGM_B_Sw = false;
    [HideInInspector] public bool BGM_C_Sw = false;

    public enum BeatResult
    {
        Tick01,//1���ڂɍ��v����
        Tick02,//2���ڂɍ��v����
        Tick03,//3���ڂɍ��v����
        Tick04,//4���ڂɍ��v����
        Miss // �����𖞂����Ȃ������ꍇ
    }

    void OnEnable() //�������҂��ŏ����x�点��B�������������擾������@������ΕύX����
    {
        Invoke("Delay", 0.3f);
    }

    private void Delay()
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

        atomSourceA = GetComponent<CriAtomSource>();
        if (atomSourceA.player != null)
        {
            currentSource = atomSourceA; // ������Ԃ�A���g�p
            CRIWARE_AisacChange.Instance.SetSource(currentSource); // AISAC�̓K�p���ݒ�
            atomSourceA.player.OnBeatSyncCallback += OnBeatSync; // �r�[�g�����C�x���g�̃R�[���o�b�N��o�^
            CRIWARE_AisacChange.Instance.PlayStart(); //�X�^�[�g��Aisac���Đ�
        }
        else
        {
            Invoke("InitDelay", 0.05f); //���������͋C�ŏ�����҂��Ă���B���f�[�^�ǂݍ��݂ƘA�g�𐮂���K�v����
        }
    }

    private void InitDelay()
    {
        if (atomSourceA.player != null)
        {
            currentSource = atomSourceA; // ������Ԃ�A���g�p
            CRIWARE_AisacChange.Instance.SetSource(currentSource); // AISAC�̓K�p���ݒ�
            atomSourceA.player.OnBeatSyncCallback += OnBeatSync; // �r�[�g�����C�x���g�̃R�[���o�b�N��o�^
            Debug.Log("����������");
            CRIWARE_AisacChange.Instance.PlayStart(); //�X�^�[�g��Aisac���Đ�

        }
        else
        {
            Invoke("InitDelay", 0.05f);
            Debug.Log("�������҂�");
        }
    }

    // ���̃I�u�W�F�N�g�̔j�󎞂Ƀr�[�g�����C�x���g�̃R�[���o�b�N���폜
    private void OnDisable()
    {
        currentSource.player.OnBeatSyncCallback -= OnBeatSync;

        if (invoke16BeatCoroutine != null)
        {
            StopCoroutine(invoke16BeatCoroutine);
            invoke16BeatCoroutine = null; // �Q�Ƃ��N���A
        }

    }

    public void ChangeBgmA(int TickNumber) //�ʏ�BGM�؂�ւ���p���\�b�h
    {
        if (currentSource == atomSourceA)
        {
            return; // ����BGM�̏ꍇ�͉������Ȃ�
        }
        BGM_A_Sw = true;
        BGM_B_Sw = false;
        BGM_C_Sw = false;

        currentSource.Stop(); // ���݂�BGM���~

        // �V����BGM��ݒ�
        currentSource = atomSourceA; // �V����������ݒ�

        // �Đ��J�n�ʒu��ݒ�
        SetStartTimeByTick(TickNumber);

        // �V����BGM���Đ�
        currentSource.Play();
        isInitializedAfterChange = true; // �y�ȕύX��ɏ��������邽�߂̃t���O��ݒ�
        CRIWARE_AisacChange.Instance.ApplyAisac(currentSource);  // ���݂�BGM�\�[�X��AISAC��K�p
        CRIWARE_AisacChange.Instance.BGM0(); // AISAC��BGM�O�ɂ���

        // �e���|���̍X�V
        if (currentSource.player != null)
        {
            currentSource.player.OnBeatSyncCallback -= OnBeatSync; // �����̃R�[���o�b�N������
            currentSource.player.OnBeatSyncCallback += OnBeatSync; // �V�����R�[���o�b�N��o�^
        }
    }

    public void ChangeBgmB(int TickNumber) //�X�L���Q�b�g�pBGM�؂�ւ���p���\�b�h
    {
        if (currentSource == atomSourceB)
        {
            return; // ����BGM�̏ꍇ�͉������Ȃ�
        }
        BGM_A_Sw = false;
        BGM_B_Sw = true;
        BGM_C_Sw = false;

        currentSource.Stop(); // ���݂�BGM���~

        // �V����BGM��ݒ�
        currentSource = atomSourceB; // �V����������ݒ�

        // �Đ��J�n�ʒu��ݒ�
        SetStartTimeByTick(TickNumber);

        // �V����BGM���Đ�
        currentSource.Play();
        isInitializedAfterChange = true; // �y�ȕύX��ɏ��������邽�߂̃t���O��ݒ�
        CRIWARE_AisacChange.Instance.ApplyAisac(currentSource);  // ���݂�BGM�\�[�X��AISAC��K�p
        CRIWARE_AisacChange.Instance.BGM0(); // AISAC��BGM�O�ɂ���

        // �e���|���̍X�V
        if (currentSource.player != null)
        {
            currentSource.player.OnBeatSyncCallback -= OnBeatSync; // �����̃R�[���o�b�N������
            currentSource.player.OnBeatSyncCallback += OnBeatSync; // �V�����R�[���o�b�N��o�^
        }

    }

    public void ChangeBgmC(int TickNumber) //�X�L���Q�b�g�pBGM�؂�ւ���p���\�b�h
    {
        if (currentSource == atomSourceC)
        {
            return; // ����BGM�̏ꍇ�͉������Ȃ�
        }
        BGM_A_Sw = false;
        BGM_B_Sw = false;
        BGM_C_Sw = true;

        currentSource.Stop(); // ���݂�BGM���~

        // �V����BGM��ݒ�
        currentSource = atomSourceC; // �V����������ݒ�

        // �Đ��J�n�ʒu��ݒ�
        SetStartTimeByTick(TickNumber);

        // �V����BGM���Đ�
        currentSource.Play();
        isInitializedAfterChange = true; // �y�ȕύX��ɏ��������邽�߂̃t���O��ݒ�
        CRIWARE_AisacChange.Instance.ApplyAisac(currentSource);  // ���݂�BGM�\�[�X��AISAC��K�p
        CRIWARE_AisacChange.Instance.BGM0(); // AISAC��BGM�O�ɂ���

        // �e���|���̍X�V
        if (currentSource.player != null)
        {
            currentSource.player.OnBeatSyncCallback -= OnBeatSync; // �����̃R�[���o�b�N������
            currentSource.player.OnBeatSyncCallback += OnBeatSync; // �V�����R�[���o�b�N��o�^
        }
    }





    private void SetStartTimeByTick(int TickNumber)
    {
        if (currentSource == null) return;

        // BPM����1���̒������v�Z
        float beatLengthInSeconds = 60f / frameRate;

        // TickNumber����Đ��J�n�ʒu���v�Z
        float startTimeInSeconds = beatLengthInSeconds * TickNumber;

        // �Đ��J�n�ʒu��ݒ�
        currentSource.player.SetStartTime((long)(startTimeInSeconds * 1000)); // �~���b�P�ʂŐݒ�
    }


    private void OnBeatSync(ref CriAtomExBeatSync.Info info)
    {
        lastBeatSyncTime = Time.time; // �r�[�g�����C�x���g�̎������X�V

        if ((info.barCount == 1 && info.beatCount == 0) || isInitializedAfterChange)
        {
            // ����������
            beatFuzzySet = BeatFuzzy / info.bpm; // �r�[�g�̃Y�����e�͈͂̌���l���v�Z
            BasicBeat = 60 / info.bpm;   // �S�������̕b�����v�Z
            frameRate = info.bpm; //�Đ����x�����̈�BPM�̏������̂܂ܓn���BBPMxx/1BPM120*xx%�ő��x����
            TempoSet?.Invoke(); //���X�N���v�g�Ƀe���|���𑗂�

            // ���������I������̂Ńt���O�����Z�b�g
            isInitializedAfterChange = false;
        }


        //�������甏���𑗐M����ׂ̃C�x���g

        //info.barCount�������ŁA�Ȃ�����info.beatCount��1�̎��Ɏ��s �C���g���̈ꏬ�߂Ŋ�������t�ɂȂ�
        if (info.barCount % 2 == 1 && info.beatCount == 0)
        {
            TempoMethodEvent1?.Invoke();

            lastBeatSyncTime01 = Time.time; // �r�[�g�����C�x���g�̎������X�V
            StartCoroutine(InvokeTempoMethodDelay1NextFrame()); // 1�t���[����ɔ���
        }
        else if (info.barCount % 2 == 1 && info.beatCount == 1)
        {
            TempoMethodEvent2?.Invoke();

            lastBeatSyncTime02 = Time.time; // �r�[�g�����C�x���g�̎������X�V
            StartCoroutine(InvokeTempoMethodDelay2NextFrame()); // 1�t���[����ɔ���
        }
        else if (info.barCount % 2 == 1 && info.beatCount == 2)//�X���[���[�h�̎��͎��s���Ȃ�
        {
            TempoMethodEvent3?.Invoke();

            lastBeatSyncTime03 = Time.time; // �r�[�g�����C�x���g�̎������X�V
            StartCoroutine(InvokeTempoMethodDelay3NextFrame()); // 1�t���[����ɔ���
        }
        else if (info.barCount % 2 == 1 && info.beatCount == 3)
        {
            TempoMethodEvent4?.Invoke();

            lastBeatSyncTime04 = Time.time; // �r�[�g�����C�x���g�̎������X�V
            StartCoroutine(InvokeTempoMethodDelay4NextFrame()); // 1�t���[����ɔ���
        }
        //info.barCount����ŁA�Ȃ�����info.beatCount��1�̎��Ɏ��s
        else if (info.barCount % 2 == 0 && info.beatCount == 0)
        {
            TempoMethodEvent5?.Invoke();

            lastBeatSyncTime01 = Time.time; // �r�[�g�����C�x���g�̎������X�V
            StartCoroutine(InvokeTempoMethodDelay5NextFrame()); // 1�t���[����ɔ���
        }

        else if (info.barCount % 2 == 0 && info.beatCount == 1)//�X���[���[�h�̎��͎��s���Ȃ�
        {
            TempoMethodEvent6?.Invoke();

            lastBeatSyncTime02 = Time.time; // �r�[�g�����C�x���g�̎������X�V
            StartCoroutine(InvokeTempoMethodDelay6NextFrame()); // 1�t���[����ɔ���
        }
        else if (info.barCount % 2 == 0 && info.beatCount == 2)//�X���[���[�h�̎��͎��s���Ȃ�)
        {
            TempoMethodEvent7?.Invoke();

            lastBeatSyncTime03 = Time.time; // �r�[�g�����C�x���g�̎������X�V
            StartCoroutine(InvokeTempoMethodDelay7NextFrame()); // 1�t���[����ɔ���
        }
        else if (info.barCount % 2 == 0 && info.beatCount == 3)
        {
            TempoMethodEvent8?.Invoke();

            lastBeatSyncTime04 = Time.time; // �r�[�g�����C�x���g�̎������X�V
            StartCoroutine(InvokeTempoMethodDelay8NextFrame()); // 1�t���[����ɔ���
        }


        // �R���[�`�����J�n����O�ɁA�ȑO�̃R���[�`�������s���ł���Β�~
        if (invoke16BeatCoroutine != null)
        {
            StopCoroutine(invoke16BeatCoroutine);
            invoke16BeatCoroutine = null;
        }
        // �R���[�`�����J�n���� 1/4, 2/4, 3/4 ����ɏ��������s
        StartCoroutine(Invoke16BeatCoroutine());

    }

    private IEnumerator Invoke16BeatCoroutine()
    {
        // 1/4����Ɏ��s
        yield return new WaitForSeconds(BasicBeat * 1 / 4);
        TempoMethod16Beat2?.Invoke();

        // 2/4����Ɏ��s
        yield return new WaitForSeconds(BasicBeat * 1 / 4); // �O���1/4�����͂��łɑҋ@�ς�
        TempoMethod16Beat3?.Invoke();

        // 3/4����Ɏ��s
        yield return new WaitForSeconds(BasicBeat * 1 / 4); // �܂������1/4���ҋ@
        TempoMethod16Beat4?.Invoke();

        // �R���[�`���I�����ɎQ�Ƃ��N���A
        invoke16BeatCoroutine = null;
    }

    private IEnumerator InvokeTempoMethodDelay1NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;
        StartCoroutine(InvokeTempoMethod2nd1NextFrame());

        TempoMethodDelay1_1?.Invoke();
    }



    private IEnumerator InvokeTempoMethodDelay2NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;
        StartCoroutine(InvokeTempoMethod2nd2NextFrame());

        TempoMethodDelay1_2?.Invoke();
    }


    private IEnumerator InvokeTempoMethodDelay3NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        StartCoroutine(InvokeTempoMethod2nd3NextFrame());

        TempoMethodDelay1_3?.Invoke();
    }
    private IEnumerator InvokeTempoMethodDelay4NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;
        StartCoroutine(InvokeTempoMethod2nd4NextFrame());

        TempoMethodDelay1_4?.Invoke();
    }
    private IEnumerator InvokeTempoMethodDelay5NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;
        StartCoroutine(InvokeTempoMethod2nd5NextFrame());

        TempoMethodDelay1_5?.Invoke();
    }
    private IEnumerator InvokeTempoMethodDelay6NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;
        StartCoroutine(InvokeTempoMethod2nd6NextFrame());

        TempoMethodDelay1_6?.Invoke();
    }
    private IEnumerator InvokeTempoMethodDelay7NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;
        StartCoroutine(InvokeTempoMethod2nd7NextFrame());

        TempoMethodDelay1_7?.Invoke();

    }
    private IEnumerator InvokeTempoMethodDelay8NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;
        StartCoroutine(InvokeTempoMethod2nd8NextFrame());

        TempoMethodDelay1_8?.Invoke();
    }


    private IEnumerator InvokeTempoMethod2nd1NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        TempoMethodDelay2_1?.Invoke();
    }

    private IEnumerator InvokeTempoMethod2nd2NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        TempoMethodDelay2_2?.Invoke();
    }
    private IEnumerator InvokeTempoMethod2nd3NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        TempoMethodDelay2_3?.Invoke();
    }
    private IEnumerator InvokeTempoMethod2nd4NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        TempoMethodDelay2_4?.Invoke();
    }
    private IEnumerator InvokeTempoMethod2nd5NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        TempoMethodDelay2_5?.Invoke();
    }
    private IEnumerator InvokeTempoMethod2nd6NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        TempoMethodDelay2_6?.Invoke();
    }
    private IEnumerator InvokeTempoMethod2nd7NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        TempoMethodDelay2_7?.Invoke();
    }
    private IEnumerator InvokeTempoMethod2nd8NextFrame()
    {
        // 1�t���[���ҋ@
        yield return null;

        TempoMethodDelay2_8?.Invoke();
    }


    public BeatResult JustBeatTick()
    {
        // OnBeatSync����x�����s����Ă��Ȃ��ꍇ�ANotExecuted��Ԃ�
        if (lastBeatSyncTime < 0) return BeatResult.Miss;
        float elapsedTime1 = Time.time - lastBeatSyncTime01;
        float elapsedTime2 = Time.time - lastBeatSyncTime02;
        float elapsedTime3 = Time.time - lastBeatSyncTime03;
        float elapsedTime4 = Time.time - lastBeatSyncTime04;

        // �o�ߎ��Ԃ��r�[�g4���̕b������beatFuzzySet�����Z�������̈ȏ�A��������beatFuzzySet�ȉ��̏ꍇ�͔��ڂɍ��v�����Ɣ���
        if (elapsedTime1 >= ((BasicBeat * 4) - beatFuzzySet) - BeatOffSet || elapsedTime1 <= beatFuzzySet - BeatOffSet)
        {
            // �����𖞂������ꍇ�̏����������ɋL�q����
            return BeatResult.Tick01;
        }
        else if (elapsedTime2 >= ((BasicBeat * 4) - beatFuzzySet) - BeatOffSet || elapsedTime2 <= beatFuzzySet - BeatOffSet)
        { return BeatResult.Tick02; }

        else if (elapsedTime3 >= ((BasicBeat * 4) - beatFuzzySet) - BeatOffSet || elapsedTime3 <= beatFuzzySet - BeatOffSet)
        { return BeatResult.Tick03; }

        else if (elapsedTime4 >= ((BasicBeat * 4) - beatFuzzySet) - BeatOffSet || elapsedTime4 <= beatFuzzySet - BeatOffSet)
        { return BeatResult.Tick04; }

        return BeatResult.Miss;
    }

    public float AllowedTimeAroundBeat�@// �r�[�g�̃Y�����e�͈͂̕b��
    {
        get
        {
            return BeatFuzzy / frameRate;
        }
    }

}