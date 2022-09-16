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
                string sql = @"INSERT INTO inmueble (direccion,ambientes,superficie,latitud,longitud,precio,uso,oferta_activa,id_propietario,id_tipo) 
                VALUES (@direccion,@ambientes,@superficie,@latitud,@longitud,@precio,@uso,@oferta_activa,@id_propietario,@id_tipo); 
                SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    //command.Parameters.AddWithValue("@", i.);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@uso", i.Uso);
                    command.Parameters.AddWithValue("@oferta_activa", i.OfertaActiva);
                    command.Parameters.AddWithValue("@id_propietario", i.IdPropietario);
                    command.Parameters.AddWithValue("@id_tipo", i.IdTipo);
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
                superficie=@superficie,latitud=@latitud,longitud=@longitud,precio=@precio,
                uso=@uso,oferta_activa=@oferta_activa,id_propietario=@id_propietario,
                id_tipo=@id_tipo 
                WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", i.Id);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@Uso", i.Uso);
                    command.Parameters.AddWithValue("@oferta_activa", i.OfertaActiva);
                    command.Parameters.AddWithValue("@id_propietario", i.IdPropietario);
                    command.Parameters.AddWithValue("@id_tipo", i.IdTipo);
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
                string sql = @"SELECT i.id,direccion,ambientes,superficie,latitud,longitud,
                precio,oferta_activa,id_propietario,id_tipo,p.nombre,
                p.apellido,ti.descripcion,uso
                FROM inmueble i 
                JOIN propietario p ON(i.id_propietario = p.id) 
                JOIN tipo_inmueble ti ON (i.id_tipo = ti.id);";
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
                            Superficie = reader.GetDecimal(3),
                            Latitud = reader.GetDecimal(4),
                            Longitud = reader.GetDecimal(5),
                            Precio = reader.GetDecimal(6),
                            OfertaActiva = reader.GetBoolean(7),
                            IdPropietario = reader.GetInt32(8),
                            IdTipo = reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                            },
                            TipoInmueble = new TipoInmueble
                            {
                                Descripcion = reader.GetString(12),
                            },
                            Uso = reader.GetInt32(13)
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
                string sql = @"SELECT i.id,direccion,ambientes,superficie,latitud,longitud,
                precio,oferta_activa,id_propietario,id_tipo,p.nombre,
                p.apellido,ti.descripcion,uso
                FROM inmueble i 
                JOIN propietario p ON(i.id_propietario = p.id) 
                JOIN tipo_inmueble ti ON (i.id_tipo = ti.id)
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
                            Superficie = reader.GetDecimal(3),
                            Latitud = reader.GetDecimal(4),
                            Longitud = reader.GetDecimal(5),
                            Precio = reader.GetDecimal(6),
                            OfertaActiva = reader.GetBoolean(7),
                            IdPropietario = reader.GetInt32(8),
                            IdTipo = reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                            },
                            TipoInmueble = new TipoInmueble
                            {
                                Descripcion = reader.GetString(12),
                            },
                            Uso = reader.GetInt32(13)
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

    }

}
