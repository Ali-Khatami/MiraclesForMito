using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MiraclesForMito.Models
{
	public enum EventViewType
	{
		None,
		Archive, // events before today (events that have already happened)
		Upcoming, // events after today
		TBA // to be announced
	}

	public class EventPaginationModel : PaginationModel
	{
		private const int DEFAULT_PAGE_SIZE = 6;

		/// <summary>
		/// States whether this is an archive or upcoming view.
		/// </summary>
		public EventViewType EventType;

		/// <summary>
		/// The instance of the site's database.
		/// </summary>
		private SiteDB _DBInstance;

		/// <summary>
		/// The message that will display when there are no items from the filters.
		/// </summary>
		public string NoEventsMessage
		{
			get
			{
				string sMessage = null;

				switch (this.EventType)
				{
					case EventViewType.Archive:
						sMessage = "Past events will show here after the start date has passed.";
						break;
					case EventViewType.Upcoming:
						sMessage = "Upcoming events will show here once added.";
						break;
					case EventViewType.TBA:
						sMessage = "Events with undetermined start dates will show up here once added.";
						break;
				}

				return sMessage;
			}
		}
		
		/// <summary>
		/// The filtered and sorted events to display.
		/// </summary>
		public IQueryable<Event> SortedAndFilteredEvents;

		public EventPaginationModel(EventViewType eventType, SiteDB dbInstance) : this(eventType, dbInstance, null, null) { }

		public EventPaginationModel(EventViewType eventType, SiteDB dbInstance, int? pageIndex, int? pageSize)
		{
			base.PageIndex = pageIndex.GetValueOrDefault(0);
			base.PageSize = pageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE);
			base.AdditionalData = JsonConvert.SerializeObject(new { EventType = (int)eventType });
			this.EventType = eventType;
			this._DBInstance = dbInstance;
			this.SortedAndFilteredEvents = this._SortAndFilterEvents();
		}

		private IQueryable<Event> _SortAndFilterEvents()
		{
			IQueryable<Event> events = null;

			switch (this.EventType)
			{
				case EventViewType.Archive:
					// find all the archive events
					events = _DBInstance.Events.Where(mitoEvent => mitoEvent.StartDate.HasValue && mitoEvent.StartDate < DateTime.Today);

					// set the total count
					base.TotalCount = events.Count();

					events = events.OrderBy(mitoEvent => mitoEvent.StartDate)
									.Skip(base.PageIndex.GetValueOrDefault(0) * base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE))
									.Take(base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE));
					break;
				case EventViewType.Upcoming:
					// find the upcoming events
					events = _DBInstance.Events.Where(mitoEvent => mitoEvent.StartDate.HasValue && mitoEvent.StartDate > DateTime.Today);

					// set the total count
					base.TotalCount = events.Count();

					events = events.OrderBy(mitoEvent => mitoEvent.StartDate)
									.Skip(base.PageIndex.GetValueOrDefault(0) * base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE))
									.Take(base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE));
					break;
				case EventViewType.TBA:
					// find the upcoming events
					events = _DBInstance.Events.Where(mitoEvent => !mitoEvent.StartDate.HasValue);

					// set the total count
					base.TotalCount = events.Count();

					events = events.OrderBy(mitoEvent => mitoEvent.Name)
									.Skip(base.PageIndex.GetValueOrDefault(0) * base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE))
									.Take(base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE));
					break;
			}

			return events;
		}
	}
}