using System;
using System.Collections.Generic;

namespace GrooveHub.Models.Entities;

public partial class Artista
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Biografia { get; set; } = null!;
}
