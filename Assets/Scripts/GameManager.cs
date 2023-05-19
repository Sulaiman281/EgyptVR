using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.DecorationMenu;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerInput.ActionEvent onSceneStart;

    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform classRTrans;

    [SerializeField] private GameObject playerMovement;

    public string joinCode;

    public PlayerVisualControls localPlayer;
    public PlayerMapRef playerMapRef;


    private void Start()
    {
        SetupQuiz();
        onSceneStart.Invoke(default);
        SetupNetwork();
    }

    private void LateUpdate()
    {
        _buyDelay -= Time.deltaTime;
    }

    public void ResetPlayerPosition()
    {
        playerMovement.SetActive(false);

        playerTrans.position = classRTrans.position;

        StartCoroutine(DelayAction(() => { playerMovement.SetActive(true); }, .5f));
    }

    public IEnumerator DelayAction(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    #region RelayServer

    private void SetupNetwork()
    {
        var status = PlayerPrefs.GetString("NetworkStatus");
        if (status.Equals("Server"))
        {
            CreateClass();
        }
        else
        {
            JoinClass(status);
        }
    }

    public async void CreateClass()
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(5);
            var code = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            // onRelayConnectionPass.Invoke(default);
            var relayServerData = new RelayServerData(allocation, "dtls");
            Debug.Log(code);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            joinCode = code;
            // NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        }
        catch (RelayServiceException)
        {
            // onRelayConnectionFailed.Invoke(default);
            SceneManager.LoadScene("Menu");
        }
    }

    public async void JoinClass(string code)
    {
        try
        {
            // var code = codeInputField.text.ToUpper();
            var relayAllocation = await RelayService.Instance.JoinAllocationAsync(code);
            if (relayAllocation == null)
            {
                // onRelayConnectionFailed.Invoke(default);
                return;
            }

            var relayServerData = new RelayServerData(relayAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            joinCode = code;
        }
        catch (RelayServiceException)
        {
            SceneManager.LoadScene("Menu");
            // onRelayConnectionFailed.Invoke(default);
        }
    }

    #endregion

    #region Quiz System

    [Header("References")] [SerializeField]
    private TMP_Text quizNoTmp;

    [SerializeField] private TMP_Text quizQuestionTmp;
    [SerializeField] private TMP_Text[] options;
    [SerializeField] private TMP_Text resultsTmp;

    [Header("Quiz Sfx")] [SerializeField] private AudioSource quizSolvedSfx;
    [SerializeField] private AudioClip correctSfx;
    [SerializeField] private AudioClip wrongSfx;

    [Header("Questions")] [SerializeField] private Quiz[] quizzes;
    private Queue<Quiz> _remainingQuestions = new();

    [Header("Events")] [SerializeField] private PlayerInput.ActionEvent onShowResults;

    private int _countPoints;

    private Quiz _quiz;

    private void SetupQuiz()
    {
        foreach (var quiz in quizzes)
        {
            _remainingQuestions.Enqueue(quiz);
        }

        _quiz = _remainingQuestions.Dequeue();
    }

    public void NextQuiz()
    {
        if (_remainingQuestions.TryDequeue(out var quiz))
        {
            _quiz = quiz;
            ShowCurrentQuiz();
        }
        else
        {
            // show the results
            resultsTmp.text = $"You have earned {_countPoints} Points!";
            onShowResults.Invoke(default);
            if (localPlayer == null) return;
            localPlayer.AddQuizPoints(_countPoints);
        }
    }

    public void ShowCurrentQuiz()
    {
        ShowQuiz(_quiz);
    }

    private void ShowQuiz(Quiz quiz)
    {
        // Create a list of all answer options and shuffle it
        List<string> answerOptions = new List<string>();
        answerOptions.Add(quiz.answer);
        answerOptions.Add(quiz.option1);
        answerOptions.Add(quiz.option2);
        answerOptions.Add(quiz.option3);
        answerOptions = ShuffleList(answerOptions);

        // Set the question and shuffled answer options
        quizNoTmp.text = _remainingQuestions.Count + "";
        quizQuestionTmp.text = quiz.question;
        for (var i = 0; i < options.Length; i++)
        {
            options[i].text = answerOptions[i];
            var btn = options[i].transform.parent.GetComponent<Button>();
            var i1 = i;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                var isCorrectAnswer = answerOptions[i1].Equals(quiz.answer);
                quiz.solved = isCorrectAnswer;
                quizSolvedSfx.clip = isCorrectAnswer ? correctSfx : wrongSfx;
                _countPoints += isCorrectAnswer ? 10 : 0;
                quizSolvedSfx.Play();
                NextQuiz();
            });
        }
    }

    private static List<string> ShuffleList(List<string> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }

        return list;
    }

    #endregion

    #region Decoration Items

    [Header("Decoration Ref")] [SerializeField]
    private TMP_Text itemNameTmp;

    [SerializeField] private Image itemSprite;
    [SerializeField] private Transform spawnPoint;

    private DecorationModel _decorationModel;
    private int _lastIndex = 0;

    private float _buyDelay;

    public void SetupDecoration()
    {
        if (_decorationModel == null)
        {
            _decorationModel = Resources.Load<DecorationModel>("DecorationsObjects");
        }

        var item = _decorationModel.models[0];
        itemNameTmp.text = item.modelName;
        itemSprite.sprite = item.modelSprite;
        _lastIndex = 0;
    }

    public void ShowItem(bool right)
    {
        if (_decorationModel == null)
        {
            _decorationModel = Resources.Load<DecorationModel>("DecorationsObjects");
        }

        var nextItem = _lastIndex + (right ? 1 : -1);
        if (nextItem >= _decorationModel.models.Length) nextItem = 0;
        if (nextItem < 0) nextItem = _decorationModel.models.Length - 1;
        _lastIndex = nextItem;
        var item = _decorationModel.models[nextItem];
        itemNameTmp.text = item.modelName;
        itemSprite.sprite = item.modelSprite;
    }

    public void BuyDecorationItem()
    {
        if (_decorationModel == null)
        {
            _decorationModel = Resources.Load<DecorationModel>("DecorationsObjects");
        }

        if (_buyDelay >= 0) return;
        var obj = _decorationModel.models[_lastIndex].modelPrefab;
        Instantiate(obj, spawnPoint.position, Quaternion.identity);
        _buyDelay = 2.5f;
    }

    #endregion
}

[Serializable]
public struct Quiz
{
    public string question;
    public string answer;
    public string option1;
    public string option2;
    public string option3;

    public bool solved;
}