using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreEntryPrefab : MonoBehaviour
{
    TextMeshProUGUI[] texts;
    Image bg;

    private void Awake()
    {
        bg = GetComponent<Image>();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }
    public void SetEntry(int pos, HighScoreEntry entry)
    {
        bg.color = pos % 2 == 0 ? new Color(0.15f, 0.15f, 0.15f, 0.4f) : new Color(0.3f, 0.3f, 0.3f, 0.4f);
        if (pos == Settings.keraysera.userLeaderBoardIndex) bg.color = new Color(0.05f, 0.8f, 0.12f, 0.4f);
        texts[0].text = $"{pos + 1}.";
        texts[1].text = entry.userName;
        texts[2].text = Global.FromFloatToTime(entry.time);
        texts[3].text = entry.kone;
    }
}
