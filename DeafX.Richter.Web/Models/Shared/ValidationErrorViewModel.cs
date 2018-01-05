using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Models.Shared
{
    public class ValidationErrorsViewModel
    {
        public ValidationErrorViewModel[] errors { get; set; }
    }

    public class ValidationErrorViewModel
    {
        public string field { get; set; }

        public string errorMessage { get; set; }
    }
}
