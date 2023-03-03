using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaEfler.Models
{
    public class RepositorioContrato : RepositorioBase
    {

        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {

        }
        public int AltaContrato(Contrato c)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO contrato (fecha_desde,fecha_hasta,monto_alquiler,inmuebleId,inquilinoId) 
                VALUES (@fecha_desde,@fecha_hasta,@monto_alquiler,@inmuebleId,@inquilinoId); 
                SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fecha_desde", c.FechaDesde);
                    command.Parameters.AddWithValue("@fecha_hasta", c.FechaHasta);
                    command.Parameters.AddWithValue("@monto_alquiler", c.MontoAlquiler);
                    command.Parameters.AddWithValue("@inmuebleId", c.IdInmueble);
                    command.Parameters.AddWithValue("@inquilinoId", c.IdInquilino);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.Id = res;
                    connection.Close();
                }
            }
            return res;
        }
        public int BajaContrato(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM contrato WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int ModificacionContrato(Contrato c)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE contrato SET fecha_desde= @fecha_desde, 
                fecha_hasta= @fecha_hasta,monto_alquiler= @monto_alquiler
                WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", c.Id);
                    command.Parameters.AddWithValue("@fecha_desde", c.FechaDesde);
                    command.Parameters.AddWithValue("@fecha_hasta", c.FechaHasta);
                    command.Parameters.AddWithValue("@monto_alquiler", c.MontoAlquiler);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public List<Contrato> ObtenerContratos()
        {
            List<Contrato> res = new List<Contrato>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,inmuebleId,
                inquilinoId,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.inmuebleId) 
                JOIN inquilino inq ON (inq.id =c.inquilinoId)
                ORDER BY fecha_desde asc;";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            MontoAlquiler = reader.GetDecimal(3),
                            IdInmueble = reader.GetInt32(4),
                            IdInquilino = reader.GetInt32(5),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(6),
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }

                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public Contrato ObtenerPorId(int id)
        {
            Contrato c = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,inmuebleId,
                inquilinoId,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.inmuebleId) 
                JOIN inquilino inq ON (inq.id =c.inquilinoId)
                WHERE c.id= @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            MontoAlquiler = reader.GetDecimal(3),
                            IdInmueble = reader.GetInt32(4),
                            IdInquilino = reader.GetInt32(5),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(6),
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }
        public List<Contrato> ObtenerContratosVigentes()
        {
            List<Contrato> res = new List<Contrato>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,inmuebleId,
                inquilinoId,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.inmuebleId) 
                JOIN inquilino inq ON (inq.id =c.inquilinoId)
                WHERE fecha_desde<= CURDATE() AND fecha_hasta> CURDATE()
                ORDER BY fecha_desde ASC";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            MontoAlquiler = reader.GetDecimal(3),
                            IdInmueble = reader.GetInt32(4),
                            IdInquilino = reader.GetInt32(5),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(6),
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }

                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public List<Contrato> ObtenerContratosNoVigentes()
        {
            List<Contrato> res = new List<Contrato>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,inmuebleId,
                inquilinoId,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.inmuebleId) 
                JOIN inquilino inq ON (inq.id =c.inquilinoId)
                WHERE fecha_desde<= CURDATE() AND fecha_hasta<= CURDATE()
                ORDER BY fecha_desde ASC";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            MontoAlquiler = reader.GetDecimal(3),
                            IdInmueble = reader.GetInt32(4),
                            IdInquilino = reader.GetInt32(5),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(6),
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }

                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public List<Pago> ObtenerPagosPorContrato(int id)
        {
            List<Pago> res = new List<Pago>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.id, numero_pago,descripcion, fecha_pago,importe,contratoId,inm.direccion,inq.nombre,inq.apellido
                FROM pago p
                JOIN contrato c ON(p.contratoId =c.id)
                JOIN inquilino inq ON(c.inquilinoId = inq.id)
                JOIN inmueble inm ON(c.inmuebleId= inm.id)
                WHERE c.id= @id
                ORDER BY fecha_pago ASC;";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Pago
                        {
                            Id = reader.GetInt32(0),
                            NumeroPago = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            FechaPago = reader.GetDateTime(3),
                            Importe = reader.GetDecimal(4),
                            IdContrato = reader.GetInt32(5),
                            Contrato = new Contrato
                            {
                                Inmueble = new Inmueble
                                {
                                    Direccion = reader.GetString(6),
                                },
                                Inquilino = new Inquilino
                                {
                                    Nombre = reader.GetString(7),
                                    Apellido = reader.GetString(8),
                                }
                            }

                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public Contrato ObtenerContratoVigente(int id)
        {
            Contrato c = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,inmuebleId,
                inquilinoId,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.inmuebleId) 
                JOIN inquilino inq ON (inq.id =c.inquilinoId)
                WHERE c.id= @id AND fecha_desde<= CURDATE() AND fecha_hasta>= CURDATE()
                ORDER BY fecha_desde ASC;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            MontoAlquiler = reader.GetDecimal(3),
                            IdInmueble = reader.GetInt32(4),
                            IdInquilino = reader.GetInt32(5),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(6),
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }
        public List<Inmueble> ObtenerInmueblesDisponibles(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<Inmueble> res = new List<Inmueble>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id,direccion,ambientes,latitud,longitud,
                                precio,propietarioId,tipoInmuebleId,p.nombre,
                                p.apellido,ti.descripcion,uso
                                FROM inmueble i
                                JOIN propietario p ON (p.id= i.propietarioId)
                                JOIN tipo_inmueble ti ON (ti.id = i.tipoInmuebleId)
                                where i.id NOT IN
                                (SELECT i.id
                                FROM inmueble i 
                                LEFT JOIN contrato c ON (i.id =c.inmuebleId) 
                                WHERE ( @fechaDesde BETWEEN fecha_desde AND fecha_hasta) OR (@fechaHasta BETWEEN fecha_desde AND fecha_hasta) OR (@fechaDesde < fecha_desde AND @fechaHasta  > fecha_hasta) ) AND oferta_activa=1;";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.Add("@fechaDesde", MySqlDbType.DateTime).Value = fechaDesde;
                    comm.Parameters.Add("@fechaHasta", MySqlDbType.DateTime).Value = fechaHasta;
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Latitud = reader.GetDecimal(3),
                            Longitud = reader.GetDecimal(4),
                            Precio = reader.GetDecimal(5),
                            IdPropietario = reader.GetInt32(6),
                            IdTipo = reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            },
                            TipoInmueble = new TipoInmueble
                            {
                                Descripcion = reader.GetString(10),
                            },
                            Uso = reader.GetInt32(11)
                        });
                    }
                    conn.Close();
                }
            }
            return res;

        }
        public Contrato ComprobarDisponibilidad(Contrato contrato)
        {
            Contrato c = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,inmuebleId,
                inquilinoId,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.inmuebleId) 
                JOIN inquilino inq ON (inq.id =c.inquilinoId)
                WHERE ( (@fechaDesde BETWEEN fecha_desde AND fecha_hasta) OR (@fechaHasta  BETWEEN fecha_desde AND fecha_hasta) OR (@fechaDesde < fecha_desde AND @fechaHasta > fecha_hasta)) AND inmuebleId= @idInmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@fechaDesde", MySqlDbType.DateTime).Value = contrato.FechaDesde;
                    command.Parameters.Add("@fechaHasta", MySqlDbType.DateTime).Value = contrato.FechaHasta;
                    command.Parameters.Add("@idInmueble", MySqlDbType.Int32).Value = contrato.IdInmueble;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            MontoAlquiler = reader.GetDecimal(3),
                            IdInmueble = reader.GetInt32(4),
                            IdInquilino = reader.GetInt32(5),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(6),
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }

        public List<Contrato> ObtenerContratosPorInmueble(int id)
        {
            List<Contrato> res = new List<Contrato>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.inmuebleId)
                WHERE i.id= @id";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Contrato
                        {
                            Id = reader.GetInt32(0),
                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public List<Contrato> ObtenerContratosPorInquilino(int id)
        {
            List<Contrato> res = new List<Contrato>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id
                FROM contrato c 
                JOIN inquilino inq ON (inq.id =c.inquilinoId)
                WHERE inq.id= @id";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Contrato
                        {
                            Id = reader.GetInt32(0),
                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
    }
}
