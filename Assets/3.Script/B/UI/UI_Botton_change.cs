using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Botton_change : MonoBehaviour
{
    [SerializeField] private GameObject currentUIObject; //현재 보여주는 UI Group 활성화 해주세요
    [SerializeField] private GameObject nextUIObject; //현재 보여주는 UI Group 비활성화 해주세요

    public void SwitchUIObject()
    {
        if (currentUIObject != null) currentUIObject.SetActive(false);

        if (nextUIObject != null) nextUIObject.SetActive(true);
        else Debug.Log("UI_Botton_change nextUIObject 이 없습니다. SerializeField이기에 컴포넌트에 GameObject를 연결해주세요");

        //교체 해서 재사용하겠습니다.
        GameObject tempObject = currentUIObject;
        currentUIObject = nextUIObject;
        nextUIObject = tempObject;
        Debug.Log("ShowNextUIObjectandSwitch");
    }

    public void SetPlayerCharactor(int Charactor_Enum)//버튼용 메서드
    {
        switch (Charactor_Enum)
        {
            case 1:
                {
                    GameManager.instance.player_chractor = Charactor.Ppipi;
                    Debug.Log($"캐릭터 선택: {Charactor.Ppipi}");
                    break;
                }
            case 2:
                {
                    GameManager.instance.player_chractor = Charactor.Sin;
                    Debug.Log($"캐릭터 선택: {Charactor.Sin}");
                    break;
                }
            case 3:
                {
                    GameManager.instance.player_chractor = Charactor.Byon;
                    Debug.Log($"캐릭터 선택: {Charactor.Byon}");
                    break;
                }
            default:
                {
                    GameManager.instance.player_chractor = Charactor.none;
                    Debug.LogError($"잘못된 캐릭터 번호({Charactor_Enum})입니다. Charactor.none으로 설정됩니다.");
                    break;
                }
        }
    }

    public void SceneLoader(string scenename) //버튼용 메소드 입니다.
    {
        StartCoroutine(LoadSceneandButtonDelay(scenename));
    }

    public IEnumerator LoadSceneandButtonDelay(string scenename)//바로 로드되면 버튼 사운드가 끊겨서 그 사운드에 맞춰서
    {
        yield return new WaitForSeconds(0.68f);// 이 시간을 맞춰 수정해주세요.
        Debug.Log("SceneLoad!");
        SceneManager.LoadScene(scenename);
    }

    public void ExitGame()//게임 종료용 메서드입니다.
    {
        Debug.Log("ExitGame!!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
