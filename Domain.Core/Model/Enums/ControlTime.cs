using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Core.Model.Enums
{
    public enum ControlSeconds
    {
        [Display(Name = "1 second")]
        Sec1 = 1,
        [Display(Name = "3 second")]
        Sec2 = 3,
        [Display(Name = "5 second")]
        Sec3 = 5,
        [Display(Name = "10 second")]
        Sec10 = 10,
    }

    public enum ControlHouers
    {
        [Display(Name = "1 hour")]
        Hour1 = 1,

        [Display(Name = "3 hours")]
        Hour3 = 3,

        [Display(Name = "6 hours")]
        Hour6 = 6,

        [Display(Name = "12 hours")]
        Hour12 = 12
    }

    public enum ControlDays
    {
        [Display(Name = "1 day")]
        Hour24 = 24,

        [Display(Name = "3 day")]
        Hour72 = 72,

        [Display(Name = "6 day")]
        Hour144 = 144,

        [Display(Name = "12 day")]
        Hour288 = 288
    }
}
