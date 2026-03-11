using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;

namespace FitCoach.Api.Mappers;

// Converts between TrainingPlan entities and DTOs.
public static class TrainingPlanMapper
{
    // Maps a TrainingPlan entity to a TrainingPlanResponse DTO
    public static TrainingPlanResponse ToResponse(TrainingPlan plan)
    {
        return new TrainingPlanResponse
        {
            Id = plan.Id,
            Goal = plan.Goal,
            FitnessLevel = plan.FitnessLevel,
            DaysPerWeek = plan.DaysPerWeek,
            DurationWeeks = plan.DurationWeeks,
            CreatedAt = plan.CreatedAt,
            Seances = plan.Seances.Select(s => new SeanceResponse
            {
                DayNumber = s.DayNumber,
                MuscleGroup = s.MuscleGroup,
                Exercices = s.Exercices.Select(e => new ExerciceResponse
                {
                    Name = e.Name,
                    Sets = e.Sets,
                    Reps = e.Reps,
                    RestSeconds = e.RestSeconds
                }).ToList()
            }).ToList()
        };
    }
}