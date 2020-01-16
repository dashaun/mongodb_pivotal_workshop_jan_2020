  
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using io.pivotal.mongodb.Data;
using io.pivotal.mongodb.Models;
using io.pivotal.mongodb.Infrastructure;
using System;
using System.Collections.Generic;

namespace io.pivotal.mongodb.Controllers {
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly INoteRepository _noteRepository;

        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [NoCache]
        [HttpGet]
        public async Task<IEnumerable<Note>> Get()
        {
            return await _noteRepository.GetAllNotes();
        }

        // GET api/notes/5 - retrieves a specific note using either Id or InternalId (BSonId)
        [HttpGet("{id}")]
        public async Task<Note> Get(string id)
        {
            return await _noteRepository.GetNote(id) ?? new Note();
        }

        // GET api/notes/text/date/size
        // ex: http://localhost:53617/api/notes/Test/2018-01-01/10000
        [NoCache]
        [HttpGet(template: "{bodyText}/{updatedFrom}/{headerSizeLimit}")]
        public async Task<IEnumerable<Note>> Get(string bodyText, 
                                                DateTime updatedFrom, 
                                                long headerSizeLimit)
        {
            return await _noteRepository.GetNote(bodyText, updatedFrom, headerSizeLimit) 
                        ?? new List<Note>();
        }

        // POST api/notes - creates a new note
        [HttpPost]
        public void Post([FromBody] NoteParam newNote)
        {
            _noteRepository.AddNote(new Note
                                        {
                                            Id = newNote.Id,
                                            Body = newNote.Body,
                                            CreatedOn = DateTime.Now,
                                            UpdatedOn = DateTime.Now,
                                            UserId = newNote.UserId
                                        });
        }

        // PUT api/notes/5 - updates a specific note
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {
            _noteRepository.UpdateNoteDocument(id, value);
        }

        // DELETE api/notes/5 - deletes a specific note
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _noteRepository.RemoveNote(id);
        }
    }
}