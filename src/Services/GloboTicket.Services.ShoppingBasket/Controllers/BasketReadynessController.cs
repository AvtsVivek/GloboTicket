using AutoMapper;
using GloboTicket.Services.ShoppingBasket.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.ShoppingBasket.Controllers
{
    [Route("api/basketReadyness")]
    [ApiController]
    public class BasketReadynessController : ControllerBase
    {
        public BasketReadynessController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.CompletedTask;
            return Ok("Its working");
        }
    }
}
