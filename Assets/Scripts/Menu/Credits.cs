using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour
{
    public enum CreditsStage
    {
        Idle,
        IsWaiting,
        IsStarting,
        IsShowing,
        IsEnding,
        HasEnded
    }

    public float delay = 6f;
    public float fadeIn = 1f;
    public float fadeOut = 1f;
    public float duration = 2f;

    public string title;
    public string[] names;

    public float heightOffset = 20f;

    public Credits nextCredits;

    private float time = 0f;
    private bool isActive = false;

    private DeathMenu menu;
    private Color color;

    public CreditsStage stage;

    private float titleWidth;
    private float nameWidth;

    private GUIStyle style;

    public void StartCredits()
    {
        stage = CreditsStage.IsWaiting;
        color = new Color(1f, 1f, 1f, 0f);
    }

	private void Start()
    {
        menu = GetComponent<DeathMenu>();
        stage = CreditsStage.Idle;

        style = new GUIStyle();
        titleWidth = style.CalcSize(new GUIContent(title)).x + 4f;
        //nameWidth = style.CalcSize(new GUIContent(name)).x * 1.2f;
	}
	
	private void Update()
    {
        time += Time.deltaTime;

        switch (stage)
        {
            case CreditsStage.Idle:

                break;
            case CreditsStage.IsWaiting:
                if (time >= delay)
                {
                    stage = CreditsStage.IsStarting;

                    time = 0;
                }
                break;
            case CreditsStage.IsStarting:
                color.a = time / fadeIn;

                if (time >= fadeIn)
                {
                    stage = CreditsStage.IsShowing;

                    time = 0;
                }
                break;
            case CreditsStage.IsShowing:
                if (time >= duration)
                {
                    stage = CreditsStage.IsEnding;

                    time = 0;
                }
                break;
            case CreditsStage.IsEnding:
                color.a = (fadeOut - time) / fadeOut;

                if (time >= fadeOut)
                {
                    stage = CreditsStage.HasEnded;

                    time = 0;
                }
                break;
            case CreditsStage.HasEnded:
                if (nextCredits != null)
                {
                    nextCredits.StartCredits();
                }

                stage = CreditsStage.Idle;
                break;
        }
	}

    private void OnGUI()
    {
        if (color.a > 0f)
        {
            GUI.contentColor = color;

            GUI.Label(new Rect(Screen.width / 2 - titleWidth / 2 - 4f, Screen.height / 2 - heightOffset, titleWidth, 30), title);

            for (int i = 0; i < names.Length; i++)
            {
                float nameWidth = style.CalcSize(new GUIContent(names[i])).x + 4f;
                GUI.Label(new Rect(Screen.width / 2 - nameWidth / 2, Screen.height / 2 + heightOffset + 30 * i, nameWidth, 30), names[i]);
            }
        }
    }
}
