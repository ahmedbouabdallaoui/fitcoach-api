using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;

namespace FitCoach.Api.Mappers;

// Converts between NutritionAdvice entities and DTOs.
public static class NutritionAdviceMapper
{
    // Maps a NutritionAdvice entity to a NutritionAdviceResponse DTO
    public static NutritionAdviceResponse ToResponse(NutritionAdvice advice)
    {
        return new NutritionAdviceResponse
        {
            Id = advice.Id,
            Goal = advice.Goal,
            DailyCalories = advice.DailyCalories,
            ProteinGrams = advice.ProteinGrams,
            CarbsGrams = advice.CarbsGrams,
            FatGrams = advice.FatGrams,
            CreatedAt = advice.CreatedAt,
            MealPlans = advice.MealPlans.Select(m => new MealPlanResponse
            {
                MealType = m.MealType,
                Description = m.Description,
                Calories = m.Calories
            }).ToList()
        };
    }
}