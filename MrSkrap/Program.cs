using System;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        string path = @"C:\Users\Nils-\OneDrive\Skrivbord\Alla\Förskolan Kåxis - Vårdnadshavare Förskola.pdf";
        PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));

        // Define questions for each page
        var pageQuestions = new Dictionary<int, string[]>
        {
            {14, new string[]
                {
                    "Jag upplever att mitt barn utvecklas och lär i förskolan",
                    "Jag upplever att mitt barns språkliga förmåga utvecklas i förskolan",
                    "Jag upplever att mitt barn får det stöd som behövs i förskolan"
                }
            },
            {15, new string[]
                {
                    "Jag upplever att förskolan bidrar till att mitt barn visar ett intresse för hållbar utveckling",
                    "Jag får information om mitt barns utveckling och lärande"
                }
            },
            {16, new string[]
                {
                    "Jag upplever att mitt barn känner sig tryggt på förskolan",
                    "Jag upplever att förskolan bidrar till att mitt barn utvecklar förmåga till empati, tolerans och omtanke",
                    "Jag upplever att personalen visar omsorg om mitt barn"
                }
            },
            {17, new string[]
                {
                    "Jag upplever att förskolan bidrar till att mitt barn utvecklar en positiv bild av sig själv",
                    "Jag upplever att förskolan medvetet främjar alla barns möjligheter att utvecklas på lika villkor oavsett kön"
                }
            },
            {18, new string[]
                {
                    "Jag upplever att förskolans ledning är tillgängliga vid behov",
                    "Jag upplever att personalen bemöter mig på ett sätt som skapar förtroende och tillit"
                }
            },
            {19, new string[]
                {
                    "Jag är nöjd med informationen jag får om maten som serveras på förskolan",
                    "Jag upplever att förskolan bidrar till att mitt barn dagligen deltar i fysiska aktiviteter"
                }
            }
        };

        // Extract data from specified pages
        foreach (var kvp in pageQuestions)
        {
            ExtractDataFromPage(pdfDoc, kvp.Key, kvp.Value);
        }

        pdfDoc.Close();
    }

    static void ExtractDataFromPage(PdfDocument pdfDoc, int pageNumber, string[] questions)
    {
        string pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(pageNumber));

        Console.WriteLine($"\nExtracted Text from Page {pageNumber}:");
        Console.WriteLine(pageText);

        foreach (var question in questions)
        {
            string pattern = $@"{Regex.Escape(question)}.*?Förskolan Kåxis 2024\s+(\d{{1,3}})";
            Console.WriteLine($"Regex pattern: {pattern}");
            ExtractAndPrintDataForQuestion(pageText, question, pattern);
        }
    }

    static void ExtractAndPrintDataForQuestion(string text, string question, string pattern)
    {
        // Extract the first number after "Förskolan Kåxis 2024"
        Regex regex = new Regex(pattern, RegexOptions.Singleline);
        Match match = regex.Match(text);

        if (match.Success)
        {
            Console.WriteLine($"{question}:");
            Console.WriteLine($"Andel instämmer (%): {match.Groups[1].Value}%");
        }
        else
        {
            Console.WriteLine($"{question}: Data not found.");
        }
    }
}