
namespace Events.Web.Models
{
    using Events.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class EventDetailsViewModel
    {
        public int Id { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string AuthorId { get;set; }

      

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Event,EventDetailsViewModel>>ViewModel
        {
            get
            {
                return e => new EventDetailsViewModel()
                {
                    Id = e.Id,
                    Author=e.Author,
                    Location=e.Location,
                    AuthorId = e.AuthorId,
                    Description = e.Description,
                    Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel)
                };
            }
        }
    }
}