namespace TrainMaster.Domain.Dto
{
    public class CourseEnrollmentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        // Informações do curso
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public string? CourseAuthor { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime CourseEndDate { get; set; }
        public bool CourseIsActive { get; set; }
    }
}

