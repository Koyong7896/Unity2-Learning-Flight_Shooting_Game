using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;//c#에서 지원(인풋, 아웃풋)
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class StartSceneManger : MonoBehaviour
{
    [SerializeField] Button btnStart;
    [SerializeField] Button btnRanking;
    [SerializeField] Button btnExitRanking;
    [SerializeField] Button btnExit;
    [SerializeField] GameObject viewRank;

    [Header("랭크 프리팹")]
    [SerializeField] GameObject fabRank;
    [SerializeField] Transform contents;    

    private void Awake()
    {
        Tool.isStartingMainScene = true;

        btnStart.onClick.AddListener(GameStart);
        btnRanking.onClick.AddListener(ShowRanking);
        btnExitRanking.onClick.AddListener(() => { viewRank.SetActive(false); });
        btnExit.onClick.AddListener(GameExit);

        #region 유니티 외부에서 json 사용 방법과 설명
        // ★ 아래 내용들은 유니티 외부에 만드는 내용임.
        //string 문자열, 키와 벨류
        //{key:value};

        //save기능, 씬과 씬을 이동할때 가지고 가야하는 데이터가 있다면 가지고 가서 불러오는 기능

        //1.플레이어 프랩스를 이용해 유니티에 저장하는 방법
        //PlayerPrefs => 유니티가 꺼져도 데이터를 보관하도록 유니티 내부에 저장

        //PlayerPrefs.SetInt("test", 999); <= 저장 방법, 숫자 데이터 한개만 저장 setint setfloat
        //데이터를 삭제하지 않는한 test안에 999라는 숫자가 저장돼있음, 게임을 삭제하면 데이터는 삭제되고 불러올수 없음
        //int value = PlayerPrefs.GetInt("test"); <= 불러오는 방법
        //Debug.Log(value);
        //PlayerPrefs.DeleteAll(); || PlayerPrefs.DeleteKey("test"); <= 삭제 방법
        //삭제 후에는 test의 int가 디폴트 0을 출력하게 됨
        //PlayerPrefs.DeleteKey("test",-1); <= 수정 방법
        //PlayerPrefs.HasKey("test"); <= 검색 방법

        //string path = Application.streamingAssetsPath;//os에 따라 읽기전용으로 사용됨
        //~/Assets/StreamingAssets <= └경로
        // ★ File 코드 안에는 암호화와 json 파일 생성 등 다양한 기능이 있음
        //File.WriteAllText(path + "/abc.json", "테스트"); <= json 파일과 내용 생성 방법
        //└테스트를 테스트22로 하면 내용이 수정됨
        //File.Delete(path + "/abc.json"); <= json 파일 삭제 방법
        //string result = File.ReadAllText(path + "/abc.json"); <= json 파일의 내용 불러오는 방법;

        //string path2 = Application.persistentDataPath + "/Jsons";//읽기와 쓰기가 가능한 폴더 위치
        //~/AppData/LocalLow/DefaultCompany/My project (1)/Jsons <= └경로
        /* persistentDataPath로 폴더 생성 방법
         * if(Directory.Exists(path2) == false)
         *{
         *   Directory.CreateDirectory(path2);
         *}*/

        //if(File.Exists(path2 + "/Test/abc.json") == true)
        //{
        //    string result = File.ReadAllText(path2 + "/Test/abc.json");
        //}
        //else //저장한 파일이 존재하지 않음
        //{
        //    //새로운 저장 위치와 데이터를 만들어 줘야함

        //    File.Create(path2 + "/Test");//폴더를 만들어줌
        //}

        #endregion
        #region 유니티 내부에서 json 사용 방법과 설명
        //cUserData cUserData = new cUserData();
        //cUserData.Name = "가나다";
        //cUserData.Score = 100;
        //cUserData cUserData2 = new cUserData();
        //cUserData.Name = "라마바";
        //cUserData.Score = 200;

        //List<cUserData> listUserData = new List<cUserData>();
        //listUserData.Add(cUserData);
        //listUserData.Add(cUserData2);

        //string jsonData = JsonUtility.ToJson(cUserData);// <= 장점 : 처리 속도가 빠름 / 단점 : 리스트 데이터를 불러올수 없음
        ////└{"Name":"가나다","Score":100}

        //cUserData user2 = new cUserData();
        //user2 = JsonUtility.FromJson<cUserData>(jsonData);

        //string jsonData = JsonUtility.ToJson(listUserData);
        //JsonUtility는 List 데이터를 json으로 변경하면 트러블이 존재함

        //List<cUserData> listUserData = new List<cUserData>();
        //listUserData.Add(new cUserData() { Name = "가나다", Score = 100});
        //listUserData.Add(new cUserData() { Name = "라마바", Score = 200});
        //string jsonData = JsonConvert.SerializeObject(listUserData); <= JsonConvert를 통하여 리스트 데이터를 json으로 변경 가능
        //List<cUserData> afterData = JsonConvert.DeserializeObject<List<cUserData>>(jsonData); <= 복호화 방법
        //└JsonUtility에 비해 빠르고 리스트도 불러올수 있지만 기능이 많아서 속도가 느린편
        #endregion
        #region JsonConVert 사용전 Newtonsoft 패키지 설치 방법
        //유니티에서 Pakeges를 우클릭 하여 Show in Explorer 통하여 해당 폴더로 들어간 후
        //manifest.json 파일내에 위 코드에 쉼표 추가 후 아래줄에 "com.unity.nuget.newtonsoft-json" :  "3.2.1" 입력
        //저장하고 유니티로 돌아가면 패키지가 다운로드 되며, JsonConVert가 사용 가능해짐.
        #endregion

        initRankView();
        viewRank.SetActive(false);
    }

    /// <summary>
    /// 랭크가 저장되어 있다면 저장된 랭크 데이터를 이용해서 랭크뷰를 만들어주고
    /// 랭크가 저장되어 있지 않다면 비어있는 랭크를 만들어 랭크뷰를 만들어줌
    /// </summary>
    private void initRankView()
    {
        List<cUserData> listUserData = null;
        clearRankView();
        if (PlayerPrefs.HasKey(Tool.rankKey) == true)//랭크 데이터가 저장이 되어있었다면
        {
            listUserData = JsonConvert.DeserializeObject<List<cUserData>>(PlayerPrefs.GetString(Tool.rankKey));
        }
        else//랭크데이터가 저장되어 있지 않았다면
        {
            listUserData = new List<cUserData>();
            int rankCount = Tool.rankCount;
            for (int iNum = 0; iNum < rankCount; ++iNum)
            {
                listUserData.Add(new cUserData() { Name = "None", Score = 0 });
            }

            string value = JsonConvert.SerializeObject(listUserData);
            PlayerPrefs.SetString(Tool.rankKey, value);
        }

        int count = listUserData.Count;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            cUserData data = listUserData[iNum];

            GameObject go = Instantiate(fabRank, contents);
            FabRanking goSc = go.GetComponent<FabRanking>();
            goSc.SetData((iNum + 1).ToString(), data.Name, data.Score);
        }
    }

    private void clearRankView()
    {
        int count = contents.childCount;
        for(int iNum = count -1; iNum > -1; --iNum)
        {
            Destroy(contents.GetChild(iNum).gameObject);
        }
    }

    private void GameStart()
    {        
        FunctionFade.Instance.ActiveFade(true, () =>
        {
            SceneManager.LoadScene(1);
            FunctionFade.Instance.ActiveFade(false);
        });
    }

    private void ShowRanking()
    {
         viewRank.SetActive(true);
    }

    private void GameExit()
    {
        //에디터에서 플레이를 끄는 방법, 에디터 전용 기능
        //빌드를 통해서 밖으로 가지고 나가서는 안됨
        //전처리, 코드가 조건에 의해서 본인이 없는것처럼 혹은 있는것처럼 동작하게 해줌

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else//유니티 에디터에서 실행하지 않았을때        
        //빌드했을때 게임 종료
        Application.Quit();        
#endif
    }
}
