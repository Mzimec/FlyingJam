using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

static class ConstantValues {
    public static int cardTypesCount = 4;
    public static float chanceToBeAttacked = 0.5f;
    public static int regionCount = 30;
    public static int startYear = 1812;
    public static int startMonth = 3;

    public static int cardHeight = 485;
    public static int cardWidth = 335;

    public static float cardScale = 1.0f;
    public static int cardMargin = 10;

    public static VisualElement CreateEmpty(VisualTreeAsset vt) {
        VisualElement blank = vt.CloneTree();
        //blank.transform.scale = new Vector3(ConstantValues.cardScale, ConstantValues.cardScale, 0.0f);
        //blank.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(0, 0));
        blank.style.marginBottom = ConstantValues.cardMargin;
        blank.style.marginRight = ConstantValues.cardMargin;
        //blank.style.width = ConstantValues.cardScale * ConstantValues.cardWidth;
        //blank.style.height = ConstantValues.cardScale * ConstantValues.cardHeight;
        return blank;
    }

    public static string GetDate(int d) {
        StringBuilder sb = new StringBuilder();
        switch (d % 12) {
            case 0:
                sb.Append("January");
                break;
            case 1:
                sb.Append("February");
                break;
            case 2:
                sb.Append("March");
                break;
            case 3:
                sb.Append("April");
                break;
            case 4:
                sb.Append("May");
                break;
            case 5:
                sb.Append("June");
                break;
            case 6:
                sb.Append("July");
                break;
            case 7:
                sb.Append("August");
                break;
            case 8:
                sb.Append("September");
                break;
            case 9:
                sb.Append("October");
                break;
            case 10:
                sb.Append("November");
                break;
            case 11:
                sb.Append("December");
                break;
        }

        string year = (ConstantValues.startYear + d / 12).ToString();
        sb.Append($" {year}");
        return sb.ToString();
    }
}
