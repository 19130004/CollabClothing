﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Brands
{
    public class BrandEditRequest
    {
        public string NameBrand { get; set; }
        public string Info { get; set; }
        public IFormFile Images { get; set; }
        public string Slug { get; set; }
        public string ImagesPath { get; set; }
    }
}
