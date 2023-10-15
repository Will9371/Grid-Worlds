// Source: https://gist.github.com/Mrchrissross/f72f085c1f354e70732089745aba05aa

using UnityEngine;

public class VectorLabelsAttribute : PropertyAttribute
{
    public readonly string[] Labels;
    public VectorLabelsAttribute(params string[] labels) { Labels = labels; }
}