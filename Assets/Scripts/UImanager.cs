using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UImanager : MonoBehaviour
{
    #region public field
    public GameObject GameOver_screen;
    public GameObject Win_Screen;
    public GameObject StartButton;
    public Text Levelpass_percentage;
    public Text coincollected_win;
    public Text Diamondcollected_win;
    public Text coincollected_gameover;
    public Text Diamondcollected_gameover;
    public Text TotalCoin;
    public Text TotalDiamond;
    public Text Racedone_text;
    public AudioSource Coinsound;
    public Image Level_progress;
    //int ,float ,bool ,vector
    [HideInInspector] public int Coincollected, TotalCoin_Val;
    [HideInInspector] public int DiamondCollect, TotalDiamond_val;
    public int RacePosition;
    [HideInInspector] public float Lprogresscount;
    public bool isRaceDone;
    [HideInInspector] public bool progress;
    #endregion


    #region monobehaviour callback
    // Start is called before the first frame update
    void Start()
    {
        TotalCoin.text = PlayerPrefs.GetInt("TotalCoin").ToString();
        TotalDiamond.text = PlayerPrefs.GetInt("TotalDiamond").ToString();
        TotalCoin_Val = PlayerPrefs.GetInt("TotalCoin");
        TotalDiamond_val = PlayerPrefs.GetInt("TotalDiamond");
        GameOver_screen.SetActive(false);
        Win_Screen.SetActive(false);
    }
    #endregion
    // Update is called once per frame
    #region public method

    public void CoinAndDiamond()
    {
        coincollected_win.text = Coincollected.ToString();
        Diamondcollected_win.text = DiamondCollect.ToString();
        coincollected_gameover.text = Coincollected.ToString();
        Diamondcollected_gameover.text = DiamondCollect.ToString();
    }
    public void Win()
    {
        isRaceDone = true;
        TotalCoin_Val += Coincollected;
        TotalDiamond_val += DiamondCollect;
        PlayerPrefs.SetInt("TotalCoin", TotalCoin_Val);
        PlayerPrefs.SetInt("TotalDiamond", TotalDiamond_val);
        TotalCoin.text = TotalCoin_Val.ToString();
        TotalDiamond.text = TotalDiamond_val.ToString();
        CoinAndDiamond();
        Win_Screen.SetActive(true);
        Racedone_text.text = RacePosition .ToString();
    }
   public void LevelProgress()
    {
        progress = true;
        StartCoroutine(Progress());
    }
   public IEnumerator Progress()
    {
       
        while (progress)
        {
            Level_progress.fillAmount += .05f;
            yield return new WaitForSeconds(.5f);
          
            if (Lprogresscount <=Level_progress.fillAmount)
            {
                progress = false;
                break;
                
            }
        }
    }
   public void GameOver()
    {
        CoinAndDiamond();
        int percentage = Mathf.FloorToInt ( Level_progress.fillAmount*100);
        Levelpass_percentage.text =percentage.ToString();
        GameOver_screen.SetActive(true);
    } 
   public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public   void CoinsoundPlay()
    {
        Coinsound.Play();
    }
    #endregion
}
