namespace TrainMaster.Domain.Dto
{
    public record QuestionDto(
        int Id,
        int CourseActivitieId,
        string Statement,
        int Order,
        decimal Points,
        List<QuestionOptionDto> Options
    );
}