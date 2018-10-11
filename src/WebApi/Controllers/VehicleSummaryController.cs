using System;
using Microsoft.AspNetCore.Mvc;
using MutualFundPerformance.WebApi.Models;

namespace MutualFundPerformance.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class VehicleSummaryController : Controller
    {
        // GET
        public IActionResult ByDay(VehicleSummaryRequest request)
        {
            try
            {
                new DateTime(
                    request.Date.Year,
                    request.Date.Month,
                    request.Date.Day);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (request.MutualFundIds.Length == 0)
            {
                return Ok(new object[0]);
            }

            return Ok();
        }
    }
}