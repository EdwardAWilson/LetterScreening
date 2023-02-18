namespace LetterScreen;

internal class Program
{
    static void Main(string[] args)
    {
        int size = args.Length;
        bool valid = true;

        if (size != 1)
        {
            Console.WriteLine("Incorrect number of arguments, please type dotnet run {Path to Files Here}\n");
            return;
        }

        //Check if path is Valid
        string FullPath = Path.GetFullPath(args[0]);
  
        if (!Directory.Exists(FullPath))
        {
            Console.WriteLine("Path Given not Valid");
            return;
        }

        string AdmissionPath = FullPath + "\\Input\\Admission";
        string ScholarshipPath = FullPath + "\\Input\\Scholarship";
        string ArchivePath = FullPath + "\\Archive";

        //Get the dates that are inputs for both Admission and Scholarship
        List<string> AdmissionDates = new List<string>();

        foreach (string s in Directory.GetDirectories(AdmissionPath, "*", SearchOption.TopDirectoryOnly))
        {
            AdmissionDates.Add(s.Remove(0, AdmissionPath.Length));
        }

        List<string> ScholarshipDates = new List<string>();

        foreach (string s in Directory.GetDirectories(ScholarshipPath, "*", SearchOption.TopDirectoryOnly))
        {
            ScholarshipDates.Add(s.Remove(0, ScholarshipPath.Length));
        }

        string[] Dates = AdmissionDates.Union(ScholarshipDates).ToArray();

        //Copy relevant dates into archive
        foreach (string Date in Dates)
        {
            string newPath = Path.GetFullPath(ArchivePath + Date);

            if(!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
        }

        archiveFiles(AdmissionPath, ArchivePath, Dates);
    }

    public static void archiveFiles(string InputPath, string OutputPath, string[] Dates)
    {
        foreach (string Date in Dates)
        {
            if (Directory.Exists(InputPath + Date))
            {
                string[] FilePaths = Directory.GetFiles(InputPath + Date);
                
                foreach (string s in FilePaths)
                {
                    string fileName = Path.GetFileName(s);
                    string destFile = Path.Combine(OutputPath + Date, fileName);
                    File.Copy(s, destFile, true);
                }
            }
        }
    }
}