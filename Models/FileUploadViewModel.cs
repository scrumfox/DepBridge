﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace depBridger.Models
{
    public class FileUploadViewModel
    {
        [NotMapped]
        public IFormFile formFile;
    }
}
