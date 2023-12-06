using System;
using System.Collections.Generic;

namespace GrooveHub.Models.Entities;

public partial class Album
{
    public int Id { get; set; }

    public string TituloAlbum { get; set; } = null!;

    public DateTime FechaLanzamiento { get; set; } = DateTime.Now;

    public virtual ICollection<Cancion> Cancion { get; set; } = new List<Cancion>();
}
