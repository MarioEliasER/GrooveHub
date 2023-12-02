using System;
using System.Collections.Generic;

namespace GrooveHub.Models.Entities;

public partial class Cancion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime Añolanzamiento { get; set; }

    public TimeOnly Duracion { get; set; }

    public int Idalbum { get; set; }

    public virtual Album IdalbumNavigation { get; set; } = null!;
}
