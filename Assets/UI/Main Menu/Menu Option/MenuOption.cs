
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MenuOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Color")]
    [SerializeField] private Color unselectedColor;
    [SerializeField] private Color selectedColor;

    [Header("Offset")]
    [SerializeField] private float selectedOffset;

    [Header("Background Color")]
    [SerializeField] private Color unselectedBackgroundColor;
    [SerializeField] private Color selectedBackgroundColor;

    [Header("Animation controls")]
    [SerializeField] private AnimationCurve selectionCurve;
    [SerializeField] private float animationTime;

    private float currentTime = 0f;
    private int direction = 0;
    private float initialTextOffset;

    private Image image;
    private TextMeshProUGUI text;
    private RectTransform textTransform;
    private Image arrowImage;

	[HideInInspector] public int id;

	private void Start()
    {
        image = GetComponent<Image>();
        arrowImage = Utils.SearchObjectWithComponent<Image>(transform, "Arrow");

		text = Utils.SearchObjectWithComponent<TextMeshProUGUI>(transform, "Text");
		textTransform = text.GetComponent<RectTransform>();
        initialTextOffset = textTransform.anchoredPosition.x;

        text.color = unselectedColor;
        image.color = unselectedBackgroundColor;
		arrowImage.color = image.color;
	}

    /// <summary>
    /// Seta o estado de seleção do botão
    /// </summary>
    /// <param name="isSelected">estado de seleção (selecionado ou não selecionado)</param>
    public void SetSelected(bool isSelected)
    {
        direction = isSelected ? 1 : -1;
	}

    public void Update()
    {
        if (direction != 0)
        {
            currentTime += Time.deltaTime * direction;
            float percent = currentTime / animationTime;
            if ((percent > 1f && direction == 1) || (percent < 0f && direction == -1))
            {
                direction = 0;
            }
			percent = Mathf.Clamp(percent, 0f, 1f);
			float value = selectionCurve.Evaluate(percent);

            text.color = new(
                Mathf.Lerp(unselectedColor.r, selectedColor.r, value),
				Mathf.Lerp(unselectedColor.g, selectedColor.g, value),
				Mathf.Lerp(unselectedColor.b, selectedColor.b, value),
				1f
            );

			image.color = new(
				Mathf.Lerp(unselectedBackgroundColor.r, selectedBackgroundColor.r, value),
				Mathf.Lerp(unselectedBackgroundColor.g, selectedBackgroundColor.g, value),
				Mathf.Lerp(unselectedBackgroundColor.b, selectedBackgroundColor.b, value),
				Mathf.Lerp(unselectedBackgroundColor.a, selectedBackgroundColor.a, value)
			);

            arrowImage.color = image.color;

            textTransform.anchoredPosition = new(
                Mathf.Lerp(initialTextOffset, selectedOffset, value), 0f);
		}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.GetComponent<MenuController>().ForceSelect(id);
        CursorController.SetCursor(CursorSprite.Grab);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		CursorController.SetCursor(CursorSprite.Default);
	}

    public virtual void Interact()
    {

    }
}
