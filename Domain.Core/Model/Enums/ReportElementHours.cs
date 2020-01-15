using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Core.Model.Enums
{
    public enum ReportElementHours
    {
        [Display(Name  ="All time")]
        AllTime = 0,

        [Display(Name = "1 hour")]
        Hour1 = 1,

        [Display(Name = "3 hours")]
        Hour3 = 3,

        [Display(Name = "6 hours")]
        Hour6 = 6,

        [Display(Name = "12 hours")]
        Hour12 = 12,

        [Display(Name = "1 day")]
        Hour24 = 24,

        [Display(Name = "7 days")]
        Hour168 = 168,

        [Display(Name = "14 days")]
        Hour336 = 336,

        [Display(Name = "30 days")]
        Hour720 = 720,

        [Display(Name = "90 days")]
        Hour2160 = 2160
    }
}
