using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bookfy.Publishers.Api.Boundaries;


public class SearchPublishers
{
    
    [FromQuery(Name = "searchTerm")] public string? SearchTerm { get; set; }
    [FromQuery(Name = "skip")] public long? Skip { get; set; }
    [FromQuery(Name = "take")] public long? Take { get; set; }
}