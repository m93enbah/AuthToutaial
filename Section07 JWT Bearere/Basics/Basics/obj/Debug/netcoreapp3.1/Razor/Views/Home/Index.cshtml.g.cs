#pragma checksum "C:\Users\m.enbeh\source\repos\AuthToutaial\Section06 Razor authentication\Basics\Basics\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5d63e66ae34a418c0856875fe229384faaece48d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 15 "C:\Users\m.enbeh\source\repos\AuthToutaial\Section06 Razor authentication\Basics\Basics\Views\Home\Index.cshtml"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5d63e66ae34a418c0856875fe229384faaece48d", @"/Views/Home/Index.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<h1>Hello World</h1>\r\n\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\m.enbeh\source\repos\AuthToutaial\Section06 Razor authentication\Basics\Basics\Views\Home\Index.cshtml"
 if (User.Identity.IsAuthenticated)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>User is authenticated</h1>\r\n");
#nullable restore
#line 8 "C:\Users\m.enbeh\source\repos\AuthToutaial\Section06 Razor authentication\Basics\Basics\Views\Home\Index.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>User is not authenticated</h1>\r\n");
#nullable restore
#line 12 "C:\Users\m.enbeh\source\repos\AuthToutaial\Section06 Razor authentication\Basics\Basics\Views\Home\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 17 "C:\Users\m.enbeh\source\repos\AuthToutaial\Section06 Razor authentication\Basics\Basics\Views\Home\Index.cshtml"
 if ((await authorizationService.AuthorizeAsync(User, "Claim.DoB")).Succeeded)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>User has DOB Claim</h1>\r\n");
#nullable restore
#line 20 "C:\Users\m.enbeh\source\repos\AuthToutaial\Section06 Razor authentication\Basics\Basics\Views\Home\Index.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IAuthorizationService authorizationService { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
