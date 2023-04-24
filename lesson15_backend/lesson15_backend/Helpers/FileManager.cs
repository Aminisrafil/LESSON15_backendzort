namespace lesson11_backend.Helpers
{
    public static class FileManager
    {
        public static string Save(IFormFile file, string folder)
        {
            string newFileName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(folder, newFileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return newFileName;
        }
    }
}
