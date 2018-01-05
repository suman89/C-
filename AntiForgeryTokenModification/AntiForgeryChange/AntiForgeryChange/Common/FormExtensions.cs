using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AntiForgeryChange.Common
{
    public static class FormExtensions
    {

        public static MvcForm BeginDataForm(this HtmlHelper html, string action, string controller, FormMethod method, object htmlAttributes)
        {
            var form = html.BeginForm(action, controller, method, htmlAttributes);
            //At this point, the form markup is rendered in BeginForm
            // we can render the token

            //With every form, we render a token, since this
            //assumes all forms are posts
            html.ViewContext.Writer.Write(html.AntiForgeryToken().ToHtmlString());

            return form;
        }

        public static MvcHtmlString ModifiedAntiForgeryToken(this HtmlHelper html)
        {
            var test = html.AntiForgeryToken();
            var check = test.ToHtmlString();
            var check1 = test.ToString();
            return test;
        }
    }
}
