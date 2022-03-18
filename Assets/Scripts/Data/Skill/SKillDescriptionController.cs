using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKillDescriptionController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TitleTxt;
    public TMPro.TextMeshProUGUI descriptionTxt;
    public GameObject title;
    private GameManager gm;
    // Start is called before the first frame update

    public void SetSkillTooltip(int index)
	{
		if (gm == null)
		{
            gm = GameManager.Instance;
        }
        var skillData = gm.GetSkillDataFromDB(gm.unitStatus[0].skills[index]);
        TitleTxt.text = skillData.skillName;
        descriptionTxt.text = skillData.description;
        title.SetActive(true);
        Invoke("OffSkillTooptip", 5);
    }
    public void OffSkillTooptip()
    {
        title.SetActive(false);
    }
}
