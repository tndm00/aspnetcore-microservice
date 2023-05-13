using System.Text;

namespace Basket.API.services
{
    public class EmailTemplateService
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _tmpFolder = Path.Combine(_baseDirectory, "EmailTemplates");

        protected string ReadEmailTemplateContent(string templateEmailName, string format = "html")
        {
            var filePath = Path.Combine(_tmpFolder, templateEmailName + "." + format);
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            var emailText = sr.ReadToEnd();
            sr.Close();

            return emailText;
        }
    }
}
