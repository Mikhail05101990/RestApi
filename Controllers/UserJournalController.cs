using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Filters;

namespace WebApi.Controllers
{

    /// <summary>
    /// Represents journal API
    /// </summary>
    [Tags("user.journal")]
    public class UserJournalController : ControllerBase
    {
        private readonly ILogger<UserJournalController> _logger;
        private readonly StoreContext _db;

        public UserJournalController(ILogger<UserJournalController> logger, StoreContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <remarks>
        /// Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.
        /// </remarks>
        [HttpPost("/api.user.journal.getRange")]
        public IActionResult GetRange([BindRequired][FromQuery]int skip, [BindRequired][FromQuery]int take, [BindRequired][FromBody]Filter filter)
        {
            var res = _db.Events.Where(x => x.CreatedAt >= filter.From & x.CreatedAt <= filter.To);

            if (string.IsNullOrEmpty(filter.Search))
                res = res.Where(x => x.Text.Contains(filter.Search));

            var items = res.Skip(skip).Take(take).Select(x => new Journal(){
                Id = x.Id,
                EventId = x.EventId,
                CreatedAt = x.CreatedAt
            }).ToList();

            return Ok(new JournalData { count = items.Count, items = items, skip = skip });
        }

        /// <remarks>
        /// Returns the information about an particular event by ID.
        /// </remarks>
        [HttpPost("/api.user.journal.getSingle")]
        public IActionResult GetSingle([BindRequired]long id)
        {
            var evt = _db.Events.FirstOrDefault(x => x.EventId == id);

            return Ok(evt);
        }
    }
}
