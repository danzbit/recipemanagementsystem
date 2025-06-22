using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using RecipeManagementSystem.Application.Caching;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Application.Hubs;
using RecipeManagementSystem.Application.Kafka;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;
using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Application.Services;

public class PdfService(
    IRecipeRepository repository,
    PdfStatusMemoryCache pdfStatusMemoryCache,
    IKafkaProducer kafkaProducer,
    IMapper mapper)
    : IPdfService
{
    private readonly string _basePath = "C:\\Users\\daniil.zbitnyev\\source\\university\\RecipeManagementSystem\\RecipeManagementSystem.Server\\RecipeManagementSystem.Import\\output";
 
    public async Task<Result<Guid>> GeneratePdf(string recipeId)
    {
        if (string.IsNullOrWhiteSpace(recipeId) || !Guid.TryParse(recipeId, out Guid parcedRecipeId))
        {
            return Result<Guid>.Failure(RequestErrors.InvalidId);
        }

        var request = new PdfRequest
        {
            RecipeId = parcedRecipeId
        };

        var result = await repository.GetById(parcedRecipeId);

        if (result.IsFailure)
        {
            return Result<Guid>.Failure(result.Error);
        }

        request.Recipe = mapper.Map<RecipeDto>(result.Value);

        var status = new PdfStatus
        {
            Id = request.Id.ToString(),
            RecipeId = request.RecipeId,
            Status = PdfStatusType.Queued
        };

        pdfStatusMemoryCache.Set(status.Id, status);
        await kafkaProducer.ProduceAsync(KafkaTopics.PdfRequests, request);

        return Result<Guid>.Success(request.Id);
    }
    
    public async Task<Result> UpdateStatus(PdfStatus status)
    {
        var statusFromCache = pdfStatusMemoryCache.Get<PdfStatus>(status.Id);

        if (statusFromCache == null)
        {
            return Result<PdfStatus>.Failure(PdfErrors.PdfStatusNotFound);
        }
        
        return await pdfStatusMemoryCache.UpdateStatus(status.Id, s =>
        {
            s.Status = status.Status;
        });
    }

    public async Task<Result<byte[]>> GetFileBytes(string requestId)
    {
        var status = pdfStatusMemoryCache.Get<PdfStatus>(requestId);
        if (status == null)
        {
            return Result<byte[]>.Failure(PdfErrors.PdfStatusNotFound);
        }

        if (status.Status != PdfStatusType.Completed || string.IsNullOrWhiteSpace(status.RecipeId.ToString()))
        {
            return Result<byte[]>.Failure(PdfErrors.PdfIsNotCompletedYet);
        }

        var result = await repository.GetById(status.RecipeId);

        if (result.IsFailure)
        {
            return Result<byte[]>.Failure(result.Error);
        }

        var file = Path.Combine(_basePath, $"{result.Value.Title}.json");
        
        if (!File.Exists(file))
        {
            return Result<byte[]>.Failure(PdfErrors.PdfFileNotExist);
        }
        
        var fileBytes = File.ReadAllBytes(file);

        return Result<byte[]>.Success(fileBytes);
    }
}