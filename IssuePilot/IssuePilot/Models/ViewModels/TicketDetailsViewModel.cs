using IssuePilot.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class TicketDetailsViewModel : TicketBaseModel
    {
        public List<string> CategoriesOfTicket { get; set; }
        public string SelectedTicketStatus { get; set; }
        public string CurrentTicketStatus { get; set; }
        public PaginatedList<Comment> CommentsOfTicket { get; set; }
        public List<string> SelectedAssignees { get; set; }
        public List<string> SelectedAssigneesIds { get; set; }
        public string CommentInputText { get; set; }
        public SelectList StatusList { get; set; }
        public List<SelectListItem> MemberList { get; set; }
        public string ClosedByUser { get; set; }
        public string CreatedByUser { get; set; }
        public int SelectedCommentId { get; set; }
        public string SelectedCommendUserId { get; set; }
        public List<string> ImageDataURLList { get; set; }

    }
}
