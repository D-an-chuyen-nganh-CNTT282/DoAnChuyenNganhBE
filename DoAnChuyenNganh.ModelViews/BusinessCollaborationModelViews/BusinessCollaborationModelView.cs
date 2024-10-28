namespace DoAnChuyenNganh.ModelViews.BusinessCollaborationModelViews
{
    public class BusinessCollaborationModelView
    {
        public enum ProjectStatus
        {
            UnderDiscussion, 
            PendingApproval, 
            Approved, 
            InProgress,
            OnHold, 
            Completed, 
            Ended 
        }
        public string BusinessId { get; set; }

        public string ProjectName { get; set; }

        public required ProjectStatus ProjectStatuss { get; set; }

        public string? Result { get; set; }
    }
}
