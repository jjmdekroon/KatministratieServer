using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Web;

namespace Superkatten.Katministratie.Host.Helpers;

public static class ExtensionMethods
{
    public static NameValueCollection QueryString(this NavigationManager navigationManager)
    {
        return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
    }

    public static string QueryString(this NavigationManager navigationManager, string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ApplicationException("Key empty or null is not allowed");
        }

        return navigationManager.QueryString()[key] ?? navigationManager.BaseUri;
    }
}
