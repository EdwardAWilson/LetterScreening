namespace LetterScreen;
using LetterService;

internal class Program
{
    static void Main(string[] args)
    {
        //Make sure that the command has the corrent number of arguments
        int size = args.Length;
        bool valid = true;

        if (size != 1)
        {
            Console.WriteLine("Incorrect number of arguments, please type dotnet run {Path to Files Here}\n");
            return;
        }


        //Check if path is Valid
        string fullPath = Path.GetFullPath(args[0]);
  
        if (!Directory.Exists(fullPath))
        {
            Console.WriteLine("Path Given not Valid");
            return;
        }

        string admissionPath = fullPath + "\\Input\\Admission";
        string scholarshipPath = fullPath + "\\Input\\Scholarship";
        string archivePath = fullPath + "\\Archive";
        string outputPath = fullPath + "\\Output";


        //Get the dates that are inputs for both Admission and Scholarship
        List<string> admissionDates = new List<string>();

        foreach (string s in Directory.GetDirectories(admissionPath, "*", SearchOption.TopDirectoryOnly))
        {
            admissionDates.Add(s.Remove(0, admissionPath.Length));
        }

        List<string> scholarshipDates = new List<string>();

        foreach (string s in Directory.GetDirectories(scholarshipPath, "*", SearchOption.TopDirectoryOnly))
        {
            scholarshipDates.Add(s.Remove(0, scholarshipPath.Length));
        }

        string[] dates = admissionDates.Union(scholarshipDates).ToArray();


        //Copy relevant dates into archive
        foreach (string date in dates)
        {
            string newPath = Path.GetFullPath(archivePath + date);

            if(!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
        }


        //Combine relavent letters into output
        LetterService service = new LetterService();

        string[][] admissionFiles = service.ArchiveFiles(admissionPath, archivePath, dates);
        string[][] scholarshipFiles = service.ArchiveFiles(scholarshipPath, archivePath, dates);

        int processed = 0;
        List<string> processedIDs = new List<string>();

        for (int date = 0; date < dates.Length; date++)
        {
            IDictionary<string, string> studentIDList = new Dictionary<string, string>();
            foreach (string file in admissionFiles[date])
            {
                studentIDList.Add(file.Split('-')[1].Substring(0, 8), file);
            }

            foreach (string file in scholarshipFiles[date])
            {
                string id = file.Split('-')[1].Substring(0, 8);

                if (studentIDList.ContainsKey(id))
                {
                    service.CombineTwoLetters(admissionPath + studentIDList[id], scholarshipPath + file, outputPath + dates[date] + "\\" + id + ".txt");
                    processed++;
                    processedIDs.Add(id);
                }
            }
        }


        //Remove the now processed files from input
        foreach(string date in dates)
        {
            try
            {
                Directory.Delete(admissionPath + date, true);
            }
            catch(Exception e)
            {
                //If directory doesn't exist here ignore and continue
            }

            try
            {
                Directory.Delete(scholarshipPath + date, true);
            }
            catch (Exception e)
            {
                //If directory doesn't exist here ignore and continue
            }
        }


        //Print off report for the day
        DateTime today = DateTime.Today;

        Console.WriteLine(today.ToString("MM/dd/yyyy") + " Report");
        Console.WriteLine("-------------------------------");
        Console.WriteLine();
        Console.WriteLine("Number of Combined Letters: " + processed);

        foreach(string id in processedIDs)
        {
            Console.WriteLine("\t" + id);
        }

        return;
    }
}