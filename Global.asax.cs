using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Finance.Controllers;
using System.Xml;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;
using System.Xml.Linq;
using System.Data.Entity.Core.EntityClient;
using System.Data.Common;

namespace Finance
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //http://blogs.microsoft.co.il/idof/2008/08/22/change-entity-framework-storage-db-schema-in-runtime/#commentmessage
            //https://stackoverflow.com/questions/2663164/changing-schema-name-on-runtime-entity-framework?rq=1
            //

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Database.SetInitializer(new FinanceDbInitialize());
            //Finance.Models.DbInit.Init();



            //var conceptualReader = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("Model1.csdl"));
            //var mappingReader = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("Model1.msl"));
            //var storageReader = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("Model1.ssdl"));

            //XNamespace storageNS = "http://schemas.microsoft.com/ado/2009/11/edm/ssdl";

            //var storageXml = XElement.Load(storageReader);
            //var conceptualXml = XElement.Load(conceptualReader);
            //var mappingXml = XElement.Load(mappingReader);

            //foreach (var entitySet in storageXml.Descendants(storageNS + "EntitySet"))
            //{
            //    var schemaAttribute = entitySet.Attributes("Schema").FirstOrDefault();
            //    //if (schemaAttribute != null)
            //    //{
            //    //    schemaAttribute.SetValue(schema);
            //    //}
            //}

            //storageXml.Save("temp.ssdl");
            //conceptualXml.Save("temp.csdl");
            //mappingXml.Save("temp.msl");

            //MetadataWorkspace workspace = new MetadataWorkspace(new List<String>(){
            //                                                    @"temp.csdl",
            //                                                    @"temp.ssdl",
            //                                                    @"temp.msl"
            //                                            }
            //                                               , new List<Assembly>());


            
        }
    }
}
