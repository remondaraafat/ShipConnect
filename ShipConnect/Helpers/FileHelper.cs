namespace ShipConnect.Helpers
{
    public class FileHelper
    {
       
            public static async Task<string> UploadFileAsync(IFormFile imageFile)
            {
                //File Size
                if (imageFile == null || imageFile.Length == 0)
                {
                    return "File can't be Null ";
                }

                if (imageFile.Length > 5 * 1024 * 1024) // 5MB limit
                {
                    return "Maximum size can be 5 MB";
                }

                //Extension
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return $"Extension is Not Valid ({string.Join(",", extension)})";
                }

                //Name changing
                try
                {
                    string fileName = Guid.NewGuid().ToString() + extension;

                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    return "/uploads/" + fileName;
                }
                catch (Exception ex)
                {

                    return $"Error uploading image: {ex.Message}";
                }
            }
            public static string GetFilePath(string fileName)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return null;
                }
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                return Path.Combine(uploadsFolder, fileName);
            }

        
    }
}
