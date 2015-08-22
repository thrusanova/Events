
namespace Events.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Events.Data;

    public class EventInputModel
    {
        [Required(ErrorMessage = "Event title is required.")]
        [StringLength(200, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DateTime StartDateTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public string Description { get; set; }


        [MaxLength(200)]
        public string Location { get; set; }

        [Display(Name="Is Public?")]
        public bool IsPublic { get; set; }

        public static EventInputModel CreateFromEvent(Event eventToEdit)
        {
            return new EventInputModel()
            {
                Title = eventToEdit.Title,
                ImageUrl=eventToEdit.ImageUrl,
                StartDateTime = eventToEdit.StartDateTime,
                Duration = eventToEdit.Duration,
                Location = eventToEdit.Location,
                Description = eventToEdit.Description,
                IsPublic = eventToEdit.IsPublic
            };
        }




       
    }


}