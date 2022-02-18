using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using ServerForReact.Abstract;
using System;
using System.IO;
using System.Threading.Tasks;
using EmptyModelMetadataProvider = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider;

namespace ServerForReact.Services
{
    public class ViewsHelper : IViewsHelper
    {
        private IRazorViewEngine razorViewEngine;
        private IServiceProvider serviceProvider;
        private ITempDataProvider tempDataProvider;
        public ViewsHelper(IRazorViewEngine razorViewEngine, IServiceProvider serviceProvider, ITempDataProvider tempDataProvider)
        {
            this.razorViewEngine = razorViewEngine;
            this.serviceProvider = serviceProvider;
            this.tempDataProvider = tempDataProvider;
        }
        public async Task<string> GetViewToHtmlAsync<T>(string viewName, T model)
        {
            var httpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProvider
            };

            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (StringWriter sw = new StringWriter())
            {
                var viewResult = razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    return string.Empty;
                }

                var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                                                   actionContext,
                                                   viewResult.View,
                                                   viewDataDictionary,
                                                   new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                                                   sw,
                                                   new HtmlHelperOptions()
                                                 );

                await viewResult.View.RenderAsync(viewContext);

                return sw.ToString();
            }
        }
    }
}
