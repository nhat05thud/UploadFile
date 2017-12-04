using CMS.EntityFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.EntityFrameWork.Context
{
    public class UploadImageContext : DbContext
    {
        public UploadImageContext() : base("UploadImageContext"){

        }
        public virtual DbSet<image> images { get; set; }
    }
}
