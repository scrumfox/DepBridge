using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace depBridger.Service
{
    public class BOMTransformerService
    {
        public string Transform(string fullpath, string fileName = "")
        {
            XslTransform xslt = new XslTransform();
            var bomXsltPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            xslt.Load(bomXsltPath + @"\bom.xslt");
            var targetFile =  @"bom_" + fileName;
            xslt.Transform(fullpath, bomXsltPath + @"\"+ targetFile);

            return targetFile;
        }
    }
}
