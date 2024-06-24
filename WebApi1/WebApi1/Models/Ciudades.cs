using System;
using System.Collections.Generic;

namespace WebApi1.Models;

public partial class Ciudades
{
    public int IdCiudades { get; set; }

    public string Nombre { get; set; } = null!;

    public int? DepartamentoId { get; set; }

    public virtual Departamentos? Departamento { get; set; }
}
