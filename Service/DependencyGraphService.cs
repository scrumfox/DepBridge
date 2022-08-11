using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace depBridger.Service
{
    public class DependencyGraphService
    {
        public bool UploadBom(string bomFilename)
        {
            // full path to file in temp location
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", bomFilename);

            var configurationService = new ConfigurationService();
            var config = configurationService.ReadConfiguration();
            string projectUid = config.ProjectUid;
            string apiKey = config.ApiKey;
            string uri = config.ApiEndPoint;
            
            string contentType = "multipart/form-data";
            var xml = System.IO.File.ReadAllText(path);
            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler);
            var multipartContent = new MultipartFormDataContent();
            var projectHeader = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
            projectHeader.Name = "project";
            var projectContent = new System.Net.Http.StringContent(projectUid);
            projectContent.Headers.ContentDisposition = projectHeader;

            multipartContent.Add(projectContent);

            var bomHeader = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
            bomHeader.Name = "bom";
            var bomContent = new System.Net.Http.StringContent(xml);
            bomContent.Headers.ContentDisposition = bomHeader;
            multipartContent.Add(bomContent);

            httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            var response = httpClient.PostAsync(uri, multipartContent).Result;

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
