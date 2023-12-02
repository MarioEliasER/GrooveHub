using System;
using System.Collections.Generic;

namespace GrooveHub.Models.Entities;

public partial class Album
{
    public int Id { get; set; }

    public string Tituloalbum { get; set; } = null!;

    public DateTime Fechalanzamiento { get; set; }

    public virtual ICollection<Cancion> Cancion { get; set; } = new List<Cancion>();
}
