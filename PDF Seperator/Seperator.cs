using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Reflection.PortableExecutable;

namespace PDF_Seperator
{
    public class Seperator
    {
        public string FilePath { get; set; } = "Files";

        public Seperator(string filePath)
        {
            FilePath = filePath;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public void Start(string directory)
        {
            
               string path = FilePath + $"\\{directory}";

            if (Directory.Exists(path))
            {
                var config = LoadConfig(path);
                var result = new List<IHKPDF>();

                foreach (var item in config)
                {
                    if (FileExists(item.FileName, path))
                    {
                        result.Add(new IHKPDF()
                        {
                            FileName = item.FileName,
                            Sites = item.Sites
                        });
                    }
                    else
                    {
                        Console.WriteLine($"File {item.FileName} not found");
                    }
                }

                foreach (var item in result)
                {
                    SeperatePDF(item.FileName, item.Sites, path);
                }
            }
            else
            {
                Console.WriteLine($"Directory: {directory} dont exist");
            }
        }

        public IHKPDF[] LoadConfig(string filePath)
        {
            string[] file = new string[1];
            List<IHKPDF> iHKPDFs = new List<IHKPDF>();

            if (File.Exists($"{filePath}\\Config.txt"))
            {
                file = File.ReadAllLines($"{filePath}\\Config.txt");


                foreach (var f in file)
                {
                    var split = f.Split(';');
                    if (split.Length > 0)
                    {
                        var sites = new List<int>();
                        for (int i = 1; i < split.Length; i++)
                        {
                            sites.Add(IsNumber(split[i]));
                        }
                        iHKPDFs.Add(new IHKPDF()
                        {
                            FileName = split[0],
                            Sites = sites.ToArray()
                        });
                    }
                }
            }
            else
            {
                File.Create($"{filePath}\\Config.txt");
            }
            return iHKPDFs.ToArray();
        }

        private int IsNumber(string site)
        {
            bool isNumber = int.TryParse(site, out var number);

            if (isNumber)
                return number;
            return 0;
        }

        public bool FileExists(string file, string filePath)
        {
            return File.Exists($"{filePath}\\" + file + ".pdf");
        }

        public void SeperatePDF(string file, int[] sites, string filePath)
        {
            if (file != String.Empty)
            {
                PdfDocument pdf = PdfReader.Open(filePath + "\\" + file + ".pdf", PdfDocumentOpenMode.Import);

                if (!Directory.Exists($"{filePath}\\{file}"))
                    Directory.CreateDirectory($"{filePath}\\{file}");

                int site = 1;
                for (int i = 0; i < sites.Length; i++)
                {
                    PdfDocument result = new PdfDocument();

                    int a = site;
                    for (int j = a; j < a + sites[i]; j++)
                    {
                        result.AddPage(pdf.Pages[j]);
                        site++;
                    }
                    result.Save($"{filePath}\\{file}\\{i + 1}.pdf");
                }
            }
            else
            {
                Console.WriteLine("No File found!");
            }
        }

    }
}
