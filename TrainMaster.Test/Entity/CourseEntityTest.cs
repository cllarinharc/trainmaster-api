using TrainMaster.Domain.Entity;

namespace TrainMaster.Test.Entities
{
    public class CourseEntityTest
    {
        [Fact]
        public void Should_Create_CourseEntity_With_Valid_Data()
        {
            // Arrange
            var name = "Curso de Teste";
            var description = "Descrição do curso";
            var startDate = new DateTime(2025, 6, 1);
            var endDate = new DateTime(2025, 7, 1);
            var isActive = true;

            // Act
            var course = new CourseEntity
            {
                Name = name,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                IsActive = isActive
            };

            // Assert
            Assert.Equal(name, course.Name);
            Assert.Equal(description, course.Description);
            Assert.Equal(startDate, course.StartDate);
            Assert.Equal(endDate, course.EndDate);
            Assert.True(course.IsActive);
        }

        [Fact]
        public void Should_Create_CourseEntity_With_Default_Values()
        {
            // Act
            var course = new CourseEntity();

            // Assert
            Assert.Null(course.Name);
            Assert.Null(course.Description);
            Assert.Equal(default(DateTime), course.StartDate);
            Assert.Equal(default(DateTime), course.EndDate);
            Assert.False(course.IsActive);
        }
    }
}
