using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Randomizer
{
    public class Randomizer
    {
        public enum Exam
        {
            GA1_AE, GA1_SI, GA2, AP1
        }

        public Randomizer()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public void Start(string directory, string examName)
        {
            List<string> paths = new List<string>();

            

            int count = 5;

            if (examName.Contains("AP1"))
                count = 4;
            
            // Sammelt die Ordner als Dateipfad ab um eine Liste daraus zu machen
            var directories = Directory.GetDirectories(directory, "*", System.IO.SearchOption.AllDirectories);

            foreach (string path in directories)
            {
                // Hier wird überprüft ob alle Dateien vorhanden sind, fehlt eine oder mehr Dateien, wird der Ordner nicht eingetragen
                if (CheckForFiles(path, count))
                {
                    paths.Add(path);
                }
            }

            if (paths.Count > 0)
            {
                var newPaths = Randomize(paths.ToArray(), count);
                PDFConverter(newPaths, examName);
            }
            else
            {
                Console.WriteLine("Keine Dateien gefunden");
            }
        }

        

        public bool CheckForFiles(string filePath, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                // Überprüft ob es 4(AP1) oder 5 GA1-2 Dateien gibt, die in Handlungsschritten unterteilt wurden
                string pdf = filePath + @"\" + i + ".pdf";
                if (!File.Exists(pdf))
                {
                    return false;
                }
            }
            return true;
        }

        public void CheckForDirectory(string directory)
        {
            // Überprüft ob ein Ordner existiert, wenn nicht, wird dieser erstellt.
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public string[] Randomize(string[] filePaths, int count)
        {
            Random random = new Random();

            List<string> newFilePaths = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                newFilePaths.Add(filePaths[random.Next(0, filePaths.Length)] + $"\\{i}.pdf");
            }

            return newFilePaths.ToArray();
        }
        public void PDFConverter(string[] pdfFiles, string examName)
        {
            
            if (!Directory.Exists("out"))
            {
                Directory.CreateDirectory("out");
            }

            // Damit der Dateiname mit AP1, GA1, GA2 usw anfängt.
            string fileName = examName + "_";

            // Unterteil den pdf datei Pfad und bekommt so den Namen des Ordners als Dateiname z.B. SO-15 für Sommer 2015
            foreach (var item in pdfFiles)
            {
                var split = item.Split('\\');
                fileName += split[split.Length - 2] + "_";
            }
            fileName += DateOnly.FromDateTime(DateTime.Now);

            

            PdfDocument one = PdfReader.Open(pdfFiles[0], PdfDocumentOpenMode.Import);
            PdfDocument two = PdfReader.Open(pdfFiles[1], PdfDocumentOpenMode.Import);
            PdfDocument three = PdfReader.Open(pdfFiles[2], PdfDocumentOpenMode.Import);
            PdfDocument four = PdfReader.Open(pdfFiles[3], PdfDocumentOpenMode.Import);
            PdfDocument five = new PdfDocument();
            if (!examName.Contains("AP1"))
                five = PdfReader.Open(pdfFiles[4], PdfDocumentOpenMode.Import);

            using (PdfDocument outPdf = new PdfDocument())
            {
                CopyPages(one, outPdf);
                CopyPages(two, outPdf);
                CopyPages(three, outPdf);
                CopyPages(four, outPdf);
                if (!examName.Contains("AP1"))
                    CopyPages(five, outPdf);

                // Speichert die neue PDF in den Out Ordner ab
                outPdf.Save($"out\\{fileName}.pdf");
            }

            // Öffnet den Out Ordner
            Process.Start("explorer.exe", "out");
        }

        void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }

    }
}
