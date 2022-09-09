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
    public class RepositorioPropietario : RepositorioBase
    {
        protected readonly string connectionString;
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {

        }
        public int AltaPropietario(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO propietario (Nombre, Apellido, Dni, Telefono, Email) " +
                    $"VALUES (@nombre, @apellido, @dni, @telefono, @email);" +
                    "SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@dni", p.DNI);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    command.Parameters.AddWithValue("@email", p.Email);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
                }
            }
            return res;
        }
        public int BajaPropietario(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"DELETE FROM propietario WHERE id = @id";
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
        public int ModificacionPropietario(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"UPDATE propietario SET nombre=@nombre, apellido=@apellido, dni=@dni, telefono=@telefono, email=@email WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", p.Id);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@dni", p.DNI);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    command.Parameters.AddWithValue("@email", p.Email);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public List<Propietario> ObtenerPropietarios()
        {
            List<Propietario> res = new List<Propietario>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT id, nombre, apellido,dni,telefono,email FROM Propietario";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            DNI = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),

                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public Propietario ObtenerPorId(int id)
        {
            Propietario p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id,nombre,apellido,dni,telefono,email FROM propietario" +
                    $" WHERE id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            DNI = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5)
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

    }

}
