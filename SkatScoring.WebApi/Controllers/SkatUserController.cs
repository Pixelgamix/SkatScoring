using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SkatScoring.Contracts.Database;
using SkatScoring.Contracts.DomainModel;
using SkatScoring.Contracts.Entities;

namespace SkatScoring.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkatUserController : ControllerBase
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly ILogger<SkatUserController> _logger;

        public SkatUserController(ILogger<SkatUserController> logger, IDatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<List<SkatUserModel>> GetSkatUserList()
        {
            List<SkatUser> skatuserlist = null;
            List<SkatUserModel> skatUserModels = null;

            await _databaseContext.ExecuteAsync(async context =>
            {
                skatuserlist = await context.SkatUserRepository.ListSkatUserAsync();

                skatUserModels = skatuserlist.Select(x => new SkatUserModel()
                    {IsSkatUserValid = x.UserValid, SkatUserName = x.UserName}).ToList();
            });

            return skatUserModels;
        }

        [HttpGet("{skatusername}")]
        public async Task<SkatUserModel> GetSkatUser(String skatusername)
        {
            SkatUserModel skatusermodel = null;
            SkatUser skatuser = null;

            await _databaseContext.ExecuteAsync(async context =>
            {
                skatuser = await context.SkatUserRepository.GetSkatUserByNameAsync(skatusername);
                if (skatuser != null)
                    skatusermodel = new SkatUserModel()
                        {IsSkatUserValid = skatuser.UserValid, SkatUserName = skatuser.UserName};
            });

            return skatusermodel;
        }

        [HttpPost]
        public async Task AddSkatUser([FromBody] SkatUserModel skatUserModel)
        {
            SkatUser skatuser = new SkatUser()
                {UserName = skatUserModel.SkatUserName, UserValid = skatUserModel.IsSkatUserValid};

            await _databaseContext.ExecuteAsync(async context =>
            {
                await context.SkatUserRepository.AddNewSkatUserAsync(skatuser);
            });
        }

        [HttpPut]
        public async Task UpdateSkatUser(SkatUserModel skatUserModel)
        {
          await _databaseContext.ExecuteAsync(async context =>
            {
               var skatuser = await context.SkatUserRepository.GetSkatUserByNameAsync(skatUserModel.SkatUserName);
               if (skatuser != null)
               {
                   skatuser.UserValid = skatUserModel.IsSkatUserValid;
                   await context.SkatUserRepository.UpdateSkatUserAsync(skatuser);
               }
            });
        }
    }
}