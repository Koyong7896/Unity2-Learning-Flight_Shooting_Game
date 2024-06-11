using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;//c#���� ����(��ǲ, �ƿ�ǲ)
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class StartSceneManger : MonoBehaviour
{
    [SerializeField] Button btnStart;
    [SerializeField] Button btnRanking;
    [SerializeField] Button btnExitRanking;
    [SerializeField] Button btnExit;
    [SerializeField] GameObject viewRank;

    [Header("��ũ ������")]
    [SerializeField] GameObject fabRank;
    [SerializeField] Transform contents;    

    private void Awake()
    {
        Tool.isStartingMainScene = true;

        btnStart.onClick.AddListener(GameStart);
        btnRanking.onClick.AddListener(ShowRanking);
        btnExitRanking.onClick.AddListener(() => { viewRank.SetActive(false); });
        btnExit.onClick.AddListener(GameExit);

        #region ����Ƽ �ܺο��� json ��� ����� ����
        // �� �Ʒ� ������� ����Ƽ �ܺο� ����� ������.
        //string ���ڿ�, Ű�� ����
        //{key:value};

        //save���, ���� ���� �̵��Ҷ� ������ �����ϴ� �����Ͱ� �ִٸ� ������ ���� �ҷ����� ���

        //1.�÷��̾� �������� �̿��� ����Ƽ�� �����ϴ� ���
        //PlayerPrefs => ����Ƽ�� ������ �����͸� �����ϵ��� ����Ƽ ���ο� ����

        //PlayerPrefs.SetInt("test", 999); <= ���� ���, ���� ������ �Ѱ��� ���� setint setfloat
        //�����͸� �������� �ʴ��� test�ȿ� 999��� ���ڰ� ���������, ������ �����ϸ� �����ʹ� �����ǰ� �ҷ��ü� ����
        //int value = PlayerPrefs.GetInt("test"); <= �ҷ����� ���
        //Debug.Log(value);
        //PlayerPrefs.DeleteAll(); || PlayerPrefs.DeleteKey("test"); <= ���� ���
        //���� �Ŀ��� test�� int�� ����Ʈ 0�� ����ϰ� ��
        //PlayerPrefs.DeleteKey("test",-1); <= ���� ���
        //PlayerPrefs.HasKey("test"); <= �˻� ���

        //string path = Application.streamingAssetsPath;//os�� ���� �б��������� ����
        //~/Assets/StreamingAssets <= �����
        // �� File �ڵ� �ȿ��� ��ȣȭ�� json ���� ���� �� �پ��� ����� ����
        //File.WriteAllText(path + "/abc.json", "�׽�Ʈ"); <= json ���ϰ� ���� ���� ���
        //���׽�Ʈ�� �׽�Ʈ22�� �ϸ� ������ ������
        //File.Delete(path + "/abc.json"); <= json ���� ���� ���
        //string result = File.ReadAllText(path + "/abc.json"); <= json ������ ���� �ҷ����� ���;

        //string path2 = Application.persistentDataPath + "/Jsons";//�б�� ���Ⱑ ������ ���� ��ġ
        //~/AppData/LocalLow/DefaultCompany/My project (1)/Jsons <= �����
        /* persistentDataPath�� ���� ���� ���
         * if(Directory.Exists(path2) == false)
         *{
         *   Directory.CreateDirectory(path2);
         *}*/

        //if(File.Exists(path2 + "/Test/abc.json") == true)
        //{
        //    string result = File.ReadAllText(path2 + "/Test/abc.json");
        //}
        //else //������ ������ �������� ����
        //{
        //    //���ο� ���� ��ġ�� �����͸� ����� �����

        //    File.Create(path2 + "/Test");//������ �������
        //}

        #endregion
        #region ����Ƽ ���ο��� json ��� ����� ����
        //cUserData cUserData = new cUserData();
        //cUserData.Name = "������";
        //cUserData.Score = 100;
        //cUserData cUserData2 = new cUserData();
        //cUserData.Name = "�󸶹�";
        //cUserData.Score = 200;

        //List<cUserData> listUserData = new List<cUserData>();
        //listUserData.Add(cUserData);
        //listUserData.Add(cUserData2);

        //string jsonData = JsonUtility.ToJson(cUserData);// <= ���� : ó�� �ӵ��� ���� / ���� : ����Ʈ �����͸� �ҷ��ü� ����
        ////��{"Name":"������","Score":100}

        //cUserData user2 = new cUserData();
        //user2 = JsonUtility.FromJson<cUserData>(jsonData);

        //string jsonData = JsonUtility.ToJson(listUserData);
        //JsonUtility�� List �����͸� json���� �����ϸ� Ʈ������ ������

        //List<cUserData> listUserData = new List<cUserData>();
        //listUserData.Add(new cUserData() { Name = "������", Score = 100});
        //listUserData.Add(new cUserData() { Name = "�󸶹�", Score = 200});
        //string jsonData = JsonConvert.SerializeObject(listUserData); <= JsonConvert�� ���Ͽ� ����Ʈ �����͸� json���� ���� ����
        //List<cUserData> afterData = JsonConvert.DeserializeObject<List<cUserData>>(jsonData); <= ��ȣȭ ���
        //��JsonUtility�� ���� ������ ����Ʈ�� �ҷ��ü� ������ ����� ���Ƽ� �ӵ��� ������
        #endregion
        #region JsonConVert ����� Newtonsoft ��Ű�� ��ġ ���
        //����Ƽ���� Pakeges�� ��Ŭ�� �Ͽ� Show in Explorer ���Ͽ� �ش� ������ �� ��
        //manifest.json ���ϳ��� �� �ڵ忡 ��ǥ �߰� �� �Ʒ��ٿ� "com.unity.nuget.newtonsoft-json" :  "3.2.1" �Է�
        //�����ϰ� ����Ƽ�� ���ư��� ��Ű���� �ٿ�ε� �Ǹ�, JsonConVert�� ��� ��������.
        #endregion

        initRankView();
        viewRank.SetActive(false);
    }

    /// <summary>
    /// ��ũ�� ����Ǿ� �ִٸ� ����� ��ũ �����͸� �̿��ؼ� ��ũ�並 ������ְ�
    /// ��ũ�� ����Ǿ� ���� �ʴٸ� ����ִ� ��ũ�� ����� ��ũ�並 �������
    /// </summary>
    private void initRankView()
    {
        List<cUserData> listUserData = null;
        clearRankView();
        if (PlayerPrefs.HasKey(Tool.rankKey) == true)//��ũ �����Ͱ� ������ �Ǿ��־��ٸ�
        {
            listUserData = JsonConvert.DeserializeObject<List<cUserData>>(PlayerPrefs.GetString(Tool.rankKey));
        }
        else//��ũ�����Ͱ� ����Ǿ� ���� �ʾҴٸ�
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
        //�����Ϳ��� �÷��̸� ���� ���, ������ ���� ���
        //���带 ���ؼ� ������ ������ �������� �ȵ�
        //��ó��, �ڵ尡 ���ǿ� ���ؼ� ������ ���°�ó�� Ȥ�� �ִ°�ó�� �����ϰ� ����

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else//����Ƽ �����Ϳ��� �������� �ʾ�����        
        //���������� ���� ����
        Application.Quit();        
#endif
    }
}
