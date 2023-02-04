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
    public class RepositorioInmueble : RepositorioBase
    {

        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }
        public int AltaInmueble(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO inmueble (direccion,ambientes,latitud,longitud,precio,uso,oferta_activa,propietarioId,tipoInmuebleId) 
                VALUES (@direccion,@ambientes,@latitud,@longitud,@precio,@uso,@oferta_activa,@propietarioId,@tipoInmuebleId); 
                SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    //command.Parameters.AddWithValue("@", i.);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@uso", i.Uso);
                    command.Parameters.AddWithValue("@oferta_activa", i.OfertaActiva);
                    command.Parameters.AddWithValue("@propietarioId", i.IdPropietario);
                    command.Parameters.AddWithValue("@tipoInmuebleId", i.IdTipo);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
                }
            }
            return res;
        }
        public int BajaInmueble(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"DELETE FROM inmueble WHERE id = @id";
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
        public int ModificacionInmueble(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE inmueble SET direccion=@direccion,ambientes=@ambientes,
                latitud=@latitud,longitud=@longitud,precio=@precio,
                uso=@uso,oferta_activa=@oferta_activa,propietarioId=@propietarioId,
                tipoInmuebleId=@tipoInmuebleId 
                WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", i.Id);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@Uso", i.Uso);
                    command.Parameters.AddWithValue("@oferta_activa", i.OfertaActiva);
                    command.Parameters.AddWithValue("@propietarioId", i.IdPropietario);
                    command.Parameters.AddWithValue("@tipoInmuebleId", i.IdTipo);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public List<Inmueble> ObtenerInmuebles()
        {
            List<Inmueble> res = new List<Inmueble>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id,direccion,ambientes,latitud,longitud,
                precio,oferta_activa,propietarioId,tipoInmuebleId,p.nombre,
                p.apellido,ti.descripcion,uso
                FROM inmueble i 
                JOIN propietario p ON(i.propietarioId = p.id) 
                JOIN tipo_inmueble ti ON (i.tipoInmuebleId = ti.id);";
                using (var comm = new MySqlCommand(sql, conn))
                {
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
                            OfertaActiva = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                            IdTipo = reader.GetInt32(8),
                            Duenio = new Propietario
                            {
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10),
                            },
                            TipoInmueble = new TipoInmueble
                            {
                                Descripcion = reader.GetString(11),
                            },
                            Uso = reader.GetInt32(12)
                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public Inmueble ObtenerPorId(int id)
        {
            Inmueble i = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id,direccion,ambientes,latitud,longitud,
                precio,oferta_activa,propietarioId,tipoInmuebleId,p.nombre,
                p.apellido,ti.descripcion,uso
                FROM inmueble i 
                JOIN propietario p ON(i.propietarioId = p.id) 
                JOIN tipo_inmueble ti ON (i.tipoInmuebleId = ti.id)
                WHERE i.id=@id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Latitud = reader.GetDecimal(3),
                            Longitud = reader.GetDecimal(4),
                            Precio = reader.GetDecimal(5),
                            OfertaActiva = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                            IdTipo = reader.GetInt32(8),
                            Duenio = new Propietario
                            {
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10),
                            },
                            TipoInmueble = new TipoInmueble
                            {
                                Descripcion = reader.GetString(11),
                            },
                            Uso = reader.GetInt32(12)
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }
        public List<Inmueble> ObtenerDisponibles()
        {
            List<Inmueble> res = new List<Inmueble>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id,direccion,ambientes,latitud,longitud,
                precio,oferta_activa,propietarioId,tipoInmuebleId,p.nombre,
                p.apellido,ti.descripcion,uso
                FROM inmueble i 
                JOIN propietario p ON(i.propietarioId = p.id) 
                JOIN tipo_inmueble ti ON (i.tipoInmuebleId = ti.id)
                WHERE oferta_activa=1;";
                using (var comm = new MySqlCommand(sql, conn))
                {
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
                            OfertaActiva = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                            IdTipo = reader.GetInt32(8),
                            Duenio = new Propietario
                            {
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10),
                            },
                            TipoInmueble = new TipoInmueble
                            {
                                Descripcion = reader.GetString(11),
                            },
                            Uso = reader.GetInt32(12)
                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public List<Contrato> ObtenerContratosPorInmueble(int id)
        {
            List<Contrato> res = new List<Contrato>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,inmuebleId,
                inquilinoId,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.inmuebleId) 
                JOIN inquilino inq ON (inq.id =c.inquilinoId)
                WHERE i.id = @id;";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    comm.CommandType = CommandType.Text;
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
    }

}
