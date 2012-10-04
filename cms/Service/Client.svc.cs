//using System;
//using System.ServiceModel.Activation;
//using Newtonsoft.Json;
//using cms.data.Dtos;
//using cms.data.EF;

////NOTE:asi zatim neni potreba

//namespace cms.Service
//{
//    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
//    public class Client : IClient
//    {
//        public string GetGridpageAction(string application, string link)
//        {
//            var db = new DataEfAuthorized(application);
//            var data = db.Get(link);

//            var settings = new JsonSerializerSettings()
//            {
//                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//            };
//            var ret = JsonConvert.SerializeObject(data, Formatting.Indented,settings);

//            return ret;
//        }

//        public GridPageDto GetGridpageJson(string ApplicationName, string Link)
//        {
//            if (Link == null) throw new ArgumentNullException("Link");
//            if (ApplicationName == null) throw new ArgumentNullException("ApplicationName");

//            var db = new DataEfAuthorized(ApplicationName);
//            return db.Get(Link);
//        }
		
//        public string EchoJson(string message)
//        {
//            return "UpdateResource" + message;
//        }

//        public string Echo()
//        {
//            //var fault = new MyApplicationFault
//            //{
//            //    Message = "ljkabsdjbasjkdbjkasd EEE"
//            //};
//            //throw new FaultException<MyApplicationFault>(fault);

//            return "xkjbsjkabd";
//        }

//    }
//}
