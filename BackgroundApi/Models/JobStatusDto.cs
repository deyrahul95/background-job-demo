using BackgroundApi.Enums;

namespace BackgroundApi.Models;

public record JobStatusDto(JobStatus Status, string? Message = null);
