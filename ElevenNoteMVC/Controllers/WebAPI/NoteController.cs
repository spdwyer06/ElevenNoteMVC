using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNoteMVC.Controllers.WebAPI
{
    [Authorize]
    [RoutePrefix("api/Note")]
    public class NoteController : ApiController
    {
        private bool SetStarState(int noteID, bool newState)
        {
            // Create the service
            var userID = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userID);

            // Get the note
            var detail = service.GetNoteByID(noteID);

            // Create the NoteEdit model instance wtih the new star state
            var updatedNote = new NoteEdit
            {
                NoteID = detail.NoteID,
                Title = detail.Title,
                Content = detail.Content,
                IsStarred = newState
            };

            // Return a value indicationg whether or not the update succeeded
            return service.UpdateNote(updatedNote);
        }

        [Route("{id}/Star")]
        [HttpPut]
        public bool ToggleStarOn(int id) => SetStarState(id, true);

        [Route("{id}/Star")]
        [HttpDelete]
        public bool ToggleStarOff(int id) => SetStarState(id, false);
    }
}
