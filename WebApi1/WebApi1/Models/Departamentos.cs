using System;
using System.Collections.Generic;

namespace WebApi1.Models;

public partial class Departamentos
{
    public int IdDepartamentos { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Ciudades> Ciudades { get; set; } = new List<Ciudades>();
}
