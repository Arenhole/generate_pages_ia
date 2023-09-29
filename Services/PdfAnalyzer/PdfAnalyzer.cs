using UglyToad.PdfPig;

namespace Services.PdfAnalyzer
{

    public class PdfAnalyzer
    {
        public static string analyze(string path)
        {
            var pdf = PdfDocument.Open(path);
            var page = pdf.GetPage(1);
            string text = page.Text;
            return text;
        }
    }
}