using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{

    public TextMeshProUGUI top1Text;
    public TextMeshProUGUI top2Text;
    public TextMeshProUGUI top3Text;
    public TextMeshProUGUI top4Text;
    public TextMeshProUGUI top5Text;

    // Start is called before the first frame update
    void Start()
    {
        SetScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetScores()
    {

        if (PlayerPrefs.HasKey("TopScore_0"))
            top1Text.text = "1: " + PlayerPrefs.GetInt("TopScore_0").ToString() + "0";

        if (PlayerPrefs.HasKey("TopScore_1"))
            top2Text.text = "2: " + PlayerPrefs.GetInt("TopScore_1").ToString() + "0";

        if (PlayerPrefs.HasKey("TopScore_2"))
            top3Text.text = "3: " + PlayerPrefs.GetInt("TopScore_2").ToString() + "0";

        if (PlayerPrefs.HasKey("TopScore_3"))
            top4Text.text = "4: " + PlayerPrefs.GetInt("TopScore_3").ToString() + "0";

        if (PlayerPrefs.HasKey("TopScore_4"))
            top5Text.text = "5: " + PlayerPrefs.GetInt("TopScore_4").ToString() + "0";

    }
}
