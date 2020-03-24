using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userID;

        public NoteService(Guid userID)
        {
            _userID = userID;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity = new Note()
            {
                OwnerID = _userID,
                Title = model.Title,
                Content = model.Content,
                CreatedUtc = DateTimeOffset.Now
            };

            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.Notes.Add(entity);
                return dbContext.SaveChanges() == 1;
            }
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.Notes
                        .Where(x => x.OwnerID == _userID)
                        .Select(x => new NoteListItem
                        {
                            NoteID = x.NoteID,
                            Title = x.Title,
                            CreatedUtc = x.CreatedUtc
                        });

                return query.ToArray();
            }
        }
    }
}
