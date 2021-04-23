// source: https://fullcalendar.io/
document.addEventListener('DOMContentLoaded', function () {
	var calendarEl = document.getElementById('calendar');

	// Create list of events
	var listOfEvents = [];
	for (let i = 0; i < listOfTicketCreateEvents.length; i++) {
		var dateString = formatDate(listOfTicketCreateEvents[i].Date);

		// Add event to list
		listOfEvents.push({
			title: listOfTicketCreateEvents[i].NumberOfTickets + " Ticket(s) erstellt",
			start: dateString
		})
	}
	for (let j = 0; j < listOfTicketClosedEvents.length; j++) {

		var dateString = formatDate(listOfTicketClosedEvents[j].Date);

		// Add event to list
		listOfEvents.push({
			title: listOfTicketClosedEvents[j].NumberOfTickets + " Ticket(s) geschlossen",
			start: dateString
		})
	}

	// Add options to calendar
	var calendar = new FullCalendar.Calendar(calendarEl, {
		initialView: 'dayGridMonth',
		initialDate: Date.now(),
		headerToolbar: {
			left: 'dayGridMonth,dayGridWeek,listYear today',
			center: 'title',
			right: 'prevYear,prev,next,nextYear'
		},
		eventDidMount: function (info) {
			$(info.el).tooltip({
				title: info.event.title
			});
		},
		events: listOfEvents,
	});

	calendar.render();
});

function formatDate(oldFormatDate) {
	var newDate = new Date(oldFormatDate);
	var date = newDate.getDate();
	var month = newDate.getMonth();
	var year = newDate.getFullYear();
	month = month + 1;

	// Format date for fullcalendar
	if (month < 10) {
		month = "0" + month;
	}
	if (date < 10) {
		date = "0" + date;
	}
	return dateString = year + "-" + month + "-" + date;
}