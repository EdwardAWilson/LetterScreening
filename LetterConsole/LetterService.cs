namespace LetterService;

public interface ILetterService
{
    /*
     * <summary>
     * Copy all the files given in the first directory given to the second directory given.
     * Return a list of all files copied over organized by the date
     * </summary>
     * 
     * <param name="inputPath">Directory path for the directory to be copied from.</param>
     * <param name="outputPath">Directory path for the directory to be copied to.</param>
     * <param name="dates">The list of dates which will be accessed and copied from.</param>
     * 
     * <return>
     * A 2D array where the first dimension corresponds each date as provided to the function
     * and the second dimension corresponds to the files that were processed for that date.
     * </return>
    */
    string[][] ArchiveFiles(string inputPath, string outputPath, string[] dates);

    /*
     * <summary>
     * Combine two letter files into one file.
     * </summary>
     * 
     * <param name="inputFile1">File path for the first letter.</param>
     * <param name="inputFile2">File path for the second letter.</param>
     * <param name="resultFile">File path for the combined letter.</param>
     */
    void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile);
}

public class LetterService : ILetterService
{
    public void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile)
    {
        Console.WriteLine(inputFile1);
        Console.WriteLine(inputFile2);
        Console.WriteLine(resultFile);
        Console.WriteLine();

        Directory.CreateDirectory(Path.GetDirectoryName(resultFile));

        //This won't work for extremely long text files, but for this exercize it works fine.
        File.WriteAllText(resultFile, File.ReadAllText(inputFile1) + File.ReadAllText(inputFile2));
    }

    public string[][] ArchiveFiles(string inputPath, string outputPath, string[] dates)
    {
        List<List<string>> datedFiles = new List<List<string>>();

        foreach (string date in dates)
        {
            List<string> files = new List<string>();

            if (Directory.Exists(inputPath + date))
            {
                string[] FilePaths = Directory.GetFiles(inputPath + date);

                foreach (string s in FilePaths)
                {
                    string fileName = Path.GetFileName(s);
                    string destFile = Path.Combine(outputPath + date, fileName);
                    files.Add(date + "\\" + fileName);
                    File.Copy(s, destFile, true);
                }
            }

            datedFiles.Add(files);
        }

        string[][] datedFilesArray = datedFiles.Select(a => a.ToArray()).ToArray();

        return datedFilesArray;
    }
}