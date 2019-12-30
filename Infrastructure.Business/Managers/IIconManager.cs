using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IIconManager
    {
        Task<OperationDetails> Create(IconDto iconDto);
        Task<int> CreateAndGetIconId(IconDto iconDto);
        Task<OperationDetails> Update(IconDto iconDto);
        IconDto GetById(int id);
    }
}
