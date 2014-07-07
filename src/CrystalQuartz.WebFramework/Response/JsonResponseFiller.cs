﻿using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using CrystalQuartz.Web.FrontController.ResponseFilling;

namespace CrystalQuartz.WebFramework.Response
{
    public class JsonResponseFiller : DefaultResponseFiller
    {
        private readonly object _model;

        public JsonResponseFiller(object model)
        {
            _model = model;
        }

        public override string ContentType
        {
            get { return "application/json"; }
        }

        protected override void InternalFillResponse(HttpResponseBase response, HttpContextBase context)
        {
            var serialized = new JavaScriptSerializer().Serialize(_model);
            using (var writer = new StreamWriter(response.OutputStream))
            {
                writer.WriteLine(serialized);
            }
        }
    }
}