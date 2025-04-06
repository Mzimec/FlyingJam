using UnityEngine;
using UnityEngine.UIElements;

static class ConstantValues {
    public static int cardTypesCount = 4;
    public static float chanceToBeAttacked = 0.5f;
    public static int regionCount = 30;

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
}
