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
    public class RepositorioContrato
    {
        protected readonly string connectionString;
        public RepositorioContrato()
        {
            connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria_efler;SslMode=none";
        }
        public int AltaTipoInmueble(TipoInmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO inquilino (Nombre, Apellido, Dni, Telefono, Email) " +
                    $"VALUES (@nombre, @apellido, @dni, @telefono, @email);" +
                    "SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    /*command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@dni", i.DNI);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    command.Parameters.AddWithValue("@email", i.Email);*/
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
                }
            }
            return res;
        }
        public int BajaTipoInmueble(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"DELETE FROM inquilino WHERE id = @id";
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
        public int ModificacionTipoInmueble(Inquilino i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"UPDATE inquilino SET nombre=@nombre, apellido=@apellido, dni=@dni, telefono=@telefono, email=@email WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", i.Id);
                    command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@dni", i.DNI);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    command.Parameters.AddWithValue("@email", i.Email);
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
                string sql = "SELECT id, fecha_desde, fecha_hasta,monto_alquiler,pago_al_dia,id_inmueble,id_inquilino FROM contrato";
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
                            PagoAlDia = reader.GetBoolean(4),
                            IdInmueble = reader.GetInt32(5),
                            IdInquilino = reader.GetInt32(6),

                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public TipoInmueble ObtenerPorId(int id)
        {
            TipoInmueble i = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id,nombre,apellido,dni,telefono,email FROM inquilino" +
                    $" WHERE id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new TipoInmueble
                        {
                            /* Id = reader.GetInt32(0),
                             Nombre = reader.GetString(1),
                             Apellido = reader.GetString(2),
                             DNI = reader.GetString(3),
                             Telefono = reader.GetString(4),
                             Email = reader.GetString(5)*/
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

    }

}
