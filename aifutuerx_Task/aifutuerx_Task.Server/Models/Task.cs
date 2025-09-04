using System;
using System.Collections.Generic;

namespace aifutuerx_Task.Server.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int StatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TaskStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
