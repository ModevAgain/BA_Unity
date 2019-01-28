using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour {

    public EndGameView EndView;
    public TextMeshProUGUI TimerTextM;
    public TextMeshProUGUI TimerTextS;
    

    [Range(0,1)]
    public float UpdateTime;

    private float _time;



	// Use this for initialization
	void Start () {

        _time = DataPipe.instance.GameData.GetGameDuration();
        StartCoroutine(TimerRoutine());

	}


    public IEnumerator TimerRoutine()
    {
        while(_time > 0)
        {
            _time -= 1;
            SetTimer(_time);

            yield return new WaitForSeconds(UpdateTime);
        }

        EndView.EndGame();
    }

    public void SetTimer(float time)
    {
        TimerTextM.text = (Mathf.FloorToInt(time / 60f)).ToString("00");
        TimerTextS.text = (Mathf.FloorToInt(time % 60f)).ToString("00");
    }
}
