using System.Configuration;
using RestSharp;

namespace MailAPI
{
    public class MailService : IMailService
    {
        public bool AddNewUser(string mailingList, string email)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               ConfigurationManager.AppSettings["MAILGUN_API_KEY"]);
            RestRequest request = new RestRequest();
            request.Resource = "lists/{list}/members";
            request.AddParameter("list", mailingList, ParameterType.UrlSegment);
            request.AddParameter("address", email);
            request.AddParameter("subscribed", true);
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);
            return response.ResponseStatus == ResponseStatus.Completed;
        }

        public bool SendMail(string mailingList, string message)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                new HttpBasicAuthenticator("api",
                                           ConfigurationManager.AppSettings["MAILGUN_API_KEY"]);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 ConfigurationManager.AppSettings["MAIL_DOMAIN"], ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "movies.remember@movies.fr");
            request.AddParameter("to", mailingList);
            request.AddParameter("subject", "Sortie ciné");
            request.AddParameter("html", message);
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);
            return response.ResponseStatus == ResponseStatus.Completed;
        }
    }
}
