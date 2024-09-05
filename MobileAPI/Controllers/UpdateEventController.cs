using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileAPI.Services;
using System;
using System.Threading;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateEventController : ControllerBase
    {
        private readonly ITruckService _truckService;
        public UpdateEventController(ITruckService truckService)
        {
            _truckService = truckService;
        }
        public bool HasPermissionEventServiceEdit { get; set; } = false;
        public int DocTypeID = 48;

        //[HttpGet]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        
    }
}
