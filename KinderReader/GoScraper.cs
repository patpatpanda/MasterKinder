using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinderReader
{
    internal class GoScraper
    {
        private readonly PdfScrapingService _pdfScrapingService;

        public GoScraper(PdfScrapingService pdfScrapingService)
        {
            _pdfScrapingService = pdfScrapingService;
        }

        public async Task<bool> Scrape(int id)
        {
            // Implementera skraplogik baserat på id, om det behövs
            // För enkelhets skull, kan vi direkt kalla skrapningstjänsten med en fast URL
            string url = "https://ssan.stockholm.se/anonym/webdokument/Delade%20dokument/Forms/AllItems.aspx?RootFolder=%2fanonym%2fwebdokument%2fDelade%20dokument%2fF%c3%b6rskolor%2f2024%2fNorra%20innerstaden&FolderCTID=0x01200015B00A3B7947284E8A98F455403CF440";
            await _pdfScrapingService.ScrapeAndSaveSurveyData(url);
            return true;
        }
    }
}
