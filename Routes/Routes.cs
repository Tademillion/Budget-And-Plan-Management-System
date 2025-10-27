using Microsoft.AspNetCore.Mvc.ApplicationModels;

[AttributeUsage(AttributeTargets.Method)]
public class BaseUrlRouteAttribute : Attribute, IActionModelConvention
{
    private readonly string _baseUrl;

    public BaseUrlRouteAttribute()
    {
        _baseUrl = "Bplan_api/app";
    }
    public void Apply(ActionModel action)
    {
        foreach (var selector in action.Selectors)
        {
            selector.AttributeRouteModel.Template = _baseUrl + "/" + selector.AttributeRouteModel.Template;
        }
    }
}
