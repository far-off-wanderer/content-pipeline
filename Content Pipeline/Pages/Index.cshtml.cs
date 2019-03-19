using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IO = System.IO;

namespace Content_Pipeline.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            var files = IO.Directory.GetFiles(@"..\data\original");
            Files = files;
        }

        public async Task OnPost(string name, IFormFile file)
        {
            if (string.IsNullOrEmpty(name) == false && IO.Path.GetExtension(file.FileName).Substring(1) == "txt")
            {
                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    IO.File.WriteAllText($@"..\data\original\{name}.txt", await reader.ReadToEndAsync());
                }
            }
            OnGet();

            Process(Files);
        }

        private void Process(string[] files)
        {
            foreach (var input in files)
            {
                var output = input.Replace(@"\original\", @"\processed\");

                var contents = IO.File.ReadAllText(input);

                IO.File.WriteAllText(output, contents.ToUpper());
            }
        }

        public string[] Files { get; private set; }
    }
}