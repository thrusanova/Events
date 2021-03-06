﻿

namespace Events.Web.Controllers
{
    using Events.Data;
    using Events.Web.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Events.Web.Extensions;

    [Authorize]
    public class EventsController : BaseController
    
    {
        //yy5u
   

        [HttpGet]
        public ActionResult My()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var events = this.db.Events
               .Where(e=>e.AuthorId==currentUserId)
               .OrderBy(e=>e.StartDateTime).Take(4)
               .Select(EventViewModel.ViewModel);

            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);
            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventInputModel model)
        {
            if (model !=null && this.ModelState.IsValid)
            {
                var e = new Event()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    StartDateTime = model.StartDateTime,
                    Duration = model.Duration,
                    Description = model.Description,
                    Location = model.Location,
                    IsPublic = model.IsPublic
                };
                this.db.Events.Add(e);
                this.db.SaveChanges();

                this.AddNotification("Event created", NotificationType.INFO);

                return this.RedirectToAction("My");
            }
            return this.View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var eventToEdit = this.LoadEvent(id);
            if (eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            var model = EventInputModel.CreateFromEvent(eventToEdit);
            return this.View(model);
        }
        

        private Event LoadEvent(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventToEdit = this.db.Events
                .Where(e => e.Id == id)
                .FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);
            return eventToEdit;
                
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,EventInputModel model)
        {
            var eventToEdit = this.LoadEvent(id);
            if (eventToEdit==null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            if (model !=null && this.ModelState.IsValid) 
            {
                eventToEdit.Title=model.Title;
                eventToEdit.StartDateTime = model.StartDateTime;
                eventToEdit.Duration = model.Duration;
                eventToEdit.Description = model.Description;
                eventToEdit.Location = model.Location;
                eventToEdit.IsPublic = model.IsPublic;

                this.db.SaveChanges();
                this.AddNotification("Event edited.", NotificationType.INFO);
                return RedirectToAction("My");

            }
            return this.View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var eventToDelete = this.LoadEvent(id);
            if (eventToDelete == null)
            {
                this.AddNotification("Cannot delete event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            var model = EventInputModel.CreateFromEvent(eventToDelete);
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, EventInputModel model)
        {
            var eventToDelete = this.LoadEvent(id);
            if (eventToDelete == null)
            {
                this.AddNotification("Cannot delete event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            if (model != null && this.ModelState.IsValid)
            {
                eventToDelete.Title = model.Title;
                eventToDelete.StartDateTime = model.StartDateTime;
                eventToDelete.Duration = model.Duration;
                eventToDelete.Description = model.Description;
                eventToDelete.Location = model.Location;
                eventToDelete.IsPublic = model.IsPublic;
                this.db.Events.Remove(eventToDelete);
                this.db.SaveChanges();
                this.AddNotification("Event deleted.", NotificationType.INFO);
                return RedirectToAction("My");

            }
            return this.View(model);
        }
        
    }
}