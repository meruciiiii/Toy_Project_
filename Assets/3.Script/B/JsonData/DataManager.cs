using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson; //플러그인
using UnityEngine;

public enum Charactor
{
    none,//0 
    Ppipi,//1
    Sin, //2
    Byon //3
}
/* 대략적인 Enum 사용방법
  다른 클래스에서 사용할때 이런 느낌으로 사용하시면 될것 같습니다.
 public class GameManager : MonoBehaviour
{
    // 1. Enum 타입의 변수 선언
    public CharacterType selectedCharacter;

    // 2. Enum을 매개변수로 받는 함수 정의
    public void SelectNewCharacter(CharacterType characterToSelect)
    {
        selectedCharacter = characterToSelect;
        Debug.Log($"선택된 캐릭터: {selectedCharacter}");
    }

    private void Start()
    {
        // 3. Enum 값을 직접 사용
        selectedCharacter = CharacterType.Song;

        // 4. 함수 호출 시 Enum 값 전달
        SelectNewCharacter(CharacterType.Byon); 
    }
}
 */

[Serializable]
public class Ranking //데이터를 여기 안에 담겠습니다.
{
    public List<Data> Listdata = new List<Data>();
}

[Serializable]
public class Data //단판 데이터 이걸 리스트에 담아서 저장하겠습니다.
{
    //캐릭터의 경우 Enum 으로 관리합시다.
    //플레이어 이름 string (키보드 입력 받아서 저장)
    //기록(시간) float double ->기록에 따라 랭킹 순서
    //기록의 경우 Time.deltatime -> 시간 초로 변환해서 20:00에서 빼서 높으면 점수가 높은거다.(6:00 칼퇴가 중요하다.)
    
    public string Playername;
    public Charactor charactor;
    public int cleartime;

    public Data(string Playername, Charactor charactor, int cleartime)
    {
        this.Playername = Playername;
        this.charactor = charactor;
        this.cleartime = cleartime;
    }
    /*
     string nameInput = "플레이어이름";
     Charactor selectedChar = Charactor.Ppipi;
     float finalTime = 45.8f; //최종 시간

     Data 객체 생성 시, 생성자에 각각 전달
     Data newRecord = new Data(nameInput, selectedChar, finalTime);  
     DataManager의 AddNewRanking(Data newRecord); 로 넣고 랭킹 섞어야 됩니다.
    이후로는 UI에 리스트 넣고 반복문 이중 포문등으로 Text를 돌려버리면 OK
    for(int i = 0, i<Ranking.jsonSaveData.Count,i++) // 3번 반복
        {
         
        }
     */
    public Data() { }//생성자
}

public class DataManager : MonoBehaviour
{
    private static DataManager instancePrivate = null;

    public static DataManager instance
    {
        get
        {
            // 이 Getter를 통해 외부에서 DataManager.Instance.xxx 형태로 접근합니다.
            if (instancePrivate == null)
            {
                // 인스턴스가 없을 경우 찾아주거나 에러를 로그합니다.
                instancePrivate = FindObjectOfType<DataManager>();
                if (instancePrivate == null)
                {
                    Debug.LogError("DataManager 인스턴스를 씬에서 찾을 수 없습니다! 씬에 DataManager 컴포넌트를 가진 GameObject가 있는지 확인하세요.");
                }
            }
            return instancePrivate;
        }
    }
    //파일 경로체크
    private string filename = "ranking_data.json";
    private static string path;//파일 저장 경로(파일명까지)

    private void Awake()
    {
        if (instancePrivate == null)
        {
            instancePrivate = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        path = Path.Combine(Application.persistentDataPath, filename);//파일경로에 파일이름을 합쳐서 string화
        //persistentDataPath 경로
        //C:\Users\[user name]\AppData\LocalLow\[company name]\[product name]

        //path = Application.persistentDataPath + "tmp.txt";

        //SaveToJson(new Data("",0,0f));
        //SaveToJson(new Data("",Charactor.Song, 0f));
    }

    private void Start()
    {
        if (!File.Exists(path))// path = Path.Combine(Application.persistentDataPath, filename); 파일 명까지의 경로가 없다면
        {
            SaveToJson(new Ranking());
            Debug.Log("랭킹파일 없음. 새로 생성.");
        }
        AddNewRanking(new Data("test", Charactor.Ppipi, UnityEngine.Random.Range(10, 50)));
        Debug.Log("저장1");
    }
    //파일 경로체크

    public void SaveToJson(Ranking ranking) //총 랭킹 데이터 저장 메서드
    {
        string jsonData = JsonMapper.ToJson(ranking); //랭킹데이터를 제이슨으로 바꾸기 전에 string에 넣어서

        File.WriteAllText(path, jsonData); //경로(파일)안에 그 string 데이터를 전부 적어넣는다.
        Debug.Log("랭킹 데이터 저장 완료");
    }

    public Ranking LoadFromJson()//총 랭킹 데이터 불러오기 메서드
    {
        if (!File.Exists(path))
        {
            Ranking newRanking = new Ranking();// return하기 위해 따로 지역변수로 담아 선언
            //SaveToJson(newRanking);
            Debug.Log("랭킹 파일 없음. 새로 생성하고 빈 랭킹 객체 반환");
            return newRanking;
        }

        // 파일을 불러옴
        string jsonData = File.ReadAllText(path); // 경로(파일)안에 그 string 데이터를 전부 받아 넣겠습니다. return하기 위해 지역변수로 담아 선언

        //Data jsonSaveData = JsonUtility.FromJson<Data>(jsonData);
        Ranking jsonSaveData = JsonMapper.ToObject<Ranking>(jsonData);

        return jsonSaveData;
    }

    // 랭킹 반영 로직

    public int Ranking_count = 4; //4위까지만 기록하겠습니다.
    public void AddNewRanking(Data newrecord) //플레이 Data를 랭킹에 추가하고 자동으로 지우는 메서드
    {
        Ranking rankingdata = LoadFromJson(); //Ranking 클래스를 일단 불러오고
        rankingdata.Listdata.Add(newrecord);//그 클래스에 있는 안에 있는 변수(리스트를)에 Data를 먼저 넣고 그 이후에 섞겠습니다. 나중에 저장도 해야됩니다.
        rankingdata.Listdata.Sort((a, b) => b.cleartime.CompareTo(a.cleartime));//List.Sort 정렬알고리즘 메서드

        if (rankingdata.Listdata.Count > Ranking_count)
        {
            rankingdata.Listdata.RemoveAt(Ranking_count);//4위 이후는 지워버리겠습니다.
        }
        SaveToJson(rankingdata);
    }

    public List<Data> GetRankingList() //밖으로 데이터 빼고 싶을때.
    {
        Ranking rankingData = LoadFromJson(); // 파일에서 데이터를 로드하거나 초기화합니다.
        return rankingData.Listdata;
    }
}
