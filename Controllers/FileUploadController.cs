using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using depBridger.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace depBridger.Controllers
{
	public class FileUploadController : Controller
    {
		[HttpPost("FileUpload")]
		public async Task<IActionResult> Index(List<IFormFile> files)
		{
			long size = files.Sum(f => f.Length);

			if (files == null || size == 0)
				return Content("file not selected");

			var filePaths = new List<string>();
			var fCount = 1;
			foreach (var formFile in files)
			{
				if (formFile.Length > 0)
				{
					// full path to file in temp location
					var targetFile = fCount.ToString() + "_" + formFile.FileName;
					var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", targetFile);

					using (var stream = new FileStream(path, FileMode.Create))
					{
						await formFile.CopyToAsync(stream);
					}

					var bomTransformerService = new BOMTransformerService();
					var bomFile = bomTransformerService.Transform(path, targetFile);

					var dependencyGraphService = new DependencyGraphService();
					dependencyGraphService.UploadBom(bomFile);
				}

				fCount++;
			}

			Cleanup();
			// process uploaded files
			// Don't rely on or trust the FileName property without validation.
			return Ok(new { count = files.Count, size, filePaths });
		}

		private void Cleanup()
        {
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			string[] fileList = System.IO.Directory.GetFiles(path, @"*.xml");
			foreach (string file in fileList)
			{
				System.IO.File.Delete(file);
			}
		}
	}
}