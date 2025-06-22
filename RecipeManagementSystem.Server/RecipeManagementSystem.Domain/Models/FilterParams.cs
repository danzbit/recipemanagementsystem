﻿namespace RecipeManagementSystem.Domain.Models;

public class FilterParams
{
    private const int MaxPageSize = 50;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? Category { get; set; }

    public string? Search { get; set; }
}