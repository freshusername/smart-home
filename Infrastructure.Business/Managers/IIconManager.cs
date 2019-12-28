using Infrastructure.Business.DTOs.Icon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Managers
{
    public interface IIconManager
    {
        void Insert(IconDto iconDto);
        int InsertGetIconId(IconDto iconDto);
        void Update(IconDto iconDto);
        IconDto GetById(int id);
    }
}
