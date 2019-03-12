using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Core.Repositories;
using ThePhoneBook.WebApi.Api.Helpers;
using ThePhoneBook.WebApi.Api.PhoneBookEntries.Dtos;
using ThePhoneBook.WebApi.Helpers;
using ThePhoneBook.WebApi.Services;

namespace ThePhoneBook.WebApi.Api.PhoneBookEntries
{
    [Route("api/v{version:apiVersion}/phone-books/{phonebookId}/entries")]
    [ApiController]
    [ApiVersion("1")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PhoneBookEntriesController : ControllerBase
    {
        private readonly ILogger<PhoneBookEntriesController> _logger;
        private readonly IPhoneBookEntryRepository _phoneBookEntryRepository;
        private readonly IPhoneBookRepository _phoneBookRepository;
        private readonly IUserInfoService _userInfoService;
        private readonly IMapper _mapper;

        public PhoneBookEntriesController(
            ILogger<PhoneBookEntriesController> logger,
            IPhoneBookEntryRepository phoneBookEntryRepository,
            IPhoneBookRepository phoneBookRepository,
            IUserInfoService userInfoService,
            IMapper mapper)
        {
            _logger = logger;
            _phoneBookEntryRepository = phoneBookEntryRepository;
            _phoneBookRepository = phoneBookRepository;
            _userInfoService = userInfoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retreives all phone book entries for user
        /// </summary>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <param name="pagingRequest">The info for paging through the entries</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /phone-books/c7ba6add-09c4-45f8-8dd0-eaca221e5d93/entries?page=1
        /// </remarks>
        /// <response code="200">Returns a paged list of the phone book's entries</response>
        /// <response code="204">No content in response body</response>
        /// <response code="404">The requested resource does not exist</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PhoneBookEntryResponse>>> GetPhoneBookEntries(Guid phoneBookId,
            [FromQuery]PagingRequest pagingRequest)
        {
            // Check if phone book exists
            if (!await _phoneBookRepository.ExistsAsync(phoneBookId).ConfigureAwait(false))
            {
                return NotFound();
            }

            IReadOnlyList<PhoneBookEntry> phoneBookEntries = await _phoneBookEntryRepository
                  .GetPhoneBookEntriesForBook(phoneBookId, pagingRequest.Page, pagingRequest.PageSize)
                  .ConfigureAwait(false);

            // Check whether the current phone book have any entries
            if (phoneBookEntries.Count < 1)
            {
                return NoContent();
            }

            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBookEntries.First().PhoneBook.UserId == userId)
            {
                // Get the number of entries for this book
                // Need it for paging
                int phoneBookEntriesCount = await _phoneBookEntryRepository.CountForBookAsync(phoneBookId)
                    .ConfigureAwait(false);

                PagingInfo pagingInfo = new PagingInfo(phoneBookEntriesCount, pagingRequest.Page, pagingRequest.PageSize);
                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(pagingInfo,
                        new JsonSerializerSettings()
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }));

                return Ok(_mapper.Map<IEnumerable<PhoneBookEntryResponse>>(phoneBookEntries));
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to request entries from a phone book owned by {OwningUser}",
              userId, phoneBookEntries.First().PhoneBook.UserId);

