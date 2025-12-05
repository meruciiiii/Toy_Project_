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
public class Data //단판 데이터
{
    //캐릭터의 경우 Enum 으로 관리합시다.
    //플레이어 이름 string (키보드 입력 받아서 저장)
    //기록(시간) float double ->기록에 따라 랭킹 순서
    //기록의 경우 Time.deltatime -> 시간 초로 변환?
    
    public string Playername;
    public Charactor charactor;
    public float cleartime;

    public Data(string Playername, Charactor charactor, float cleartime)
    {
        this.Playername = Playername;
        this.charactor = charactor;
        this.cleartime = cleartime;
    }
}

[Serializable]
public class Ranking
{
    public List<Data> ranking = new List<Data>();
}

public class RankingRW : MonoBehaviour
{
    private static string path;
    private string filename = "ranking_data.json";

    private void Awake()
    {
        path = Application.persistentDataPath + "tmp.txt";
        SaveToJson(new Data("",0,0f));
        //SaveToJson(new Data("",Charactor.Song, 0f));
    }

    
    public static void SaveToJson(Data jsonSaveData)
    {
        // A -> B가는 작업
        string jsonData = JsonUtility.ToJson(jsonSaveData);

        // 파일에 저장 Ctrl + S
        File.WriteAllText(path, jsonData);
    }

    public Data LoadFromJson()//시작할때 로드
    {
        if (!File.Exists(path))
            return null;

        // 파일을 불러옴
        string jsonData = File.ReadAllText(path);

        // B -> C가는 작업
        Data jsonSaveData = JsonUtility.FromJson<Data>(jsonData);

        return jsonSaveData;
    }
}
