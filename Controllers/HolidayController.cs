using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KKBYFinals.Services;
using KKBYFinals.Models;

namespace KKBYFinals.Controllers;

[Authorize(Roles = "Admin,Cashier,Kitchen Staff")]
public class HolidayController : Controller
{
    private readonly HolidayService _holidayService;

    public HolidayController(HolidayService holidayService)
    {
        _holidayService = holidayService;
    }

    public async Task<IActionResult> Index(int? year)
    {
        int selectedYear = year ?? DateTime.Now.Year;

        try
        {
            List<Holiday> holidays =
                await _holidayService.GetHolidaysAsync(selectedYear);

            ViewBag.Year = selectedYear;

            return View(holidays);
        }
        catch (HttpRequestException)
        {
            ViewBag.Year = selectedYear;
            ViewBag.Error = "Unable to retrieve holidays. Please try again.";
            return View(new List<Holiday>());
        }
    }
}
