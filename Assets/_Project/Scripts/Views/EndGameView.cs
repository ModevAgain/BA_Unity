using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EndGameView : MonoBehaviour {

    public Image Mask;
    public Image BG;

    public TextMeshProUGUI Plat1;
    public CanvasGroup Plat1CG;
    public TextMeshProUGUI Plat2;
    public CanvasGroup Plat2CG;

    public TextMeshProUGUI Thanks;

    public CanvasGroup RestartCG;
    


    // Use this for initialization
    void Awake () {

        Mask.rectTransform.DOSizeDelta(Vector2.zero, 0);
        BG.enabled = false;
        Plat1CG.transform.DOScale(0, 0f);
        Plat1CG.DOFade(0, 0);
        Plat2CG.transform.DOScale(0, 0f);
        Plat2CG.DOFade(0, 0);
        Thanks.transform.DOScale(0, 0f);
        Thanks.DOFade(0, 0);
        RestartCG.DOFade(0, 0);
        RestartCG.blocksRaycasts = false;


    }
	


    public void EndGame()
    {
        StartCoroutine(EndGameAnimation(DataPipe.instance.ResourceManager.Platform1Count, DataPipe.instance.ResourceManager.Platform2Count));
    }

    public IEnumerator EndGameAnimation(int p1, int p2)
    {
        BG.enabled = true;

        yield return Mask.rectTransform.DOSizeDelta(new Vector2(2048 * 1.2f, 2048 * 1.2f), 2f).SetEase(Ease.InCubic).WaitForCompletion();

        yield return new WaitForSeconds(1);

        Plat1.text = p1.ToString("00");
        Plat2.text = p2.ToString("00");


        Plat1CG.DOFade(1, 0.3f).SetEase(Ease.Linear);
        yield return Plat1CG.transform.DOScale(1, 0.3f).SetEase(Ease.OutElastic).WaitForCompletion();

        yield return new WaitForSeconds(1);

        Plat2CG.DOFade(1, 0.3f).SetEase(Ease.Linear);
        yield return Plat2CG.transform.DOScale(1, 0.3f).SetEase(Ease.OutElastic).WaitForCompletion();

        yield return new WaitForSeconds(1);

        Thanks.DOFade(1, 0.3f).SetEase(Ease.Linear);
        Thanks.transform.DOScale(1, 0.4f).SetEase(Ease.OutElastic);

        yield return new WaitForSeconds(2);

        RestartCG.DOFade(1, 0.2f).OnComplete(() => RestartCG.blocksRaycasts = true);
        
    }

    public void RestartGame()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("Start");
    }
}
