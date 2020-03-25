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
                            IsStarred = x.IsStarred,
                            CreatedUtc = x.CreatedUtc
                        });

                return query.ToArray();
            }
        }

        public NoteDetail GetNoteByID(int id)
        {
            using(var dbContext = new ApplicationDbContext())
            {
                var entity = dbContext.Notes
                    .Single(x => x.NoteID == id && x.OwnerID == _userID);

                return new NoteDetail
                {
                    NoteID = entity.NoteID,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
            }
        }

        public bool UpdateNote(NoteEdit model)
        {
            using(var dbContext = new ApplicationDbContext())
            {
                var entity = dbContext.Notes
                    .Single(x => x.NoteID == model.NoteID && x.OwnerID == _userID);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return dbContext.SaveChanges() == 1;
            }
        }

        public bool DeleteNote(int noteID)
        {
            using(var dbContext = new ApplicationDbContext())
            {
                var entity = dbContext.Notes
                    .Single(x => x.NoteID == noteID && x.OwnerID == _userID);

                dbContext.Notes.Remove(entity);

                return dbContext.SaveChanges() == 1;
            }
        }
    }
}
