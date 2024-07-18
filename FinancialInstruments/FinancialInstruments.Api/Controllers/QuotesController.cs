using FinancialInstruments.Domain.Models;
using FinancialInstruments.Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialInstruments.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class QuotesController : ControllerBase
	{
		private readonly IQuoteService _quoteService;
		public QuotesController(IQuoteService quoteService)
		{
			_quoteService = quoteService;
		}

		[HttpGet]
		public Instruments GetAll()
		{
			var instruments = new Instruments();
			return instruments;
		}

		[HttpGet("{ticker}")]
		public async Task<Quote> GetQuote(string ticker)
		{
			return await _quoteService.GetQuoteAsync(ticker);
		}
	}
}
