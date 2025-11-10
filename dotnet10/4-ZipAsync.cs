using System.IO.Compression;
using System.Text;

internal class ZipAsync
{
    public static async Task ExecuteAsync()
    {
        // Extract a Zip archive
        await ZipFile.ExtractToDirectoryAsync("./ZipArchive/archive.zip", "./ZipArchive/Destination", overwriteFiles: true);

        Directory.CreateDirectory("./ZipArchive/Destination");
        File.Delete("./ZipArchive/Destination/archive2.zip");
        // Create a Zip archive
        await ZipFile.CreateFromDirectoryAsync("./ZipArchive/Source", "./ZipArchive/Destination/archive2.zip", CompressionLevel.SmallestSize, includeBaseDirectory: true, entryNameEncoding: Encoding.UTF8);

        // Open an archive
        await using (ZipArchive openArchive = await ZipFile.OpenReadAsync("./ZipArchive/archive.zip"))
        {

        }

        // Fine-grained manipulation
        using FileStream archiveStream = File.Open("./ZipArchive/archive.zip", FileMode.OpenOrCreate);
        await using ZipArchive archive = await ZipArchive.CreateAsync(archiveStream, ZipArchiveMode.Update, leaveOpen: false, entryNameEncoding: Encoding.UTF8);
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            if (string.IsNullOrWhiteSpace(entry.Name)) continue;

            // Extract an entry to the filesystem
            await entry.ExtractToFileAsync(destinationFileName: "file.txt", overwrite: true);

            // Open an entry's stream
            await using Stream entryStream = await entry.OpenAsync();

            using StreamReader reader = new StreamReader(entryStream);
            string content = await reader.ReadToEndAsync();
            Console.WriteLine(entry.FullName + ": " + content);
        }

        // Create an entry from a filesystem object
        ZipArchiveEntry createdEntry = await archive.CreateEntryFromFileAsync(sourceFileName: "ZipArchive/Source/new-file.txt", entryName: "new-file.txt");
    }
}
