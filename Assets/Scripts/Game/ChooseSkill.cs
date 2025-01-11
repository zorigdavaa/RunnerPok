using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using ZPackage;

public class ChooseSkill : MonoBehaviour
{
    public Image SkillImage;
    public TextMeshProUGUI TextDescription;
    public Skill skill;
    Button Button;
    // Start is called before the first frame update
    void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(Choose);
    }

    public void SetSkill(Skill skillComing)
    {
        skill = skillComing;
        SkillImage.sprite = skill.Sprite;
        TextDescription.text = skill.Text;
        gameObject.SetActive(true);
    }
    public void Choose()
    {
        Z.Player.AddToSkill(skill);
        Skills.Instance.PlayerChosen(this);
    }
}
