using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkatScoring.Contracts.Entities;

namespace SkatScoring.Contracts.Database
{
    public interface ISkatUserRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skatuser"></param>
        /// <returns></returns>
        Task AddNewSkatUserAsync(SkatUser skatuser);

        Task<List<SkatUser>> ListSkatUserAsync();

        Task UpdateSkatUserAsync(SkatUser skatuser);

        Task<SkatUser> GetSkatUserByNameAsync(String name);
    }
}