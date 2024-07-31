﻿namespace RuleConfigurator.Application.Models;

public class Event
{
    public int Id { get; set; }

    public virtual Match Match { get; set; }

    public decimal Odd { get; set; }

    public string Outcome { get; set; }
}