            // phone book does not belong to the user
            // forbidden request
            return Forbid();
        }

        /// <summary>
        /// Retreives a specific phone book for user
        /// </summary>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <param name="id">The id of the phone book entry</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /phone-books/c7ba6add-09c4-45f8-8dd0-eaca221e5d93/entries/c7ba6add-09c4-45f8-8dd0-eaca221e5d94
        /// </remarks>
        /// <response code="200">Returns the entries for the specified book</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        /// <response code="404">The requested resource does not exist</response>
        [HttpGet("{id}", Name = "GetPhoneBookEntry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PhoneBookEntryResponse>>> GetPhoneBookEntry(Guid phoneBookId, Guid id)
        {
            // Check if phone book exists
            if (!await _phoneBookRepository.ExistsAsync(phoneBookId).ConfigureAwait(false))
            {
                return NotFound();
            }

            // Check if phone book entry exists
            PhoneBookEntry phoneBookEntry = await _phoneBookEntryRepository.GetPhoneBookEntryWithPhoneBook(id)
                .ConfigureAwait(false);

            if (phoneBookEntry == null)
            {
                return NotFound();
            }

            // Check if the phone book belongs to the current user
            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBookEntry.PhoneBook.UserId == userId)
            {
                return Ok(_mapper.Map<PhoneBookEntryResponse>(phoneBookEntry));
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to request a phone book entry from a phone book owned by {OwningUser}",
              userId, phoneBookEntry.PhoneBook.UserId);

            // phone book does not belong to the user
            // forbidden request
            return Forbid();
        }

        /// <summary>
        /// Deletes an existing phone book entry
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /phone-books/c7ba6add-09c4-45f8-8dd0-eaca221e5d93/entries/c7ba6add-09c4-45f8-8dd0-eaca221e5d94
        ///
        /// </remarks>
        /// <param name="id">The id of the phone book entry to delete</param>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <response code="204">No content in response body</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        /// <response code="404">The requested resource does not exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePhoneBookEntry(Guid phoneBookId, Guid id)
        {
            // Check if phone book exists
            if (!await _phoneBookRepository.ExistsAsync(phoneBookId).ConfigureAwait(false))
            {
                return NotFound();
            }

            // Check if phone book entry exists
            PhoneBookEntry phoneBookEntry = await _phoneBookEntryRepository.GetPhoneBookEntryWithPhoneBook(id)
                .ConfigureAwait(false);

            if (phoneBookEntry == null)
            {
                return NotFound();
            }

            // Check if the phone book belongs to the current user
            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBookEntry.PhoneBook.UserId == userId)
            {
                _phoneBookEntryRepository.Delete(phoneBookEntry);
                await _phoneBookEntryRepository.SaveChangesAsync().ConfigureAwait(false);

                return NoContent();
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to delete a phone book entry from a phone book owned by {OwningUser}",
                          userId, phoneBookEntry.PhoneBook.UserId);

            // the phone book does not belong to the user
            // forbidden request
            return Forbid();
        }

        /// <summary>
        /// Updates an existing phone book entry
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /phone-books/c7ba6add-09c4-45f8-8dd0-eaca221e5d93/entries/c7ba6add-09c4-45f8-8dd0-eaca221e5d94
        ///     {
        ///         "title": "The phone book title",
        ///         "description": "The phone book description"
        ///     }
        /// </remarks>
        /// <param name="id">The id of the phone book entry to update</param>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <param name="phoneBookEntryUpdateRequest">The info to update the phone book with</param>
        /// <response code="200">Returns successfully updated updated entry</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        /// <response code="404">The requested resource does not exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PhoneBookEntryResponse>> UpdatePhoneBookEntry(Guid phoneBookId, Guid id,
            [FromBody] PhoneBookEntryUpdateRequest phoneBookEntryUpdateRequest)
        {
            // Check if phone book exists
            if (!await _phoneBookRepository.ExistsAsync(phoneBookId).ConfigureAwait(false))
            {
                return NotFound();
            }

            // Check if phone book entry exists
            PhoneBookEntry phoneBookEntry = await _phoneBookEntryRepository.GetPhoneBookEntryWithPhoneBook(id)
                .ConfigureAwait(false);

            if (phoneBookEntry == null)
            {
                return NotFound();
            }

            // Check if the phone book belongs to the current user
            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBookEntry.PhoneBook.UserId == userId)
            {
                _mapper.Map(phoneBookEntryUpdateRequest, phoneBookEntry);
                _phoneBookEntryRepository.Update(phoneBookEntry);
                await _phoneBookRepository.SaveChangesAsync();

                return Ok(_mapper.Map<PhoneBookEntryResponse>(phoneBookEntry));
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to add a phonebook entry to a phone book owned by {OwningUser}",
              userId, phoneBookEntry.PhoneBook.UserId);

            // the phone book does not belong to the user
            // forbidden request
            return Forbid();
        }

        /// <summary>
        /// Creates a new phone book entry
        /// </summary>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <param name="phoneBookEntryCreateRequest">The info to create the phone book entry with</param>
        /// <response code="201">The entry was successfully created and added to the phone book</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        /// <response code="404">Resource does not exist</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PhoneBookEntryResponse>> CreatePhoneBookEntry(Guid phoneBookId,
            [FromBody] PhoneBookEntryCreateRequest phoneBookEntryCreateRequest)
        {
            // Check if phone book exists
            PhoneBook phoneBook = await _phoneBookRepository.GetByIdAsync(phoneBookId).ConfigureAwait(false);
            if (phoneBook == null)
            {
                return NotFound();
            }

            // Check if the phone book belongs to the current user
            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBook.UserId == userId)
            {
                PhoneBookEntry phoneBookEntry = _mapper.Map<PhoneBookEntry>(phoneBookEntryCreateRequest);

                phoneBookEntry = await _phoneBookEntryRepository
                    .CreatePhoneBookEntryForBook(phoneBookId, phoneBookEntry)
                    .ConfigureAwait(false);

                await _phoneBookRepository.SaveChangesAsync();

                return CreatedAtRoute("GetPhoneBookEntry", new { phonebookId = phoneBook.Id, id = phoneBookEntry.Id },
                    _mapper.Map<PhoneBookEntryResponse>(phoneBookEntry));
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to request add a phonebook entry to a phone book owned by {OwningUser}",
                          userId, phoneBook.UserId);

            // the phone book does not belong to the user
            // forbidden request
            return Forbid();
        }
    }
}