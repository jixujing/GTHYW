using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour,ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector2 menuPosition;
    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public FadeEventSO fadeEvent;
    public VoidEventSo newGameEvent;
    public VoidEventSo backToMenuEvent;


    [Header("����")]
    public GameSceneSO firstLoadSccene;
    public GameSceneSO menuScene;
    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;

    [Header("�㲥")]
    public VoidEventSo afterSceneLoadedEvent;
    public SceneLoadEventSO unloadedSceneEvent;
    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadSccene.sceneReference,LoadSceneMode.Additive);
        //  currentLoadedScene = firstLoadSccene;
        //currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
    }

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        // NewGame();
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackMenuToEvent;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackMenuToEvent;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnBackMenuToEvent()
    {
        sceneToLoad = menuScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadSccene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }


    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        if (currentLoadedScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            loadNewScene();
        }
        }



    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            fadeEvent.fadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        //�㲥�¼�����Ѫ����ʾ
        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

        yield return currentLoadedScene.sceneReference.UnLoadScene();

        //�ر�����
        playerTrans.gameObject.SetActive(false);

        //�����³���
        loadNewScene();
    }

    private void loadNewScene()
    {
       var loadingOption =  sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadedScene = sceneToLoad;
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeEvent.fadeOut(fadeDuration);
        }

        isLoading = false;

        if (currentLoadedScene.sceneType == SceneType.Location)
        //����������Ϻ��¼�
        afterSceneLoadedEvent.RaiseEvent();
    }



    //ʵ�ֽӿ�
    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadedScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
        if (data.characterPositionDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPositionDict[playerID];
            sceneToLoad = data.GetSaveScene();
            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}
